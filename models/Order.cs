using Laba8var.mixins;
using Newtonsoft.Json.Linq;
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

        // Перевод в словарь
        public Dictionary<string, object> ToDict()
        {
            return new Dictionary<string, object>
            {
                ["OrderId"] = OrderId,
                ["Customer"] = Customer,
                ["TableNumber"] = Table.TableNumber,
                ["TotalCost"] = TotalCost,
                ["Items"] = items.Select(i => i.ToDict()).ToList()
            };
        }

        // Перевод из словаря
        public static Order FromDict(Dictionary<string, object> dict)
        {
            var order = new Order(
                Convert.ToInt32(dict["OrderId"]),
                dict["Customer"].ToString(),
                Convert.ToInt32(dict["TableNumber"])
            );

            if (dict["Items"] is IEnumerable<object> itemsEnum)
            {
                foreach (var itemObj in itemsEnum)
                {
                    if (itemObj is Dictionary<string, object> itemDict)
                    {
                        var type = itemDict["type"].ToString();
                        MenuItem item = type switch
                        {
                            "Dish" => Dish.FromDict(itemDict),
                            "Drink" => Drink.FromDict(itemDict),
                            "Dessert" => Dessert.FromDict(itemDict),
                            _ => throw new InvalidOperationException($"Неизвестный тип: {type}")
                        };
                        order.AddItem(item);
                    }
                    else if (itemObj is JObject jo)
                    {
                        var item = MenuItem.FromDict(jo);
                        order.AddItem(item);
                    }
                }
            }
            return order;
        }
    }
}
