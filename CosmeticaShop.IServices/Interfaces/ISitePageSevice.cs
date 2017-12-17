using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.SitePage;

namespace CosmeticaShop.IServices.Interfaces
{
    public interface ISitePageSevice
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
        SitePageModel GetSitePageModel(EnumSitePage page);

        /// <summary>
        /// Обновить информацию о странице
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse UpdateSitePage(SitePageModel model);

        #endregion

        #region [ Слайдер ]



        #endregion

        #endregion
    }
}
