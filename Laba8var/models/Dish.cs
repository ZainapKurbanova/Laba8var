using System;
using System.Collections.Generic;
using Laba8var.TemplateMethod;

namespace Laba8var.Models
{
    public class Dish : MenuItem, Preparable
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

        // Реализация метода Prepare из интерфейса Preparable
        public void Prepare()
        {
            var cookingProcess = new DishCookingProcess(this);
            cookingProcess.Prepare();
        }

        public override string ToString()
        {
            string ing = Ingredients.Count > 0 ? string.Join(", ", Ingredients) : "нет ингредиентов";
            return $"Блюдо: {Name}, Цена: {Price}, Ингредиенты: {ing}";
        }
    }
}