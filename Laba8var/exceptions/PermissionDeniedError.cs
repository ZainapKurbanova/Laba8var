using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.exceptions
{
    public class PermissionDeniedError : Exception
    {
        public PermissionDeniedError(string message) : base(message) { }
    }
}
