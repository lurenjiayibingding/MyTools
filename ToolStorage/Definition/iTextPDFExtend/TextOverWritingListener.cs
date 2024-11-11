using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Text;

namespace ToolStorage.Definition.iTextPDFExtend
{
    /// <summary>
    /// 以TextRenderInfo为单位对文本进行重写的监听类
    /// </summary>
    public class TextOverWritingListener : IEventListener
    {
        /// <summary>
        /// 需要被重写的文本
        /// </summary>
        private readonly string searchText;
        /// <summary>
        /// 重写后的文本
        /// </summary>
        private readonly string substituteText;
        /// <summary>
        /// pdf页面对象
        /// </summary>
        private readonly PdfPage pdfPage;
        /// <summary>
        /// pdf中的文本块信息集合
        /// </summary>
        private readonly List<TextChunk> textInfos = new List<TextChunk>();
        ///// <summary>
        ///// pdf中的单个字符信息集合
        ///// </summary>
        //private readonly List<SingleTextChunkInfo> singleTextInfos = new List<SingleTextChunkInfo>();
        /// <summary>
        /// pdf中的文本块信息集合
        /// </summary>
        private readonly List<TextChunk> singleTextInfos = new List<TextChunk>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="substituteText"></param>
        /// <param name="pdfPage"></param>
        /// <param name="useInlineFont"></param>
        /// <param name="font"></param>
        public TextOverWritingListener(string searchText, string substituteText, PdfPage pdfPage)
        {
            this.searchText = searchText;
            this.substituteText = substituteText;
            this.pdfPage = pdfPage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICollection<EventType> GetSupportedEvents()
        {
            return null;
        }

        /// <summary>
        /// 以TextRenderInfo为单位替换pdf中的文本
        /// </summary>
        public void ReplaceText(bool useInlineFont, PdfFont font)
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
                ReplaceTextInPdf(index, searchText.Length, useInlineFont, font);
            }
        }

        /// <summary>
        /// 以TextRenderInfo为单位向pdf文件中绘制矩形遮盖和新文本
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        private void ReplaceTextInPdf(int startIndex, int length, bool useInlineFont, PdfFont font)
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
                //重绘的文字设置为黑色
                pdfCanvas.SetFillColorRgb(0, 0, 0);
                pdfCanvas.BeginText();
                if (useInlineFont)
                {
                    pdfCanvas.SetFontAndSize(chunk.Font, chunk.DrawFontSize);
                }
                else
                {
                    pdfCanvas.SetFontAndSize(font, chunk.DrawFontSize);
                }
                pdfCanvas.MoveText(chunk.Position.Get(0), chunk.Position.Get(1));
                pdfCanvas.NewlineShowText(chunk.Position.Get(0), chunk.Position.Get(1), newText); // 绘制新文本
                pdfCanvas.EndText();
            }
        }

        /// <summary>
        /// 以单个文字为单位替换pdf中的文本
        /// </summary>
        /// <param name="useInlineFont">是否使用pdf中的原字体</param>
        /// <param name="font">若不使用原字体需要指定新的字体，若使用原字体可为null</param>
        public void ReplaceSingleText(bool useInlineFont, PdfFont font)
        {
            foreach (TextChunk chunk in textInfos)
            {
                singleTextInfos.AddRange(chunk.GetSingleTextChunkInfo());
            }

            StringBuilder fullText = new StringBuilder();
            foreach (var info in singleTextInfos)
            {
                fullText.Append(info.Text);
            }

            string strFullText = fullText.ToString();
            var index = strFullText.IndexOf(searchText, StringComparison.InvariantCulture);
            if (index != -1)
            {
                ReplaceSingleTextInPdf(index, searchText.Length, useInlineFont, font);
            }
        }

        /// <summary>
        /// 以单个文字为单位向pdf中绘制遮盖层和新文本
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="useInlineFont"></param>
        /// <param name="font"></param>
        //todo：这里进行两次循环是因为逐字匹配时对每个文字的坐标获取不太准确，只进行一次循环的话后一个子的矩形遮盖会遮挡前一个字的部分笔画
        private void ReplaceSingleTextInPdf(int startIndex, int length, bool useInlineFont, PdfFont font)
        {
            // 我们假设替换后的文本长度与原文本长度一致。如果长度不一致，需要调整此处的排版逻辑。
            PdfCanvas pdfCanvas = new PdfCanvas(pdfPage);
            // 删除旧文本的区域

            for (int i = startIndex; i < startIndex + length; i++)
            {
                var chunk = singleTextInfos[i];

                //设置矩形填充颜色为白色6
                pdfCanvas.SetFillColorRgb(1, 1, 1);
                //绘制空白矩形区域以覆盖旧文本
                pdfCanvas.Rectangle(chunk.GetRectangle());
                //在每次绘制矩形之后使用 Fill() 方法来填充矩形的颜色。Fill() 会根据当前的填充颜色来绘制矩形
                pdfCanvas.Fill();

                //重绘的文字设置为黑色
                pdfCanvas.SetFillColorRgb(0, 0, 0);
                //需要绘制的新文本
                var newText = substituteText.Length > i - startIndex ? substituteText[i - startIndex].ToString() : string.Empty;

                pdfCanvas.BeginText();
                if (useInlineFont)
                {
                    pdfCanvas.SetFontAndSize(chunk.Font, chunk.DrawFontSize);
                }
                else
                {
                    pdfCanvas.SetFontAndSize(font, chunk.DrawFontSize);
                }
                pdfCanvas.MoveText(chunk.Position.Get(0), chunk.Position.Get(1));
                //绘制文字时可以设置单词和字符间距
                pdfCanvas.NewlineShowText(0, 0, newText); // 绘制新文本
                //以默认方式绘制文字
                //pdfCanvas.ShowText(newText); // 绘制新文本
                pdfCanvas.EndText();
            }
        }
    }
}