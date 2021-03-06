﻿using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.SitePage;
using CosmeticaShop.Services;

namespace CosmeticaShop.Web.Controllers
{
    public class BaseController : Controller
    {
        #region [ Сервисы ]
        
        private ISitePageSevice _sitePageSevice = new SitePageSevice();

        #endregion

        public BaseController() { }

        #region [ Настройки страницы ]

        /// <summary>
        /// Установить настройки странице (с запросом на получение настроек)
        /// </summary>
        /// <param name="page">Страница</param>
        public void SetSitePageSettings(EnumSitePage page)
        {
            var model = _sitePageSevice.GetSitePageSettings(page);
            ViewBag.Title = model.Title;
            ViewBag.Keywords = model.SeoKeywords;
            ViewBag.Description = model.SeoDescription;
        }

        /// <summary>
        /// Установить настройки странице
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="keywords">Ключевые слова</param>
        /// <param name="description">Описание</param>
        public void SetSitePageSettings(string title, string keywords, string description)
        {
            ViewBag.Title = title;
            ViewBag.Keywords = keywords;
            ViewBag.Description = description;
        }

        /// <summary>
        /// Установить настройки странице
        /// </summary>
        /// <param name="model"></param>
        public void SetSitePageSettings(SitePageModel model)
        {
            ViewBag.Title = model.Title;
            ViewBag.Keywords = model.SeoKeywords;
            ViewBag.Description = model.SeoDescription;
        }

        #endregion

        protected override void Initialize(RequestContext requestContext)
        {
            var ci = new CultureInfo("ro");
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            base.Initialize(requestContext);
        }
    }
}