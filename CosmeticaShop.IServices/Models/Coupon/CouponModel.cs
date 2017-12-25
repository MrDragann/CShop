using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.IServices.Models.Coupon
{
    public class CouponModel
    {
        #region [ Свойства ]

        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Код купона
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Скидка в %
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Дата создания 
        /// </summary>
        public DateTime DateCreate { get; set; }

        #endregion
    }
}
