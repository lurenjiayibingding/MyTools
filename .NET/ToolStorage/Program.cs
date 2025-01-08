using System;
using ToolStorage.Definition;

namespace ToolStorage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //FFmpegHelper ffmpeg = new FFmpegHelper();
            //ffmpeg.MergeAllVideo(@"C:\Users\刘继光的PC\Desktop\视频文件", @"C:\Users\刘继光的PC\Desktop\视频文件\new5.mp4");
            //ffmpeg.WriteListFile(@"D:\2024春节\2024021010");
            //ffmpeg.MergeAllVideo(@"D:\2024春节\2024021010", @"D:\2024春节\2024021010\merge.mp4");

            //ffmpeg.WriteListFile(@"D:\2024春节\2024021010");
            //ffmpeg.MergeAllVideo(@"E:\2024春节\20240205", @"E:\2024春节\20240205\20240205.mp4");

            //var allDicect = FileHelper.GetAllDirectoryPaths(@"D:\下载\百度\哆啦A梦");
            //foreach (var path in allDicect)
            //{
            //    var num = path.Substring(path.Length - 2, 2);
            //    ImageToPDF.ConvertByiText(FileHelper.GetAllFilePaths(path), @$"{path}\哆啦A梦_{num.ToString()}.pdf");
            //}


            //var searchText = "类和泛型";
            //var substituteText = "泛型和类";
            //iTextPDFHelper.ReplaceText(@"C:\Users\刘继光的PC\Desktop\离职测试\C#经典实例.经典.必读.非常好.pdf", searchText, substituteText);

            PictureHelper.BatchConvertPngToJpg(@"E:\迅雷下载\新建文件夹\新建文件夹");

            Console.WriteLine("Hello, World!");
        }
    }
}
