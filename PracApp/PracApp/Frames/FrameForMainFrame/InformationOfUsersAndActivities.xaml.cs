using ContextLib;
using ContextLib.Context.Tables;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PracApp.Frames.FrameForMainFrame
{
    public partial class InformationOfUsersAndActivities : Page
    {
        public User us { get; set; }

        private ObservableCollection<string> logMessages = new ObservableCollection<string>();
        private DispatcherTimer timer;
        public SeriesCollection ProcessTimeSeries { get; set; } = new SeriesCollection();
        private List<ProcessInfo> processLogs = new List<ProcessInfo>();
        private ObservableCollection<ProcessTimeData> processTimeCollection = new ObservableCollection<ProcessTimeData>();
        private Dictionary<string, PieSeries> seriesLookup = new Dictionary<string, PieSeries>();
        private string[] targetProcesses = {
            "chrome", "msedge", "firefox", "notepad", "krita", "devenv"
        };

        public InformationOfUsersAndActivities()
        {
            InitializeComponent();
            LogListBox.ItemsSource = logMessages;
            DataContext = this;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            logMessages.Add("Мониторинг остановлен");

            await SendStatsToServerAsync();
        }

        public void EnterUser(User user)
        {
            this.us = user;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            logMessages.Add("Мониторинг запущен...");
        }

        private async System.Threading.Tasks.Task SendStatsToServerAsync()
        {
            if (us == null || us.ID == 0)
            {
                logMessages.Add("❌ Пользователь не установлен");
                return;
            }

            try
            {
                using var client = new TcpClient();
                await client.ConnectAsync("127.0.0.1", 6666).WaitAsync(TimeSpan.FromSeconds(10));

                using var stream = client.GetStream();
                using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                using var reader = new StreamReader(stream, Encoding.UTF8);

                await writer.WriteLineAsync("SEND_STATS");
                await writer.WriteLineAsync(us.ID.ToString());

                var json = JsonSerializer.Serialize(processLogs);
                var jsonLength = Encoding.UTF8.GetByteCount(json);
                await writer.WriteLineAsync(jsonLength.ToString());
                await writer.WriteAsync(json);
                await writer.FlushAsync();

                string? response = await reader.ReadLineAsync();
                if (response != null && response.StartsWith("OK"))
                {
                    logMessages.Add("Данные успешно отправлены");
                    processLogs.Clear();
                }
                else
                {
                    logMessages.Add($"Ответ сервера: {response ?? "нет ответа"}");
                }
            }
            catch (TimeoutException)
            {
                logMessages.Add("Превышено время ожидания");
            }
            catch (Exception ex)
            {
                logMessages.Add($"Ошибка: {ex.Message}");
            }
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            var primaryProcesses = new Dictionary<string, Process>();
            var processes = Process.GetProcesses();

            foreach (var proc in processes)
            {
                try
                {
                    if (ShouldTrackProcess(proc))
                    {
                        string name = proc.ProcessName;
                        if (!primaryProcesses.ContainsKey(name) || proc.StartTime < primaryProcesses[name].StartTime)
                        {
                            primaryProcesses[name] = proc;
                        }
                    }
                }
                catch { }
            }

            var currentRunningIds = new HashSet<string>(primaryProcesses.Values.Select(p => p.Id.ToString()));

            foreach (var proc in primaryProcesses.Values)
            {
                string procId = proc.Id.ToString();
                var existing = processLogs.Find(p => p.ProcessId == procId);
                if (existing == null)
                {
                    processLogs.Add(new ProcessInfo
                    {
                        ProcessId = procId,
                        ProcessName = proc.ProcessName,
                        StartTime = DateTime.Now,
                        ExitMessageShown = false
                    });

                    logMessages.Add($"Запущен {proc.ProcessName} \n(ID: {proc.Id}) в {DateTime.Now}");
                }
            }

            foreach (var p in new List<ProcessInfo>(processLogs))
            {
                bool stillRunning = currentRunningIds.Contains(p.ProcessId);

                if (stillRunning && p.StartTime.HasValue)
                {
                    p.TotalWorkTime += DateTime.Now - p.StartTime.Value;
                    p.StartTime = DateTime.Now;
                }
                else if (!stillRunning && !p.ExitMessageShown)
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

            UpdatePieChartData();
            RefreshChart();
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

        public void RefreshChart()
        {
            foreach (var item in processTimeCollection)
            {
                double formattedValue = Math.Round(item.Hours, 6); 

                if (seriesLookup.TryGetValue(item.Name, out var series))
                {
                    series.Values[0] = formattedValue;
                }
                else
                {
                    var newSeries = new PieSeries
                    {
                        Title = item.Name,
                        Values = new ChartValues<double> { formattedValue },
                        DataLabels = true,
                        LabelPoint = point => $"{point.Y:N4}" 
                    };
                    ProcessTimeSeries.Add(newSeries);
                    seriesLookup[item.Name] = newSeries;
                }
            }
        }

        private bool ShouldTrackProcess(Process proc)
        {
            string name = proc.ProcessName;

            if (targetProcesses.Contains(name, StringComparer.OrdinalIgnoreCase))
                return true;

            try
            {
                if (proc.WorkingSet64 > 200 * 1024 * 1024)
                    return true;
            }
            catch { }

            return false;
        }
    }
}
