using Hardcodet.Wpf.TaskbarNotification;
using PracApp.Frames;
using System.ComponentModel;
using System.Diagnostics;
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


        AuthPage AP = new AuthPage();

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public MainWindow()
        {
            InitializeComponent();
            NavFrame.Content = AP;
        }

        

        private void MaximizeToTaskbar()
        {
            RECT workArea = new RECT();
            SystemParametersInfo(SPI_GETWORKAREA, 0, ref workArea, 0);


            int workAreaHeight = workArea.Bottom - workArea.Top;
            int workAreaWidth = workArea.Right - workArea.Left;




            this.Left = workArea.Left;
            this.Top = workArea.Top;
            this.Width = workAreaWidth;
            this.Height = workAreaHeight;
            this.WindowState = WindowState.Normal;
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

        private void MaxOrNormAppButt_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                MaximizeToTaskbar();
            }
        }

        private void MiniButt_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Minimized) this.WindowState = WindowState.Minimized;
        }


        private void TopPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}