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

        private string[] targetProcesses = { "chrome", "notepad", "krita" };

        public InformationOfUsersAndActivities()
        {
            InitializeComponent();
            LogListBox.ItemsSource = logMessages;
            DataContext = this;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            // Можно стартовать таймер сразу или по кнопке
            // timer.Start();
        }

        public void RefreshChart()
        {
            ProcessTimeSeries.Clear();

            foreach (var item in processTimeCollection)
            {
                ProcessTimeSeries.Add(new PieSeries
                {
                    Title = item.Name,
                    Values = new ChartValues<double> { item.Hours },
                    DataLabels = true
                });
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

            // группируем по имени процесса
            var groupedData = new Dictionary<string, double>();

            foreach (var p in processLogs)
            {
                // добавляем время в минуты или часы
                double hours = p.TotalWorkTime.TotalHours;
                if (groupedData.ContainsKey(p.ProcessName))
                    groupedData[p.ProcessName] += hours;
                else
                    groupedData[p.ProcessName] = hours;
            }

            // создаем элементы коллекции для диаграммы
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
            
            // Создаем HashSet идентификаторов текущих запущенных целей процессов
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
                catch { } // Обработка ошибок доступа к свойствам процессов
            }

            // Обработка процессов, которые еще не зарегистрированы или запущены
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
                            // Новый запущенный процесс
                            var newProc = new ProcessInfo
                            {
                                ProcessId = procId,
                                ProcessName = proc.ProcessName,
                                StartTime = DateTime.Now, // или proc.StartTime, если хотите
                                ExitMessageShown = false
                            };
                            processLogs.Add(newProc);
                            logMessages.Add($"Запущен {proc.ProcessName} \n(ID: {proc.Id}) в {DateTime.Now}");
                        }
                    }
                }
                catch { }
            }

            // Обработка завершения процессов
            foreach (var p in new List<ProcessInfo>(processLogs))
            {
                bool stillRunning = currentRunningIds.Contains(p.ProcessId);
                // Обновление времени работы
                if (stillRunning)
                {
                    // Обновляем время работы (прибавляем интервал)
                    if (p.StartTime.HasValue)
                    {
                        p.TotalWorkTime += DateTime.Now - p.StartTime.Value;
                        p.StartTime = DateTime.Now; // обновляем стартовое время для следующего интервала
                    }
                }
                else
                {
                    // Процесс завершился впервые
                    if (!p.ExitMessageShown)
                    {
                        if (p.StartTime.HasValue)
                        {
                            p.EndTime = DateTime.Now;
                            p.TotalWorkTime += p.EndTime.Value - p.StartTime.Value; // добавляем финальный интервал
                        }
                        logMessages.Add($"Процесс ID: {p.ProcessId} завершился. \nОбщее время работы: {p.TotalWorkTime}");
                        p.ExitMessageShown = true; // отмечаем, что сообщение выведено
                    }
                }
            }
        }
    }
    
}
