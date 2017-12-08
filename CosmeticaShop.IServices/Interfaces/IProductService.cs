using CosmeticaShop.IServices.Models.Brand;
using CosmeticaShop.IServices.Models.Responses;

namespace CosmeticaShop.IServices.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Добавить бренд
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse AddBrand(BrandModel model);
    }
}
