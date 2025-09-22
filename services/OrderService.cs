using Laba8var.permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.services
{
    public class OrderService
    {
        [CheckPermissions("edit_order")]
        public void ChangeOrder()
        {
            Console.WriteLine("Заказ изменён!");
        }
    }
}
