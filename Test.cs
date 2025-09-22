using Laba8var.ChainOfResponsibility;
using Laba8var.exceptions;
using Laba8var.Factories;
using Laba8var.Meta;
using Laba8var.Models;
using Laba8var.permissions;
using Laba8var.TemplateMethod;
using System;
using System.Collections.Generic;
using Laba8var.services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Laba8var
{
    class Program
    {
        static void Main(string[] args)
        {
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
            Console.WriteLine($"Итоговая сумма заказа: {order.TotalCost:C}\n");

            // удаляем позицию
            order.RemoveItem(caesar);

            // помечаем заказ как готовый
            order.MarkAsReady();

            // проверяем вывод ToString()
            Console.WriteLine(order);

            // Тестирование MenuItemMeta 
            Console.WriteLine("\n--- Тест MenuItemMeta ---");
            Console.WriteLine("Зарегистрированные типы меню:");
            foreach (var typeName in MenuItemMeta.RegisteredTypes)
            {
                Console.WriteLine($"- {typeName}");
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
            Console.WriteLine($"\nСоздано через MenuItemMeta: {dessert}");

            // Создаем пункты меню через фабрику 
            Console.WriteLine("\n\n--- Тестирование MenuItemFactory ---");

            var steak = MenuItemFactory.CreateItem("dish",
                7, "Стейк ", 1200m, "Горячие блюда",
                new List<string> { "Говядина", "Травы", "Специи" });

            var mojito = MenuItemFactory.CreateItem("drink",
                8, "Мохито", 350m, "Коктейли", 400.0);

            var cheesecake = MenuItemFactory.CreateItem("dessert",
                9, "Чизкейк", 450m, "Десерты", 280);

            Console.WriteLine("Созданные пункты меню через фабрику:");
            Console.WriteLine($"1. {steak}");
            Console.WriteLine($"2. {mojito}");
            Console.WriteLine($"3. {cheesecake}");

            // Создаем новый заказ через фабрику
            var factoryOrder = new Order(202, "Анна Петрова", 8);
            factoryOrder.AddItem(steak);
            factoryOrder.AddItem(mojito);
            factoryOrder.AddItem(cheesecake);

            Console.WriteLine($"\nЗаказ созданный через фабрику: {factoryOrder}");
            Console.WriteLine($"Итоговая сумма: {factoryOrder.TotalCost:C}");

            // Создаем цепочку обработчиков
            Console.WriteLine("\n\n--- Тестирование Chain of Responsibility ---");

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
            Console.WriteLine("Обработка незначительного изменения:");
            waiter.HandleRequest(minorChange);

            Console.WriteLine("\nОбработка изменения ингредиентов:");
            waiter.HandleRequest(ingredientChange);

            Console.WriteLine("\nОбработка серьезного изменения:");
            waiter.HandleRequest(majorChange);

            // Тестирование шаблонного метода
            Console.WriteLine("\n\n--- Тестирование шаблонного метода приготовления ---");

            // Приводим объекты к конкретным типам
            var steakDish = steak as Dish;
            var mojitoDrink = mojito as Drink;
            var cheesecakeDessert = cheesecake as Dessert;

            // Проверяем, что приведение типов прошло успешно
            if (steakDish != null && mojitoDrink != null && cheesecakeDessert != null)
            {
                // Тестирование процесса приготовления для каждого типа блюд
                Console.WriteLine("\nПроцесс приготовления стейка:");
                var steakProcess = new DishCookingProcess(steakDish);
                steakProcess.Prepare();

                Console.WriteLine("\nПроцесс приготовления мохито:");
                var mojitoProcess = new DrinkCookingProcess(mojitoDrink);
                mojitoProcess.Prepare();

                Console.WriteLine("\nПроцесс приготовления чизкейка:");
                var cheesecakeProcess = new DessertCookingProcess(cheesecakeDessert);
                cheesecakeProcess.Prepare();
            }
            else
            {
                Console.WriteLine("Ошибка приведения типов для тестирования шаблонного метода");
            }

            // Тестирование проверки прав доступа
            Console.WriteLine("\nТестирование проверки прав доступа");
            var user = new User { Name = "Alice", Permissions = new[] { "view_order" } };
            var service = new OrderService();
            try
            {
                PermissionChecker.InvokeWithPermission(user, service.ChangeOrder);
            }
            catch (PermissionDeniedError ex)
            {
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(order2);
            }
            catch (OrderNotFoundError ex)
            {
                Console.WriteLine($"Ошибка поиска заказа: {ex.Message}");
            }

            // Тестирование сериализации и десериализации
            Console.WriteLine("\nТестирование сериализации и десериализации");
            // сериализуем заказ в словарь
            var orderDict = factoryOrder.ToDict();
            // ДЕСЕРИАЛИЗАЦИЯ ИЗ DICT
            var restoredOrderFromDict = Order.FromDict(orderDict);
            Console.WriteLine("\nВосстановленный заказ из ToDict:");
            Console.WriteLine(restoredOrderFromDict);
            Console.WriteLine($"Сумма восстановленного заказа (из dict): {restoredOrderFromDict.TotalCost:C}");
            // СЕРИАЛИЗАЦИЯ В JSON
            var json = JsonSerializer.Serialize(orderDict, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("order.json", json);
            Console.WriteLine("\nJSON сохранён в order.json");
            //ДЕСЕРИАЛИЗАЦИЯ ИЗ JSON
            var loadedJson = File.ReadAllText("order.json");
            var parsed = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(loadedJson);
            if (parsed == null)
            {
                Console.WriteLine("Ошибка: не удалось распарсить JSON.");
            }
            else
            {
                Dictionary<string, object> plainDict = ConvertJsonElementDict(parsed);
                var restoredOrderFromJson = Order.FromDict(plainDict);
                Console.WriteLine("\nВосстановленный заказ из JSON:");
                Console.WriteLine(restoredOrderFromJson);
                Console.WriteLine($"Сумма восстановленного заказа (из json): {restoredOrderFromJson.TotalCost:C}");
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
            Console.WriteLine("\n--- Сравнение пунктов меню ---");

            // Создаем пункты меню для сравнения
            var salad = new Dish(10, "Салат", 300m, "Салаты", new List<string> { "Листья салата", "Огурцы" });
            var soup = new Dish(11, "Суп", 250m, "Супы", new List<string> { "Вода", "Овощи" });
            var cola = new Drink(12, "Кола", 250m, "Напитки", 0.5);
            var colaCopy = new Drink(12, "Кола", 250m, "Напитки", 0.5);

            // Сравнение через Equals (__eq__)
            Console.WriteLine($"\n1️⃣ Проверка Equals (__eq__):");
            Console.WriteLine($"Сравнение {cola.Name} и {colaCopy.Name}: {cola.Equals(colaCopy)}");  // true
            Console.WriteLine($"Сравнение {salad.Name} и {soup.Name}: {salad.Equals(soup)}");      // false

            // Сравнение операторов < и > (__lt__, __gt__)
            Console.WriteLine($"\n2️⃣ Проверка операторов < и >:");
            Console.WriteLine($"{salad.Name} ({salad.Price:C}) > {soup.Name} ({soup.Price:C}) ? {salad > soup}");
            Console.WriteLine($"{salad.Name} ({salad.Price:C}) < {soup.Name} ({soup.Price:C}) ? {salad < soup}");
            Console.WriteLine($"{soup.Name} ({soup.Price:C}) < {cola.Name} ({cola.Price:C}) ? {soup < cola}");
            Console.WriteLine($"{cola.Name} ({cola.Price:C}) > {soup.Name} ({soup.Price:C}) ? {cola > soup}");

            // Сортировка списка и вывод
            var itemsToSort = new List<MenuItem> { salad, soup, cola };
            Console.WriteLine("\n3️⃣ Список пунктов меню до сортировки:");
            foreach (var item in itemsToSort)
                Console.WriteLine($"- {item.Name}, Цена: {item.Price:C}");

            itemsToSort.Sort(); // Использует CompareTo (__lt__, __gt__)
            Console.WriteLine("\n4️⃣ Список пунктов меню после сортировки (по цене, затем по имени):");
            foreach (var item in itemsToSort)
                Console.WriteLine($"- {item.Name}, Цена: {item.Price:C}");

        }
    }
}