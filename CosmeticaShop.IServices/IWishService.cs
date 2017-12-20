using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
