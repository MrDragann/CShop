using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.Wish;

namespace CosmeticaShop.IServices
{
    public interface IWishService
    {
        /// <summary>
        /// Получить список желаемого
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <returns></returns>
        List<WishModel> GetWishs(Guid userId);

        /// <summary>
        /// Получить список желаемого из куки
        /// </summary>
        /// <returns></returns>
        List<WishModel> GetCookieWishs();

        /// <summary>
        /// Дополнить желаемое из куки
        /// </summary>
        /// <returns></returns>
        void ComplementWishs(Guid userId);

        /// <summary>
        /// Удалить товар из желаемого
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="productId">Ид товара</param>
        /// <returns></returns>
        BaseResponse DeleteWish(Guid? userId, int productId);
    }
}
