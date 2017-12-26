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
        /// <summary>
        /// Получить рандомную строку
        /// </summary>
        ///  <param name="size">Размер строки</param>
        /// <returns></returns>
        public static string GetRandomString(int size)
        {
            int[] arr = new int[size]; // сделаем длину пароля в 16 символов
            Random rnd = new Random();
            string password = "";

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = rnd.Next(33, 125);
                password += (char)arr[i];
            }
            return password;
        }
    }
}
