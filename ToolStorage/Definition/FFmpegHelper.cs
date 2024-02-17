using System.Diagnostics;

namespace ToolStorage.Definition
{
    public class FFmpegHelper
    {
        public void MergeAllVideo(string VideoDirectory, string OutputPath)
        {
            var command = @$"ffmpeg -f concat -safe 0 -i {VideoDirectory}\video_list.txt -c copy {OutputPath}";
            //Execute(command);
            ExecuteCMD(command);
            //ExecuteCMD("ipconfig");
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
