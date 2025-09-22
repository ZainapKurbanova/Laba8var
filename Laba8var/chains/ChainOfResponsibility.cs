using System;
using System.Collections.Generic;
using Laba8var.Models;

namespace Laba8var.ChainOfResponsibility
{
    /// <summary>
    /// Тип изменения заказа.
    /// Используется для определения уровня сложности изменений.
    /// </summary>
    public enum ChangeType
    {
        /// <summary>
        /// Незначительное изменение (например, соус, специи).
        /// </summary>
        Minor,

        /// <summary>
        /// Изменение ингредиентов (например, замена мяса).
        /// </summary>
        IngredientChange,

        /// <summary>
        /// Серьёзное изменение (например, отмена всего заказа).
        /// </summary>
        Major
    }

    /// <summary>
    /// Класс, представляющий запрос на изменение заказа.
    /// Передаётся по цепочке обработчиков.
    /// </summary>
    public class ChangeRequest
    {
        /// <summary>
        /// Тип изменения (незначительное, ингредиенты, серьёзное).
        /// </summary>
        public ChangeType Type { get; set; }

        /// <summary>
        /// Описание изменения, заданное пользователем.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Заказ, к которому относится изменение.
        /// </summary>
        public Order Order { get; set; }
    }

    /// <summary>
    /// Абстрактный обработчик изменений заказа.
    /// Реализует базовую логику цепочки обязанностей.
    /// </summary>
    public abstract class OrderHandler
    {
        /// <summary>
        /// Следующий обработчик в цепочке.
        /// </summary>
        protected OrderHandler nextHandler;

        /// <summary>
        /// Устанавливает следующего обработчика в цепочке.
        /// </summary>
        /// <param name="next">Экземпляр обработчика.</param>
        public void SetNextHandler(OrderHandler next)
        {
            nextHandler = next;
        }

        /// <summary>
        /// Абстрактный метод обработки запроса.
        /// Должен быть реализован в конкретных обработчиках.
        /// </summary>
        /// <param name="request">Запрос на изменение заказа.</param>
        public abstract void HandleRequest(ChangeRequest request);
    }

    /// <summary>
    /// Официант – первый уровень обработки.
    /// Может обрабатывать только незначительные изменения.
    /// </summary>
    public class Waiter : OrderHandler
    {
        /// <inheritdoc />
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

    /// <summary>
    /// Шеф-повар – второй уровень обработки.
    /// Может обрабатывать запросы на изменение ингредиентов.
    /// </summary>
    public class Chef : OrderHandler
    {
        /// <inheritdoc />
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

    /// <summary>
    /// Управляющий – последний уровень обработки.
    /// Может одобрять любые изменения заказа.
    /// </summary>
    public class Manager : OrderHandler
    {
        /// <inheritdoc />
        public override void HandleRequest(ChangeRequest request)
        {
            Console.WriteLine($"Управляющий одобрил изменение: {request.Description}");
        }
    }
}
