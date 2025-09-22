using Laba8var.permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.services
{
    /// <summary>Сервис управления заказами</summary>
    public class OrderService
    {
        /// <summary>Изменяет заказ (требуется право edit_order)</summary>
        [CheckPermissions("edit_order")]
        public void ChangeOrder()
        {
            Console.WriteLine("Заказ изменён!");
        }
    }
}