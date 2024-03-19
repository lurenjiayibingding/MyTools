using iText.Kernel.Pdf;
using iText.Layout;
using Newtonsoft.Json;

namespace ToolStorage.Definition
{
    /// <summary>
    /// 将多个图片合并为一个pdf文件
    /// </summary>
    public class ImageToPDF
    {
        public static void ConvertByiTextSharp(IEnumerable<string> imageFilePaths, string outputPdfPath)
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
                        iText.Layout.Element.Image img = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(imageFile));
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