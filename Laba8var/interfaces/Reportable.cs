using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.Interfaces
{
    /// <summary>
    /// Интерфейс для объектов, которые могут генерировать отчет.
    /// </summary>
    internal interface Reportable
    {
        /// <summary>
        /// Генерирует отчет для объекта и возвращает его в виде строки.
        /// </summary>
        /// <returns>Строковое представление отчета.</returns>
        string GenerateReport();
    }
}
