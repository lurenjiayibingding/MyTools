using Microsoft.Win32;
using System.Drawing.Text;
using System.Runtime.Versioning;

namespace ToolStorage.Definition
{
    /// <summary>
    /// Windows系统帮助类
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class WinSystemHelper
    {
        /// <summary>
        /// 获取已经安装的所有字体的名称及其安装目录
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllFontDic()
        {
            var dic = new Dictionary<string, string>();
            var localRegistryKey = Registry.LocalMachine;
            //若OpenSubKey方法的writable参数设置为true，则应用程序在Win7以上系统中需要以管理员身份运行
            //需要增加应用程序配置清单文件app.manifest，并且配置requestedExecutionLevel节点中的level为requireAdministrator
            var fontRegistryKey = localRegistryKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Fonts", false);
            var fontNames = fontRegistryKey.GetValueNames();
            foreach (var item in fontNames)
            {
                //获取字体的文件名
                string fontName = fontRegistryKey.GetValue(item).ToString();
                string suffix = fontName.Substring(fontName.Length - 4).ToUpper();
                if ((suffix == ".TTF" || suffix == "TTC") && fontName.Substring(1, 2).ToUpper() != @":\")
                {
                    string val = item.Substring(0, item.Length - 11);
                    dic[val] = @"C:\WINDOWS\Fonts\" + fontName;
                }
            }

            return dic;
        }

        /// <summary>
        /// 得到安装在系统中的所有字体的名称集合
        /// </summary>
        public static List<string> GetAllFontsName()
        {
            var result = new List<string>();
            //需注意InstalledFontCollection仅能获取到在InstalledFontCollection对象实例化之前安装的字体信息
            var fonts = new InstalledFontCollection();
            foreach (var font in fonts.Families)
            {
                result.Add(font.Name);
            }
            return result;
        }
    }
}
