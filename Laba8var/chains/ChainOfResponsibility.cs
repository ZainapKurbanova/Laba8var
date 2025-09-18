using System;
using System.Collections.Generic;
using Laba8var.Models;

namespace Laba8var.ChainOfResponsibility
{
    // Тип изменения заказа
    public enum ChangeType
    {
        Minor,           // Незначительное изменение
        IngredientChange, // Изменение ингредиентов
        Major            // Серьезное изменение
    }

    // Запрос на изменение заказа
    public class ChangeRequest
    {
        public ChangeType Type { get; set; }
        public string Description { get; set; }
        public Order Order { get; set; }
    }

    // Абстрактный обработчик
    public abstract class OrderHandler
    {
        protected OrderHandler nextHandler;

        public void SetNextHandler(OrderHandler next)
        {
            nextHandler = next;
        }

        public abstract void HandleRequest(ChangeRequest request);
    }

    // Официант - обрабатывает незначительные изменения
    public class Waiter : OrderHandler
    {
        public override void HandleRequest(ChangeRequest request)
        {
            if (request.Type == ChangeType.Minor)
            {
                Console.WriteLine($"Официант одобрил изменение: {request.Description}");
            }
            else if (nextHandler != null)
            {
                nextHandler.HandleRequest(request);
            }
            else
            {
                Console.WriteLine($"Запрос не может быть обработан: {request.Description}");
            }
        }
    }

    // Шеф-повар - обрабатывает изменения ингредиентов
    public class Chef : OrderHandler
    {
        public override void HandleRequest(ChangeRequest request)
        {
            if (request.Type == ChangeType.IngredientChange)
            {
                Console.WriteLine($"Шеф-повар одобрил изменение: {request.Description}");
            }
            else if (nextHandler != null)
            {
                nextHandler.HandleRequest(request);
            }
            else
            {
                Console.WriteLine($"Запрос не может быть обработан: {request.Description}");
            }
        }
    }

    // Управляющий - обрабатывает любые изменения
    public class Manager : OrderHandler
    {
        public override void HandleRequest(ChangeRequest request)
        {
            Console.WriteLine($"Управляющий одобрил изменение: {request.Description}");
        }
    }
}