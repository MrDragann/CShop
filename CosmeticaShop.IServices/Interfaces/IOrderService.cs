﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.Responses;

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
        /// <returns></returns>
        BaseResponse AddProductInCart(int productId, Guid userId, int quantity);
    }
}