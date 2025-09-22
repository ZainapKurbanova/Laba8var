using System;
using System.Collections.Generic;
using Laba8var.TemplateMethod;

namespace Laba8var.Models
{
    /// <summary>Представляет десерт в меню</summary>
    public class Dessert : MenuItem, Preparable
    {
        private int calories;

        /// <summary>Создает новый экземпляр десерта</summary>
        public Dessert(int itemId, string name, decimal price, string category, int calories, bool isAvailable = true)
            : base(itemId, name, price, category, isAvailable)
        {
            Calories = calories;
        }

        /// <summary>Калорийность десерта</summary>
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

        /// <summary>Рассчитывает стоимость с учетом скидки</summary>
        public override decimal CalculateCost(decimal discountPercent = 0)
        {
            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentException("Скидка должна быть от 0 до 100%.");

            return Price - Price * discountPercent / 100;
        }

        /// <summary>Запускает процесс приготовления десерта</summary>
        public void Prepare()
        {
            var cookingProcess = new DessertCookingProcess(this);
            cookingProcess.Prepare();
        }

        /// <summary>Возвращает строковое представление десерта</summary>
        public override string ToString()
        {
            return $"Десерт: {Name}, Цена: {Price}, Калорийность: {Calories} ккал";
        }

        /// <summary>Преобразует объект в словарь</summary>
        public override Dictionary<string, object> ToDict()
        {
            var dict = base.ToDict();
            dict["Calories"] = Calories;
            return dict;
        }

        /// <summary>Создает объект из словаря</summary>
        public static Dessert FromDict(Dictionary<string, object> dict)
        {
            return new Dessert(
                Convert.ToInt32(dict["id"]),
                dict["name"].ToString(),
                Convert.ToDecimal(dict["price"]),
                dict["category"].ToString(),
                Convert.ToInt32(dict["Calories"]),
                Convert.ToBoolean(dict["isAvailable"])
            );
        }
    }
}