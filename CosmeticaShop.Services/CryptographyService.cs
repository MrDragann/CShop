using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.Services
{
    public static class CryptographyService
    {
        /// <summary>
        /// Хеширование строки по стандарту SHA256
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetHashString(this string s)
        {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(s), 0, Encoding.UTF8.GetByteCount(s));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
