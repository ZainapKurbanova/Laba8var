using Laba8var.ChainOfResponsibility;
using Laba8var.exceptions;
using Laba8var.Factories;
using Laba8var.Meta;
using Laba8var.Models;
using Laba8var.permissions;
using Laba8var.TemplateMethod;
using Laba8var.Services;
using System;
using System.Collections.Generic;
using Laba8var.services;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Laba8var
{
    class Program
    {
        static void Main(string[] args)
        {
            var configureLogger = LoggerConfig.ConfigureLogger();
            var logger = LoggerConfig.CreateLogger<Program>();
            // создаём пару блюд
            var caesar = new Dish(
                1,
                "Цезарь",
                350m,
                "Салаты",
                new List<string> { "Салат", "Курица", "Пармезан" }
            );

            var latte = new Drink(
                2,
                "Латте",
                200m,
                "Напитки",
                250.0
            );

            // проверяем логи и нотификации при смене доступности
            caesar.IsAvailable = false;
            latte.IsAvailable = false;

            // создаём заказ
            var order = new Order(101, "Иван Иванов", 5);

            // добавляем позиции
            order.AddItem(caesar);
            order.AddItem(latte);

            // пересчитываем общую сумму (автоматически в AddItem)
            logger.LogInformation($"Итоговая сумма заказа: {order.TotalCost:C}\n");

            // удаляем позицию
            order.RemoveItem(caesar);

            // помечаем заказ как готовый
            order.MarkAsReady();

            // проверяем вывод ToString()
            logger.LogInformation(JsonSerializer.Serialize(order));

            // Тестирование MenuItemMeta 
            logger.LogInformation("\n--- Тест MenuItemMeta ---");
            logger.LogInformation("Зарегистрированные типы меню:");
            foreach (var typeName in MenuItemMeta.RegisteredTypes)
            {
                logger.LogInformation($"- {typeName}");
            }

            // Пример создания нового блюда через MenuItemMeta
            var dessert = MenuItemMeta.Create(
                "Dessert",
                3,
                "Тирамису",
                400m,
                "Десерты",
                320,
                true
            );
            logger.LogInformation($"\nСоздано через MenuItemMeta: {dessert}");

            // Создаем пункты меню через фабрику 
            logger.LogInformation("\n\n--- Тестирование MenuItemFactory ---");

            var steak = MenuItemFactory.CreateItem("dish",
                7, "Стейк ", 1200m, "Горячие блюда",
                new List<string> { "Говядина", "Травы", "Специи" });

            var mojito = MenuItemFactory.CreateItem("drink",
                8, "Мохито", 350m, "Коктейли", 400.0);

            var cheesecake = MenuItemFactory.CreateItem("dessert",
                9, "Чизкейк", 450m, "Десерты", 280);

            logger.LogInformation("Созданные пункты меню через фабрику:");
            logger.LogInformation($"1. {steak}");
            logger.LogInformation($"2. {mojito}");
            logger.LogInformation($"3. {cheesecake}");

            // Создаем новый заказ через фабрику
            var factoryOrder = new Order(202, "Анна Петрова", 8);
            factoryOrder.AddItem(steak);
            factoryOrder.AddItem(mojito);
            factoryOrder.AddItem(cheesecake);

            logger.LogInformation($"\nЗаказ созданный через фабрику: {factoryOrder}");
            logger.LogInformation($"Итоговая сумма: {factoryOrder.TotalCost:C}");

            // Создаем цепочку обработчиков
            logger.LogInformation("\n\n--- Тестирование Chain of Responsibility ---");

            var waiter = new Waiter();
            var chef = new Chef();
            var manager = new Manager();

            waiter.SetNextHandler(chef);
            chef.SetNextHandler(manager);

            // Создаем тестовые запросы на изменение
            var minorChange = new ChangeRequest
            {
                Type = ChangeType.Minor,
                Description = "Добавить дополнительный соус",
                Order = factoryOrder
            };

            var ingredientChange = new ChangeRequest
            {
                Type = ChangeType.IngredientChange,
                Description = "Заменить говядину на курицу",
                Order = factoryOrder
            };

            var majorChange = new ChangeRequest
            {
                Type = ChangeType.Major,
                Description = "Отменить весь заказ",
                Order = factoryOrder
            };

            // Обрабатываем запросы через цепочку
            logger.LogInformation("Обработка незначительного изменения:");
            waiter.HandleRequest(minorChange);

            logger.LogInformation("\nОбработка изменения ингредиентов:");
            waiter.HandleRequest(ingredientChange);

            logger.LogInformation("\nОбработка серьезного изменения:");
            waiter.HandleRequest(majorChange);

            // Тестирование шаблонного метода
            logger.LogInformation("\n\n--- Тестирование шаблонного метода приготовления ---");

            // Приводим объекты к конкретным типам
            var steakDish = steak as Dish;
            var mojitoDrink = mojito as Drink;
            var cheesecakeDessert = cheesecake as Dessert;

            // Проверяем, что приведение типов прошло успешно
            if (steakDish != null && mojitoDrink != null && cheesecakeDessert != null)
            {
                Preparable[] preparables = new Preparable[]
                {
                new DishCookingProcess(steakDish),
                new DrinkCookingProcess(mojitoDrink),
                new DessertCookingProcess(cheesecakeDessert)
                };

                foreach (var preparable in preparables)
                {
                    preparable.Prepare(); // Полиморфный вызов через интерфейс
                    logger.LogInformation("");
                }
            }
            else
            {
                logger.LogInformation("Ошибка приведения типов для тестирования шаблонного метода");
            }

            // Тестирование проверки прав доступа
            logger.LogInformation("\nТестирование проверки прав доступа");
            var user = new User { Name = "Alice", Permissions = new[] { "view_order" } };
            var service = new OrderService();
            try
            {
                PermissionChecker.InvokeWithPermission(user, service.ChangeOrder);
            }
            catch (PermissionDeniedError ex)
            {
                logger.LogInformation(ex.Message);
            }

            // Тестирование проверки пункта меню
            var invalidDish = new Dish(1, "Салат Цезарь", 350m, "Салаты", new List<string> { "Листья салата", "Курица", "Пармезан" });
            var order1 = new Order(101, "Иван Иванов", 5);
            order1.AddItem(invalidDish);

            // Тестирование проверки заказа
            try
            {
                var orders = new List<Order>();
                int searchId = 202;
                var order2 = orders.Find(o => o.OrderId == searchId);
                if (order2 == null)
                    throw new OrderNotFoundError($"Заказ с ID {searchId} не найден.");
                logger.LogInformation(JsonSerializer.Serialize(order2));
            }
            catch (OrderNotFoundError ex)
            {
                logger.LogInformation($"Ошибка поиска заказа: {ex.Message}");
            }

            // Тестирование сериализации и десериализации
            logger.LogInformation("\nТестирование сериализации и десериализации");
            // сериализуем заказ в словарь
            var orderDict = factoryOrder.ToDict();
            // ДЕСЕРИАЛИЗАЦИЯ ИЗ DICT
            var restoredOrderFromDict = Order.FromDict(orderDict);
            logger.LogInformation("\nВосстановленный заказ из ToDict:");
            logger.LogInformation(JsonSerializer.Serialize(restoredOrderFromDict));
            logger.LogInformation($"Сумма восстановленного заказа (из dict): {restoredOrderFromDict.TotalCost:C}");
            // СЕРИАЛИЗАЦИЯ В JSON
            var json = JsonSerializer.Serialize(orderDict, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("order.json", json);
            logger.LogInformation("\nJSON сохранён в order.json");
            //ДЕСЕРИАЛИЗАЦИЯ ИЗ JSON
            var loadedJson = File.ReadAllText("order.json");
            var parsed = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(loadedJson);
            if (parsed == null)
            {
                logger.LogInformation("Ошибка: не удалось распарсить JSON.");
            }
            else
            {
                Dictionary<string, object> plainDict = ConvertJsonElementDict(parsed);
                var restoredOrderFromJson = Order.FromDict(plainDict);
                logger.LogInformation("\nВосстановленный заказ из JSON:");
                logger.LogInformation(JsonSerializer.Serialize(restoredOrderFromJson));
                logger.LogInformation($"Сумма восстановленного заказа (из json): {restoredOrderFromJson.TotalCost:C}");
            }

            static object ConvertJsonElement(JsonElement el)
            {
                switch (el.ValueKind)
                {
                    case JsonValueKind.Object:
                        var d = new Dictionary<string, object>();
                        foreach (var p in el.EnumerateObject())
                            d[p.Name] = ConvertJsonElement(p.Value);
                        return d;

                    case JsonValueKind.Array:
                        var list = new List<object>();
                        foreach (var it in el.EnumerateArray())
                            list.Add(ConvertJsonElement(it));
                        return list;

                    case JsonValueKind.String:
                        return el.GetString() ?? string.Empty;

                    case JsonValueKind.Number:
                        if (el.TryGetInt32(out var i)) return i;
                        if (el.TryGetInt64(out var l)) return l;
                        if (el.TryGetDouble(out var dnum)) return dnum;
                        return el.GetDecimal(); // fallback to decimal

                    case JsonValueKind.True:
                        return true;
                    case JsonValueKind.False:
                        return false;
                    case JsonValueKind.Null:
                        return null!;
                    default:
                        return null!;
                }
            }

            static Dictionary<string, object> ConvertJsonElementDict(Dictionary<string, JsonElement> dict)
            {
                return dict.ToDictionary(kv => kv.Key, kv => ConvertJsonElement(kv.Value));
            }
            logger.LogInformation("\n--- Сравнение пунктов меню ---");

            // Создаем пункты меню для сравнения
            var salad = new Dish(10, "Салат", 300m, "Салаты", new List<string> { "Листья салата", "Огурцы" });
            var soup = new Dish(11, "Суп", 250m, "Супы", new List<string> { "Вода", "Овощи" });
            var cola = new Drink(12, "Кола", 250m, "Напитки", 0.5);
            var colaCopy = new Drink(12, "Кола", 250m, "Напитки", 0.5);

            // Сравнение через Equals (__eq__)
            logger.LogInformation($"\n  Проверка Equals (__eq__):");
            logger.LogInformation($"Сравнение {cola.Name} и {colaCopy.Name}: {cola.Equals(colaCopy)}");  // true
            logger.LogInformation($"Сравнение {salad.Name} и {soup.Name}: {salad.Equals(soup)}");      // false

            // Сравнение операторов < и > (__lt__, __gt__)
            logger.LogInformation($"\n Проверка операторов < и >:");
            logger.LogInformation($"{salad.Name} ({salad.Price:C}) > {soup.Name} ({soup.Price:C}) ? {salad > soup}");
            logger.LogInformation($"{salad.Name} ({salad.Price:C}) < {soup.Name} ({soup.Price:C}) ? {salad < soup}");
            logger.LogInformation($"{soup.Name} ({soup.Price:C}) < {cola.Name} ({cola.Price:C}) ? {soup < cola}");
            logger.LogInformation($"{cola.Name} ({cola.Price:C}) > {soup.Name} ({soup.Price:C}) ? {cola > soup}");

            // Сортировка списка и вывод
            var itemsToSort = new List<MenuItem> { salad, soup, cola };
            logger.LogInformation("\n3 Список пунктов меню до сортировки:");
            foreach (var item in itemsToSort)
                logger.LogInformation($"- {item.Name}, Цена: {item.Price:C}");

            itemsToSort.Sort(); // Использует CompareTo (__lt__, __gt__)
            logger.LogInformation("\nСписок пунктов меню после сортировки (по цене, затем по имени):");
            foreach (var item in itemsToSort)
                logger.LogInformation($"- {item.Name}, Цена: {item.Price:C}");

        }
    }
}