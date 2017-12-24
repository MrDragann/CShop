using System;
using System.Collections.Generic;
using System.Linq;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Models;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Brand;
using CosmeticaShop.IServices.Models.Coupon;
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
        /// Добавить товар в желаемое куки
        /// </summary>
        /// <param name="productId">Ид товара</param>     
        /// <returns></returns>
        BaseResponse AddProductInCoockieWish(int productId);

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

        /// <summary>
        /// Получить самые продоваемые товары
        /// </summary>
        /// <returns></returns>
        List<ProductBaseModel> GetBestSellers();



        /// <summary>
        /// Получить брэнды для главной страницы
        /// </summary>
        /// <returns></returns>
        List<BrandModel> GetBrands();

        /// <summary>
        /// Добавить отзыв
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="productId">Ид товара</param>
        /// <param name="message">Сообщение отзыва</param>
        /// <returns></returns>
        BaseResponse AddReview(Guid userId, int productId, string message);

        /// <summary>
        /// Проверить возможность оставление отзыва
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="productId">Ид товара</param>
        /// <returns></returns>
        BaseResponse ValidationReview(Guid userId, int productId);

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

        #region [ Купоны ]

        /// <summary>
        /// Получить список купонов
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PaginationResponse<CouponModel> GetFilteredCoupons(PaginationRequest<BaseFilter> request);

        /// <summary>
        /// Получить модель купона
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<CouponModel> GetCouponModel(int id);

        /// <summary>
        /// Редактирование купона
        /// </summary>
        /// <param name="model">модель с данными</param>
        /// <returns></returns>
        BaseResponse<int> CouponEdit(CouponModel model);

        /// <summary>
        /// Добавление нового купона
        /// </summary>
        /// <param name="model">модель с данными</param>
        /// <returns></returns>
        BaseResponse<int> CouponAdd(CouponModel model);

        /// <summary>
        /// Удаление купона
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        BaseResponse CouponDelete(int couponId);

        #endregion

        #endregion
    }
}
