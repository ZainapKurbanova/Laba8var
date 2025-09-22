using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.exceptions
{
    public class InvalidItemError : Exception
    {
        public InvalidItemError(string message) : base(message) { }
    }
}
