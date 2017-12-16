using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Brand;
using CosmeticaShop.IServices.Models.Product;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.Services.Static;

namespace CosmeticaShop.Services
{
    public class ProductService: IProductService
    {
        #region [ Публичная часть ]

        #region [ Товары ]
        /// <summary>
        /// Добавить товар в желаемое
        /// </summary>
        /// <param name="productId">Ид товара</param>
        /// <param name="userId">Ид пользователя</param>
        /// <returns></returns>
        public BaseResponse AddProductInWish(int productId, Guid userId)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var product = db.Products.FirstOrDefault(x => x.Id == productId);
                    if (product == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Товар не был найден");
                    var user = db.Users.FirstOrDefault(x => x.Id == userId);
                    if (user == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Пользователь не был найден");
                    db.WishLists.Add(new WishList()
                    {
                        UserId = user.Id,
                        ProductId = product.Id,
                        DateCreate = DateTime.Now
                    });
                    db.SaveChanges();
                    return new BaseResponse(EnumResponseStatus.Success,"Товар успешно добавлен в желаемое");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }

        //todo:небольшой пример0))0
        /// <summary>
        /// Получить товары со скидкой
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<ProductBaseModel> GetDiscountProducts(PaginationRequest request)
        {
            using (var db = new DataContext())
            {
                //todo:?наверное лучше сделать decimal?
                var query = db.Products.Include(x => x.Brand).Where(x => x.Discount > 0) as IQueryable<Product>;
                request.Load(query);

                var products = query.Select(ConvertToProductBaseModel).ToList();
                products.ForEach(x =>
                {
                    x.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, x.Id.ToString());
                });
                return products;
            }
        }

        #region [ Конвертирование ]

        private ProductBaseModel ConvertToProductBaseModel(Product m)
        {
            return new ProductBaseModel
            {
                Id = m.Id,
                Name = m.Name,
                BrandName = m.Brand?.Name,
                Price = m.Price,
                DiscountPercent = m.Discount,
                //todo:вынести в функцию
                DiscountPrice = Math.Floor(m.Price-(m.Price*m.Discount/100))
            };
        }

        #endregion

        #endregion

        #region [ Бренды ]



        #endregion

        #endregion

        #region [ Административная часть ]

        #region [ Товары ]

