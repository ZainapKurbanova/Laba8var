using System;
using System.Collections.Generic;
using Laba8var.TemplateMethod;

namespace Laba8var.Models
{
    /// <summary>
    /// Основное блюдо меню. Реализует <see cref="Preparable"/>.
    /// </summary>
    public class Dish : MenuItem, Preparable
    {
        private List<string> ingredients;

        /// <summary>
        /// Создаёт новое блюдо.
        /// </summary>
        public Dish(int itemId, string name, decimal price, string category, List<string> ingredients, bool isAvailable = true)
            : base(itemId, name, price, category, isAvailable)
        {
            Ingredients = ingredients;
        }

        /// <summary>Ингредиенты блюда.</summary>
        public List<string> Ingredients
        {
            get => ingredients;
            set => ingredients = value ?? new List<string>();
        }

        /// <summary>Вычисляет стоимость с учётом скидки.</summary>
        public override decimal CalculateCost(decimal discountPercent = 0)
        {
            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentException("Скидка должна быть 0–100%.");
            return Price - Price * discountPercent / 100;
        }

        /// <summary>Приготовление блюда.</summary>
        public void Prepare() => new DishCookingProcess(this).Prepare();

        /// <summary>Строковое представление блюда.</summary>
        public override string ToString()
        {
            string ing = Ingredients.Count > 0 ? string.Join(", ", Ingredients) : "нет ингредиентов";
            return $"Блюдо: {Name}, Цена: {Price}, Ингредиенты: {ing}";
        }

        /// <summary>Преобразует блюдо в словарь.</summary>
        public override Dictionary<string, object> ToDict()
        {
            var dict = base.ToDict();
            dict["Ingredients"] = new List<string>(Ingredients);
            return dict;
        }

        /// <summary>Создаёт блюдо из словаря.</summary>
        public static Dish FromDict(Dictionary<string, object> dict)
        {
            var ingredients = (dict["Ingredients"] as List<object> ?? new List<object>())
                .Select(x => x?.ToString() ?? "").ToList();
            return new Dish(
                Convert.ToInt32(dict["id"]),
                dict["name"].ToString(),
                Convert.ToDecimal(dict["price"]),
                dict["category"].ToString(),
                ingredients,
                Convert.ToBoolean(dict["isAvailable"])
            );
        }
    }
}
