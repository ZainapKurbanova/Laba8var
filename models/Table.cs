using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.Models
{
    public class Table
    {
        public int TableNumber { get; }

        public Table(int tableNumber)
        {
            if (tableNumber <= 0)
                throw new ArgumentException("Номер стола должен быть положительным числом.");
            TableNumber = tableNumber;
        }

        public override string ToString()
        {
            return $"Стол №{TableNumber}";
        }
    }
}
