using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Utility
{

    /// <summary>
    /// 配置文件操作帮助类
    /// </summary>
    public static class ConfigHelper
    {
        private static readonly Configuration config =
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        /// <summary>
        /// 增加AppSetting配置节的配置内容，如果存在同名key,则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddAppSetting(string key, string value)
        {
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save();
            ConfigurationManager.RefreshSection(@"appSettings");// 刷新命名节，在下次检索它时将从磁盘重新读取它。记住应用程序要刷新节点
        }

        /// <summary>
        /// 取得AppSetting配置节的配置内容
        /// </summary>
        /// <param name="key">配置节的key</param>
        /// <returns>string类型的配置内容</returns>
        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
            //return ConfigurationManager.AppSettings.Get(key);

        }

    }
}
