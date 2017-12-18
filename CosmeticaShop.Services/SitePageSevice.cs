using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.SitePage;
using CosmeticaShop.IServices.Models.Slider;
using CosmeticaShop.Services.Static;

namespace CosmeticaShop.Services
{
    public class SitePageSevice: ISitePageSevice
    {
        #region [ Публичная ]

        #region [ Настройки страниц ]

        /// <summary>
        /// Получить настройки страницы
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public SitePageModel GetSitePageSettings(EnumSitePage page)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var sitePage = db.SitePages.AsNoTracking().Where(x => x.Id == (int)page).Select(x => new SitePageModel
                    {
                        Id = (EnumSitePage)x.Id,
                        Title = x.Title,
                        SeoKeywords = x.SeoKeywords,
                        SeoDescription = x.SeoDescription
                    }).FirstOrDefault() ?? new SitePageModel();
                    return sitePage;
                }
            }
            catch (Exception ex)
            {
                return new SitePageModel();
            }
        }

        #endregion

        #region [ Слайдер ]

        /// <summary>
        /// Получить список слайдов
        /// </summary>
        /// <returns></returns>
        public List<SliderModel> GetSlides()
        {
            try
            {
                using (var db = new DataContext())
                {
                    var slides = db.Sliders.AsNoTracking().Where(x=>x.IsActive)
                        .OrderBy(x=>x.Priority).Select(x => new SliderModel
                    {
                        Id = x.Id,
                        PhotoUrl = x.PhotoUrl
                    }).ToList();
                    return slides;
                }
            }
            catch (Exception ex)
            {
                return new List<SliderModel>();
            }
        }

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
                        Title = x.Title,
                        SeoKeywords = x.SeoKeywords,
                        SeoDescription = x.SeoDescription,
                        Content = x.Content,
                        ExtraContent = x.ExtraContent
                    }).FirstOrDefault() ?? new SitePageModel();
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

        /// <summary>
        /// Получить отфильтрованный список слайдов
        /// </summary>
        /// <param name="request">фильтр</param>
        /// <returns></returns>
        public PaginationResponse<SliderEditModel> GetFilteredSlides(PaginationRequest<BaseFilter> request)
        {
            using (var db = new DataContext())
            {
                var query = db.Sliders.AsNoTracking()
                    .OrderByDescending(x => x.DateCreate) as IQueryable<Slider>;
                
                var model = new PaginationResponse<SliderEditModel> { Count = query.Count() };
                if (request.Skip.HasValue)
                    query = query.Skip(request.Skip.Value);
                if (request.Take.HasValue)
                    query = query.Take(request.Take.Value);
                model.Items = query.Select(x => new SliderEditModel
                {
                    Id = x.Id,
                    DateCreate = x.DateCreate,
                    PhotoUrl = x.PhotoUrl,
                    IsActive = x.IsActive
                }).ToList();
                return model;
            }
        }
        /// <summary>
        /// Получить модель слайда для редактирования
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<SliderEditModel> GetSlideModel(int id)
        {
            using (var db = new DataContext())
            {
                var product = db.Sliders.AsNoTracking().Where(x => x.Id == id).OrderBy(x=>x.Priority)
                    .Select(x => new SliderEditModel
                    {
                        Id = x.Id,
                        PhotoUrl = x.PhotoUrl,
                        IsActive = x.IsActive
                    }).FirstOrDefault();
                if (product == null)
                    return new BaseResponse<SliderEditModel>(EnumResponseStatus.Error, "Слайд не найден", new SliderEditModel());
                return new BaseResponse<SliderEditModel>(EnumResponseStatus.Success, product);
            }
        }
        /// <summary>
        /// Создать слайд
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<int> AddSlide(SliderEditModel model)
        {
            try
            {
                if (model.PhotoFile == null)
                    return new BaseResponse<int>(EnumResponseStatus.ValidationError, "Изображение не выбрано");

                using (var db = new DataContext())
                {
                    var maxPriority = db.Sliders.AsNoTracking().Where(x => x.SitePage == model.SitePage)
                                          .Max(x => x.Priority) ?? 1;
                    var newsSlide = new Slider
                    {
                        //todo:?мб на страницах будут разные слайдеры
                        SitePage = (int)EnumSitePage.Home,
                        DateCreate = DateTime.Now,
                        IsActive = model.IsActive,
                        Priority = maxPriority
                    };
                    db.Sliders.Add(newsSlide);

                    db.SaveChanges();
                    newsSlide.PhotoUrl = FileManager.SaveImage(model.PhotoFile, EnumDirectoryType.Slider,
                        Guid.NewGuid().ToString());
                    db.SaveChanges();
                    return new BaseResponse<int>(EnumResponseStatus.Success, "Слайд успешно сохранен", newsSlide.Id);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>(EnumResponseStatus.Exception, ex.Message);
            }
        }
        /// <summary>
        /// Редактирование слайда
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<int> EditSlide(SliderEditModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var old = db.Sliders.FirstOrDefault(x => x.Id == model.Id);
                    if (old == null)
                        return new BaseResponse<int>(EnumResponseStatus.Error, "Слайд не найден");
                    
                    old.IsActive = model.IsActive;
                    if (model.PhotoFile != null)
                    {
                        FileManager.DeleteFile(EnumDirectoryType.Slider, fileName:old.PhotoUrl);
                        old.PhotoUrl = FileManager.SaveImage(model.PhotoFile, EnumDirectoryType.Slider,
                            Guid.NewGuid().ToString());
                    }
                    db.SaveChanges();
                    return new BaseResponse<int>(EnumResponseStatus.Success, "Слайд успешно изменен", old.Id);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>(EnumResponseStatus.Exception, ex.Message);
            }
        }
        /// <summary>
        /// Удалить слайд
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse DeleteSlide(int id)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var slide = db.Sliders.FirstOrDefault(x => x.Id == id);
                    if (slide == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Слайд не найден");
                    db.Sliders.Remove(slide);
                    db.SaveChanges();
                    FileManager.DeleteFile(EnumDirectoryType.Slider, fileName: slide.PhotoUrl);
                    return new BaseResponse(EnumResponseStatus.Success, "Слайд успешно удален");
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
