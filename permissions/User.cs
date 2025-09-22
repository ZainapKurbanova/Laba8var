using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.permissions
{
    public class User
    {
        public string Name { get; set; }
        public string[] Permissions { get; set; }

        public bool HasPermission(string permission)
        {
            return Array.Exists(Permissions, p => p == permission);
        }
    }
}
