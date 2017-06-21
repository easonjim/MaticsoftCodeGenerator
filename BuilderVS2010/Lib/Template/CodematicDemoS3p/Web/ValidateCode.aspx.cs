using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
namespace <$$namespace$$>.Web
{
    public partial class ValidateCode : System.Web.UI.Page
    {
        private void Page_Load(object sender, System.EventArgs e)
        {
            string checkCode = GetRandomCode(4);
            Session["CheckCode"] = checkCode;
            SetPageNoCache();
            CreateImage(checkCode);
        }

        /// <summary>
        /// ����ҳ�治������
        /// </summary>
        private void SetPageNoCache()
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Response.AppendHeader("Pragma", "No-Cache");
        }

        private string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,M,N,P,Q,R,S,T,U,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(35);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);//��������
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }
        private string GetRandomCode(int CodeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,M,N,P,Q,R,S,T,U,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(33);

                while (temp == t)
                {
                    t = rand.Next(33);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }

            return RandomCode;
        }
        private void CreateImage(string checkCode)
        {
            int iwidth = (int)(checkCode.Length * 14);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 20);
            Graphics g = Graphics.FromImage(image);
            Font f = new System.Drawing.Font("Arial ", 10);//, System.Drawing.FontStyle.Bold);
            Brush b = new System.Drawing.SolidBrush(Color.Black);
            Brush r = new System.Drawing.SolidBrush(Color.FromArgb(166, 8, 8));

            //g.FillRectangle(new System.Drawing.SolidBrush(Color.Blue),0,0,image.Width, image.Height);
            //			g.Clear(Color.AliceBlue);//����ɫ
            g.Clear(System.Drawing.ColorTranslator.FromHtml("#99C1CB"));//����ɫ

            char[] ch = checkCode.ToCharArray();
            for (int i = 0; i < ch.Length; i++)
            {
                if (ch[i] >= '0' && ch[i] <= '9')
                {
                    //�����ú�ɫ��ʾ
                    g.DrawString(ch[i].ToString(), f, r, 3 + (i * 12), 3);
                }
                else
                {   //��ĸ�ú�ɫ��ʾ
                    g.DrawString(ch[i].ToString(), f, b, 3 + (i * 12), 3);
                }
            }

            //forѭ����������һЩ�����ˮƽ��
            //			Pen blackPen = new Pen(Color.Black, 0);
            //			Random rand = new Random();
            //			for (int i=0;i<5;i++)
            //			{
            //				int y = rand.Next(image.Height);
            //				g.DrawLine(blackPen,0,y,image.Width,y);
            //			}

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //history back ���ظ� 
            Response.Cache.SetNoStore();//��һ�� 		
            Response.ClearContent();
            Response.ContentType = "image/Jpeg";
            Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            image.Dispose();
        }        
    }
}
