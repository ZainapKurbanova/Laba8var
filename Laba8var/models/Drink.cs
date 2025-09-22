using System;
using Laba8var.TemplateMethod;

namespace Laba8var.Models
{
    /// <summary>Напиток в меню</summary>
    public class Drink : MenuItem, Preparable
    {
        private double volume;

        /// <summary>Создает напиток</summary>
        public Drink(int itemId, string name, decimal price, string category, double volume, bool isAvailable = true)
            : base(itemId, name, price, category, isAvailable)
        {
            Volume = volume;
        }

        /// <summary>Объем в мл</summary>
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

        /// <summary>Рассчитывает стоимость</summary>
        public override decimal CalculateCost(decimal discountPercent = 0)
        {
            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentException("Скидка должна быть от 0 до 100%.");

            return Price - Price * discountPercent / 100;
        }

        /// <summary>Готовит напиток</summary>
        public void Prepare()
        {
            var cookingProcess = new DrinkCookingProcess(this);
            cookingProcess.Prepare();
        }

        /// <summary>Строковое представление</summary>
        public override string ToString()
        {
            return $"Напиток: {Name}, Цена: {Price}, Объем: {Volume} мл";
        }

        /// <summary>В словарь</summary>
        public override Dictionary<string, object> ToDict()
        {
            var dict = base.ToDict();
            dict["Volume"] = Volume;
            return dict;
        }

        /// <summary>Из словаря</summary>
        public static Drink FromDict(Dictionary<string, object> dict)
        {
            return new Drink(
                Convert.ToInt32(dict["id"]),
                dict["name"].ToString(),
                Convert.ToDecimal(dict["price"]),
                dict["category"].ToString(),
                Convert.ToDouble(dict["Volume"]),
                Convert.ToBoolean(dict["isAvailable"])
            );
        }
    }
}