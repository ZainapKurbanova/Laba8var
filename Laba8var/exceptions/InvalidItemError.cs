using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.exceptions
{
    /// <summary>
    /// Исключение, возникающее при некорректном элементе.
    /// </summary>
    public class InvalidItemError : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="InvalidItemError"/> с указанным сообщением.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public InvalidItemError(string message) : base(message) { }
    }
}
