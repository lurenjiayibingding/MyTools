using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolStorage.Definition
{
    /// <summary>
    /// 
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 移除文件名中的后缀
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string RemoveFileSuffix(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return string.Empty;
            }
            return string.Join(".", filePath.Split(".")[0..^1]);
        }
    }
}