using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Models;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Navigation;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.SitePage;
using CosmeticaShop.IServices.Models.Slider;

namespace CosmeticaShop.IServices.Interfaces
{
    public interface ISitePageSevice
    {
        #region [ Публичная ]

        #region [ Навигация ]

        /// <summary>
        /// Получить навигацию сайта
        /// </summary>
        /// <returns></returns>
        NavigationViewModel GetSiteNavigation();

        /// <summary>
        /// Получить список всех городов
        /// </summary>
        /// <returns></returns>
        List<DictionaryModel> GetAllCities();

        #endregion

        #region [ Настройки страниц ]

        /// <summary>
        /// Получить настройки страницы
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        SitePageModel GetSitePageSettings(EnumSitePage page);

        #endregion

        #region [ Слайдер ]

        /// <summary>
        /// Получить список слайдов
        /// </summary>
        /// <returns></returns>
        List<SliderModel> GetSlides();

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

        /// <summary>
        /// Получить отфильтрованный список слайдов
        /// </summary>
        /// <param name="request">фильтр</param>
        /// <returns></returns>
        PaginationResponse<SliderEditModel> GetFilteredSlides(PaginationRequest<BaseFilter> request);

        /// <summary>
        /// Получить модель слайда для редактирования
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<SliderEditModel> GetSlideModel(int id);

        /// <summary>
        /// Создать слайд
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<int> AddSlide(SliderEditModel model);

        /// <summary>
        /// Редактирование слайда
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<int> EditSlide(SliderEditModel model);

        /// <summary>
        /// Удалить слайд
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse DeleteSlide(int id);

        #endregion

        #endregion
    }
}
