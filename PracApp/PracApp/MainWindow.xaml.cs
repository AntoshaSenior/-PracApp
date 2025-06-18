using Hardcodet.Wpf.TaskbarNotification;
using System.ComponentModel;
using System.Windows;

namespace PracApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
                this.WindowState = WindowState.Maximized;
            }
        }

        private void MiniButt_Click(object sender, RoutedEventArgs e)
        {
            if(this.WindowState != WindowState.Minimized) this.WindowState = WindowState.Minimized;
        }
    }
}