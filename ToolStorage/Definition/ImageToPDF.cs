using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using Newtonsoft.Json;

namespace ToolStorage.Definition
{
    /// <summary>
    /// 将多个图片合并为一个pdf文件
    /// </summary>
    public class ImageToPDF
    {
        /*
         * 通过itext7处理pdf时还需要安装itext7.bouncy-castle-adapter包
         */

        /// <summary>
        /// 默认的最简单的将多个图片合并为一个pdf的方法
        /// </summary>
        /// <param name="imageFilePaths">需要合并到pdf中的所有图片的文件路径集合</param>
        /// <param name="outputPdfPath">合并之后pdf文件的保存路径</param>
        public static void ConvertByiText(IEnumerable<string> imageFilePaths, string outputPdfPath)
        {
            if (imageFilePaths == null || !imageFilePaths.Any())
            {
                return;
            }

            try
            {
                using (FileStream fos = new FileStream(outputPdfPath, FileMode.OpenOrCreate))
                {
                    PdfWriter writer = new PdfWriter(fos);
                    PdfDocument pdfDoc = new PdfDocument(writer);
                    Document doc = new Document(pdfDoc);

                    foreach (var imageFile in imageFilePaths)
                    {
                        if (!File.Exists(imageFile))
                        {
                            Console.WriteLine($"文件{imageFile}不存在");
                            continue;
                        }

                        Image img = new Image(ImageDataFactory.Create(imageFile));
                        doc.Add(img);
                        Console.WriteLine($"图片{imageFile}已添加到文件{outputPdfPath}中");
                    }

                    doc.Close();
                }

                Console.WriteLine($"PDF {outputPdfPath} 创建完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建{outputPdfPath}时发生异常:" + JsonConvert.SerializeObject(ex));
            }
        }

        /// <summary>
        /// 默认的最简单的将多个图片合并为一个pdf的方法
        /// </summary>
        /// <param name="imageFilePaths">需要合并到pdf中的所有图片的文件路径集合</param>
        /// <param name="outputPdfPath">合并之后pdf文件的保存路径</param>
        public static void AdaptiveConvertByiText(IEnumerable<string> imageFilePaths, string outputPdfPath)
        {
            if (imageFilePaths == null || !imageFilePaths.Any())
            {
                return;
            }

            try
            {
                using (FileStream fos = new FileStream(outputPdfPath, FileMode.OpenOrCreate))
                {
                    PdfWriter writer = new PdfWriter(fos);
                    PdfDocument pdfDoc = new PdfDocument(writer);
                    Document doc = new Document(pdfDoc);

                    //文档默认的边框距离
                    float leftMargin = doc.GetLeftMargin();
                    float rightMargin = doc.GetRightMargin();
                    float topMargin = doc.GetTopMargin();
                    float bottomMargin = doc.GetBottomMargin();

                    //页面宽度和高度
                    float pageWidth = PageSize.A4.GetWidth();
                    float pageHeight = PageSize.A4.GetHeight();

                    foreach (var imagePath in imageFilePaths)
                    {
                        if (!File.Exists(imagePath))
                        {
                            Console.WriteLine($"文件{imagePath}不存在");
                            continue;
                        }

                        ImageData imageData = ImageDataFactory.Create(imagePath);
                        Image img = new Image(imageData);
                        //img.SetAutoScale(true);
                        //img.SetProperty(Property.HORIZONTAL_ALIGNMENT, HorizontalAlignment.CENTER);
                        //img.SetProperty(Property.VERTICAL_ALIGNMENT, VerticalAlignment.MIDDLE);

                        //图片宽度和高度
                        float imgWidth = img.GetImageWidth();
                        float imgHeight = img.GetImageHeight();

                        // 计算缩放比例  
                        float scaleWidth = pageWidth / imgWidth;
                        float scaleHeight = pageHeight / imgHeight;
                        float scale = Math.Min(scaleWidth, scaleHeight);

                        // 缩放图片  
                        float scaledWidth = imgWidth * scale;
                        float scaledHeight = imgHeight * scale;
                        img = img.SetWidth(scaledWidth).SetHeight(scaledHeight);



                        //计算图片居中放置的坐标
                        float xPosition = (pageWidth - scaledWidth) / 2;
                        float yPosition = (pageHeight - scaledHeight) / 2;

                        // 设置图片绝对位置并添加到文档
                        img.SetFixedPosition(leftMargin, yPosition);
                        doc.Add(img);

                        //添加换页符
                        doc.Add(new AreaBreak());
                        Console.WriteLine($"图片{imagePath}已添加到文件{outputPdfPath}中");
                    }

                    doc.Close();
                }

                Console.WriteLine($"PDF {outputPdfPath} 创建完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建{outputPdfPath}时发生异常:" + JsonConvert.SerializeObject(ex));
            }
        }
    }
}