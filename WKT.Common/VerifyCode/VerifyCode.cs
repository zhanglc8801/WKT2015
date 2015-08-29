using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.UI;
using System.Drawing.Drawing2D;
using System.IO;

namespace WKT.Common
{
    /// <summary>
    /// 生成验证码的类
    /// by sxd
    /// </summary>
    public class ValidateCode
    {
        public string CreateRandomCode(int length)
        {
            int rand;
            char code;
            string randomcode = String.Empty;

            //生成一定长度的验证码
            System.Random random = new Random();
            for( int i = 0; i < length; i++ )
            {
                rand = random.Next();

                if( rand % 3 == 0 )
                {
                    code = (char)('A' + (char)(rand % 26));
                }
                else
                {
                    code = (char)('0' + (char)(rand % 10));
                }

                randomcode += code.ToString();
            }
            return randomcode;
        }

        public Bitmap CreateImage(string randomcode, bool isBorder)
        {
            int randAngle = 45; //随机转动角度
            int mapwidth = (int)(randomcode.Length * 19);
            Bitmap map = new Bitmap(mapwidth, 25);//创建图片背景
            Graphics graph = Graphics.FromImage(map);
            graph.Clear(Color.AliceBlue);//清除画面，填充背景
            if(isBorder==true)
                graph.DrawRectangle(new Pen(Color.Black, 0), 0, 0, map.Width - 1, map.Height - 1);//画一个边框
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//模式
            Random rand = new Random();

            //背景噪点生成
            Pen blackPen = new Pen(Color.LightGray, 0);
            for( int i = 0; i < 50; i++ )
            {
                int x = rand.Next(0, map.Width);
                int y = rand.Next(0, map.Height);
                graph.DrawRectangle(blackPen, x, y, 1, 1);
            }


            //验证码旋转，防止机器识别
            char[] chars = randomcode.ToCharArray();//拆散字符串成单字符数组

            //文字距中
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            //定义颜色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //定义字体
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };

            for( int i = 0; i < chars.Length; i++ )
            {
                int cindex = rand.Next(7);
                int findex = rand.Next(5);

                Font f = new System.Drawing.Font(font[findex], 14, System.Drawing.FontStyle.Bold);//字体样式(参数2为字体大小)
                Brush b = new System.Drawing.SolidBrush(c[cindex]);

                Point dot = new Point(14, 14);//字体位置
                //graph.DrawString(dot.X.ToString(),fontstyle,new SolidBrush(Color.Black),10,150);//测试X坐标显示间距的
                float angle = rand.Next(-randAngle, randAngle);//转动的度数

                graph.TranslateTransform(dot.X, dot.Y);//移动光标到指定位置
                graph.RotateTransform(angle);
                graph.DrawString(chars[i].ToString(), f, b, 1, 1, format);
                //graph.DrawString(chars[i].ToString(),fontstyle,new SolidBrush(Color.Blue),1,1,format);
                graph.RotateTransform(-angle);//转回去
                graph.TranslateTransform(2, -dot.Y);//移动光标到指定位置
            }
            //graph.DrawString(randomcode,fontstyle,new SolidBrush(Color.Blue),2,2); //标准随机码

            //生成图片
            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //map.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            //context.Response.ClearContent();
            //context.Response.ContentType = "image/gif";
            //context.Response.BinaryWrite(ms.ToArray());
            //graph.Dispose();
            //map.Dispose();
            return map;
        }

        public byte[] CreateImageToByte(string code, bool isBorder)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            Bitmap image = CreateImage(code, isBorder);
            try
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] byteCode = ms.GetBuffer();
                return byteCode;
            }
            finally
            {
                ms.Close();
                ms = null;
                image.Dispose();
                image = null;
            }
        }

    }
}
