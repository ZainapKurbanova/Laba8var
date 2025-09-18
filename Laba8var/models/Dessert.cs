using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.Models
{
    public class Dessert : MenuItem
    {
        private int calories;

        public Dessert(int itemId, string name, decimal price, string category, int calories, bool isAvailable = true)
            : base(itemId, name, price, category, isAvailable)
        {
            Calories = calories;
        }

        public int Calories
        {
            get => calories;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Калорийность не может быть отрицательной.");
                calories = value;
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
            return $"Десерт: {Name}, Цена: {Price}, Калорийность: {Calories} ккал";
        }
    }
}
