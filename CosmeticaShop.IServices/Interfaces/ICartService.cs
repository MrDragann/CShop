using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.Order;
using CosmeticaShop.IServices.Models.Responses;

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

        /// <summary>
        /// Получение данных о корзине из куки
        /// </summary>
        /// <returns>ModelCarts.</returns>
        List<OrderProductsModel> GetCookieCart();

        /// <summary>
        /// Удалить товар из корзины
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="productId">Ид товара</param>
        /// <returns></returns>
        BaseResponse DeleteProduct(Guid userId, int productId);

    }
}
