using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.mixins
{
    public interface INotificationMixin
    {
        void SendNotification(string notification)
        {
            var note = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [NOTIFY] {notification}";
            Console.WriteLine(note);
        }
    }
}
