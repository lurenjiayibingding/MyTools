using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
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