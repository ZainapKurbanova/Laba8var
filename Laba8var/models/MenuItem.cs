using Laba8var.mixins;
using System;
namespace Laba8var.Models
{
    public abstract class MenuItem : IComparable<MenuItem>, ILoggingMixin, INotificationMixin
    {
        private int itemId;
        private string name;
        private decimal price;
        private string category;
        private bool isAvailable;

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

        /// Уникальный идентификатор пункта меню
        public int ItemId
        {
            get => itemId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Идентификатор должен быть больше нуля.");
                itemId = value;
            }
        }

        /// Название блюда/напитка
        public string Name
        {
            get => name;
            set => name = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("Название не может быть пустым.");
        }

        /// Цена
        public decimal Price
        {
            get => price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Цена не может быть отрицательной.");
                price = value;
            }
        }

        /// Категория (например: «Супы», «Напитки», «Десерты»)
        public string Category
        {
            get => category;
            set => category = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("Категория не может быть пустой.");
        }

        /// Доступен ли пункт меню для заказа
        public bool IsAvailable
        {
            get => isAvailable;
            set
            {
                if (isAvailable == value) return;
                isAvailable = value;

                // логируем смену доступности
                var status = value ? "доступен" : "недоступен";
                ((ILoggingMixin)this).LogAction($"Пункт меню #{ItemId} «{Name}» теперь {status}");

                // если стал недоступен — отправляем уведомление
                if (!value)
                    ((INotificationMixin)this).SendNotification($"Пункт меню «{Name}» закончился");
            }
        }

        /// Абстрактный метод расчёта стоимости. Реализуется в наследниках
        public abstract decimal CalculateCost(decimal discountPercent = 0);

        /// Строковое представление объекта
        public override string ToString()
        {
            return $"Пункт меню: {Name}, Цена: {Price:C}, Категория: {Category}, Доступен: {IsAvailable}";
        }

        /// Сравнение по цене, а при равенстве — по названию
        public int CompareTo(MenuItem other)
        {
            if (other == null) return 1;

            int priceComparison = Price.CompareTo(other.Price);

            if (priceComparison != 0)
                return priceComparison;

            return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator <(MenuItem left, MenuItem right) => left.CompareTo(right) < 0;
        public static bool operator >(MenuItem left, MenuItem right) => left.CompareTo(right) > 0;
        public static bool operator ==(MenuItem left, MenuItem right) => Equals(left, right);
        public static bool operator !=(MenuItem left, MenuItem right) => !Equals(left, right);
    }
}

