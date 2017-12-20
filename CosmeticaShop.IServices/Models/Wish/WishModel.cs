using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.Product;

namespace CosmeticaShop.IServices.Models.Wish
{
    public  class WishModel
    {
        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ид пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Ид товара
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Дата добавления
        /// </summary>
        //todo:а нужна ли?
        public DateTime DateCreate { get; set; }
        /// <summary>
        /// Модель товара
        /// </summary>
        public ProductBaseModel Product { get; set; }
    }
}
