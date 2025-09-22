using System;
using Laba8var.TemplateMethod;

namespace Laba8var.Models
{
    public class Drink : MenuItem, Preparable
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

        // Реализация метода Prepare из интерфейса Preparable
        public void Prepare()
        {
            var cookingProcess = new DrinkCookingProcess(this);
            cookingProcess.Prepare();
        }

        public override string ToString()
        {
            return $"Напиток: {Name}, Цена: {Price}, Объем: {Volume} мл";
        }

        // Перевод в словарь
        public override Dictionary<string, object> ToDict()
        {
            var dict = base.ToDict();
            dict["Volume"] = Volume;
            return dict;
        }

        // Перевод из словаря
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