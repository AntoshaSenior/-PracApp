using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextLib
{
    public class ProcessInfo
    {
        public string ProcessId { get; set; }
        public string ProcessName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeSpan TotalWorkTime { get; set; } = TimeSpan.Zero;

        public bool ExitMessageShown { get; set; } = false;
        public bool IsRunning => EndTime == null;
    }
}
