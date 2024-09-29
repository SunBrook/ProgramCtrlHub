/*
 * @author: S 2024/9/27 17:05:12
 */

using System.Configuration;

namespace WindowsCtrlHub.tools
{
    /// <summary>
    /// 获取Appkey配置值
    /// </summary>
    public static class AppConfigValue
    {
        /// <summary>
        /// 获取Appkey配置值
        /// </summary>
        /// <param name="appKey">appKey</param>
        /// <returns></returns>
        public static string Get(string appKey)
        {
#if DEBUG
            return ConfigurationManager.AppSettings[$"debug.{appKey}"];
#elif RELEASE
            return ConfigurationManager.AppSettings[appKey];
#else
            throw new System.Exception("编译配置错误");
#endif


        }
    }
}
