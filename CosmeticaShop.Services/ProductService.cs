using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Brand;
using CosmeticaShop.IServices.Models.Category;
using CosmeticaShop.IServices.Models.Coupon;
using CosmeticaShop.IServices.Models.Product;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.User;
using CosmeticaShop.Services.Static;

namespace CosmeticaShop.Services
{
    public class ProductService : IProductService
    {
        private ICategoryService _categoryService = new CategoryService();

        #region [ Публичная часть ]

        #region [ Товары ]


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
                var query = db.Products.Include(x => x.Brand)
                    .Where(x => !x.IsDelete.HasValue && x.IsActive && x.Discount > 0) as IQueryable<Product>;
                request.Load(query);

                var products = query.Select(ConvertToProductBaseModel).ToList();
                products.ForEach(x =>
                {
                    x.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, x.Id.ToString());
                });
                return products;
            }
        }
        /// <summary>
        /// Получить товары c фильтрацией
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<ProductBaseModel> GetProducts(ProductFilterModel request)
        {
            using (var db = new DataContext())
            {
                var query = db.Products.AsNoTracking()
                    .Include(x => x.Brand).Include(x => x.Categories).Include(x => x.ProductTags)
                    .Where(x => !x.IsDelete.HasValue && x.IsActive).ToList();
                if (request?.BrandiesId?.Count > 0)
                {
                    query = query.Where(x => request.BrandiesId.Any(m => m == x.BrandId)).ToList();
                }
                if (request?.CategoriesId?.Count > 0)
                {
                    var allcategories = _categoryService.GetAllCategories(parentsId: request.CategoriesId);
                    var childsId = request.CategoriesId.Select(x => x).ToList();
                    GetChildCategories(allcategories, childsId);
                    childsId = childsId.Distinct().ToList();
                    query = query.Where(x => childsId.Any(m => x.Categories.Any(c => c.Id == m))).ToList();
                }
                if (!string.IsNullOrEmpty(request?.Search))
                {
                    query = query.Where(x => x.Name.ToLower().Contains(request.Search.ToLower())).ToList();
                }
                if (request?.TagsId?.Count > 0)
                {
                    query = query.Where(x => request.TagsId.Any(m => x.ProductTags.Any(t => t.Id == m))).ToList();
                }
                if (request != null && request.Discount)
                {
                    query = query.Where(x => x.Discount > 0).OrderByDescending(x => x.Discount).ToList();
                }
                var products = query.Select(ConvertToProductBaseModel).ToList();
                products.ForEach(x =>
                {
                    x.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, x.Id.ToString());
                });
                return products;
            }
        }

        /// <summary>
        /// Получить дочерние категории
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="childList"></param>
        private void GetChildCategories(List<CategoryModel> categories, List<int> childList)
        {
            foreach (var item in categories)
            {
                childList.Add(item.Id);
                if (item.ChildCategories != null)
                {
                    GetChildCategories(item.ChildCategories, childList);
                }
            }
        }
        /// <summary>
        /// Получить рекомендованные товары
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        public List<ProductBaseModel> GetRecomendProducts(int take)
        {
            using (var db = new DataContext())
            {
                var query = db.Products.Include(x => x.Brand).Include(x => x.Categories)
                    .Where(x => !x.IsDelete.HasValue && x.IsActive && x.IsRecommended).ToList();
                var products = query.Select(ConvertToProductBaseModel).ToList();
                var productsRandom = CalculationService.GetRandomProducts(products, take);
                productsRandom.ForEach(x =>
                {
                    x.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, x.Id.ToString());
                });
                return productsRandom;
            }
        }
        /// <summary>
        /// Получить товары со скидкой
        /// </summary>
        /// <returns></returns>
        public List<ProductBaseModel> GetRandomDiscountProducts()
        {
            using (var db = new DataContext())
            {
                var query = db.Products.Include(x => x.Brand).Include(x => x.Categories)
                    .Where(x => !x.IsDelete.HasValue && x.IsActive && x.Discount > 0).ToList();
                var products = query.Select(ConvertToProductBaseModel).ToList();
                var productsRandom = CalculationService.GetRandomProducts(products, 4);
                productsRandom.ForEach(x =>
                {
                    x.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, x.Id.ToString());
                });
                return productsRandom;
            }
        }
        /// <summary>
        /// Получить похожие товары товары
        /// </summary>
        /// <param name="productId">Ид товара</param>
        /// <returns></returns>
        public List<ProductBaseModel> GetSimilarProducts(int productId)
        {
            using (var db = new DataContext())
            {
                var allProducts = db.Products.AsNoTracking().Include(x => x.Brand).Include(x => x.Categories).Include(x => x.SimilarProducts)
                    .Where(x => !x.IsDelete.HasValue && x.IsActive).ToList();
                var product = allProducts.FirstOrDefault(x => x.Id == productId);
                if (product == null)
                    return new List<ProductBaseModel>();
                var similarProducts = product.SimilarProducts;
                var products = similarProducts.Select(ConvertToProductBaseModel).ToList();
                var productsRandom = CalculationService.GetRandomProducts(products, 4);
                if (productsRandom.Count < 4)
                {
                    var categoriesId = product.Categories.Select(x => x.Id);
                    var similarCategories = allProducts.Where(x => categoriesId.Any(m => x.Categories.Any(c => c.Id == m))).ToList();
                    var similarCategoriesRandom = CalculationService.GetRandomProducts(similarCategories.Select(ConvertToProductBaseModel).ToList(), 4 - productsRandom.Count);
                    productsRandom.AddRange(similarCategoriesRandom);
                    // если похожих все еще меньше 4, то борем любой случайный товар
                    if (productsRandom.Count < 4)
                    {
                        var randomProducts = CalculationService.GetRandomProducts(allProducts.Select(ConvertToProductBaseModel).ToList(), 4 - productsRandom.Count);
                        productsRandom.AddRange(randomProducts);
                    }
                }
                productsRandom.ForEach(x =>
                {
                    x.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, x.Id.ToString());
                });
                return productsRandom;
            }
        }
        /// <summary>
        /// Получить самые продоваемые товары
        /// </summary>
        /// <returns></returns>
        public List<ProductBaseModel> GetBestSellers()
        {
            using (var db = new DataContext())
            {
                var orders = db.OrderProducts.Include(x => x.Product.Brand).ToList();
                var query = orders.GroupBy(x => x.ProductId).Select(x => new ProductBaseModel
                {
                    Id = x.Key,
                    Name = x.Select(p => p.Product).First().Name,
                    Price = x.Select(p => p.Product).First().Price,
                    BrandName = x.Select(p => p.Product).First().Brand?.Name,
                    DiscountPercent = x.Select(p => p.Product).First().Discount,
                    DiscountPrice = CalculationService.GetDiscountPrice(x.Select(p => p.Product).First().Price, x.Select(p => p.Product).First().Discount),
                    PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, x.Key.ToString())
                }).ToList();
                var model = query.OrderByDescending(x => x.Id, new PupularProductSort(orders)).Take(8).ToList();
                var products = new List<ProductBaseModel>();
                products.AddRange(model);
                return products;
            }
        }
        private class PupularProductSort : IComparer<int>
        {

            private List<OrderProduct> Orders { get; set; }

            public PupularProductSort(List<OrderProduct> orders)
            {
                Orders = orders;
            }
            int IComparer<int>.Compare(int product1, int product2)
            {
                var count1 = Orders.Count(x => x.ProductId == product1);
                var count2 = Orders.Count(x => x.ProductId == product2);
                if (count1 > count2)
                    return 1;
                if (count1 < count2)
                    return -1;
                return 0;
            }

        }
        /// <summary>
        /// Получить товар
        /// </summary>
        /// <param name="id">Ид товара</param>
        /// <returns></returns>
        public ProductEditModel GetProduct(int id)
        {
            using (var db = new DataContext())
            {
                var users = db.Users.ToList();
                var product = db.Products.Include(x => x.Brand).Include(x => x.ProductReviews).FirstOrDefault(x => x.Id == id);
                foreach (var productProductReview in product.ProductReviews)
                {
                    var user = users.FirstOrDefault(x => x.Id == productProductReview.UserId);
                    if (user == null)
                        productProductReview.User = new User();
                    else
                    {
                        productProductReview.User = user;
                    }
                }
                var model = ConvertToProductEditModel(product);
                model.Photos = FileManager.GetFileUrls(EnumDirectoryType.Product, id.ToString());
                //model.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, model.Id.ToString());
                return model;
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
                DiscountPrice = CalculationService.GetDiscountPrice(m.Price, m.Discount),
                TagsId = m.ProductTags?.Select(x => x.Id).ToList()
            };
        }
        private ProductEditModel ConvertToProductEditModel(Product m)
        {
            return new ProductEditModel
            {
                Id = m.Id,
                Name = m.Name,
                Code = m.Code,
                Price = m.Price,
                BrandId = m.BrandId,
                BrandName = m.Brand?.Name,
                CategoriesId = m.Categories?.Select(x => x.Id).ToList(),
                DateCreate = m.DateCreate,
                Description = m.Description,
                Discount = m.Discount,
                IsActive = m.IsActive,
                IsInStock = m.IsInStock,
                TagsId = m.ProductTags?.Select(x => x.Id).ToList(),
                KeyUrl = m.KeyUrl,
                PhotoUrl = m.PhotoUrl,
                SeoDescription = m.SeoDescription,
                SeoKeywords = m.SeoKeywords,
                Reviews = m.ProductReviews?.Select(ConvertToReviewModel).ToList(),
                DiscountPercent = m.Discount,
                DiscountPrice = CalculationService.GetDiscountPrice(m.Price, m.Discount)
            };
        }

        private ReviewModel ConvertToReviewModel(ProductReview m)
        {
            return new ReviewModel
            {
                Id = m.Id,
                Content = m.Content,
                DateCreate = m.DateCreate,
                DateCreateJs = JavaScriptDateConverter.Convert(m.DateCreate),
                ProductId = m.ProductId,
                User = new UserDetailModel() { FirstName = m.User?.FirstName, LastName = m.User?.LastName, Id = m.UserId }
            };
        }

        #endregion

        #endregion

        #region [ Бренды ]

        /// <summary>
        /// Получить брэнды для главной страницы
        /// </summary>
        /// <returns></returns>
        public List<BrandModel> GetBrands()
        {
            using (var db = new DataContext())
            {
                var brands = db.Brands.ToList().Select(x => new BrandModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    KeyUrl = x.KeyUrl,
                    IsActive = x.IsActive,
                    PhotoUrl = x.PhotoUrl
                }).ToList();
                List<BrandModel> brandsRandom = new List<BrandModel>();
                int k;
                Random rand = new Random();
                var count = 4;
                if (brands.Count < 4)
                    count = brands.Count;
                for (int i = 0; i < count; i++)
                {
                    while (true)
                    {
                        k = rand.Next(brands.Count);
                        if (brandsRandom.All(x => brands[k].Id != x.Id))
                        {
                            brandsRandom.Add(brands[k]);
                            break;
                        }
                    }
                }
                return brandsRandom;
            }
        }

        #endregion

        #region Отзывы
        /// <summary>
        /// Добавить отзыв
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="productId">Ид товара</param>
        /// <param name="message">Сообщение отзыва</param>
        /// <returns></returns>
        public BaseResponse AddReview(Guid userId, int productId, string message)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var product = db.Products.Include(x => x.ProductReviews).FirstOrDefault(x => x.Id == productId);
                    if (product == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Товар не найден");
                    var validation = ValidationReview(userId, productId);
                    if (validation.IsSuccess)
                    {
                        product.ProductReviews.Add(new ProductReview()
                        {
                            UserId = userId,
                            ProductId = product.Id,
                            DateCreate = DateTime.Now,
                            Content = message
                        });
                        db.SaveChanges();
                        return new BaseResponse(EnumResponseStatus.Success, "Отзыв успешно оставлен");
                    }
                    return validation;
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }
        /// <summary>
        /// Проверить возможность оставление отзыва
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="productId">Ид товара</param>
        /// <returns></returns>
        public BaseResponse ValidationReview(Guid userId, int productId)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var user = db.Users.FirstOrDefault(x => x.Id == userId);
                    if (user == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Пользователь не найден");
                    var orderHeaders = db.OrderHeaders.Include(x => x.OrderProducts).Where(x => x.UserId == userId);
                    foreach (var orderHeader in orderHeaders)
                    {
                        if (orderHeader.Status == (int)EnumStatusOrder.Complete && orderHeader.OrderProducts.Any(x => x.ProductId == productId))
                            return new BaseResponse(EnumResponseStatus.Success, "Отзыв можно написать");
                    }
                    return new BaseResponse(EnumResponseStatus.Error, "Отзыв нельзя написать");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }

        #endregion

        #region [ Желаемое ]

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
                    AddProductInCoockieWish(productId);
                    return new BaseResponse(EnumResponseStatus.Success, "Товар успешно добавлен в желаемое");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }
        /// <summary>
        /// Добавить товар в желаемое куки
        /// </summary>
        /// <param name="productId">Ид товара</param>     
        /// <returns></returns>
        public BaseResponse AddProductInCoockieWish(int productId)
        {
            HttpCookie cookieReq = HttpContext.Current.Request.Cookies["UserWish"];
            if (cookieReq == null || string.IsNullOrWhiteSpace(cookieReq.Values["productId"]))
            {
                HttpCookie aCookie = new HttpCookie("UserWish");

                aCookie.Values["productId"] = productId.ToString();

                aCookie.Expires = DateTime.Now.AddDays(30);
                HttpContext.Current.Response.Cookies.Add(aCookie);
            }
            else
            {
                var cookieProducts = cookieReq.Values["productId"].Split(',').Select(int.Parse).ToList();
                if (cookieProducts.Contains(productId))
                {

                }
                else
                {
                    cookieReq.Values["productId"] += "," + productId;
                    HttpContext.Current.Response.Cookies.Add(cookieReq);
                }
            }
            return new BaseResponse(EnumResponseStatus.Success, "Товар успешно добавлен в желаемое");
        }

        #endregion

        #region Тэги
        /// <summary>
        /// Получить тэги для фильтра
        /// </summary>
        /// <param name="products">Товары</param>
        /// <returns></returns>
        public List<TagModel> GetTagsForFilter(List<ProductBaseModel> products)
        {
            using (var db = new DataContext())
            {
                var tagsIds = new List<int>();
                foreach (var product in products)
                {
                    foreach (var id in product.TagsId)
                    {
                        if (tagsIds.Any(x => x == id))
                            continue;
                        tagsIds.Add(id);
                    }
                }
                var tagsModel = new List<TagModel>();
                var tags = db.ProductTags.ToList();
                foreach (var tagId in tagsIds)
                {
                    var tag = tags.FirstOrDefault(x => x.Id == tagId);
                    if (tag != null)
                        tagsModel.Add(new TagModel() { Id = tag.Id, Name = tag.Name });
                }
                return tagsModel.Take(30).ToList();
            }
        }

        #endregion

        #endregion

        #region [ Административная часть ]

        #region [ Товары ]

        /// <summary>
        /// Получить отфильтрованный список товаров
        /// </summary>
        /// <param name="request">фильтр</param>
        /// <returns></returns>
        public PaginationResponse<ProductEditModel> GetFilteredProducts(PaginationRequest<ProductEditModel> request)
        {
            using (var db = new DataContext())
            {
                var query = db.Products.AsNoTracking().Where(x => !x.IsDelete.HasValue)
                    .OrderByDescending(x => x.DateCreate) as IQueryable<Product>;

                //if (request.CategoryId.HasValue)
                //    query = query.Where(x => x.CategoryId == request.CategoryId.Value);
                if (!string.IsNullOrWhiteSpace(request.Filter.Name))
                    query = query.Where(x => x.Name.ToLower().Contains(request.Filter.Name.ToLower()));
                if (!string.IsNullOrWhiteSpace(request.Filter.Code))
                    query = query.Where(x => x.Code.ToLower().Contains(request.Filter.Code.ToLower()));
                var model = new PaginationResponse<ProductEditModel> { Count = query.Count() };
                if (request.Skip.HasValue)
                    query = query.Skip(request.Skip.Value);
                if (request.Take.HasValue)
                    query = query.Take(request.Take.Value);
                model.Items = query.Select(x => new ProductEditModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Price = x.Price,
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
                var product = db.Products.AsNoTracking()
                    .Include(x => x.Categories).Include(x => x.ProductTags).Include(x => x.SimilarProducts)
                    .Where(x => x.Id == productId).ToList()
                    .Select(x => new ProductEditModel
                    {
                        Id = x.Id,
                        BrandId = x.BrandId,
                        Name = x.Name,
                        Code = x.Code,
                        Description = x.Description,
                        SeoDescription = x.SeoDescription,
                        SeoKeywords = x.SeoKeywords,
                        Price = x.Price,
                        Discount = x.Discount,
                        IsRecommended = x.IsRecommended,
                        IsInStock = x.IsInStock,
                        IsActive = x.IsActive,
                        CategoriesId = x.Categories.Select(c => c.Id).ToList(),
                        TagsId = x.ProductTags.Select(t => t.Id).ToList(),
                        SimilarProducts = x.SimilarProducts.Select(sp => new ProductEditModel
                        {
                            Id = sp.Id,
                            Name = sp.Name,
                            Price = sp.Price,
                            IsActive = sp.IsActive
                        }).ToList()
                    }).FirstOrDefault();
                if (product == null)
                    return new BaseResponse<ProductEditModel>(EnumResponseStatus.Error, "Товар не найден", new ProductEditModel());
                product.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, productId.ToString());
                product.Photos = FileManager.GetFileUrls(EnumDirectoryType.Product, productId.ToString());
                product.SimilarProducts.ForEach(x =>
                {
                    x.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, x.Id.ToString());
                });
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
                        Code = model.Code,
                        BrandId = model.BrandId,
                        Description = model.Description,
                        DateCreate = DateTime.Now,
                        IsRecommended = model.IsRecommended,
                        IsInStock = model.IsInStock,
                        IsActive = model.IsActive,
                        SeoDescription = model.SeoDescription,
                        SeoKeywords = model.SeoKeywords,
                        KeyUrl = StringHelper.GetUrl(model.KeyUrl, allKeyUrls),
                        Price = model.Price,
                        Discount = model.Discount
                    };
                    if (model.CategoriesId != null)
                        newProduct.Categories = db.Categories.Where(x => model.CategoriesId.Contains(x.Id)).ToList();
                    if (model.TagsId != null)
                        newProduct.ProductTags = db.ProductTags.Where(x => model.TagsId.Contains(x.Id)).ToList();

                    // добавление похожих товаров
                    var similarProductsId = model.SimilarProducts?.Select(x => x.Id).ToList() ?? new List<int>();
                    newProduct.SimilarProducts = db.Products.Where(x => similarProductsId.Contains(x.Id)).ToList();

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
                    var old = db.Products.Include(x => x.Categories)
                        .Include(x => x.ProductTags).Include(x => x.SimilarProducts)
                        .FirstOrDefault(x => x.Id == model.Id);
                    if (old == null)
                        return new BaseResponse<int>(EnumResponseStatus.Error, "Товар не найден");

                    var allKeyUrls = db.Products.Where(x => x.Id != model.Id).Select(x => x.KeyUrl).ToList();

                    old.Name = model.Name;
                    old.Code = model.Code;
                    old.BrandId = model.BrandId;
                    old.SeoDescription = model.SeoDescription;
                    old.SeoKeywords = model.SeoKeywords;
                    old.IsRecommended = model.IsRecommended;
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

                    // похожие товары
                    var similarProductsId = model.SimilarProducts?.Select(x => x.Id).ToList() ?? new List<int>();
                    old.SimilarProducts = db.Products.Where(x => similarProductsId.Contains(x.Id)).ToList();

                    if (model.PhotoFile != null)
                    {
                        FileManager.DeleteFile(EnumDirectoryType.Product, old.Id.ToString(), old.PhotoUrl);
                        old.PhotoUrl = FileManager.SaveImage(model.PhotoFile, EnumDirectoryType.Product,
                            Guid.NewGuid().ToString(), old.Id.ToString());
                    }
                    db.SaveChanges();
                    return new BaseResponse<int>(EnumResponseStatus.Success, "Товар успешно изменен", old.Id);
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
                    var product = db.Products.Include(x => x.SimilarProducts).Include(x => x.Products)
                        .FirstOrDefault(x => x.Id == productId);
                    if (product == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Товар не найден");
                    //db.Products.Remove(product);
                    product.IsDelete = DateTime.Now;
                    db.SaveChanges();
                    //FileManager.DeleteDirectory(EnumDirectoryType.Product, product.Id.ToString());
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
                    if (db.ProductTags.AsNoTracking().Any(x => x.Name == model.Name && x.Id != model.Id))
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
        public BaseResponse<BrandModel> AddBrand(BrandModel model)
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
                    model.Id = brand.Id;
                    return new BaseResponse<BrandModel>(EnumResponseStatus.Success, model);
                }
                catch (Exception ex)
                {
                    return new BaseResponse<BrandModel>(EnumResponseStatus.Exception, ex.Message, model);
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
                    return new BaseResponse<BrandModel>(EnumResponseStatus.Success, brand);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<BrandModel>(EnumResponseStatus.Exception, ex.Message);
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
                        return new BaseResponse<BrandModel>(EnumResponseStatus.Error, "Бренд не найден",model);

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
                    return new BaseResponse<BrandModel>(EnumResponseStatus.Success, "Бренд успешно обновлен",model);
                }
                catch (Exception ex)
                {
                    return new BaseResponse<BrandModel>(EnumResponseStatus.Exception, ex.Message,model);
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
                    var brand = db.Brands.Include(x => x.Products).FirstOrDefault(x => x.Id == brandId);
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

        #region [ Купоны ]

        /// <summary>
        /// Получить список купонов
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PaginationResponse<CouponModel> GetFilteredCoupons(PaginationRequest<BaseFilter> request)
        {
            using (var db = new DataContext())
            {
                var query = db.Coupons.AsNoTracking().Where(x => !x.IsDelete.HasValue)
                    .OrderByDescending(x => x.DateCreate) as IQueryable<Coupon>;

                if (!string.IsNullOrEmpty(request.Filter.Term))
                    query = query.Where(x => x.Code.ToLower().Contains(request.Filter.Term.ToLower()));

                var model = new PaginationResponse<CouponModel> { Count = query.Count() };

                query = request.Load(query);

                model.Items = query.Select(x => new CouponModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Discount = x.Discount,
                    DateCreate = x.DateCreate
                }).ToList();
                return model;
            }
        }

        /// <summary>
        /// Получить модель купона
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<CouponModel> GetCouponModel(int id)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var model = db.Coupons.AsNoTracking().Where(x => x.Id == id)
                        .Select(x => new CouponModel
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Discount = x.Discount
                        }).FirstOrDefault();
                    if (model == null)
                        return new BaseResponse<CouponModel>(EnumResponseStatus.Error, "Купон не найден");
                    return new BaseResponse<CouponModel>(EnumResponseStatus.Success, model);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<CouponModel>(EnumResponseStatus.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Редактирование купона
        /// </summary>
        /// <param name="model">модель с данными</param>
        /// <returns></returns>
        public BaseResponse<int> CouponEdit(CouponModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    if (db.Coupons.AsNoTracking().Any(x => x.Code == model.Code && x.Id != model.Id))
                        return new BaseResponse<int>(EnumResponseStatus.ValidationError, "Купон с таким кодом уже существует");
                    var coupon = db.Coupons.FirstOrDefault(x => x.Id == model.Id);
                    if (coupon == null)
                        return new BaseResponse<int>(EnumResponseStatus.Error, "Купон не найден");
                    coupon.Code = model.Code;
                    coupon.Discount = model.Discount;
                    db.SaveChanges();
                    return new BaseResponse<int>(EnumResponseStatus.Success, "Купон успешно изменен", model.Id);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>(EnumResponseStatus.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Добавление нового купона
        /// </summary>
        /// <param name="model">модель с данными</param>
        /// <returns></returns>
        public BaseResponse<int> CouponAdd(CouponModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    if (db.Coupons.AsNoTracking().Any(x => x.Code == model.Code))
                        return new BaseResponse<int>(EnumResponseStatus.ValidationError, "Купон с таким кодом уже существует");
                    var coupon = new Coupon
                    {
                        Code = model.Code,
                        Discount = model.Discount,
                        DateCreate = DateTime.Now
                    };
                    db.Coupons.Add(coupon);
                    db.SaveChanges();
                    return new BaseResponse<int>(EnumResponseStatus.Success, "Купон успешно добавлен", coupon.Id);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>(EnumResponseStatus.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Удаление купона
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public BaseResponse CouponDelete(int couponId)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var coupon = db.Coupons.FirstOrDefault(x => x.Id == couponId);
                    if (coupon == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Купон не найден");
                    coupon.IsDelete = DateTime.Now;
                    db.SaveChanges();
                    return new BaseResponse(0, "Купон успешно удален");
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
