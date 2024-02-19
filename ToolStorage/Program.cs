using ToolStorage.Definition;

namespace ToolStorage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FFmpegHelper ffmpeg=new FFmpegHelper();
            //ffmpeg.MergeAllVideo(@"C:\Users\刘继光的PC\Desktop\视频文件", @"C:\Users\刘继光的PC\Desktop\视频文件\new5.mp4");

            ffmpeg.WriteListFile(@"D:\2024春节\2024021010");

            Console.WriteLine("Hello, World!");
        }
    }
}
