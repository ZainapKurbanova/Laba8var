using Laba8var.Models;

namespace Laba8var.TemplateMethod
{
    /// <summary>
    /// Абстрактный шаблонный метод приготовления.
    /// </summary>
    public abstract class CookingProcess : Preparable
    {
        /// <summary>Шаблонный метод, определяющий общий процесс приготовления.</summary>
        public void Prepare()
        {
            PrepareIngredients();
            Cook();
            Serve();
        }

        /// <summary>Подготовка ингредиентов.</summary>
        protected abstract void PrepareIngredients();

        /// <summary>Основной процесс приготовления.</summary>
        protected abstract void Cook();

        /// <summary>Подача блюда.</summary>
        protected abstract void Serve();
    }

    /// <summary>Процесс приготовления блюд.</summary>
    public class DishCookingProcess : CookingProcess
    {
        private readonly Dish _dish;

        public DishCookingProcess(Dish dish) => _dish = dish;

        protected override void PrepareIngredients() =>
            Console.WriteLine($"Подготавливаем ингредиенты для {_dish.Name}: {string.Join(", ", _dish.Ingredients)}");

        protected override void Cook() =>
            Console.WriteLine($"Готовим {_dish.Name} по специальному рецепту");

        protected override void Serve() =>
            Console.WriteLine($"Подаем {_dish.Name} с гарниром и соусом");
    }

    /// <summary>Процесс приготовления напитков.</summary>
    public class DrinkCookingProcess : CookingProcess
    {
        private readonly Drink _drink;

        public DrinkCookingProcess(Drink drink) => _drink = drink;

        protected override void PrepareIngredients() =>
            Console.WriteLine($"Подготавливаем ингредиенты для {_drink.Name}");

        protected override void Cook() =>
            Console.WriteLine($"Смешиваем и готовим {_drink.Name} объемом {_drink.Volume} мл");

        protected override void Serve() =>
            Console.WriteLine($"Подаем {_drink.Name} в охлажденном стакане");
    }

    /// <summary>Процесс приготовления десертов.</summary>
    public class DessertCookingProcess : CookingProcess
    {
        private readonly Dessert _dessert;

        public DessertCookingProcess(Dessert dessert) => _dessert = dessert;

        protected override void PrepareIngredients() =>
            Console.WriteLine($"Подготавливаем ингредиенты для {_dessert.Name} ({_dessert.Calories} ккал)");

        protected override void Cook() =>
            Console.WriteLine($"Выпекаем {_dessert.Name} при определенной температуре");

        protected override void Serve() =>
            Console.WriteLine($"Украшаем и подаем {_dessert.Name} с сиропом или ягодами");
    }
}