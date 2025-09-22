using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Laba8var.Models;

namespace Laba8var.Meta
{
    /// <summary>
    /// Статический класс для работы с метаданными MenuItem.
    /// Позволяет регистрировать и создавать экземпляры всех подклассов <see cref="MenuItem"/> динамически.
    /// </summary>
    public static class MenuItemMeta
    {
        /// <summary>
        /// Словарь, сопоставляющий имя типа MenuItem с его типом.
        /// </summary>
        private static readonly Dictionary<string, Type> registry;

        /// <summary>
        /// Статический конструктор. Инициализирует реестр всех доступных наследников <see cref="MenuItem"/>.
        /// </summary>
        static MenuItemMeta()
        {
            registry = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(MenuItem).IsAssignableFrom(t) && !t.IsAbstract)
                .ToDictionary(t => t.Name.ToLower(), t => t);
        }

        /// <summary>
        /// Создает экземпляр <see cref="MenuItem"/> по имени типа.
        /// </summary>
        /// <param name="typeName">Имя типа MenuItem (регистр не важен).</param>
        /// <param name="args">Аргументы конструктора для создаваемого MenuItem.</param>
        /// <returns>Новый экземпляр соответствующего <see cref="MenuItem"/>.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если тип не зарегистрирован.</exception>
        public static MenuItem Create(string typeName, params object[] args)
        {
            var key = typeName.ToLower();
            if (!registry.TryGetValue(key, out var type))
                throw new ArgumentException($"MenuItem type '{typeName}' is not registered.");

            return (MenuItem)Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// Получает имена всех зарегистрированных типов <see cref="MenuItem"/>.
        /// </summary>
        public static IEnumerable<string> RegisteredTypes => registry.Keys;
    }
}
