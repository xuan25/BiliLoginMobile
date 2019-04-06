﻿using System;
using System.Drawing;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BiliLoginMobile
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public delegate void LoggedInDel(CookieCollection cookies);
        public event LoggedInDel LoggedIn;

        public delegate void ConnectionFailedDel(LoginWindow sender, WebException ex);
        public event ConnectionFailedDel ConnectionFailed;

        public LoginWindow()
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
            QrImageBox.Source = BitmapToImageSource(qrImage);
        }

        private void BiliLoginQR_LoggedIn(CookieCollection cookies)
        {
            LoggedIn?.Invoke(cookies);
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