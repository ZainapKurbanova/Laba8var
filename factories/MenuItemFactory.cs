using System;
using System.Collections.Generic;
using Laba8var.Models;

namespace Laba8var.Factories
{
    public static class MenuItemFactory
    {
        // Создает пункт меню указанного типа
        public static MenuItem CreateItem(string itemType, params object[] args)
        {
            return itemType.ToLower() switch
            {
                "dish" => CreateDish(args),
                "drink" => CreateDrink(args),
                "dessert" => CreateDessert(args),
                _ => throw new ArgumentException($"Неизвестный тип пункта меню: {itemType}")
            };
        }

        // Создает объект блюда
        private static Dish CreateDish(object[] args)
        {
            if (args.Length < 5)
                throw new ArgumentException("Недостаточно параметров для Dish");

            return new Dish(
                (int)args[0],
                (string)args[1],
                (decimal)args[2],
                (string)args[3],
                (List<string>)args[4],
                args.Length > 5 ? (bool)args[5] : true
            );
        }

        // Создает объект напитка
        private static Drink CreateDrink(object[] args)
        {
            if (args.Length < 5)
                throw new ArgumentException("Недостаточно параметров для Drink");

            return new Drink(
                (int)args[0],
                (string)args[1],
                (decimal)args[2],
                (string)args[3],
                (double)args[4],
                args.Length > 5 ? (bool)args[5] : true
            );
        }

        // Создает объект десерта
        private static Dessert CreateDessert(object[] args)
        {
            if (args.Length < 5)
                throw new ArgumentException("Недостаточно параметров для Dessert");

            return new Dessert(
                (int)args[0],
                (string)args[1],
                (decimal)args[2],
                (string)args[3],
                (int)args[4],
                args.Length > 5 ? (bool)args[5] : true
            );
        }
    }
}