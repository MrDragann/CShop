using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.Order;

namespace CosmeticaShop.IServices.Interfaces
{
    public interface ICartService
    {
        /// <summary>
        /// Получить корзину пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<OrderProductsModel> GetCart(Guid userId);

    }
}
