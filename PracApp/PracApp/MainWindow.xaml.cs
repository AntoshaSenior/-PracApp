using Hardcodet.Wpf.TaskbarNotification;
using PracApp.Frames;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace PracApp
{
    public partial class MainWindow : Window
    {

        [DllImport("user32.dll")]
        static extern bool SystemParametersInfo(int uiAction, int uiParam, ref RECT pvParam, int fWinIni);
        const int SPI_GETWORKAREA = 0x0030;
        bool isWindowToTaskBar = false;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }


        AuthPage AP;

        public MainWindow()
        {
            InitializeComponent();
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            AP = new AuthPage();
            
            NavFrame.Content = AP;
        }

        private void MaxOrNormAppButt_Click(object sender, RoutedEventArgs e)
        {
            RECT workArea = new RECT();
            SystemParametersInfo(SPI_GETWORKAREA, 0, ref workArea, 0);
            int workAreaHeight, workAreaWidth;

            if (isWindowToTaskBar)
            {
                workAreaHeight = workArea.Bottom / 2;
                workAreaWidth = workArea.Right / 2;

                this.Width = workAreaWidth;
                this.Height = workAreaHeight;

                isWindowToTaskBar = false;
            }
            else
            {
                workAreaHeight = workArea.Bottom - workArea.Top;
                workAreaWidth = workArea.Right - workArea.Left;

                this.Left = workArea.Left;
                this.Top = workArea.Top;
                this.Width = workAreaWidth;
                this.Height = workAreaHeight;
                
                isWindowToTaskBar = true;
            }
        }

        private void MiniButt_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Minimized) this.WindowState = WindowState.Minimized;
        }


        private void TopPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void RestoreWindow_Click(object sender, RoutedEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            TrayIcon.Dispose();
            Application.Current.Shutdown();
        }

        private void CloseAppButt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}