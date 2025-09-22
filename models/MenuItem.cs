using Laba8var.mixins;
using System;
using Newtonsoft.Json.Linq;
using Laba8var.exceptions;

namespace Laba8var.Models
{
    /// <summary>
    /// Абстрактный класс для всех пунктов меню (блюда, напитки, десерты).
    /// Содержит базовые свойства и методы сравнения.
    /// </summary>
    public abstract class MenuItem : IComparable<MenuItem>, ILoggingMixin, INotificationMixin
    {
        private int itemId;
        private string name;
        private decimal price;
        private string category;
        private bool isAvailable;

        /// <summary>
        /// Конструктор базового пункта меню
        /// </summary>
        protected MenuItem(int itemId, string name, decimal price, string category, bool isAvailable = true)
        {
            ItemId = itemId;
            Name = name;
            Price = price;
            Category = category;
            IsAvailable = isAvailable;

            // Логируем создание пункта меню
            ((ILoggingMixin)this).LogAction($"Создан пункт меню: {name} (ID: {itemId}, Категория: {category})");
        }

        // --- Свойства ---
        /// Уникальный идентификатор
        public int ItemId
        {
            get => itemId;
            set
            {
                if (value <= 0)
                    throw new InvalidItemError("Идентификатор должен быть больше нуля.");
                itemId = value;
            }
        }

        /// Название
        public string Name
        {
            get => name;
            set => name = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new InvalidItemError("Название не может быть пустым.");
        }

        /// Цена
        public decimal Price
        {
            get => price;
            set
            {
                if (value < 0)
                    throw new InvalidItemError("Цена не может быть отрицательной.");
                price = value;
            }
        }

        /// Категория (салаты, напитки и т.д.)
        public string Category
        {
            get => category;
            set => category = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new InvalidItemError("Категория не может быть пустой.");
        }

        /// Доступность для заказа
        public bool IsAvailable
        {
            get => isAvailable;
            set
            {
                if (isAvailable == value) return;
                isAvailable = value;

                // логируем смену статуса
                var status = value ? "доступен" : "недоступен";
                ((ILoggingMixin)this).LogAction($"Пункт меню #{ItemId} «{Name}» теперь {status}");

                // если стал недоступен — отправляем уведомление
                if (!value)
                    ((INotificationMixin)this).SendNotification($"Пункт меню «{Name}» закончился");
            }
        }

        // --- Абстрактные методы ---
        /// Метод для расчёта стоимости (реализуется в наследниках).
        public abstract decimal CalculateCost(decimal discountPercent = 0);

        // --- Переопределение ToString ---
        public override string ToString()
        {
            return $"Пункт меню: {Name}, Цена: {Price:C}, Категория: {Category}, Доступен: {IsAvailable}";
        }

        // Реализация IComparable Аналог __lt__ и __gt__
        /// Сравнение по цене, если цены равны — по названию
        public int CompareTo(MenuItem other)
        {
            if (other == null) return 1;

            int priceComparison = Price.CompareTo(other.Price);
            if (priceComparison != 0)
                return priceComparison;

            return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        // -Equals и GetHashCode  Аналог __eq__
        /// Проверка равенства по ID, имени и цене
        public override bool Equals(object obj)
        {
            if (obj is MenuItem other)
            {
                return ItemId == other.ItemId &&
                       string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase) &&
                       Price == other.Price;
            }
            return false;
        }

        /// Хэш-код должен соответствовать Equals
        public override int GetHashCode()
        {
            return HashCode.Combine(ItemId, Name.ToLowerInvariant(), Price);
        }

        // --- Перегрузка операторов ---
        public static bool operator <(MenuItem left, MenuItem right) => left.CompareTo(right) < 0;
        public static bool operator >(MenuItem left, MenuItem right) => left.CompareTo(right) > 0;
        public static bool operator <=(MenuItem left, MenuItem right) => left.CompareTo(right) <= 0;
        public static bool operator >=(MenuItem left, MenuItem right) => left.CompareTo(right) >= 0;
        public static bool operator ==(MenuItem left, MenuItem right) => Equals(left, right);
        public static bool operator !=(MenuItem left, MenuItem right) => !Equals(left, right);

        // --- Сериализация ---
        /// Преобразование в словарь
        public virtual Dictionary<string, object> ToDict()
        {
            return new Dictionary<string, object>
            {
                ["type"] = GetType().Name,
                ["id"] = this.ItemId,
                ["name"] = this.Name,
                ["price"] = this.Price,
                ["category"] = this.Category,
                ["isAvailable"] = this.IsAvailable
            };
        }

        /// Восстановление из JObject (после JSON-десериализации)
        public static MenuItem FromDict(JObject jo)
        {
            var type = jo.Value<string>("type") ?? throw new InvalidItemError("Missing type in item data");
            var id = jo["id"]?.Value<int>() ?? 0;
            var name = jo.Value<string>("name") ?? string.Empty;
            var price = jo["price"]?.Value<decimal>() ?? 0m;
            var category = jo.Value<string>("category") ?? string.Empty;
            var isAvailable = jo["isAvailable"]?.Value<bool>() ?? true;

            switch (type.ToLowerInvariant())
            {
                case "dish":
                    var ingredients = jo["ingredients"] != null
                        ? jo["ingredients"].ToObject<List<string>>()
                        : new List<string>();
                    return new Dish(id, name, price, category, ingredients) { IsAvailable = isAvailable };

                case "drink":
                    var volume = jo["volume"]?.Value<double>() ?? 0.0;
                    return new Drink(id, name, price, category, volume) { IsAvailable = isAvailable };

                case "dessert":
                    var calories = jo["calories"]?.Value<int>() ?? 0;
                    return new Dessert(id, name, price, category, calories) { IsAvailable = isAvailable };

                default:
                    throw new InvalidItemError($"Unknown menu item type: {type}");
            }
        }
    }
}
