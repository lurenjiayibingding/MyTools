using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Text;

namespace ToolStorage.Definition.iTextPDFExtend
{
    /// <summary>
    /// 对文本进行覆盖的监听类
    /// </summary>
    public class TextRenderInfoCoverListener : IEventListener
    {
        /// <summary>
        /// 需要被覆盖的文本
        /// </summary>
        private readonly string searchText;
        private readonly PdfPage pdfPage;
        private readonly List<TextChunk> textInfos = new List<TextChunk>();
        private readonly List<TextChunk> singleTextChunkInfos = new List<TextChunk>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="pdfPage"></param>
        public TextRenderInfoCoverListener(string searchText, PdfPage pdfPage)
        {
            this.searchText = searchText;
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

        public ICollection<EventType> GetSupportedEvents()
        {
            return null;
        }

        /// <summary>
        /// 以TextRenderInfo为单位覆盖文本
        /// </summary>
        public void CoverText()
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
                CoverTextInPdf(index, searchText.Length);
            }
        }

        /// <summary>
        /// 以TextRenderInfo为单位向pdf文件中绘制矩形覆盖框
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        private void CoverTextInPdf(int startIndex, int length)
        {
            PdfCanvas pdfCanvas = new PdfCanvas(pdfPage);
            //设置矩形填充颜色为白色
            pdfCanvas.SetFillColorRgb(1, 1, 1);
            // 删除旧文本的区域
            for (int i = startIndex; i < startIndex + length; i++)
            {
                var chunk = textInfos[i];
                //绘制空白矩形区域以覆盖旧文本
                pdfCanvas.Rectangle(chunk.GetRectangle());
                //在每次绘制矩形之后使用 Fill() 方法来填充矩形的颜色。Fill() 会根据当前的填充颜色来绘制矩形
                pdfCanvas.Fill();
            }
        }

        /// <summary>
        /// 以单个文字为单位覆盖文本
        /// </summary>
        public void CoverSingleText()
        {
            StringBuilder fullText = new StringBuilder();
            foreach (var info in textInfos)
            {
                singleTextChunkInfos.AddRange(info.GetSingleTextChunkInfo());
                fullText.Append(info.Text);
            }

            string strFullText = fullText.ToString();
            var index = strFullText.IndexOf(searchText, StringComparison.InvariantCulture);
            if (index != -1)
            {
                CoverSingleTextInPdf(index, searchText.Length);
            }
        }

        /// <summary>
        /// 以单个文字为单位向pdf文件中绘制矩形覆盖框
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        private void CoverSingleTextInPdf(int startIndex, int length)
        {
            PdfCanvas pdfCanvas = new PdfCanvas(pdfPage);
            //设置矩形填充颜色为白色
            pdfCanvas.SetFillColorRgb(1, 1, 1);
            // 删除旧文本的区域
            for (int i = startIndex; i < startIndex + length; i++)
            {
                var chunk = singleTextChunkInfos[i];
                //绘制空白矩形区域以覆盖旧文本
                pdfCanvas.Rectangle(chunk.GetRectangle());
                //在每次绘制矩形之后使用 Fill() 方法来填充矩形的颜色。Fill() 会根据当前的填充颜色来绘制矩形
                pdfCanvas.Fill();
            }
        }
    }
}