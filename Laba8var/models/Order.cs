using Laba8var.mixins;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Laba8var.Models
{
    /// <summary>
    /// Заказ клиента. Реализует <see cref="ILoggingMixin"/> и <see cref="INotificationMixin"/>.
    /// </summary>
    public class Order : ILoggingMixin, INotificationMixin
    {
        private List<MenuItem> items;

        /// <summary>Идентификатор заказа.</summary>
        public int OrderId { get; }

        /// <summary>Имя клиента.</summary>
        public string Customer { get; set; }

        /// <summary>Стол, за которым оформлен заказ.</summary>
        public Table Table { get; }

        /// <summary>Общая стоимость заказа.</summary>
        public decimal TotalCost { get; private set; }

        /// <summary>Список пунктов меню (только для чтения).</summary>
        public IReadOnlyList<MenuItem> Items => items.AsReadOnly();

        /// <summary>Создаёт новый заказ.</summary>
        public Order(int orderId, string customer, int tableNumber)
        {
            if (string.IsNullOrWhiteSpace(customer))
                throw new ArgumentException("Имя клиента не может быть пустым.");

            OrderId = orderId;
            Customer = customer;
            Table = new Table(tableNumber);
            items = new List<MenuItem>();

            ((ILoggingMixin)this).LogAction($"Заказ №{OrderId} создан для {Customer} за столом №{tableNumber}");
            ((INotificationMixin)this).SendNotification($"Новый заказ #{OrderId} оформлен");
        }

        /// <summary>Добавляет пункт меню и пересчитывает стоимость.</summary>
        public void AddItem(MenuItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            items.Add(item);
            CalculateTotal();
            ((ILoggingMixin)this).LogAction($"В заказ №{OrderId} добавлен пункт: {item.Name}");
        }

        /// <summary>Удаляет пункт меню и пересчитывает стоимость.</summary>
        public void RemoveItem(MenuItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            items.Remove(item);
            CalculateTotal();
            ((ILoggingMixin)this).LogAction($"Из заказа №{OrderId} удален пункт: {item.Name}");
        }

        /// <summary>Пересчитывает общую стоимость заказа.</summary>
        public void CalculateTotal() => TotalCost = items.Sum(i => i.CalculateCost());

        /// <summary>Отмечает заказ как готовый и отправляет уведомление.</summary>
        public void MarkAsReady()
        {
            ((INotificationMixin)this).SendNotification($"Заказ №{OrderId} готов! Клиент: {Customer}, Стол: {Table.TableNumber}");
            ((ILoggingMixin)this).LogAction($"Заказ №{OrderId} отмечен как готовый");
        }

        /// <summary>Строковое представление заказа.</summary>
        public override string ToString() =>
            $"Заказ №{OrderId}, Клиент: {Customer}, {Table}, Сумма: {TotalCost}, Позиции: {items.Count}";

        /// <summary>Преобразует заказ в словарь для сериализации.</summary>
        public Dictionary<string, object> ToDict() => new()
        {
            ["OrderId"] = OrderId,
            ["Customer"] = Customer,
            ["TableNumber"] = Table.TableNumber,
            ["TotalCost"] = TotalCost,
            ["Items"] = items.Select(i => i.ToDict()).ToList()
        };

        /// <summary>Создаёт заказ из словаря.</summary>
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
                        order.AddItem(MenuItem.FromDict(jo));
                    }
                }
            }
            return order;
        }
    }
}

