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
        /// 将目录中的所有视频文件的文件名写入到一个文本文件中
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <param name="listFileName">文本文件名</param>
        public void WriteListFile(string directoryPath, string listFileName = "video_list.txt")
        {
            var fileNameList = FileHelper.GetAllVideoName(directoryPath).OrderBy(n => n.Length).ThenBy(n => n).ToArray();
            StringBuilder strContent = new StringBuilder();
            int totalFileCount = fileNameList.Count();
            for (int i = 0; i < totalFileCount; i++)
            {
                if (videoSuffix.Contains(FileHelper.GetSuffix(fileNameList[i]).ToUpperInvariant()))
                {
                    strContent.Append($"file '{fileNameList[i]}'{Environment.NewLine}");
                }
                if (i != 0 && i % bufferLine == 0)
                {
                    FileHelper.AppendText($@"{directoryPath}\{listFileName}", strContent.ToString());
                    strContent.Clear();
                }
            }
            if (strContent.Length > 0)
            {
               FileHelper.AppendText($@"{directoryPath}\{listFileName}", strContent.ToString());
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
