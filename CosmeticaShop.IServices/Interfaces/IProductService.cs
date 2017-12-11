using System;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Brand;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.IServices.Models.Responses;

namespace CosmeticaShop.IServices.Interfaces
{
    public interface IProductService
    {
        #region [ Публичная ]

        /// <summary>
        /// Добавить товар в желаемое
        /// </summary>
        /// <param name="productId">Ид товара</param>
        /// <param name="userId">Ид пользователя</param>
        /// <returns></returns>
        BaseResponse AddProductInWish(int productId, Guid userId);

        #endregion

        #region [ Адмиистративная ]

        #region [ Бренды ]

        /// <summary>
        /// Получить список брендов для постранички
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PaginationResponse<BrandModel> GetFilteredBrands(PaginationRequest<BaseFilter> request);

        /// <summary>
        /// Добавить бренд
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse AddBrand(BrandModel model);

        /// <summary>
        /// Получить модель бренда
        /// </summary>
        /// <param name="brandId">Ид бренда</param>
        /// <returns></returns>
        BaseResponse<BrandModel> GetBrandModel(int brandId);

        /// <summary>
        /// Редактирование бренда
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<BrandModel> EditBrand(BrandModel model);

        /// <summary>
        /// Удалить бренд
        /// </summary>
        /// <param name="brandId">Ид бренда</param>
        /// <returns></returns>
        BaseResponse DeleteBrand(int brandId);

        #endregion

        #endregion
    }
}
