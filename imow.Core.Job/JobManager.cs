using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using imow.Core.Job.Jobs;
using Imow.Framework.Engine.TypeFinder;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace imow.Core.Job
{

    public class JobManager : IDisposable
    {


        private IScheduler _sched;
        private bool _canStart;

        public JobManager Init()
        {
            ISchedulerFactory schedf = new StdSchedulerFactory();
            _sched = schedf.GetScheduler();
            //TODO:这里只设置了一个触发器,要更具配置文件去动态创建触发器
            var typeFinder = new WebAppTypeFinder();
            var jobTypes = typeFinder.FindClassesOfType<IImowjob>().ToList();
            foreach (var jobType in jobTypes)
            {
                IImowjob imowjob = (IImowjob) Activator.CreateInstance(jobType);
                var trigger = imowjob.GeTrigger();
                if (trigger == null)
                {
                    int seconds = 30;
                    trigger =TriggerBuilder.Create().StartNow()
                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(seconds).RepeatForever()).Build();
                }
                IJobDetail job = JobBuilder.Create(jobType).Build();
                //加入作业调度池中
                _sched.ScheduleJob(job, trigger);
            }
            _canStart = jobTypes.Count > 0;
            return this;
        }

        public void Start()
        {
            if (_canStart)
            {
                _sched.Start();
            }
            else
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            _sched = null;
        }
    }
}