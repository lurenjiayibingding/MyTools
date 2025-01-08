//using Emgu.CV;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ToolStorage.Definition
//{
//    internal class VideoMerge
//    {
//        public static void Merge(string inputPath1, string inputPath2, string outputPath)
//        {
//            using (var capture1 = new VideoCapture(inputPath1))
//            using (var capture2 = new VideoCapture(inputPath2))
//            {
//                if (!capture1.IsOpened || !capture2.IsOpened)
//                {
//                    Console.WriteLine("无法打开视频文件");
//                    return;
//                }

//                // 获取视频的帧率、宽度和高度
//                int frameRate = Math.Max(capture1.Fps, capture2.Fps);
//                int width = Math.Max(capture1.FrameWidth, capture2.FrameWidth);
//                int height = Math.Max(capture1.FrameHeight, capture2.FrameHeight);

//                // 创建一个与输出视频大小匹配的图像矩阵
//                Mat combinedFrame = new Mat(height * 2, width, DepthType.Cv8U, 3);

//                // 创建视频写入对象
//                using (var writer = new VideoWriter(outputPath, FourCC.XVID, frameRate, new Size(width, height * 2), true))
//                {
//                    while (true)
//                    {
//                        Mat frame1, frame2;
//                        bool isSuccess1 = capture1.Read(frame1);
//                        bool isSuccess2 = capture2.Read(frame2);

//                        if (!isSuccess1 || !isSuccess2)
//                            break; // 如果读取到视频末尾，则跳出循环

//                        // 将两个帧拼接在一起
//                        CvInvoke.CopyMakeBorder(frame1, frame1, 0, height, 0, 0, BorderType.Replicate); // 填充空白区域以适应新高度
//                        combinedFrame_roi(frame1, new Rect(0, 0, width, height));
//                        combinedFrame_roi(frame2, new Rect(0, height, width, height));

//                        // 写入新的组合帧
//                        writer.Write(combinedFrame);

//                        // 可选：显示或保存当前帧
//                        // CvInvoke.Imshow("Combined Video", combinedFrame);
//                        // CvInvoke.WaitKey(1);
//                    }
//                }
//            }
//        }

//        private static void combinedFrame_roi(Mat src, Rect roi)
//        {
//            src.CopyTo(src, roi);
//        }
//    }
//}
