using System;
using System.IO;
using System.Net.Http;
using System.Web;
using Autofac;
using Imow.Framework.Engine;
using Imow.Framework.Tool;
using imow.Core.config;

namespace imow.Framework.Config
{
    public class ConfigManager<T> where T : class, IConfig
    {
        public bool SaveConfig(T configInfo) 
        {
            var configInfoFile = ImowEngineContext.Current.Resolve<T>().GetConfigPath();
            bool isok= SaveConfigInfo(configInfo, IOHelper.GetMapPath(configInfoFile));
            //保存成功之后更新容器
            return isok;
        }

   
        public T GetConfigInfo()
        {
            var configInfoFile = ImowEngineContext.Current.Resolve<T>().GetConfigPath();
            return LoadConfigInfo(IOHelper.GetMapPath(configInfoFile)) ;
        }

        /// <summary>
        /// 从文件中加载配置信息
        /// </summary>
        /// <param name="configInfoFile">配置信息文件路径</param>
        /// <returns>配置信息</returns>
        private T LoadConfigInfo(string configInfoFile) 
        {
            return (T)IOHelper.DeserializeFromXML<T>(configInfoFile);
        }

        /// <summary>
        /// 从文件中加载配置信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="configInfoFile">配置信息文件路径</param>
        /// <returns>配置信息</returns>
        private object LoadConfigInfo(Type type,string configInfoFile) 
        {
            return IOHelper.DeserializeFromXML(type,configInfoFile);
        }

        /// <summary>
        /// 将配置信息保存到文件中
        /// </summary>
        /// <param name="configInfo">配置信息</param>
        /// <param name="configInfoFile">保存路径</param>
        /// <returns>是否保存成功</returns>
        private bool SaveConfigInfo(IConfig configInfo, string configInfoFile)
        {
            return IOHelper.SerializeToXml(configInfo, configInfoFile);
        }

    }
}