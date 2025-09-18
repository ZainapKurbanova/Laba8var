using Laba8var.ChainOfResponsibility;
using Laba8var.Factories;
using Laba8var.Meta;
using Laba8var.Models;
using Laba8var.TemplateMethod;
using System;
using System.Collections.Generic;

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

        }
    }
}