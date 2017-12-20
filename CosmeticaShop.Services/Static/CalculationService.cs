using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.Services.Static
{
    public static class CalculationService
    {
        /// <summary>
        /// Получить скидку товара
        /// </summary>
        /// <param name="price">Цена</param>
        /// <param name="discount">Скидка в процентах</param>
        /// <returns></returns>
        public static decimal GetDiscountPrice(decimal price, decimal discount)
        {
            return Math.Floor(price - price * discount / 100);
        }
    }
}
