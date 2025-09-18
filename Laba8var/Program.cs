using System;
using System.Collections.Generic;
using Laba8var.Models;
using Laba8var.Meta; // для MenuItemMeta

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
        }
    }
}