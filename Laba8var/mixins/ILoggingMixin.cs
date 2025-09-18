using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.mixins
{
    public interface ILoggingMixin
    {
        void LogAction(string action)
        {
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {action}";
            Console.WriteLine(logEntry);
            File.AppendAllText("log.txt", logEntry + Environment.NewLine);
        }
    }
}
