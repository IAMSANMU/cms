using System;
using System.IO;
using System.Text;
using imow.Core.config;
using Imow.Framework.Engine;
using Imow.Framework.Extensions;

namespace imow.Framework.Tool
{
	/// <summary>
	/// ID 生成帮助类
	/// </summary>
    public static class ID
    {
		private static string BaseIdFilePath;
		private static int SequenceStep = 100;
		private static IdWorker DefaultIdWorker;

		static ID()
		{
		    var config = ImowEngineContext.Current.ResolveConfig<ImowConfig>();
			//从配置文件中获取
			var zoneSetting = config.Idzone;
			if (string.IsNullOrEmpty(zoneSetting))
			{
				throw new ArgumentNullException("id-zone", "id生成规则中的Zone未进行配置。");
			}
			var machineSetting = config.Idmachine; 
			if (string.IsNullOrEmpty(machineSetting))
			{
				throw new ArgumentNullException("id-machine", "id生成规则中的Machine未进行配置。");
			}
			var seqStepSetting = config.Idstep;
			if (string.IsNullOrEmpty(seqStepSetting))
			{
				throw new ArgumentNullException("id-step", "id生成规则中的Step未进行配置。");
			}
			var zone = int.Parse(zoneSetting);
			var machine = int.Parse(machineSetting);
			var idFilePath = config.Idfile;
			idFilePath = string.IsNullOrEmpty(idFilePath) ? "IdFiles" : idFilePath;
			BaseIdFilePath = imow.Framework.Tool.PathHelper.Combine(imow.Framework.Tool.PathHelper.GetAppBasePath(), idFilePath);
			if (!Directory.Exists(BaseIdFilePath))
			{
				Directory.CreateDirectory(BaseIdFilePath);
			}
			ID.SequenceStep = int.Parse(seqStepSetting);
			//创建默认的Id生成器
			DefaultIdWorker = new IdWorker(zone, machine);
		}

		/// <summary>
		/// 获取下个ID
		/// </summary>
		public static long GetNextId()
		{
			return ID.DefaultIdWorker.GetNextId();
		}

		public class IdWorker
		{
			/*
				10:16  time(day) 32768		2014-2103
				26:28 sequence (268435456) 256M	3106/s
				54:4 (zone)		16	
				58:6 (machine)	256	
			*/
			private const int ZONEBITS = 4;
			private const int MAXZONEID = (2 << ZONEBITS) - 1;
			private const int MACHINEBITS = 6;
			private const int MAXMACHINEID = (2 << MACHINEBITS) - 1;
			private const int SEQUENCEBITS = 28;
			private const int DAYSHIFT = SEQUENCEBITS + ZONEBITS + MACHINEBITS;
			private const int SEQUENCESHIFT = ZONEBITS + MACHINEBITS;
			private const int ZONESHIFT = MACHINEBITS;
			private const long dayTicks = 86400L * 1000 * 10000;
			private static readonly long startTime = (new DateTime(2014, 1, 1)).Ticks;
			private long _curDay = startTime / dayTicks;
			private object lockObj = new object();
			private int _zone;
			private int _machine;
			private long _zoneMachine;
			private long _sequence = 0L;
			private long _maxSequence = 0L;
			private string _seedFilePath;

			public IdWorker(int zone, int machine)
			{
				if (zone > MAXZONEID)
				{
					throw new ArgumentOutOfRangeException("zone", "参数不能大于" + MAXZONEID);
				}
				if (machine > MAXMACHINEID)
				{
					throw new ArgumentOutOfRangeException("machine", "参数不能大于" + MAXMACHINEID);
				}
				this._zone = zone;
				this._machine = machine;
				this._zoneMachine = (long)((zone << ZONESHIFT) | machine);
				this._seedFilePath = imow.Framework.Tool.PathHelper.Combine(ID.BaseIdFilePath, string.Concat(this._zone, "-", this._machine, ".id"));
			}

			public long GetNextId()
			{
                byte[] bytes = Guid.NewGuid().ToByteArray();
                return BitConverter.ToInt64(bytes, 0).ToString().Substring(0,15).Tolong();
                //var day = (DateTime.UtcNow.Ticks - startTime) / dayTicks;
                //long rd;
                //lock (lockObj)
                //{
                //	if (this._sequence >= this._maxSequence || day != this._curDay)
                //	{
                //		this.ReadSeedBuffer((int)day);
                //		_curDay = day;
                //	}
                //	rd = this._sequence++;
                //}
                //return (day << DAYSHIFT) | (rd << SEQUENCESHIFT) | this._zoneMachine;
            }

            private void ReadSeedBuffer(int day)
			{
				for (int i = 0; i < 10; i++)
				{
					try
					{
						using (var fs = new FileStream(
							this._seedFilePath,
							FileMode.OpenOrCreate,
							FileAccess.ReadWrite,
							FileShare.Read
							))
						{
							int seed;
							int curDay;
							var buf = new byte[256];
							int l = fs.Read(buf, 0, buf.Length);
							if (l == 0)
							{
								curDay = 0;
								seed = 0;//Math.Abs((int)DateTime.Now.Ticks) & random_mask;
							}
							else
							{
								var s = Encoding.UTF8.GetString(buf, 0, l).Trim();
								var idx = s.Split('|');
								if (idx.Length == 1)
								{
									seed = int.Parse(s);
									curDay = day;
								}
								else
								{
									seed = int.Parse(idx[0].Trim());
									curDay = int.Parse(idx[1].Trim());
								}
							}
							if (curDay != day)
							{
								curDay = day;
								seed = 0;
							}
							this._sequence = seed;
							this._maxSequence = seed + ID.SequenceStep;
							//留空主要为了不出现截断情况
							string refreshContent = string.Format("{0}|{1}                                 "
														, this._maxSequence.ToString()
														, curDay.ToString());
							fs.Seek(0, SeekOrigin.Begin);
							l = Encoding.UTF8.GetBytes(refreshContent, 0, refreshContent.Length, buf, 0);
							fs.Write(buf, 0, l);
							fs.Flush();
						}
						return;
					}
					catch (IOException e)
					{
						if (i == 9)
						{
							//sw.log.write("ident", e);
							throw e;
						}
					}
                    catch (System.Exception e)
					{
						//sw.log.write("ident", e);
						throw e;
					}
					System.Threading.Thread.Sleep(100);
				}
			}
		}
    }
}
