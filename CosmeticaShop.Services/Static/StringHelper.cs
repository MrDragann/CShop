using System.Collections.Generic;

namespace CosmeticaShop.Services.Static
{
    public static class StringHelper
    {
        /// <summary>
        /// Получить уникальную строку  для Url
        /// </summary>
        /// <param name="keyUrl">исходная строка</param>
        /// <param name="allString">список занятых строк</param>
        /// <returns></returns>
        public static string GetUrl(string keyUrl, List<string> allString)
        {
            if (!allString.Contains(keyUrl))
                return keyUrl;
            for (var i = 1; ; i++)
            {
                var newStr = keyUrl + "(" + i + ")";
                if (!allString.Contains(newStr))
                    return newStr;
            }
        }
    }
}
