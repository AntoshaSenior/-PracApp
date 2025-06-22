using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private Process? trackedProcess;
        private DateTime startTime;
        private List<Process>? LProcess;


        public MainPage()
        {
            InitializeComponent();
            AddAllTrackedProcesses();
        }


        public void AddAllTrackedProcesses()
        {
            var topProcesses = Process.GetProcesses()
                .OrderByDescending(p => p.WorkingSet64)
                .Take(50)
                .Select(p => new
                {
                    Name = p.ProcessName,
                    MemoryMB = p.WorkingSet64 / 1024 / 1024
                })
                .ToList();

            
            dt.ItemsSource = topProcesses;
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
