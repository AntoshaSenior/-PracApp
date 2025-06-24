using ContextLib.Context.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
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
        User? user = new User();
        Role? role = new Role();
        Team? team = new Team();
        Status? status = new Status();
        Project? project = new Project();
        UserToProject? userToProject = new UserToProject();
        ContextLib.Context.Tables.Task? task = new ContextLib.Context.Tables.Task();
        
        public AuthPage()
        {
            InitializeComponent();
        }

        private void AuthBut_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Text;
            if (username != "" && password != "")
                if (SendAuthInfo(username, password))
                {
                    MainPage mp = new MainPage();
                    mp.User = this.user;
                    mp.Role = this.role;
                    mp.Team = this.team;
                    mp.UserToProject = this.userToProject;
                    mp.Project = this.project;
                    mp.Status = this.status;


                    this.NavigationService.Navigate(mp);
                }
                    
                else
                {
                    MessageBox.Show("неверные имя пользователя или пароль");
                }
            else
            {
                MessageBox.Show("заполните поля");
            }

            //this.NavigationService.Navigate(new MainPage(user));
        }

        public bool SendAuthInfo(string username,string password)
        {
            using var socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            try
            {
                string s = string.Concat(username,"\n",password);
                byte[] buffer = new byte[1024];

                socket.ConnectAsync("127.0.0.1", 6666);
                buffer = Encoding.UTF8.GetBytes(s);
                socket.Send(buffer);

                buffer = new byte[1024];
                int recByte = socket.Receive(buffer);
                string jsonUser = Encoding.UTF8.GetString(buffer).Trim('\0');

                //buffer = new byte[1024];
                //recByte = socket.Receive(buffer);
                //string jsonRole = Encoding.UTF8.GetString(buffer).Trim('\0');

                //buffer = new byte[1024];
                //recByte = socket.Receive(buffer);
                //string jsonTeam = Encoding.UTF8.GetString(buffer).Trim('\0');

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };


                user = JsonSerializer.Deserialize<User>(jsonUser,options);
                //role = JsonSerializer.Deserialize<Role>(jsonRole,options);
                //team = JsonSerializer.Deserialize<Team>(jsonTeam,options);


                socket.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
