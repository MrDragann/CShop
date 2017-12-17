using System;
using System.Collections.Generic;
using System.Linq;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Models;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Brand;
using CosmeticaShop.IServices.Models.Product;
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

        /// <summary>
        /// Получить товары со скидкой
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        List<ProductBaseModel> GetDiscountProducts(PaginationRequest request);

        /// <summary>
        /// Получить товар
        /// </summary>
        /// <param name="id">Ид товара</param>
        /// <returns></returns>
        ProductEditModel GetProduct(int id);

        /// <summary>
        /// Получить товары c фильтрацией
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        List<ProductBaseModel> GetProducts(ProductFilterModel request);
        #endregion

        #region [ Адмиистративная ]

        #region [ Товары ]

        /// <summary>
        /// Получить отфильтрованный список товаров
        /// </summary>
        /// <param name="request">фильтр</param>
        /// <returns></returns>
        PaginationResponse<ProductEditModel> GetFilteredProducts(PaginationRequest<BaseFilter> request);

        /// <summary>
        /// Получить модель товара для редактирования
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        BaseResponse<ProductEditModel> GetProductModel(int productId);

        /// <summary>
        /// Создать товар
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<int> AddProduct(ProductEditModel model);

        /// <summary>
        /// Редактировать товар
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<int> EditProduct(ProductEditModel model);

        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        BaseResponse DeleteProduct(int productId);

        #endregion

        #region [ Теги товаров ]

        /// <summary>
        /// Получить список все тегов товаров
        /// </summary>
        /// <returns></returns>
        List<BaseModel> GetProductTagsList();

        /// <summary>
        /// Получить список тегов товаров
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PaginationResponse<DictionaryModel> GetFilteredProductTags(PaginationRequest<BaseFilter> request);

        /// <summary>
        /// Редактирование тега товара
        /// </summary>
        /// <param name="model">модель с данными</param>
        /// <returns></returns>
        BaseResponse ProductTagEdit(DictionaryModel model);

        /// <summary>
        /// Добавление нового тега товара
        /// </summary>
        /// <param name="model">модель с данными</param>
        /// <returns></returns>
        BaseResponse ProductTagAdd(DictionaryModel model);

        /// <summary>
        /// Удаление тега товара
        /// </summary>
        /// <param name="productTagId"></param>
        /// <returns></returns>
        BaseResponse ProductTagDelete(int productTagId);

        #endregion

        #region [ Бренды ]

        /// <summary>
        /// Получить список брендов для постранички
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PaginationResponse<BrandModel> GetFilteredBrands(PaginationRequest<BaseFilter> request);

        /// <summary>
        /// Получить список всех брендов с базовой инфой
        /// </summary>
        /// <returns></returns>
        List<BaseModel> GetAllBrandsBase();

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
