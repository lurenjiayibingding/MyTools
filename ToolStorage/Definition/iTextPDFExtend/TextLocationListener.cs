using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Text;
using Newtonsoft.Json;

namespace ToolStorage.Definition.iTextPDFExtend
{
    /// <summary>
    /// itext7文本定位扩展类
    /// </summary>
    public class TextLocationListener : IEventListener
    {
        private readonly string searchText;
        private readonly string replacementText;
        private readonly PdfPage pdfPage;
        private readonly List<TextChunk> textInfos = new List<TextChunk>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="replacementText"></param>
        public TextLocationListener(string searchText, string replacementText, PdfPage pdfPage)
        {
            this.searchText = searchText;
            this.replacementText = replacementText;
            this.pdfPage = pdfPage;
        }

        public void EventOccurred(IEventData data, EventType type)
        {
            if (type == EventType.RENDER_TEXT)
            {
                TextRenderInfo info = (TextRenderInfo)data;
                // 调用 preserveGraphicsState 来保留图形状态
                info.PreserveGraphicsState();
                string text = info.GetText();
                if (!string.IsNullOrEmpty(text))
                {
                    textInfos.Add(new TextChunk(info));
                }
            }
        }

        public ICollection<EventType> GetSupportedEvents()
        {
            return null;
        }

        public void ReplaceText()
        {
            StringBuilder fullText = new StringBuilder();
            foreach (var info in textInfos)
            {
                fullText.Append(info.Text);
            }

            string strFullText = fullText.ToString();
            var index = strFullText.IndexOf(searchText, StringComparison.InvariantCulture);
            if (index != -1)
            {
                ReplaceTextInPdf(index, searchText.Length);
            }
        }

        private void ReplaceTextInPdf(int startIndex, int length)
        {
            // 我们假设替换后的文本长度与原文本长度一致。如果长度不一致，需要调整此处的排版逻辑。
            PdfCanvas pdfCanvas = new PdfCanvas(pdfPage);
            // 删除旧文本的区域
            for (int i = startIndex; i < startIndex + length; i++)
            {
                var chunk = textInfos[i];

                //绘制空白矩形区域以覆盖旧文本
                pdfCanvas.Rectangle(chunk.GetRectangle());
                //设置矩形填充颜色为白色
                pdfCanvas.SetFillColorRgb(1, 1, 1);
                //在每次绘制矩形之后使用 Fill() 方法来填充矩形的颜色。Fill() 会根据当前的填充颜色来绘制矩形
                pdfCanvas.Fill();

                //if (replacementText.Length > i - startIndex)
                //{
                //    //新绘制的文字为黑色
                //    pdfCanvas.SetFillColorRgb(0, 0, 0);
                //    pdfCanvas.BeginText();
                //    pdfCanvas.SetFontAndSize(chunk.Font, chunk.FontSize);
                //    pdfCanvas.MoveText(chunk.Position.Get(0), chunk.Position.Get(1));
                //    //pdfCanvas.ShowText(replacementText[i - startIndex].ToString()); // 绘制新文本
                //    pdfCanvas.NewlineShowText(chunk.Position.Get(0), chunk.Position.Get(1), replacementText[i - startIndex].ToString()); // 绘制新文本
                //    pdfCanvas.EndText();
                //}
            }
        }
    }

    class TextChunk
    {
        public string Text { get; private set; }
        public float FontSize { get; private set; }
        public iText.Kernel.Pdf.Canvas.Parser.Data.TextRenderInfo RenderInfo { get; private set; }
        public PdfFont Font { get; private set; }
        public Vector Position { get; private set; }

        public TextChunk(TextRenderInfo renderInfo)
        {
            this.Text = renderInfo.GetText();
            this.Font = renderInfo.GetFont();
            this.FontSize = renderInfo.GetFontSize();
            this.Position = renderInfo.GetBaseline().GetStartPoint(); // 获取字符的起始坐标
            this.RenderInfo = renderInfo;
        }

        // 获取文本所在区域的矩形
        public iText.Kernel.Geom.Rectangle GetRectangle()
        {
            var ascentLine = RenderInfo.GetAscentLine();
            var descentLine = RenderInfo.GetDescentLine();

            return new iText.Kernel.Geom.Rectangle(
                descentLine.GetStartPoint().Get(0),
                descentLine.GetStartPoint().Get(1),
                ascentLine.GetEndPoint().Get(0) - descentLine.GetStartPoint().Get(0),
                ascentLine.GetEndPoint().Get(1) - descentLine.GetStartPoint().Get(1)
            );
        }
    }
}