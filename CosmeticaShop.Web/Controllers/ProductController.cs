﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Pagination;
using CosmeticaShop.Services;
using CosmeticaShop.Web.Infrastructure;
using CosmeticaShop.Web.Models;
using CosmeticaShop.IServices.Models.Product;

namespace CosmeticaShop.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService = new ProductService();
        private readonly IOrderService _orderService = new OrderService();
        private readonly ICategoryService _categoryService = new CategoryService();
        /// <summary>
        /// Страница со списоком товаров
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(ProductFilterModel request)
        {
            SetSitePageSettings(EnumSitePage.Products);
            const int take = 21;
            var products = _productService.GetProducts(request);
    
            if (!request.Page.HasValue)
                request.Page = 1;
            if(request.CategoriesId==null)
                request.CategoriesId = new List<int>();
            // если не существует такой страницы
            if (request.Page > products.Count / take + 1)
            {
                request.Page = 1;
            }
                
            var model = new ProductsViewModel()
            {

                Products = products,
                Brands = _productService.GetAllBrandsBase(),
                Pagination = new PaginationModel(take)
                {
                    Count = products.Count,
                    Skip = ((int)request.Page - 1) * take,
                    Take = take,
                    PageNumber = (int)request.Page,
                    PageSize = (int)Math.Round(products.Count / Convert.ToDecimal(take))
                },
                Filter = request,
                Categories = _categoryService.GetAllCategories(),
                Tags = _productService.GetTagsForFilter(products)
            };
            model.Products = model.Products.Skip(model.Pagination.Skip).Take(model.Pagination.Take).ToList();
            return View(model);
        }
        /// <summary>
        /// Добавить товар в корзину
        /// </summary>
        /// <param name="productId">Ид товара</param>
        /// <param name="quantity">Количество товара</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddInCart(int productId, int quantity)
        {
            var user = new WebUser();
            if (user.IsAuthorized)
            {
                var response = _orderService.AddProductInCart(productId, user.UserId, quantity,false);
                return Json(response);
            }
            var responseCopokie = _orderService.AddProductInCoockieCart(productId, quantity,false);
            return Json(responseCopokie);
        }
        /// <summary>
        /// Добавить товар в желаемое
        /// </summary>
        /// <param name="productId">Ид товара</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddInWish(int productId)
        {
            var user = new WebUser();
            if (user.IsAuthorized)
            {
                var response = _productService.AddProductInWish(productId, user.UserId);
                return Json(response);
            }
            else
            {
                var response = _productService.AddProductInCoockieWish(productId);
                return Json(response);
            }
         
        }
        /// <summary>
        /// Детальная страница товара
        /// </summary>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            var product = _productService.GetProduct(id);
            var user = new WebUser();
            var possibilityReview = false;
            if (user.IsAuthorized)
            {
                possibilityReview = _productService.ValidationReview(user.UserId, id).IsSuccess;
            }
            var model = new ProductDetailsView()
            {
                Product = product,
                PossibilityReview = possibilityReview,
                SimilarProduct = _productService.GetSimilarProducts(id)
            };
            SetSitePageSettings(model.Product.Name, model.Product.SeoKeywords, model.Product.SeoDescription);
            return View(model);
        }
        /// <summary>
        /// Детальная страница товара
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(string productKeyUrl)
        {
            var product = _productService.GetProductByKeyUrl(productKeyUrl);
            if (!product.IsSuccess)
                return RedirectToAction("Index", "Home");
            var user = new WebUser();
            var possibilityReview = false;
            if (user.IsAuthorized)
            {
                possibilityReview = _productService.ValidationReview(user.UserId, product.Value.Id).IsSuccess;
            }
            var model = new ProductDetailsView()
            {
                Product = product.Value,
                PossibilityReview = possibilityReview,
                SimilarProduct = _productService.GetSimilarProducts(product.Value.Id)
            };
            SetSitePageSettings(model.Product.Name, model.Product.SeoKeywords, model.Product.SeoDescription);
            return View("~/Views/Product/Details.cshtml",model);
        }
        /// <summary>
        /// Добавить отзыв
        /// </summary>
        /// <param name="message">Сообщение отзыва</param>
        /// <param name="productId">Ид товара</param>
        /// <returns></returns>
        public ActionResult AddReview(string message,int productId)
        {
            var userId = new WebUser().UserId;
            var response = _productService.AddReview(userId, productId, message);
            return Json(response);
        }
        /// <summary>
        /// Получить тэги для фильтра
        /// </summary>
        /// <param name="products">Товары</param>
        /// <returns></returns>
        public ActionResult GetTagsForFilter(List<ProductBaseModel> products)
        {
            var res = _productService.GetTagsForFilter(products);
            return Json(res);
        }
    }
}