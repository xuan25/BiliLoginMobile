using Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BiliLoginMobile
{
    class BiliLoginQR
    {
        public delegate void LoginUrlRecievedDel(string url);
        public event LoginUrlRecievedDel LoginUrlRecieved;

        public delegate void QRImageLoadedDel(Bitmap qrImage);
        public event QRImageLoadedDel QRImageLoaded;

        public delegate void LoggedInDel(CookieCollection cookies);
        public event LoggedInDel LoggedIn;

        public BiliLoginQR(Window parent)
        {
            parent.Closing += Parent_Closing;
        }

        private void Parent_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Stop();
        }

        public void Begin()
        {
            WebRequest request = WebRequest.Create("https://passport.bilibili.com/qrcode/getLoginUrl");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string result = reader.ReadToEnd();
            Console.WriteLine(result);
            reader.Close();
            response.Close();
            dataStream.Close();

            dynamic getLoginUrl = JsonParser.Parse(result);
            LoginUrlRecieved?.Invoke(getLoginUrl.data.url);
            Bitmap qrBitmap = RenderQrCode(getLoginUrl.data.url);
            QRImageLoaded?.Invoke(qrBitmap);
            StartLoginListener(getLoginUrl.data.oauthKey);
        }

        public void Stop()
        {
            if (loginListenerThread != null)
                loginListenerThread.Abort();
        }

        Thread loginListenerThread;
        private void StartLoginListener(string oauthKey)
        {
            Stop();
            loginListenerThread = new Thread(LoginListener);
            loginListenerThread.Start(oauthKey);
        }

        private void LoginListener(object oauthKey)
        {
            while (true)
            {
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://passport.bilibili.com/qrcode/getLoginInfo");
                byte[] data = Encoding.UTF8.GetBytes("oauthKey=" + oauthKey);
                request.Method = "POST";
                request.ContentLength = data.Length;
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.CookieContainer = new CookieContainer();
                Stream postStream = request.GetRequestStream();
                postStream.Write(data, 0, data.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string result = reader.ReadToEnd();
                CookieCollection cookieCollection = response.Cookies;
                reader.Close();
                response.Close();
                dataStream.Close();
                postStream.Close();
                Console.WriteLine(result);
                
                dynamic loginInfo = JsonParser.Parse(result);
                if (loginInfo.status)
                {
                    LoggedIn?.Invoke(cookieCollection);
                    break;
                }

                Thread.Sleep(1000);
            }
        }

        private Bitmap RenderQrCode(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, false);
            return qrCodeImage;
        }
    }
}
