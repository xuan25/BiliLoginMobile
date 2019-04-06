using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;

namespace BiliLoginMobile
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

        LoginWindow loginWindow;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loginWindow = new LoginWindow();
            loginWindow.LoggedIn += LoginWindow_LoggedIn;
            loginWindow.ConnectionFailed += LoginWindow_ConnectionFailed;
            loginWindow.Show();
        }

        private void LoginWindow_LoggedIn(CookieCollection cookies)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Cookie c in cookies)
            {
                stringBuilder.Append(c.Name + " : " + c.Value + "\n");
            }
            Dispatcher.Invoke(new Action(() =>
            {
                loginWindow.Close();
                LoginInfoBox.Text = stringBuilder.ToString();
            }));
            
        }

        private void LoginWindow_ConnectionFailed(LoginWindow sender, WebException ex)
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
