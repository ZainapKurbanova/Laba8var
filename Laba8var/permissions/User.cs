using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.permissions
{
    /// <summary>
    /// Пользователь с набором разрешений.
    /// </summary>
    public class User
    {
        /// <summary>Имя пользователя.</summary>
        public string Name { get; set; }

        /// <summary>Список разрешений пользователя.</summary>
        public string[] Permissions { get; set; }

        /// <summary>Проверяет, имеет ли пользователь указанное разрешение.</summary>
        /// <param name="permission">Название разрешения.</param>
        /// <returns>True, если разрешение есть, иначе false.</returns>
        public bool HasPermission(string permission)
        {
            return Array.Exists(Permissions, p => p == permission);
        }
    }
}