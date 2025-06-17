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


    }
}