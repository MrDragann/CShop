using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.Order;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.User;

namespace CosmeticaShop.IServices.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// Добавить товар в корзину
        /// </summary>
        /// <param name="productId">Ид товара</param>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="quantity">Количество</param>
        /// <param name="isCookie">Добавление в корзину через куки</param>
        /// <returns></returns>
        BaseResponse AddProductInCart(int productId, Guid userId, int quantity, bool isCookie);

        /// <summary>
        /// Добавить товар в корзину куки
        /// </summary>
        /// <param name="productId">Ид товара</param>  
        /// <param name="quantity">Количество товара</param>
        /// <param name="isAuth">Добавляет авторизованый пользователь?</param>
        /// <returns></returns>
        BaseResponse AddProductInCoockieCart(int productId, int quantity, bool isAuth);

        /// <summary>
        /// Добавить товар в корзину
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="productsOrder">Товары для заказа</param>
        /// <returns></returns>
        BaseResponse<int> PreparationOrder(Guid userId, List<OrderProductsModel> productsOrder);
        /// <summary>
        /// Получить историю заказов
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        List<OrderHeaderModel> GetHistoryOrders(Guid userId);

        /// <summary>
        /// Получить историю заказов
        /// </summary>
        /// <param name="orderId">Ид заказа</param>
        OrderHeaderModel GetOrder(int orderId);

        /// <summary>
        /// Добавить заказ
        /// </summary>
        /// <param name="orderId">Ид заказа</param>
        /// <param name="address">Адрес доставки</param>
        BaseResponse AddOrder(int orderId, AddressModel address);
    }
}
