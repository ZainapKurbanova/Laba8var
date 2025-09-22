using Laba8var.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.permissions
{
    /// <summary>Проверяет права доступа для методов</summary>
    public static class PermissionChecker
    {
        /// <summary>Выполняет действие с проверкой прав</summary>
        public static void InvokeWithPermission(User user, Action action)
        {
            var method = action.Method;
            var attr = method.GetCustomAttribute<CheckPermissionsAttribute>();
            if (attr != null && !user.HasPermission(attr.RequiredPermission))
            {
                throw new PermissionDeniedError(
                    $"У пользователя {user.Name} нет прав доступа для: {attr.RequiredPermission}");
            }

            action();
        }
    }
}