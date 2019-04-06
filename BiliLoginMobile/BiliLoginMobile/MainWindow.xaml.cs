using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            loginWindow.Show();
        }

        private void LoginWindow_LoggedIn(System.Net.CookieCollection cookies)
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
    }
}
