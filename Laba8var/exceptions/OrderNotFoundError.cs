using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.exceptions
{
    /// <summary>
    /// Исключение, возникающее, когда заказ не найден.
    /// </summary>
    public class OrderNotFoundError : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="OrderNotFoundError"/> с указанным сообщением.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public OrderNotFoundError(string message) : base(message) { }
    }
}
