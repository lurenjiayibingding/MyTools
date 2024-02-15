using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTool.BLL
{
    internal class CreateImage
    {
        /// <summary>
        /// 保存图片
        /// </summary>
        public void SaveImage()
        {
            Bitmap map = new Bitmap(1080, 2340);
            Graphics g = Graphics.FromImage(map);

            Brush b = new SolidBrush(Color.FromArgb(0, 0, 0));
            Rectangle rectangle = new Rectangle(0, 0, map.Width, map.Height);
            g.FillRectangle(b, rectangle);

            Font f = new Font("Cascadia Mono", 150, FontStyle.Bold);
            //Brush b2 = System.Drawing.Brushes.White;
            Brush b2 = new SolidBrush(Color.FromArgb(255, 215, 0));
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            RectangleF rect = new RectangleF(0, 350, map.Width, map.Height);
            RectangleF rect2 = new RectangleF(0, 650, map.Width, map.Height);
            RectangleF rect3 = new RectangleF(0, 950, map.Width, map.Height);
            RectangleF rect4 = new RectangleF(0, 1250, map.Width, map.Height);
            RectangleF rect5 = new RectangleF(0, 1550, map.Width, map.Height);

            //g.DrawString("圖", f, b2, rect, sf);
            //g.DrawString("樣", f, b2, rect2, sf);
            //g.DrawString("圖", f, b2, rect3, sf);
            //g.DrawString("森", f, b2, rect4, sf);
            //g.DrawString("破", f, b2, rect5, sf);
            //map.Save(@"D:\图片\图样.jpg", ImageFormat.Jpeg);

            g.DrawString("一夜暴富", f, b2, rect, sf);
            map.Save(@"D:\图片\暴富.jpg", ImageFormat.Jpeg);
        }
    }
}
