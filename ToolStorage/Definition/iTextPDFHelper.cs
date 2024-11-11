using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Layout;
using iText.Layout.Element;
using Newtonsoft.Json;
using ToolStorage.Definition.iTextPDFExtend;
using static iText.Kernel.Font.PdfFontFactory;

namespace ToolStorage.Definition
{
    /// <summary>
    /// 通过iText7处理Pdf文档
    /// </summary>
    public class iTextPDFHelper
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

                        iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(imageFile));
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
        /// 将多个图片合并为一个pdf，图片在每个页面中保持居中
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

                    var imageFilePathArray = imageFilePaths.ToArray();
                    int total = imageFilePathArray.Length;
                    for (int i = 0; i < total; i++)
                    {
                        var imagePath = imageFilePathArray[i];
                        if (!File.Exists(imagePath))
                        {
                            Console.WriteLine($"文件{imagePath}不存在");
                            continue;
                        }

                        ImageData imageData = ImageDataFactory.Create(imagePath);
                        iText.Layout.Element.Image img = new iText.Layout.Element.Image(imageData);
                        //img.SetAutoScale(true);
                        //img.SetProperty(Property.HORIZONTAL_ALIGNMENT, HorizontalAlignment.CENTER);
                        //img.SetProperty(Property.VERTICAL_ALIGNMENT, VerticalAlignment.MIDDLE);

                        //图片宽度和高度
                        float imgWidth = img.GetImageWidth();
                        float imgHeight = img.GetImageHeight();

                        // 计算缩放比例  
                        float scaleWidth = (pageWidth - leftMargin - rightMargin) / imgWidth;
                        float scaleHeight = (pageHeight - topMargin - bottomMargin) / imgHeight;
                        float scale = Math.Min(scaleWidth, scaleHeight);

                        // 缩放图片  
                        float scaledWidth = imgWidth * scale;
                        float scaledHeight = imgHeight * scale;
                        img = img.SetWidth(scaledWidth).SetHeight(scaledHeight);


                        //计算图片居中放置的坐标
                        float xPosition = scaledWidth >= pageWidth ? leftMargin : (pageWidth - scaledWidth) / 2;
                        float yPosition = scaledHeight >= pageHeight ? topMargin : (pageHeight - scaledHeight) / 2;

                        // 设置图片绝对位置并添加到文档
                        img.SetFixedPosition(xPosition, yPosition);
                        doc.Add(img);

                        //添加换页符
                        if (i < total - 1)
                        {
                            doc.Add(new AreaBreak());
                        }
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

        /// <summary>
        /// 删除pdf中的指定页
        /// 只会删除指定页面但是会保留书签，目录等其他信息
        /// </summary>
        /// <param name="inputPath">需要删除的pdf文件的路径</param>
        /// <param name="pageNums">需要删除的页数</param>
        public static void RemoveSpecifiedPage(string inputPath, IEnumerable<int> pageNums)
        {
            var outputPath = inputPath.Split('.').First() + "（副本）.pdf";
            using (PdfReader pdfReader = new PdfReader(inputPath))
            {
                using (PdfWriter writer = new PdfWriter(outputPath))
                {
                    using (PdfDocument document = new PdfDocument(pdfReader, writer))
                    {
                        int i = 0;
                        foreach (var item in pageNums)
                        {
                            document.RemovePage(item - i);
                            i++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 删除pdf中的指定页
        /// 同时会删除书签，目录等其他信息
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="pageNums"></param>
        public static void RemoveSpecifiedPage2(string inputPath, IEnumerable<int> pageNums)
        {
            var pdfReader = new PdfReader(inputPath);
            var inputDocument = new PdfDocument(pdfReader);
            var outputPath = inputPath.Split('.').First() + "（副本）.pdf";
            var pdfWriter = new PdfWriter(outputPath);
            var outputDocument = new PdfDocument(pdfWriter);

            var pageTotal = inputDocument.GetNumberOfPages();
            for (var i = 1; i <= pageTotal; i++)
            {
                if (!pageNums.Contains(i))
                {
                    var pdfPage = inputDocument.GetPage(i);
                    outputDocument.AddPage(pdfPage.CopyTo(outputDocument));
                }
            }

            outputDocument.Close();
            inputDocument.Close();
            pdfWriter.Close();
            pdfReader.Close();
        }

        /// <summary>
        /// 从pdf中删除指定范围内的页数
        /// </summary>
        /// <param name="inputPath">需要删除的pdf文件的路径</param>
        /// <param name="startPage">删除范围的开始页</param>
        /// <param name="endPage">删除范围的结束页</param>
        public static void RemvoeRangePage(string inputPath, int startPage, int endPage)
        {
            if (!File.Exists(inputPath))
            {
                return;
            }
            var outputPath = inputPath.Split('.').First() + "（副本）.pdf";
            if (File.Exists(outputPath))
            {
                return;
            }

            using (PdfReader reader = new PdfReader(inputPath))
            {
                using (PdfWriter writer = new PdfWriter(outputPath))
                {
                    using (PdfDocument document = new PdfDocument(reader, writer))
                    {
                        var pageTotal = document.GetNumberOfPages();
                        if (pageTotal < startPage)
                        {
                            return;
                        }
                        if (pageTotal < endPage)
                        {
                            endPage = pageTotal;
                        }
                        for (int i = 1; i <= endPage - startPage + 1; i++)
                        {
                            document.RemovePage(startPage);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 替换pdf中的文本
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="searchText"></param>
        /// <param name="substituteText"></param>
        public static void ReplaceText(string inputPath, string searchText, string substituteText)
        {
            //新输入的文本长度不得大于被替换的文本，否则会导致排版问题
            if (substituteText.Length > searchText.Length)
            {
                return;
            }

            if (!File.Exists(inputPath))
            {
                return;
            }
            var outputPath = inputPath.Split('.').First() + "（副本）.pdf";

            //指定该了固定的字体，未使用pdf中的字体
            var font = PdfFontFactory.CreateFont("C:/WINDOWS/Fonts/SIMHEI.TTF", PdfEncodings.IDENTITY_H, EmbeddingStrategy.FORCE_EMBEDDED, false);

            using (PdfReader reader = new PdfReader(inputPath))
            {
                using (PdfWriter writer = new PdfWriter(outputPath))
                {
                    using (PdfDocument document = new PdfDocument(reader, writer))
                    {
                        var pageTotal = document.GetNumberOfPages();
                        for (int i = 1; i <= pageTotal; i++)
                        {
                            var currentPage = document.GetPage(i);
                            var strategy = new TextOverWritingListener(searchText, substituteText, currentPage);
                            PdfCanvasProcessor processor = new PdfCanvasProcessor(strategy);
                            processor.ProcessPageContent(currentPage);
                            // 在处理完成之后调用替换逻辑
                            strategy.ReplaceSingleText(false, font);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 覆盖pdf中的文本
        /// </summary>
        /// <param name="inputPath"></param>
        /// <param name="searchText"></param>
        /// <param name="substituteText"></param>
        public static void CoverText(string inputPath, string searchText)
        {
            if (!File.Exists(inputPath))
            {
                return;
            }
            var outputPath = string.Join(".", inputPath.Split('.').ToList().Slice(0, -1)) + "（副本）.pdf";

            //指定该了固定的字体，未使用pdf中的字体
            var font = PdfFontFactory.CreateFont("C:/WINDOWS/Fonts/SIMHEI.TTF", PdfEncodings.IDENTITY_H, EmbeddingStrategy.FORCE_EMBEDDED, false);

            using (PdfReader reader = new PdfReader(inputPath))
            {
                using (PdfWriter writer = new PdfWriter(outputPath))
                {
                    using (PdfDocument document = new PdfDocument(reader, writer))
                    {
                        var pageTotal = document.GetNumberOfPages();
                        for (int i = 1; i <= pageTotal; i++)
                        {
                            var currentPage = document.GetPage(i);
                            var strategy = new TextRenderInfoCoverListener(searchText, currentPage);
                            PdfCanvasProcessor processor = new PdfCanvasProcessor(strategy);
                            processor.ProcessPageContent(currentPage);
                            // 在处理完成之后调用替换逻辑
                            strategy.CoverText();
                        }
                    }
                }
            }
        }
    }
}