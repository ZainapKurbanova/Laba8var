using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.permissions
{
    /// <summary>
    /// Атрибут для методов, требующих определённого разрешения.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckPermissionsAttribute : Attribute
    {
        /// <summary>Необходимое разрешение для метода.</summary>
        public string RequiredPermission { get; }

        /// <summary>Создаёт атрибут с указанным разрешением.</summary>
        /// <param name="requiredPermission">Название разрешения.</param>
        public CheckPermissionsAttribute(string requiredPermission)
        {
            RequiredPermission = requiredPermission;
        }
    }
}