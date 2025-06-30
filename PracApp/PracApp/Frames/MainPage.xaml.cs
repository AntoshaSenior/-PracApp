using ContextLib.Context.Tables;
using Microsoft.VisualBasic;
using PracApp.Frames.FrameForMainFrame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PracApp.Frames
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        
        InformationOfUsersAndActivities? IOU;
        
        
        public User User { get; set; }
        public Role Role {  get; set; }
        public Team Team { get; set; }
        public UserToProject UserToProject { get; set; }
        public Project Project { get; set; }
        public Status Status { get; set; }



        public MainPage()
        {
            InitializeComponent();


            
            IOU = new InformationOfUsersAndActivities();
            IOU.us = User;
            NavFrame.Navigate(IOU);
                
            
                

            //using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //try
            //{
                
            //    byte[] buffer = new byte[1024];

            //    socket.ConnectAsync("127.0.0.1", 6666);
            //    buffer = Encoding.UTF8.GetBytes("d");
            //    socket.Send(buffer);

            //    buffer = new byte[1024];
            //    int recByte = socket.Receive(buffer);
            //    string json = Encoding.UTF8.GetString(buffer).Trim('\0');

            //    var options = new JsonSerializerOptions
            //    {
            //        PropertyNameCaseInsensitive = true
            //    };


            //    user = JsonSerializer.Deserialize<User>(json, options);

            //    socket.Close();
                
            //}
            //catch
            //{
                
            //}
        }
        public void EnterUser(User user)
        {
            User = user;
            IOU.us = user;
        }



 
    }
}
