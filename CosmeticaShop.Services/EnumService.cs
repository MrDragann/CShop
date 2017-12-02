using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models;

namespace CosmeticaShop.Services
{
    /// <summary>
    /// Сервис для работы с Enum-мами
    /// </summary>
    public static class EnumService
    {
        /// <summary>
        /// Получить список значений Enum-а ({Id},{Name})
        /// </summary>
        /// <typeparam name="TEnum">Тип Enum-а</typeparam>
        /// <param name="isDefaultName">Использвать наименование Enam-а иначе значение с атрибута Description</param>
        /// <returns></returns>
        public static List<DictionaryModel> GetEnumList<TEnum>(bool isDefaultName = false)
        {
            var list = new List<DictionaryModel>();

            foreach (Enum type in Enum.GetValues(typeof(TEnum)))
            {
                if (Convert.ToInt32(type) != 0)
                    list.Add(new DictionaryModel(Convert.ToInt32(type), isDefaultName ? type.ToString() : GetEnumDescription(type)));
            }
            return list;
        }

        /// <summary>
        /// Получить значение с атрибута Description Enum-a
        /// </summary>
        /// <typeparam name="TEnum">Тип Enum-а</typeparam>
        /// <param name="value">Enum</param>
        /// <returns></returns>
        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            try
            {
                if (value != null)
                {
                    var attribute = value.GetType()
                        .GetField(value.ToString())
                        .GetCustomAttributes(false)
                        .First() as DescriptionAttribute;

                    if (attribute != null) return attribute.Description;
                }
                return "Unknown";
            }
            catch (Exception ex)
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Получить список флагов Enum'а
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetFlags<T>(this T input)
        {
            foreach (Enum value in Enum.GetValues(input.GetType()))
            {
                if ((int)(object)value != 0)
                {
                    if (((Enum)(object)input).HasFlag(value))
                        yield return (T)(object)value;
                }
            }
        }
    }
}
