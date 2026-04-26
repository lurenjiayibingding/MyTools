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


                //var searchText = "离职证明";
                //var substituteText = "考勤证明";
                //iTextPDFHelper.ReplaceText(@"C:\Users\liujiguang\Desktop\悲催牛马\追觅\离职证明.pdf", searchText, substituteText);

                //PictureHelper.BatchConvertPngToJpg(@"E:\迅雷下载\新建文件夹\新建文件夹");

                //PictureHelper.DrawJpgImg(2000, 2000, System.Drawing.Color.FromArgb(255, 255, 255), @"C:\Users\liujiguang\Desktop\新建文件夹 (2)\1.jpg");

                //var dicturePath = @"C:\Users\liujiguang\Desktop\新建文件夹 (2)\追梦少年ゼ_非常喜欢的壁纸，相册有原图";
                //PictureHelper.HorizontalMerge(new List<string> {
                //    @$"{dicturePath}\03.jpg",
                //    @$"{dicturePath}\01.jpg",
                //    @$"{dicturePath}\02.jpg" },
                //    @$"{dicturePath}\04.jpg");


                FileHelper.Remove(@"E:\下江南2", null, SearchOption.AllDirectories, @"E:\下江南", new string[] { ".arw" }, SearchOption.TopDirectoryOnly);


                Console.WriteLine("Hello, World!");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
