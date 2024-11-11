using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas.Parser.Data;

namespace ToolStorage.Definition.iTextPDFExtend
{
    /// <summary>
    /// 自定义的文本块信息
    /// </summary>
    public class TextChunk
    {
        /// <summary>
        /// 文本块信息
        /// </summary>
        public TextRenderInfo RenderInfo { get; private set; }
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// 文本字体
        /// </summary>
        public PdfFont Font { get; private set; }
        /// <summary>
        /// 字体名称
        /// </summary>
        public string FontName { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        public float FontSize { get; private set; }
        /// <summary>
        /// 文本位置的起始坐标信息
        /// </summary>
        public Vector Position { get; private set; }
        /// <summary>
        /// 重绘文本块时实际的字体大小
        /// </summary>
        public float DrawFontSize
        {
            get
            {
                //从TextRenderInfo返回的字体大小是在绘制文本时来自当前图形状态的字体大小值。此值尚未包括当前文本和转换矩阵对绘制的文本的转换。
                //因此，只要通过这些矩阵的字体大小值，就必须对垂直向量进行变换，并根据结果确定有效大小。
                //TextRenderInfo.GetTextMatrix()值实际上包含文本矩阵和当前转换矩阵的乘积，因此我们只需要使用该值。
                Vector sizeHighVector = new Vector(0, FontSize, 0);
                Matrix matrix = RenderInfo.GetTextMatrix();
                return sizeHighVector.Cross(matrix).Length();
            }
            set
            {
                DrawFontSize = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="renderInfo"></param>
        public TextChunk(TextRenderInfo renderInfo)
        {
            this.Text = renderInfo.GetText();
            this.Font = renderInfo.GetFont();
            this.FontSize = renderInfo.GetFontSize();
            this.Position = renderInfo.GetBaseline().GetStartPoint(); // 获取字符的起始坐标
            this.RenderInfo = renderInfo;
            FontName = Font?.GetFontProgram()?.GetFontNames()?.GetFontName();
        }

        // 获取文本所在区域的矩形
        public Rectangle GetRectangle()
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SingleTextChunkInfo> GetSingleTextChunkInfo()
        {
            var result = new List<SingleTextChunkInfo>();
            var length = Text.Length;
            if (length == 1)
            {
                result.Add(new SingleTextChunkInfo
                {
                    Text = Text,
                    Rectangle = GetRectangle(),
                    Font = Font,
                    FontName = FontName,
                    FontSize = FontSize,
                    DrawFontSize = DrawFontSize,
                    Position = Position,
                });
            }
            else
            {
                var singleInfos = RenderInfo.GetCharacterRenderInfos();
                foreach (var singleInfo in singleInfos)
                {
                    if (singleInfo != null)
                    {
                        //var startPoint = singleInfo.GetBaseline().GetStartPoint();
                        //var endPoint = singleInfo.GetBaseline().GetEndPoint();
                        var ascentLine = singleInfo.GetAscentLine();
                        var descentLine = singleInfo.GetDescentLine();

                        result.Add(new SingleTextChunkInfo
                        {
                            Text = singleInfo.GetText(),
                            Rectangle = new Rectangle(
                                descentLine.GetStartPoint().Get(0),
                                descentLine.GetStartPoint().Get(1),
                                descentLine.GetEndPoint().Get(0) - descentLine.GetStartPoint().Get(0),
                                ascentLine.GetStartPoint().Get(1) - descentLine.GetStartPoint().Get(1)),

                            Font = Font,
                            FontName = FontName,
                            FontSize = FontSize,
                            DrawFontSize = DrawFontSize,
                            Position = singleInfo.GetBaseline().GetStartPoint(),
                        });
                    }
                }
            }
            return result;
        }
    }

    public class SingleTextChunkInfo
    {
        /// <summary>
        /// 单个文本的内容
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 单个文本的坐标矩形信息
        /// </summary>
        public Rectangle Rectangle { get; set; }
        /// <summary>
        /// 文本字体
        /// </summary>
        public PdfFont Font { get; set; }
        /// <summary>
        /// 字体名称
        /// </summary>
        public string FontName { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        public float FontSize { get; set; }
        /// <summary>
        /// 重绘文本块时实际的字体大小
        /// </summary>
        public float DrawFontSize { get; set; }
        /// <summary>
        /// 文本位置的起始坐标信息
        /// </summary>
        public Vector Position { get; set; }
    }
}