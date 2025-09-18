using Laba8var.mixins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.Models
{
    public class Order : ILoggingMixin, INotificationMixin
    {
        private List<MenuItem> items;

        public int OrderId { get; }
        public string Customer { get; set; }
        public Table Table { get; }   // композиция
        public decimal TotalCost { get; private set; }

        public IReadOnlyList<MenuItem> Items => items.AsReadOnly(); // только для чтения снаружи

        public Order(int orderId, string customer, int tableNumber)
        {
            if (string.IsNullOrWhiteSpace(customer))
                throw new ArgumentException("Имя клиента не может быть пустым.");

            OrderId = orderId;
            Customer = customer;
            Table = new Table(tableNumber); // композиция — Table создаётся внутри заказа
            items = new List<MenuItem>();

            // Логируем создание заказа
            ((ILoggingMixin)this).LogAction($"Заказ №{OrderId} создан для клиента {Customer} за столом №{tableNumber}");
            ((INotificationMixin)this).SendNotification($"Новый заказ #{OrderId} оформлен");
        }

        public void AddItem(MenuItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Пункт меню не может быть null.");

            items.Add(item);
            CalculateTotal();

            // Логируем добавление пункта меню
            ((ILoggingMixin)this).LogAction($"В заказ №{OrderId} добавлен пункт: {item.Name} (ID: {item.ItemId})");

        }

        public void RemoveItem(MenuItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Пункт меню не может быть null.");

            items.Remove(item);
            CalculateTotal();

            // Логируем удаление пункта 
            ((ILoggingMixin)this).LogAction($"Из заказа №{OrderId} удален пункт: {item.Name} (ID: {item.ItemId})");
        }

        public void CalculateTotal()
        {
            TotalCost = items.Sum(i => i.CalculateCost());
        }

        // Новый метод для отметки заказа как готового
        public void MarkAsReady()
        {
            ((INotificationMixin)this).SendNotification($"Заказ №{OrderId} готов! Клиент: {Customer}, Стол: {Table.TableNumber}");
            ((ILoggingMixin)this).LogAction($"Заказ №{OrderId} отмечен как готовый");
        }
        public override string ToString()
        {
            return $"Заказ №{OrderId}, Клиент: {Customer}, {Table}, Сумма: {TotalCost}, Позиции: {items.Count}";
        }
    }
}
