using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.Product;

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

        /// <summary>
        /// Получить рандомную элементы массива
        /// </summary>
        ///  <param name="products">Товары</param>
        /// <param name="take">Размер рандомного массива</param>
        /// <returns></returns>
        public static List<ProductBaseModel> GetRandomProducts(List<ProductBaseModel> products,int take)
        {
            List<ProductBaseModel> productsRandom = new List<ProductBaseModel>();
            int k;
            Random rand = new Random();
            var count = take;
            if (products.Count < take)
                count = products.Count;
            for (int i = 0; i < count; i++)
            {
                while (true)
                {
                    k = rand.Next(products.Count);
                    if (productsRandom.All(x => products[k].Id != x.Id))
                    {
                        productsRandom.Add(products[k]);
                        break;
                    }
                }
            }
            return productsRandom;
        }
    }
}
