using System.Diagnostics;
using System.Text;

namespace ToolStorage.Definition
{
    public class FFmpegHelper
    {

        private readonly string[] videoSuffix;
        private readonly int bufferLine = 2000;

        public FFmpegHelper()
        {
            videoSuffix = new string[] { "MP4", "MOV", "WMV", "FLV", "AVI", "MKV" };
        }

        /// <summary>
        /// 合并目录下的所有视频文件为一个文件
        /// </summary>
        /// <param name="VideoDirectory">目录路径</param>
        /// <param name="OutputPath">合并后文件的路径</param>
        public void MergeAllVideo(string VideoDirectory, string OutputPath)
        {
            var listFileName = "video_list.txt";
            WriteListFile(VideoDirectory, listFileName);

            var command = @$"ffmpeg -f concat -safe 0 -i {VideoDirectory}\{listFileName} -c copy {OutputPath}";
            //Execute(command);
            ExecuteCMD(command);
            //ExecuteCMD("ipconfig");
        }

        /// <summary>
        /// 得到一个目录中所有文件的文件名集合
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>目录下的文件名集合</returns>
        public string[] GetAllVideoName(string directoryPath)
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
        /// 将目录中的所有视频文件的文件名写入到一个文本文件中
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <param name="listFileName">文本文件名</param>
        public void WriteListFile(string directoryPath, string listFileName = "video_list.txt")
        {
            var fileNameList = GetAllVideoName(directoryPath).OrderBy(n => n.Length).ThenBy(n => n).ToArray();
            StringBuilder strContent = new StringBuilder();
            int totalFileCount = fileNameList.Count();
            for (int i = 0; i < totalFileCount; i++)
            {
                if (videoSuffix.Contains(GetSuffix(fileNameList[i]).ToUpperInvariant()))
                {
                    strContent.Append($"file '{fileNameList[i]}'{Environment.NewLine}");
                }
                if (i != 0 && i % bufferLine == 0)
                {
                    AppendText($@"{directoryPath}\{listFileName}", strContent.ToString());
                    strContent.Clear();
                }
            }
            if (strContent.Length > 0)
            {
                AppendText($@"{directoryPath}\{listFileName}", strContent.ToString());
            }
        }

        /// <summary>
        /// 得到文件后缀
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件的后缀</returns>
        private string GetSuffix(string filePath)
        {
            return filePath.Split('.').Last();
        }

        /// <summary>
        /// 向一个文本文件中追加内容
        /// </summary>
        /// <param name="textFilePath">文本文件的路径</param>
        /// <param name="content">需要追加的内容</param>
        private void AppendText(string textFilePath, string content)
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

        public void Execute(string command)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = @"C:\Install\ffmpeg\bin\ffmpeg.exe";
                p.StartInfo.Arguments = " " + command;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                //p.StandardInput.WriteLine(command);
                p.BeginErrorReadLine();
                var output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                p.Close();

                Console.WriteLine(output);
            }
        }

        public void ExecuteCMD(string command)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.StandardInput.WriteLine(command);
                p.StandardInput.WriteLine("exit");
                p.StandardInput.AutoFlush = true;
                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                p.Close();
                Console.WriteLine(output);
            }
        }
    }
}
