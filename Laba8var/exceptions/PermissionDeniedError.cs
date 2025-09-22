using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.exceptions
{
    /// <summary>
    /// Исключение, возникающее при отказе в доступе или недостатке прав.
    /// </summary>
    public class PermissionDeniedError : Exception
    {
        /// <summary>
        /// Инициализирует новый экземпляр <see cref="PermissionDeniedError"/> с указанным сообщением.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public PermissionDeniedError(string message) : base(message) { }
    }
}
