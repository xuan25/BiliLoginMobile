using System;
using System.Drawing;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BiliLogin
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MoblieLoginWindow : Window
    {
        public delegate void LoggedInDel(MoblieLoginWindow sender, CookieCollection cookies);
        public event LoggedInDel LoggedIn;

        public delegate void ConnectionFailedDel(MoblieLoginWindow sender, WebException ex);
        public event ConnectionFailedDel ConnectionFailed;

        public MoblieLoginWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshQRCode();
        }

        private void ContentGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ReloadBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshQRCode();
        }

        public void RefreshQRCode()
        {
            BiliLoginQR biliLoginQR = new BiliLoginQR(this);
            biliLoginQR.QRImageLoaded += BiliLoginQR_QRImageLoaded;
            biliLoginQR.LoggedIn += BiliLoginQR_LoggedIn;
            biliLoginQR.ConnectionFailed += BiliLoginQR_ConnectionFailed;
            biliLoginQR.Begin();
        }

        private void BiliLoginQR_QRImageLoaded(Bitmap qrImage)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                QrImageBox.Source = BitmapToImageSource(qrImage);
            }));
        }

        private void BiliLoginQR_LoggedIn(CookieCollection cookies)
        {
            LoggedIn?.Invoke(this, cookies);
        }

        private void BiliLoginQR_ConnectionFailed(BiliLoginQR sender, WebException ex)
        {
            ConnectionFailed?.Invoke(this, ex);
        }

        private BitmapSource BitmapToImageSource(Bitmap bitmap)
        {
            IntPtr ip = bitmap.GetHbitmap();
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return bitmapSource;
        }
    }
}