        /// <summary>
        /// Получить отфильтрованный список товаров
        /// </summary>
        /// <param name="request">фильтр</param>
        /// <returns></returns>
        public PaginationResponse<ProductEditModel> GetFilteredProducts(PaginationRequest<BaseFilter> request)
        {
            using (var db = new DataContext())
            {
                var query = db.Products.AsNoTracking()
                    .OrderByDescending(x => x.DateCreate) as IQueryable<Product>;

                //if (request.CategoryId.HasValue)
                //    query = query.Where(x => x.CategoryId == request.CategoryId.Value);
                if (!string.IsNullOrWhiteSpace(request.Filter.Term))
                    query = query.Where(x => x.Name.ToLower().Contains(request.Filter.Term.ToLower()));
                var model = new PaginationResponse<ProductEditModel> { Count = query.Count() };
                if (request.Skip.HasValue)
                    query = query.Skip(request.Skip.Value);
                if (request.Take.HasValue)
                    query = query.Take(request.Take.Value);
                model.Items = query.Select(x => new ProductEditModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    DateCreate = x.DateCreate,
                    KeyUrl = x.KeyUrl,
                    IsActive = x.IsActive
                }).ToList();
                model.Items.ForEach(x =>
                {
                    x.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, x.Id.ToString());
                });
                return model;
            }
        }
        /// <summary>
        /// Получить модель товара для редактирования
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public BaseResponse<ProductEditModel> GetProductModel(int productId)
        {
            using (var db = new DataContext())
            {
                var product = db.Products.Include(x=>x.Categories).Include(x=>x.ProductTags).AsNoTracking().Where(x => x.Id == productId)
                    .Select(x => new ProductEditModel
                    {
                        Id = x.Id,
                        BrandId = x.BrandId,
                        Name = x.Name,
                        Description = x.Description,
                        SeoDescription = x.SeoDescription,
                        SeoKeywords = x.SeoKeywords,
                        Price = x.Price,
                        Discount = x.Discount,
                        IsInStock = x.IsInStock,
                        IsActive = x.IsActive,
                        CategoriesId = x.Categories.Select(c=>c.Id).ToList(),
                        TagsId = x.ProductTags.Select(t=>t.Id).ToList()
                    }).FirstOrDefault();
                if (product == null)
                    return new BaseResponse<ProductEditModel>(EnumResponseStatus.Error, "Товар не найден", new ProductEditModel());
                product.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, productId.ToString());
                return new BaseResponse<ProductEditModel>(EnumResponseStatus.Success, product);
            }
        }
        /// <summary>
        /// Создать товар
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<int> AddProduct(ProductEditModel model)
        {
            try
            {
                //if (model.PhotoFile == null)
                //    return new BaseResponse<int>(EnumResponseStatus.ValidationError, "Изображение не выбрано");

                using (var db = new DataContext())
                {
                    var allKeyUrls = db.Products.AsNoTracking().Select(x => x.KeyUrl).ToList();
                    var newProduct = new Product
                    {
                        Name = model.Name,
                        BrandId = model.BrandId,
                        Description = model.Description,
                        DateCreate = DateTime.Now,
                        IsInStock = model.IsInStock,
                        IsActive = model.IsActive,
                        SeoDescription = model.SeoDescription,
                        SeoKeywords = model.SeoKeywords,
                        KeyUrl = StringHelper.GetUrl(model.KeyUrl, allKeyUrls),
                        Price = model.Price,
                        Discount = model.Discount
                    };
                    if(model.CategoriesId!=null)
                        newProduct.Categories = db.Categories.Where(x => model.CategoriesId.Contains(x.Id)).ToList();
                    if (model.TagsId != null)
                        newProduct.ProductTags = db.ProductTags.Where(x => model.TagsId.Contains(x.Id)).ToList();
                    db.Products.Add(newProduct);

                    db.SaveChanges();
                    //newProduct.PhotoUrl = FileManager.SaveImage(model.PhotoFile, EnumDirectoryType.Product,
                    //    Guid.NewGuid().ToString(), newProduct.Id.ToString());
                    db.SaveChanges();
                    return new BaseResponse<int>(EnumResponseStatus.Success, "Товар успешно сохранен", newProduct.Id);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>(EnumResponseStatus.Exception, ex.Message);
            }
        }
        /// <summary>
        /// Редактировать товар
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<int> EditProduct(ProductEditModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var old = db.Products.Include(x=>x.Categories).Include(x=>x.ProductTags)
                        .FirstOrDefault(x => x.Id == model.Id);
                    if (old == null)
                        return new BaseResponse<int>(EnumResponseStatus.Error, "Товар не найден");

                    var allKeyUrls = db.Products.Where(x => x.Id != model.Id).Select(x => x.KeyUrl).ToList();

                    old.Name = model.Name;
                    old.BrandId = model.BrandId;
                    old.SeoDescription = model.SeoDescription;
                    old.SeoKeywords = model.SeoKeywords;
                    old.IsInStock = model.IsInStock;
                    old.IsActive = model.IsActive;
                    old.KeyUrl = StringHelper.GetUrl(model.KeyUrl, allKeyUrls);
                    old.Description = model.Description;
                    old.Price = model.Price;
                    old.Discount = model.Discount;
                    old.Categories = model.CategoriesId != null
                        ? db.Categories.Where(x => model.CategoriesId.Contains(x.Id)).ToList()
                        : null;
                    old.ProductTags = model.TagsId != null
                        ? db.ProductTags.Where(x => model.TagsId.Contains(x.Id)).ToList()
                        : null;

                    if (model.PhotoFile != null)
                    {
                        FileManager.DeleteFile(EnumDirectoryType.Product, old.Id.ToString(), old.PhotoUrl);
                        old.PhotoUrl = FileManager.SaveImage(model.PhotoFile, EnumDirectoryType.Product,
                            Guid.NewGuid().ToString(), old.Id.ToString());
                    }
                    db.SaveChanges();
                    return new BaseResponse<int>(EnumResponseStatus.Success, "Товар успешно изменен",old.Id);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>(EnumResponseStatus.Exception, ex.Message);
            }
        }
        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public BaseResponse DeleteProduct(int productId)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var product = db.Products.FirstOrDefault(x => x.Id == productId);
                    if (product == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Товар не найден");
                    db.Products.Remove(product);
                    db.SaveChanges();
                    FileManager.DeleteDirectory(EnumDirectoryType.Product, product.Id.ToString());
                    return new BaseResponse(EnumResponseStatus.Success, "Товар успешно удален");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }

        #endregion

        #region [ Теги товаров ]

        /// <summary>
        /// Получить список все тегов товаров
        /// </summary>
        /// <returns></returns>
        public List<BaseModel> GetProductTagsList()
        {
            using (var db = new DataContext())
            {
                var productTags = db.ProductTags.AsNoTracking().Select(x => new BaseModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
                return productTags;
            }
        }

        /// <summary>
        /// Получить список тегов товаров
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PaginationResponse<DictionaryModel> GetFilteredProductTags(PaginationRequest<BaseFilter> request)
        {
            using (var db = new DataContext())
            {
                var query = db.ProductTags.AsNoTracking()
                    .OrderByDescending(x => x.Id) as IQueryable<ProductTag>;

                if (!string.IsNullOrEmpty(request.Filter.Term))
                    query = query.Where(x => x.Name.ToLower().Contains(request.Filter.Term.ToLower()));

                var model = new PaginationResponse<DictionaryModel> { Count = query.Count() };

                query = request.Load(query);

                model.Items = query.Select(x => new DictionaryModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
                return model;
            }
        }

        /// <summary>
        /// Редактирование тега товара
        /// </summary>
        /// <param name="model">модель с данными</param>
        /// <returns></returns>
        public BaseResponse ProductTagEdit(DictionaryModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    if(db.ProductTags.AsNoTracking().Any(x=>x.Name==model.Name && x.Id!=model.Id))
                        return new BaseResponse(EnumResponseStatus.ValidationError, "Тег с таким наименованием уже существует");
                    var productTag = db.ProductTags.FirstOrDefault(x => x.Id == model.Id);
                    if (productTag == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Тег товара не найден");
                    productTag.Name = model.Name;
                    db.SaveChanges();
                    return new BaseResponse(EnumResponseStatus.Success, "Тег товара успешно изменен");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Добавление нового тега товара
        /// </summary>
        /// <param name="model">модель с данными</param>
        /// <returns></returns>
        public BaseResponse ProductTagAdd(DictionaryModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    if (db.ProductTags.AsNoTracking().Any(x => x.Name == model.Name))
                        return new BaseResponse(EnumResponseStatus.ValidationError, "Тег с таким наименованием уже существует");
                    var productTag = new ProductTag
                    {
                        Name = model.Name
                    };
                    db.ProductTags.Add(productTag);
                    db.SaveChanges();
                    return new BaseResponse(EnumResponseStatus.Success, "Тег товара успешно добавлен");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Удаление тега товара
        /// </summary>
        /// <param name="productTagId"></param>
        /// <returns></returns>
        public BaseResponse ProductTagDelete(int productTagId)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var productTag = db.ProductTags.FirstOrDefault(x => x.Id == productTagId);
                    if (productTag == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Тег товара не найден");
                    db.ProductTags.Remove(productTag);
                    db.SaveChanges();
                    return new BaseResponse(0, "Тег товара успешно удален");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }

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
        /// Получить список всех брендов с базовой инфой
        /// </summary>
        /// <returns></returns>
        public List<BaseModel> GetAllBrandsBase()
        {
            using (var db = new DataContext())
            {
                var brands = db.Brands.AsNoTracking().Select(x => new BaseModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
                return brands;
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
