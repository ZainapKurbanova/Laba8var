using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba8var.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий объекты, которые можно приготовить.
    /// </summary>
    public interface Preparable
    {
        /// <summary>
        /// Описывает процесс приготовления объекта.
        /// </summary>
        void Prepare();
    }
}
