using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace Laba8var.Services
{
    /// <summary>
    /// Конфигурация системы логирования с Serilog.
    /// Логи пишутся в консоль и файл.
    /// </summary>
    public static class LoggerConfig
    {
        private static ILoggerFactory _factory;

        /// <summary>
        /// Настройка фабрики логгеров.
        /// </summary>
        /// <returns>ILoggerFactory</returns>
        public static ILoggerFactory ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
                .CreateLogger();

            _factory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            return _factory;
        }

        /// <summary>
        /// Создает логгер для указанного типа.
        /// </summary>
        /// <typeparam name="T">Тип класса для логгера</typeparam>
        /// <returns>ILogger<T></returns>
        public static ILogger<T> CreateLogger<T>()
        {
            if (_factory == null)
                ConfigureLogger();

            return _factory.CreateLogger<T>();
        }
    }
}
