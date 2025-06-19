using Hardcodet.Wpf.TaskbarNotification;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace PracApp
{
    public partial class MainWindow : Window
    {
        
        [DllImport("user32.dll")]
        static extern bool SystemParametersInfo(int uiAction, int uiParam, ref RECT pvParam, int fWinIni);

        const int SPI_GETWORKAREA = 0x0030;

        private Process trackedProcess;
        private DateTime startTime;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
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
                MaximizeToTaskbar();
            }
        }

        private void MiniButt_Click(object sender, RoutedEventArgs e)
        {
            if(this.WindowState != WindowState.Minimized) this.WindowState = WindowState.Minimized;
        }

        private async void StartOrTrackProcess_Click(object sender, RoutedEventArgs e)
        {
            string processName = "notepad";
            var processes = Process.GetProcessesByName(processName);

            if (processes.Length > 0)
            {
                trackedProcess = processes[0];
                startTime = trackedProcess.StartTime;
            }
            else
            {
                trackedProcess = new Process();
                trackedProcess.StartInfo.FileName = "notepad.exe";
                trackedProcess.Start();
                startTime = trackedProcess.StartTime;

            }

            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, args) =>
            {
                if (trackedProcess.HasExited)
                {
                    timer.Stop();

                    var endTime = DateTime.Now;
                    var duration = endTime - startTime;
                    StatusText.Text = $"Процесс завершен. Работал {duration.TotalSeconds:F2} секунд.";
                }
                else
                {
                    var currentDuration = DateTime.Now - startTime;
                    StatusText.Text = $"Процесс работает. Время: {currentDuration.ToString(@"hh\:mm\:ss")}";
                }
            };

            timer.Start();





            await Task.Run(() => trackedProcess.WaitForExit());
        }



    }
}