using System;
using ToolStorage.Definition;

namespace ToolStorage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
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
                //iTextPDFHelper.RemoveSpecifiedPage2(@"C:\Users\liujiguang\Desktop\人工智能：现代方法（第4版）.pdf", new int[] { 1 });

                //PictureHelper.BatchConvertPngToJpg(@"E:\迅雷下载\新建文件夹\新建文件夹");

                //PictureHelper.DrawJpgImg(2000, 2000, System.Drawing.Color.FromArgb(255, 255, 255), @"C:\Users\liujiguang\Desktop\新建文件夹 (2)\1.jpg");

                var dicturePath = @"C:\Users\liujiguang\Desktop\新建文件夹 (2)\追梦少年ゼ_非常喜欢的壁纸，相册有原图";
                PictureHelper.HorizontalMerge(new List<string> {
                    @$"{dicturePath}\03.jpg",
                    @$"{dicturePath}\01.jpg",
                    @$"{dicturePath}\02.jpg" },
                    @$"{dicturePath}\04.jpg");

                Console.WriteLine("Hello, World!");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
