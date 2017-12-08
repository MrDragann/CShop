using System;
using System.Linq;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Brand;
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

        #endregion

        #endregion
    }
}
