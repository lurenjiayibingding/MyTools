using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using System.Drawing;
using System.Drawing.Imaging;

namespace ToolStorage.Definition
{
    /// <summary>
    /// 图片处理工具
    /// </summary>
    public class PictureHelper
    {
        /// <summary>
        /// 批量将图片从PNG转为JPG
        /// </summary>
        /// <param name="directoryPath">PNG图片文件夹</param>
        public static void BatchConvertPngToJpg(string directoryPath)
        {
            var directoryInfo = System.IO.Directory.GetFiles(directoryPath);
            Parallel.ForEach(directoryInfo, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 2 }, n =>
            {
                var jpgPath = StringHelper.RemoveFileSuffix(n) + ".jpg";
                Console.WriteLine($"将文件{n}另存为{jpgPath}");
                MultiPlatformConvertPngToJpg(n, jpgPath);
            });
        }

        /// <summary>
        /// 在Windows平台中将PNG转为JPG的方法
        /// </summary>
        /// <param name="pngPath"></param>
        /// <param name="jpgPath"></param>
        public static void ConvertPngToJpg(string pngPath, string jpgPath)
        {
            try
            {
                using (Bitmap pngImage = new Bitmap(pngPath))
                {
                    var jpgEncoder = ImageCodecInfo.GetImageEncoders().FirstOrDefault(n => n.FormatID == ImageFormat.Jpeg.Guid);
                    var jpgQuality = new EncoderParameters(1);
                    //设置质量为100（最高质量）
                    jpgQuality.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                    //保存为JPG文件
                    pngImage.Save(jpgPath, jpgEncoder, jpgQuality);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }
        }

        /// <summary>
        /// 跨平台的的将PNG转为JPG的方法
        /// </summary>
        /// <param name="pngPath"></param>
        /// <param name="jpgPath"></param>
        public static void MultiPlatformConvertPngToJpg(string pngPath, string jpgPath)
        {
            try
            {
                using (Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(pngPath))
                {
                    var encode = new JpegEncoder
                    {
                        Quality = 100
                    };
                    image.Save(jpgPath, encode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }
        }
    }
}
