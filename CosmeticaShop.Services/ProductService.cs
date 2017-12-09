using System;
using System.Linq;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Brand;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.Services.Static;

namespace CosmeticaShop.Services
{
    public class ProductService: IProductService
    {
        #region [ Публичная часть ]

        #region [ Товары ]



        #endregion

        #region [ Бренды ]



        #endregion

        #endregion

        #region [ Административная часть ]

        #region [ Товары ]



        #endregion

        #region [ Бренды ]

        /// <summary>
        /// Получить список брендов для постранички
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PaginationResponse<BrandModel> GetFilteredBrands(PaginationRequest<BaseFilter> request)
        {
            using (var db = new DataContext())
            {
                var query = db.Brands.AsNoTracking()
                    .OrderByDescending(x => x.Id) as IQueryable<Brand>;

                if (!string.IsNullOrEmpty(request.Filter.Term))
                    query = query.Where(x => x.Name.ToLower().Contains(request.Filter.Term.ToLower()));
                
                var model = new PaginationResponse<BrandModel> { Count = query.Count() }; 

                query = request.Load(query);

                model.Items = query.Select(x => new BrandModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    KeyUrl = x.KeyUrl,
                    IsActive = x.IsActive,
                    PhotoUrl = x.PhotoUrl
                }).ToList();
                return model;
            }
        }

        /// <summary>
        /// Добавить бренд
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse AddBrand(BrandModel model)
        {
            if (model.PhotoFile == null)
                return new BaseResponse<BrandModel>(EnumResponseStatus.ValidationError, "Изображение не выбрано", model);

            using (var db = new DataContext())
            {
                try
                {
                    var allKeyUrls = db.Brands.AsNoTracking().Select(x => x.KeyUrl).ToList();
                    var brand = new Brand
                    {
                        Name = model.Name,
                        KeyUrl = StringHelper.GetUrl(model.KeyUrl, allKeyUrls),
                        SeoKeywords = model.SeoKeywords,
                        SeoDescription = model.SeoDescription,
                        IsActive = model.IsActive
                    };
                    db.Brands.Add(brand);

                    db.SaveChanges();
                    if (model.PhotoFile != null)
                    {
                        brand.PhotoUrl = FileManager.SaveImage(model.PhotoFile, EnumDirectoryType.Brand,
                            Guid.NewGuid().ToString());
                        db.SaveChanges();
                    }

                    return new BaseResponse(EnumResponseStatus.Success);
                }
                catch (Exception ex)
                {
                    return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
                }
            }
        }

        /// <summary>
        /// Получить модель бренда
        /// </summary>
        /// <param name="brandId">Ид бренда</param>
        /// <returns></returns>
        public BaseResponse<BrandModel> GetBrandModel(int brandId)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var brand = db.Brands.AsNoTracking().Where(x => x.Id == brandId)
                        .Select(x => new BrandModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            PhotoUrl = x.PhotoUrl,
                            IsActive = x.IsActive,
                            SeoDescription = x.SeoDescription,
                            SeoKeywords = x.SeoKeywords
                        }).FirstOrDefault();
                    return new BaseResponse<BrandModel>(EnumResponseStatus.Success,brand);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<BrandModel>(EnumResponseStatus.Exception,ex.Message);
            }
        }

        /// <summary>
        /// Редактирование бренда
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<BrandModel> EditBrand(BrandModel model)
        {
            using (var db = new DataContext())
            {
                try
                {
                    var brand = db.Brands.FirstOrDefault(x => x.Id == model.Id);

                    if (brand == null)
                        return new BaseResponse<BrandModel>(EnumResponseStatus.Error, "Бренд не найден");

                    var allKeyUrls = db.Brands.AsNoTracking().Where(x => x.Id != model.Id).Select(x => x.KeyUrl).ToList();

                    brand.Name = model.Name;
                    brand.KeyUrl = StringHelper.GetUrl(model.KeyUrl, allKeyUrls);
                    brand.IsActive = model.IsActive;
                    brand.SeoKeywords = model.SeoKeywords;
                    brand.SeoDescription = model.SeoDescription;
                    
                    if (model.PhotoFile != null)
                    {
                        FileManager.DeleteFile(EnumDirectoryType.Brand, fileName: brand.PhotoUrl);
                        brand.PhotoUrl = FileManager.SaveImage(model.PhotoFile, EnumDirectoryType.Brand,
                            Guid.NewGuid().ToString());
                    }
                    db.SaveChanges();
                    return new BaseResponse<BrandModel>(EnumResponseStatus.Success, "Бренд успешно обновлен");
                }
                catch (Exception ex)
                {
                    return new BaseResponse<BrandModel>(EnumResponseStatus.Exception, ex.Message);
                }
            }
        }

        /// <summary>
        /// Удалить бренд
        /// </summary>
        /// <param name="brandId">Ид бренда</param>
        /// <returns></returns>
        public BaseResponse DeleteBrand(int brandId)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var brand = db.Brands.FirstOrDefault(x => x.Id == brandId);
                    if (brand == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Бренд не найден");

                    db.Brands.Remove(brand);
                    db.SaveChanges();
                    FileManager.DeleteFile(EnumDirectoryType.Brand, fileName: brand.PhotoUrl);
                    return new BaseResponse(EnumResponseStatus.Success, "Товар успешно удален");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }

        #endregion

        #endregion
    }
}
