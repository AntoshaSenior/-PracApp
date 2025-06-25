using ContextLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Wpf;

namespace PracApp.Frames.FrameForMainFrame
{
    public partial class InformationOfUsersAndActivities : Page
    {
        private ObservableCollection<string> logMessages = new ObservableCollection<string>();
        private DispatcherTimer timer;
        public SeriesCollection ProcessTimeSeries { get; set; } = new SeriesCollection();
        private HashSet<string> runningProcesses = new HashSet<string>();
        private List<ProcessInfo> processLogs = new List<ProcessInfo>();
        private ObservableCollection<ProcessTimeData> processTimeCollection = new ObservableCollection<ProcessTimeData>();
        private Dictionary<string, PieSeries> seriesLookup = new Dictionary<string, PieSeries>();

        private string[] targetProcesses = { 
            "chrome","msedge","firefox",
            "opera","brave","vivaldi",
            "iexplore" , "notepad", "krita","devenv"
        };

        public InformationOfUsersAndActivities()
        {
            InitializeComponent();
            LogListBox.ItemsSource = logMessages;
            DataContext = this;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            
            // timer.Start();
        }

        public void RefreshChart()
        {
            foreach (var item in processTimeCollection)
            {
                if (seriesLookup.TryGetValue(item.Name, out var series))
                {
                    series.Values[0] = item.Hours;
                }
                else
                {
                    var newSeries = new PieSeries
                    {
                        Title = item.Name,
                        Values = new ChartValues<double> { item.Hours },
                        DataLabels = true
                    };
                    ProcessTimeSeries.Add(newSeries);
                    seriesLookup[item.Name] = newSeries;
                }
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            logMessages.Add("Мониторинг запущен...");
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            logMessages.Add("Мониторинг остановлен");
        }
        private void UpdatePieChartData()
        {
            processTimeCollection.Clear();

            
            var groupedData = new Dictionary<string, double>();

            foreach (var p in processLogs)
            {
                
                double hours = p.TotalWorkTime.TotalHours;
                if (groupedData.ContainsKey(p.ProcessName))
                    groupedData[p.ProcessName] += hours;
                else
                    groupedData[p.ProcessName] = hours;
            }

            
            foreach (var kvp in groupedData)
            {
                processTimeCollection.Add(new ProcessTimeData
                {
                    Name = kvp.Key,
                    Hours = kvp.Value
                });
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var processes = Process.GetProcesses();
            
            
            var currentRunningIds = new HashSet<string>();
            foreach (var proc in processes)
            {
                try
                {
                    if (targetProcesses.Contains(proc.ProcessName, StringComparer.OrdinalIgnoreCase))
                    {
                        currentRunningIds.Add(proc.Id.ToString());
                    }
                }
                catch { }
            }

            
            foreach (var proc in processes)
            {
                try
                {
                    if (targetProcesses.Contains(proc.ProcessName, StringComparer.OrdinalIgnoreCase))
                    {
                        string procId = proc.Id.ToString();

                        var existing = processLogs.Find(p => p.ProcessId == procId);
                        if (existing == null)
                        {
                           
                            var newProc = new ProcessInfo
                            {
                                ProcessId = procId,
                                ProcessName = proc.ProcessName,
                                StartTime = DateTime.Now,
                                ExitMessageShown = false
                            };
                            processLogs.Add(newProc);
                            logMessages.Add($"Запущен {proc.ProcessName} \n(ID: {proc.Id}) в {DateTime.Now}");
                        }
                    }
                }
                catch { }
            }

            
            foreach (var p in new List<ProcessInfo>(processLogs))
            {
                bool stillRunning = currentRunningIds.Contains(p.ProcessId);
                
                if (stillRunning)
                {
                    
                    if (p.StartTime.HasValue)
                    {
                        p.TotalWorkTime += DateTime.Now - p.StartTime.Value;
                        p.StartTime = DateTime.Now;
                    }
                }
                else
                {
                    
                    if (!p.ExitMessageShown)
                    {
                        if (p.StartTime.HasValue)
                        {
                            p.EndTime = DateTime.Now;
                            p.TotalWorkTime += p.EndTime.Value - p.StartTime.Value;
                        }
                        logMessages.Add($"Процесс ID: {p.ProcessId} завершился. \nОбщее время работы: {p.TotalWorkTime}");
                        p.ExitMessageShown = true;
                    }
                }
            }
            UpdatePieChartData();
            RefreshChart();

        }
    }
    
}
