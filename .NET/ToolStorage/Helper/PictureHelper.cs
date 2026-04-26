using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        /// <param name="savePath"></param>
        public static void DrawJpgImg(int width, int height, System.Drawing.Color color, string savePath)
        {
            using (Bitmap map = new Bitmap(width, height))
            {
                using (Graphics graphics = Graphics.FromImage(map))
                {
                    graphics.Clear(color);
                }
                map.Save(savePath, ImageFormat.Jpeg);
            }
            Console.WriteLine($"图片{savePath}生成成功");
        }

        /// <summary>
        /// 水平合并图片
        /// </summary>
        /// <param name="imagePaths"></param>
        /// <param name="outputJpgPath"></param>
        public static void HorizontalMerge(IEnumerable<string> imagePaths, string outputJpgPath)
        {
            try
            {
                var images = imagePaths.Select(path => SixLabors.ImageSharp.Image.Load<Rgba32>(path)).ToList();
                if (images.Count == 0)
                {
                    Console.WriteLine("未找到图片，无法合并。");
                    return;
                }

                // 计算目标高度（取所有图片的最大高度）
                int targetHeight = images.Max(img => img.Height);

                // 计算每张图片等比例缩放后的宽度
                var resizedImages = images.Select(img =>
                {
                    if (img.Height == targetHeight)
                        return img;
                    int newWidth = (int)Math.Round(img.Width * (targetHeight / (double)img.Height));
                    var clone = img.Clone(ctx => ctx.Resize(newWidth, targetHeight));
                    img.Dispose();
                    return clone;
                }).ToList();

                int totalWidth = resizedImages.Sum(img => img.Width);

                using (var result = new SixLabors.ImageSharp.Image<Rgba32>(totalWidth, targetHeight))
                {
                    int offsetX = 0;
                    foreach (var img in resizedImages)
                    {
                        result.Mutate(ctx => ctx.DrawImage(img, new SixLabors.ImageSharp.Point(offsetX, 0), 1f));
                        offsetX += img.Width;
                        img.Dispose();
                    }

                    var encoder = new JpegEncoder { Quality = 100 };
                    result.Save(outputJpgPath, encoder);
                }
                Console.WriteLine($"图片已合并并保存到: {outputJpgPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }
        }
    }
}
