using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Laba8var.mixins
{
    /// <summary>
    /// Миксин для отправки уведомлений.
    /// </summary>
    public interface INotificationMixin
    {
        /// <summary>
        /// Отправляет уведомление в консоль с отметкой времени.
        /// </summary>
        /// <param name="notification">Текст уведомления.</param>
        void SendNotification(string notification)
        {
            var note = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [NOTIFY] {notification}";
            Console.WriteLine(note);
        }
    }
}