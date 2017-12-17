using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.Data;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.SitePage;

namespace CosmeticaShop.Services
{
    public class SitePageSevice: ISitePageSevice
    {
        #region [ Публичная ]

        #region [ Настройки страниц ]



        #endregion

        #region [ Слайдер ]



        #endregion

        #endregion

        #region [ Административная ]

        #region [ Настройки страниц ]

        /// <summary>
        /// Получить модель страницы
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public SitePageModel GetSitePageModel(EnumSitePage page)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var sitePage = db.SitePages.AsNoTracking().Where(x => x.Id == (int) page).Select(x => new SitePageModel
                    {
                        Id = (EnumSitePage)x.Id,
                        Title = x.Title
                    }).FirstOrDefault();
                    return sitePage;
                }
            }
            catch (Exception ex)
            {
                return new SitePageModel();
            }
        }

        /// <summary>
        /// Обновить информацию о странице
        /// </summary>
        /// <param name="model"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public BaseResponse UpdateSitePage(SitePageModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var sitePage = db.SitePages.FirstOrDefault(x => x.Id == (int) model.Id);
                    if(sitePage==null)
                        return new BaseResponse(EnumResponseStatus.Error,"Страница не найдена");
                    sitePage.Title = model.Title;
                    sitePage.SeoKeywords = model.SeoKeywords;
                    sitePage.SeoDescription = model.SeoDescription;
                    sitePage.Content = model.Content;
                    sitePage.ExtraContent = model.ExtraContent;
                    db.SaveChanges();
                    return new BaseResponse(EnumResponseStatus.Success, "Настройки страницы успешно обновлены");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception,ex.Message);
            }
        }

        #endregion

        #region [ Слайдер ]



        #endregion

        #endregion
    }
}
