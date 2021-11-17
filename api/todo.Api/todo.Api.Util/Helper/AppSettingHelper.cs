using Microsoft.Extensions.Configuration;

namespace todo.Api.Util.Helper
{
    public class AppSettingHelper
    {
        private static IConfiguration _config;

        public AppSettingHelper(IConfiguration configuration)
        {
            _config = configuration;
        }

        /// <summary>
        /// 读取指定节点的字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadAppSettings(string key)
        {
            try
            {
                if (key.Any())
                {
                    return _config[key];
                }
            }
            catch
            {
                return "";
            }
            return "";
        }
    }
}
