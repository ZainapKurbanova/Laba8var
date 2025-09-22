using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.Models
{
    /// <summary>
    /// Представляет стол в ресторане.
    /// </summary>
    public class Table
    {
        /// <summary>Номер стола.</summary>
        public int TableNumber { get; }

        /// <summary>Создаёт новый стол с указанным номером.</summary>
        /// <param name="tableNumber">Положительный номер стола.</param>
        /// <exception cref="ArgumentException">Если номер стола ≤ 0.</exception>
        public Table(int tableNumber)
        {
            if (tableNumber <= 0)
                throw new ArgumentException("Номер стола должен быть положительным числом.");
            TableNumber = tableNumber;
        }

        /// <summary>Строковое представление стола.</summary>
        public override string ToString() => $"Стол №{TableNumber}";
    }
}