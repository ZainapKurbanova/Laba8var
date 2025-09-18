using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.Models
{
    public class Drink : MenuItem
    {
        private double volume;

        public Drink(int itemId, string name, decimal price, string category, double volume, bool isAvailable = true)
            : base(itemId, name, price, category, isAvailable)
        {
            Volume = volume;
        }

        public double Volume
        {
            get => volume;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Объем должен быть положительным числом.");
                volume = value;
            }
        }

        public override decimal CalculateCost(decimal discountPercent = 0)
        {
            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentException("Скидка должна быть от 0 до 100%.");

            return Price - Price * discountPercent / 100;
        }

        public override string ToString()
        {
            return $"Напиток: {Name}, Цена: {Price}, Объем: {Volume} мл";
        }
    }

}
