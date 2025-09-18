using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.Models
{

    public class Dish : MenuItem
    {
        private List<string> ingredients;

        public Dish(int itemId, string name, decimal price, string category, List<string> ingredients, bool isAvailable = true)
            : base(itemId, name, price, category, isAvailable)
        {
            Ingredients = ingredients;
        }

        public List<string> Ingredients
        {
            get => ingredients;
            set => ingredients = value ?? new List<string>();
        }

        public override decimal CalculateCost(decimal discountPercent = 0)
        {
            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentException("Скидка должна быть от 0 до 100%.");

            return Price - Price * discountPercent / 100;
        }

        public override string ToString()
        {
            string ing = Ingredients.Count > 0 ? string.Join(", ", Ingredients) : "нет ингредиентов";
            return $"Блюдо: {Name}, Цена: {Price}, Ингредиенты: {ing}";
        }
    }

}
