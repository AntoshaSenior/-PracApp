using ContextLib.Context.Tables;
using System;
using System.Collections.Generic;
using System.IO;
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

                    mp.EnterUser(user);


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

        public bool SendAuthInfo(string username, string password)
        {
            try
            {
                using TcpClient client = new TcpClient("127.0.0.1", 6666);
                using NetworkStream stream = client.GetStream();
                using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                // Отправка команды авторизации
                writer.WriteLine("AUTH");
                writer.WriteLine(username);
                writer.WriteLine(password);

                // Получение статуса
                string status = reader.ReadLine();
                if (status == "OK")
                {
                    string json = reader.ReadLine();
                    user = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка соединения: " + ex.Message);
                return false;
            }
        }
    }
}
