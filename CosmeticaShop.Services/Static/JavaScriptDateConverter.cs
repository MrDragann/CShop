using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.Services.Static
{
    /// <summary>
    /// Предоставляет метод Convert для преобразования между значениями DateTime C # и значениями синтаксического значения Int64 для JavaScript.
    /// </summary>
    public static class JavaScriptDateConverter
    {
        // 
        private static DateTime _jan1st1970 = new DateTime(1970, 1, 1);

        /// <summary>
        ///Преобразует DateTime в (анализируемый JavaScript) Int64.
        /// </summary>
        /// <param name="from">DateTime для преобразования</param>
        /// <returns>Целое число, представляющее количество миллисекунд с 1 января 1970 года 00:00:00 UTC.</returns>
        public static long Convert(DateTime from)
        {
            return System.Convert.ToInt64((from - _jan1st1970).TotalMilliseconds);
        }

        /// <summary>
        /// Преобразует (JavaScript-анализируемый) Int64 в DateTime.
        /// </summary>
        /// <param name="from">Целое число, представляющее количество миллисекунд с 1 января 1970 года 00:00:00 UTC.</param>
        /// <returns>Дата как DateTime</returns>
        public static DateTime Convert(long from)
        {
            return _jan1st1970.AddMilliseconds(from);
        }
    }
}
