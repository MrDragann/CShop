using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Models.Product;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.Wish;
using CosmeticaShop.Services.Static;

namespace CosmeticaShop.Services
{
    public class WishService : IWishService
    {
        /// <summary>
        /// Получить список желаемого
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <returns></returns>
        public List<WishModel> GetWishs(Guid userId)
        {
            using (var db = new DataContext())
            {
                var wishs = db.WishLists.Include(x => x.Product).Where(x => x.UserId == userId).ToList();
                var response = wishs.Select(ConvertWishModel).ToList();
                foreach (var wishModel in response)
                {
                    wishModel.Product.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, wishModel.Product.Id.ToString());
                }
                return response;
            }
        }
        /// <summary>
        /// Получить список желаемого из куки
        /// </summary>
        /// <returns></returns>
        public List<WishModel> GetCookieWishs()
        {
            HttpCookie cookieReq = HttpContext.Current.Request.Cookies["UserWish"];
            if (cookieReq == null || string.IsNullOrWhiteSpace(cookieReq.Values["productId"]))
            {
                return new List<WishModel>();
            }
            var cookieProducts = cookieReq.Values["productId"].Split(',').Select(int.Parse).ToList();

            List<WishModel> cookieWish = new List<WishModel>();
            for (int i = 0; i < cookieProducts.Count(); i++)
            {
                cookieWish.Add(new WishModel { ProductId = cookieProducts[i], });
            }
            using (var db = new DataContext())
            {
                var products = db.Products.Include(x => x.Brand).ToList();
                foreach (var wishModel in cookieWish)
                {
                    var product = products.FirstOrDefault(x => x.Id == wishModel.ProductId);
                    if (product != null)
                        wishModel.Product = new ProductBaseModel()
                        {
                            Price = product.Price,
                            BrandName = product.Brand?.Name,
                            DiscountPercent = product.Discount,
                            DiscountPrice = CalculationService.GetDiscountPrice(product.Price, product.Discount),
                            Name = product.Name,
                            Id = product.Id,
                            PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, wishModel.ProductId.ToString())
                        };
                }
            }
            return cookieWish;
        }

        /// <summary>
        /// Дополнить желаемое из куки
        /// </summary>
        /// <returns></returns>
        public void ComplementWishs(Guid userId)
        {
            using (var db = new DataContext())
            {
                var wishList = db.WishLists.Where(x => x.UserId == userId).ToList();
                var products = db.Products.ToList();
                HttpCookie cookieReq = HttpContext.Current.Request.Cookies["UserWish"];
                if (cookieReq == null || string.IsNullOrWhiteSpace(cookieReq.Values["productId"]))
                {
                    HttpCookie aCookie = new HttpCookie("UserWish");
                    var productsId = wishList.Select(x => x.ProductId.ToString()).ToList();
                    var cooikeIds = string.Join(",", productsId);

                    aCookie.Values["productId"] = cooikeIds;
                    aCookie.Values["userId"] = userId.ToString();

                    aCookie.Expires = DateTime.Now.AddDays(30);
                    HttpContext.Current.Response.Cookies.Add(aCookie);
                }
                else
                {
                    var cookieProducts = cookieReq.Values["productId"].Split(',').Select(int.Parse).ToList();
                    if (string.IsNullOrWhiteSpace(cookieReq.Values["userId"]))
                    {
                        cookieReq.Values["userId"] = userId.ToString();
                    }
                    List<WishList> cookieWish = new List<WishList>();
                    for (int i = 0; i < cookieProducts.Count(); i++)
                    {
                        if (wishList.Any(x => x.ProductId == cookieProducts[i]))
                            continue;
                        var product = products.FirstOrDefault(x => x.Id == cookieProducts[i]);
                        if (product != null)
                            cookieWish.Add(new WishList { ProductId = cookieProducts[i], DateCreate = DateTime.Now, UserId = userId });
                    }
                    db.WishLists.AddRange(cookieWish);
                    db.SaveChanges();
                    foreach (var wish in wishList)
                    {
                        for (int i = 0; i < cookieProducts.Count(); i++)
                        {
                            if (cookieProducts.Contains(wish.ProductId))
                                continue;
                            cookieReq.Values["productId"] += "," + wish.ProductId;
                        }
                    }
                    HttpContext.Current.Response.Cookies.Add(cookieReq);
                }
            }
        }

        /// <summary>
        /// Удалить товар из желаемого
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="productId">Ид товара</param>
        /// <returns></returns>
        public BaseResponse DeleteWish(Guid? userId, int productId)
        {
            using (var db = new DataContext())
            {
                HttpCookie cookieReq = HttpContext.Current.Request.Cookies["UserWish"];
                if (!string.IsNullOrWhiteSpace(cookieReq?.Values["userId"]))
                {
                    var cookieUserId = cookieReq.Values["userId"].Split(',').Select(Guid.Parse).ToList();
                    userId = cookieUserId[0];
                }
                if (userId != null)
                {
                    var wishList = db.WishLists.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId);
                    if (wishList == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Пользователь не найден");
                    db.WishLists.Remove(wishList);
                    db.SaveChanges();
                }
                if (cookieReq != null)
                {
                    var cookieProducts = cookieReq.Values["productId"].Split(',').Select(int.Parse).ToList();
                    cookieProducts.Remove(productId);
                    cookieReq.Values["productId"] = string.Join(",", cookieProducts.ToArray());
                    HttpContext.Current.Response.Cookies.Add(cookieReq);
                }
                return new BaseResponse(EnumResponseStatus.Success, "Товар успешно удален из желаемого");
            }
        }
        #region Конвертирование

        private WishModel ConvertWishModel(WishList m)
        {
            return new WishModel
            {
                UserId = m.UserId,
                DateCreate = m.DateCreate,
                ProductId = m.ProductId,
                Id = m.Id,
                Product = new ProductBaseModel
                {
                    BrandName = m.Product?.Name,
                    DiscountPercent = m.Product.Discount,
                    Id = m.Product.Id,
                    Price = m.Product.Price,
                    PhotoUrl = m.Product?.PhotoUrl,
                    Name = m.Product?.Name,
                    DiscountPrice = CalculationService.GetDiscountPrice(m.Product.Price, m.Product.Discount)

                }
            };
        }

        #endregion
    }
}
