using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using System.Text;

namespace ToolStorage.Definition.iTextPDFExtend
{
    /// <summary>
    /// 以TextRenderInfo为单位对文本进行重写的监听类
    /// </summary>
    public class TextRenderInfoOverWritingListener
    {
        /// <summary>
        /// 需要被重写的文本
        /// </summary>
        private readonly string searchText;
        /// <summary>
        /// 重写后的文本
        /// </summary>
        private readonly string substituteText;
        private readonly PdfPage pdfPage;
        private readonly PdfFont font;
        private readonly List<TextChunk> textInfos = new List<TextChunk>();
        private readonly List<SingleTextChunkInfo> singleTextInfos = new List<SingleTextChunkInfo>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="substituteText"></param>
        /// <param name="pdfPage"></param>
        /// <param name="pdfFont"></param>
        public TextRenderInfoOverWritingListener(string searchText, string substituteText, PdfPage pdfPage, PdfFont pdfFont)
        {
            this.searchText = searchText;
            this.substituteText = substituteText;
            this.pdfPage = pdfPage;
            this.font = pdfFont;
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
                    singleTextInfos.AddRange((new TextChunk(info)).GetSingleTextChunkInfo());
                }
            }
        }

        public ICollection<EventType> GetSupportedEvents()
        {
            return null;
        }

        /// <summary>
        /// 替换pdf中的文本
        /// </summary>
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

        /// <summary>
        /// 向pdf文件中绘制矩形遮盖和新文本
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
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
                //pdfCanvas.Stroke();
                //设置矩形填充颜色为白色
                pdfCanvas.SetFillColorRgb(1, 1, 1);
                //在每次绘制矩形之后使用 Fill() 方法来填充矩形的颜色。Fill() 会根据当前的填充颜色来绘制矩形
                pdfCanvas.Fill();

                //需要绘制的新文本
                var newText = substituteText.Length > i - startIndex ? substituteText[i - startIndex].ToString() : string.Empty;
                var currentFont = chunk.Font;
                //重绘的文字设置为黑色
                pdfCanvas.SetFillColorRgb(0, 0, 0);
                pdfCanvas.BeginText();
                pdfCanvas.SetFontAndSize(currentFont, chunk.DrawFontSize);
                pdfCanvas.MoveText(chunk.Position.Get(0), chunk.Position.Get(1));
                pdfCanvas.NewlineShowText(chunk.Position.Get(0), chunk.Position.Get(1), newText); // 绘制新文本
                pdfCanvas.EndText();
            }
        }
    }
}