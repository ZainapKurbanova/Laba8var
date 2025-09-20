using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.permissions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckPermissionsAttribute : Attribute
    {
        public string RequiredPermission { get; }

        public CheckPermissionsAttribute(string requiredPermission)
        {
            RequiredPermission = requiredPermission;
        }
    }
}
