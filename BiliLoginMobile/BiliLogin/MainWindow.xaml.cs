using System;
using System.Drawing;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BiliLogin
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MoblieLoginWindow loginWindow = new MoblieLoginWindow();
            loginWindow.LoggedIn += LoginWindow_LoggedIn;
            loginWindow.ConnectionFailed += LoginWindow_ConnectionFailed;
            loginWindow.Show();
        }

        private void LoginWindow_LoggedIn(MoblieLoginWindow sender, CookieCollection cookies, uint uid)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                sender.Topmost = false;
                sender.Hide();
            }));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (Cookie c in cookies)
            {
                stringBuilder.Append(c.Name + " : " + c.Value + "\n");
            }
            Dispatcher.Invoke(new Action(() =>
            {
                LoginInfoBox.Text = stringBuilder.ToString();
            }));

            UserInfo userInfo = UserInfo.GetUserInfo(cookies);

            Dispatcher.Invoke(new Action(() =>
            {
                UserInfoBox.Text = string.Format("用户名: {0}\n用户状态: {1}\n用户等级: {2}\n大会员状态: {3}", userInfo.Uname, userInfo.UserStatus, userInfo.CurrentLevel, userInfo.VipStatus);
            }));
            
            Dispatcher.Invoke(new Action(() =>
            {
                UserFaceImage.Source = BitmapToImageSource(userInfo.GetFaceBitmap());
                if (userInfo.NameplateCurrent != "")
                    NameplateImage.Source = BitmapToImageSource(userInfo.GetNamePlateBitmap());
                if (userInfo.PendantCurrent != "")
                    PendantImage.Source = BitmapToImageSource(userInfo.GetPendantBitmap());
            }));

            Dispatcher.Invoke(new Action(() =>
            {
                sender.Close();
            }));


        }

        private BitmapSource BitmapToImageSource(Bitmap bitmap)
        {
            bitmap.Save("1.jpg");
            IntPtr ip = bitmap.GetHbitmap();
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return bitmapSource;
        }

        private void LoginWindow_ConnectionFailed(MoblieLoginWindow sender, WebException ex)
        {
            new Thread(delegate ()
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    sender.Topmost = false;
                }));
                MessageBox.Show("网络错误", "登录", MessageBoxButton.OK);
                Dispatcher.Invoke(new Action(() =>
                {
                    sender.Close();
                }));
            }).Start();
        }
    }
}
