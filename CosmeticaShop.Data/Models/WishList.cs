using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.Data.Models
{
    public class WishList
    {
        #region

        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ид товара
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Дата добавления
        /// </summary>
        //todo:а нужна ли?
        public DateTime DateCreate { get; set; }

        #endregion

        #region [ Связанные объекты ]

        /// <summary>
        /// Связь с товаром
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Связь с пользователями
        /// </summary>
        public List<User> Users { get; set; }

        #endregion
    }
}
