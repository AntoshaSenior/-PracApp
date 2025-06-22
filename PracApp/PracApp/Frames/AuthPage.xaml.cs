using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PracApp.Frames
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {

        public AuthPage()
        {
            InitializeComponent();
        }

        private void AuthBut_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Text;

            SendAuthInfo(username,password);
            
        }

        public static async Task SendAuthInfo(string username,string password)
        {
            using var socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            try
            {
                string s = string.Concat(username,"\n",password);

                await socket.ConnectAsync("127.0.0.1", 6666);

                byte[] buffer = new byte[1024];

                buffer = Encoding.Default.GetBytes(s);

                socket.Send(buffer);

                socket.Close();


                

            }
            catch
            {
                
            }
        }
    }
}
