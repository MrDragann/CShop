using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.Data.Models
{
    public class UserAddress
    {
        #region [ Свойства ]

        /// <summary>
        /// Ид пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Страна
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Город
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }

        #endregion

        #region [ Связанные объекты ]

        /// <summary>
        /// Связь с пользователем
        /// </summary>
        public User User { get; set; }

        #endregion
    }
}
