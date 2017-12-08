using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.IServices.Enums
{
    /// <summary>
    /// Список статусов ответа
    /// </summary>
    public enum EnumResponseStatus
    {
        /// <summary>
        /// Успешно
        /// </summary>
        Success = 0,
        /// <summary>
        /// Ошибка валидации
        /// </summary>
        ValidationError = 1,
        /// <summary>
        /// Ошибка
        /// </summary>
        Error = 2,
        /// <summary>
        /// Исключение
        /// </summary>
        Exception = 3
    }
}
