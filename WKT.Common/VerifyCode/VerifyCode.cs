using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.UI;
using System.Drawing.Drawing2D;
using System.IO;

namespace WKT.Common
{
    /// <summary>
    /// ������֤�����
    /// by sxd
    /// </summary>
    public class ValidateCode
    {
        public string CreateRandomCode(int length)
        {
            int rand;
            char code;
            string randomcode = String.Empty;

            //����һ�����ȵ���֤��
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
            int randAngle = 45; //���ת���Ƕ�
            int mapwidth = (int)(randomcode.Length * 19);
            Bitmap map = new Bitmap(mapwidth, 25);//����ͼƬ����
            Graphics graph = Graphics.FromImage(map);
            graph.Clear(Color.AliceBlue);//������棬��䱳��
            if(isBorder==true)
                graph.DrawRectangle(new Pen(Color.Black, 0), 0, 0, map.Width - 1, map.Height - 1);//��һ���߿�
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//ģʽ
            Random rand = new Random();

            //�����������
            Pen blackPen = new Pen(Color.LightGray, 0);
            for( int i = 0; i < 50; i++ )
            {
                int x = rand.Next(0, map.Width);
                int y = rand.Next(0, map.Height);
                graph.DrawRectangle(blackPen, x, y, 1, 1);
            }


            //��֤����ת����ֹ����ʶ��
            char[] chars = randomcode.ToCharArray();//��ɢ�ַ����ɵ��ַ�����

            //���־���
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            //������ɫ
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //��������
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "����" };

            for( int i = 0; i < chars.Length; i++ )
            {
                int cindex = rand.Next(7);
                int findex = rand.Next(5);

                Font f = new System.Drawing.Font(font[findex], 14, System.Drawing.FontStyle.Bold);//������ʽ(����2Ϊ�����С)
                Brush b = new System.Drawing.SolidBrush(c[cindex]);

                Point dot = new Point(14, 14);//����λ��
                //graph.DrawString(dot.X.ToString(),fontstyle,new SolidBrush(Color.Black),10,150);//����X������ʾ����
                float angle = rand.Next(-randAngle, randAngle);//ת���Ķ���

                graph.TranslateTransform(dot.X, dot.Y);//�ƶ���굽ָ��λ��
                graph.RotateTransform(angle);
                graph.DrawString(chars[i].ToString(), f, b, 1, 1, format);
                //graph.DrawString(chars[i].ToString(),fontstyle,new SolidBrush(Color.Blue),1,1,format);
                graph.RotateTransform(-angle);//ת��ȥ
                graph.TranslateTransform(2, -dot.Y);//�ƶ���굽ָ��λ��
            }
            //graph.DrawString(randomcode,fontstyle,new SolidBrush(Color.Blue),2,2); //��׼�����

            //����ͼƬ
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
