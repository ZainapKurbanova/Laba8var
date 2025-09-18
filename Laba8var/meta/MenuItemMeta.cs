using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Laba8var.Models;

namespace Laba8var.Meta
{
    public static class MenuItemMeta
    {
        private static readonly Dictionary<string, Type> registry;

        static MenuItemMeta()
        {
            registry = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(MenuItem).IsAssignableFrom(t) && !t.IsAbstract)
                .ToDictionary(t => t.Name.ToLower(), t => t);
        }

        public static MenuItem Create(string typeName, params object[] args)
        {
            var key = typeName.ToLower();
            if (!registry.TryGetValue(key, out var type))
                throw new ArgumentException($"MenuItem type '{typeName}' is not registered.");

            return (MenuItem)Activator.CreateInstance(type, args);
        }

        public static IEnumerable<string> RegisteredTypes => registry.Keys;
    }
}
