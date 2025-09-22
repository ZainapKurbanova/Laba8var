using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.mixins
{
    /// <summary>Интерфейс-миксин для логирования действий</summary>
    public interface ILoggingMixin
    {
        /// <summary>Записывает действие в лог с временной меткой</summary>
        /// <param name="action">Описание действия для логирования</param>
        void LogAction(string action)
        {
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {action}";
            Console.WriteLine(logEntry);
            File.AppendAllText("log.txt", logEntry + Environment.NewLine);
        }
    }
}