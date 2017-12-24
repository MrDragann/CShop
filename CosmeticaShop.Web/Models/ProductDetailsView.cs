using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CosmeticaShop.IServices.Models.Product;

namespace CosmeticaShop.Web.Models
{
    public class ProductDetailsView
    {
        /// <summary>
        /// Товар
        /// </summary>
        public ProductEditModel Product { get; set; }
        /// <summary>
        /// Возможность оставление отзыва
        /// </summary>
        public bool PossibilityReview { get; set; }
    }
}