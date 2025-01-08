using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolStorage.Definition
{
    public class FileHelper
    {
        /// <summary>
        /// 根据文件名或者文件路径得到文件后缀
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件的后缀</returns>
        public static string GetSuffix(string filePath)
        {
            return filePath.Split('.').Last();
        }

        /// <summary>
        /// 向一个文本文件中追加内容
        /// </summary>
        /// <param name="textFilePath">文本文件的路径</param>
        /// <param name="content">需要追加的内容</param>
        public static void AppendText(string textFilePath, string content)
        {
            FileMode mode;
            if (!File.Exists(textFilePath))
            {
                mode = FileMode.Create;
            }
            else
            {
                mode = FileMode.Append;
            }

            var bytes = !string.IsNullOrWhiteSpace(content) ? Encoding.UTF8.GetBytes(content) : Array.Empty<byte>();

            if (bytes != null && bytes.Length > 0)
            {
                using (FileStream fs = new FileStream(textFilePath, mode, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        /// 得到一个目录中所有文件的文件名集合
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>目录下的文件名集合</returns>
        public static string[] GetAllFilePaths(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                return new string[0];
            }
            if (!Directory.Exists(directoryPath))
            {
                return new string[0];
            }
            return Directory.GetFiles(directoryPath);
        }

        /// <summary>
        /// 得到一个目录中的所有子目录
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>目录下的文件名集合</returns>
        public static string[] GetAllDirectoryPaths(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                return new string[0];
            }
            if (!Directory.Exists(directoryPath))
            {
                return new string[0];
            }
            return Directory.GetDirectories(directoryPath);
        }
    }
}