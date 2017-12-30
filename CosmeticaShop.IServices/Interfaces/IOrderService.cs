using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Order;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.User;
using OrderAdmin = CosmeticaShop.IServices.Models.Order.Admin;

namespace CosmeticaShop.IServices.Interfaces
{
    public interface IOrderService
    {
        #region [ Публичная ]

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
        /// <param name="couponCode">Купон</param>
        /// <returns></returns>
        BaseResponse<int> PreparationOrder(Guid userId, List<OrderProductsModel> productsOrder, string couponCode);

        /// <summary>
        /// Подготовка заказа (Принятия купона)
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="productsOrder">Товары для заказа</param>
        /// <param name="couponCode">Купон</param>
        /// <returns></returns>
        BaseResponse<decimal> AcceptCoupon(Guid userId, List<OrderProductsModel> productsOrder, string couponCode);
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
        BaseResponse<AddOrderResponse> AddOrder(int orderId, AddressModel address, string email, Guid? userId);

        #endregion

        #region [ Административная ]

        /// <summary>
        /// Получить список заказов
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PaginationResponse<OrderAdmin.OrderHeaderModel> GetFilteredOrders(PaginationRequest<BaseFilter> request);

        /// <summary>
        /// Получить модель заказа
        /// </summary>
        /// <param name="orderId">Ид заказа</param>
        /// <returns></returns>
        BaseResponse<OrderAdmin.OrderHeaderModel> GetOrderHeaderModel(int orderId);

        /// <summary>
        /// Изменить статус заказу
        /// </summary>
        /// <param name="orderId">Ид заказа</param>
        /// <param name="status">Статус</param>
        /// <returns></returns>
        BaseResponse ChangeOrderStatus(int orderId, int status);

        #endregion
    }
}
