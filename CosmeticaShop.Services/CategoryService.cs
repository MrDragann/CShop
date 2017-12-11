using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Category;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.Services.Static;

namespace CosmeticaShop.Services
{
    public class CategoryService: ICategoryService
    {
        #region [ Публичная часть ]



        #endregion

        #region [ Административная часть ]

        /// <summary>
        /// Получить список категорий товаров
        /// </summary>
        /// <returns></returns>
        public List<BaseModel> GetBaseProductCategories()
        {
            using (var db = new DataContext())
            {
                var categories = new List<CategoryModel>();
                var allCategories = db.Categories.AsNoTracking().AsNoTracking()
                    .Select(_ => new CategoryModel() { Id = _.Id, Name = _.Name, ParentId = _.ParentId }).ToList();
                var parentCategories = allCategories.Where(_ => !_.ParentId.HasValue).ToList();

                if (parentCategories.Any())
                    categories = FillCategories(parentCategories, allCategories, true);
                else
                    categories = new List<CategoryModel>();

                var result = new List<BaseModel>();

                FillCat(result, categories);

                return result;
            }
        }

        private static void FillCat(List<BaseModel> result, List<CategoryModel> childs)
        {
            foreach (var item in childs)
            {
                if (item.ParentId.HasValue)
                    item.Name = $"{item.Parent?.Name} >> {item.Name}";
                else
                    item.Name = $"{item.Name}";
                result.Add(new BaseModel
                {
                    Id = item.Id,
                    Name = item.Name
                });
                if (item.ChildCategories != null)
                    FillCat(result, item.ChildCategories);
            }
        }

        /// <summary>
        /// Создать новую категорию
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse AddCategory(CategoryModel model)
        {
            using (var db = new DataContext())
            {
                try
                {
                    var maxPriority = db.Categories.AsNoTracking().Where(x=>x.ParentId==model.ParentId)
                                          .Max(x => x.Priority) ?? 1;
                    var allKeyUrls = db.Categories.AsNoTracking().Select(x => x.KeyUrl).ToList();
                    var newCategory = new Category
                    {
                        Name = model.Name,
                        KeyUrl = StringHelper.GetUrl(model.KeyUrl, allKeyUrls),
                        ParentId = model.ParentId,
                        IsActive = model.IsActive,
                        Priority = maxPriority
                    };
                    db.Categories.Add(newCategory);

                    db.SaveChanges();

                    return new BaseResponse(EnumResponseStatus.Success);
                }
                catch (Exception ex)
                {
                    return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
                }
            }
        }

        /// <summary>
        /// Получить категорию для редактирования
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public CategoryModel GetCategoryModel(int categoryId)
        {
            using (var db = new DataContext())
            {
                var category = db.Categories.Where(x => x.Id == categoryId)
                                   .Select(x => new CategoryModel
                                   {
                                       Id = x.Id,
                                       Name = x.Name,
                                       KeyUrl = x.KeyUrl,
                                       ParentId = x.ParentId,
                                       IsActive = x.IsActive
                                   }).FirstOrDefault() ?? new CategoryModel();

                return category;
            }
        }

        /// <summary>
        /// Редактировать категорию
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse EditCategory(CategoryModel model)
        {
            using (var db = new DataContext())
            {
                try
                {
                    var category = db.Categories.FirstOrDefault(x => x.Id == model.Id);

                    if (category == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Категория не найдена");

                    var allKeyUrls = db.Categories.Where(x => x.Id != model.Id).Select(x => x.KeyUrl).ToList();

                    category.Name = model.Name;
                    category.KeyUrl = StringHelper.GetUrl(model.KeyUrl, allKeyUrls);
                    category.ParentId = model.ParentId;
                    category.IsActive = model.IsActive;
                    
                    db.SaveChanges();
                    return new BaseResponse(EnumResponseStatus.Success, "Категория успешно обновлена");
                }
                catch (Exception ex)
                {
                    return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
                }
            }
        }

        /// <summary>
        /// Получить список всех категорий
        /// </summary>
        /// <param name="parentId">Ид родителя</param>
        /// <returns></returns>
        public List<CategoryModel> GetAllCategories(int? parentId = null)
        {
            using (var db = new DataContext())
            {
                List<CategoryModel> model;
                var allCategories = db.Categories.AsNoTracking().Include(x => x.Parent)
                    .OrderBy(x => x.Name).Select(x => new CategoryModel
                    {
                        Id = x.Id,
                        ParentId = x.ParentId,
                        Priority = x.Priority,
                        Name = x.Name,
                        KeyUrl = x.KeyUrl
                    }).ToList();
                var parentCategories = parentId.HasValue
                    ? allCategories.Where(_ => _.ParentId == parentId).OrderBy(x => x.Priority).ToList()
                    : allCategories.Where(_ => !_.ParentId.HasValue).OrderBy(x => x.Priority).ToList();
                if (!parentCategories.Any() && parentId.HasValue)
                    parentCategories = allCategories.Where(x => x.Id == parentId.Value).OrderBy(x => x.Priority).ToList();
                if (parentCategories.Any())
                    model = FillCategories(parentCategories, allCategories);
                else
                    model = new List<CategoryModel>();
                return model;
            }
        }

        /// <summary>
        /// Заполнить список заголовков категорий с помощью рекурсии
        /// </summary>
        /// <param name="parents">Список категорий - родителей</param>
        /// <param name="allCategories">Список всех категорий</param>
        /// <param name="isFillParent">Получить объект родителя</param>
        /// <returns></returns>
        private static List<CategoryModel> FillCategories(List<CategoryModel> parents, List<CategoryModel> allCategories, bool isFillParent = false)
        {
            foreach (var item in parents)
            {
                var parentCategories = allCategories.Where(_ => _.ParentId == item.Id).ToList();
                if (item.ParentId.HasValue && isFillParent)
                    item.Parent = allCategories.FirstOrDefault(x => x.Id == item.ParentId);
                if (parentCategories.Any())
                    item.ChildCategories = FillCategories(parentCategories, allCategories, isFillParent).OrderBy(x => x.Priority).ToList();
            }
            return parents;
        }
        
        /// <summary>
        /// Удалить категорию
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public BaseResponse DeleteCategory(int categoryId)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var category = db.Categories.Include(x => x.ChildCategories)
                        .FirstOrDefault(x => x.Id == categoryId);
                    if (category == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Категория не найдена");

                    db.Categories.Remove(category);
                    db.SaveChanges();
                    if (category.Priority.HasValue)
                    {
                        NormalizeCategoriesPriority(category.ParentId, db);
                    }
                    return new BaseResponse(EnumResponseStatus.Success, "Категория успешно удалена");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Нормализовать приоритеты категорий
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="db"></param>
        private void NormalizeCategoriesPriority(int? parentId, DataContext db)
        {
            var categories = db.Categories
                .Where(x => x.ParentId == parentId && x.Priority.HasValue)
                .OrderBy(x => x.Priority).ToList();
            var minPriority = 1;
            var firstCategory = categories.FirstOrDefault();
            if (firstCategory != null)
            {
                if (firstCategory.Priority != minPriority)
                {
                    firstCategory.Priority = minPriority;
                    var priority = minPriority;
                    for (var i = 1; i < categories.Count; i++)
                    {
                        if (categories[i] != null)
                        {
                            priority++;
                            categories[i].Priority = priority;
                        }
                    }
                }
                else
                {
                    for (var i = 1; i < categories.Count; i++)
                    {
                        if (categories[i] != null)
                        {
                            if (categories[i].Priority != categories[i - 1].Priority + 1)
                            {
                                categories[i].Priority = categories[i - 1].Priority + 1;
                            }
                        }
                    }
                }
            }
            db.SaveChanges();
        }

        /// <summary>
        /// Повысить приоритет категории
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public BaseResponse UpCategoryPriority(int? parentId, int categoryId)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var category = db.Categories.FirstOrDefault(x => x.Id == categoryId);
                    if (category == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Категория не найдена");
                    var categories = db.Categories.Where(x => x.ParentId == parentId && x.Priority.HasValue)
                        .OrderBy(x => x.Priority).ToList();
                    for (var i = 1; i < categories.Count; i++)
                    {
                        if (categories[i] != null)
                        {
                            if (categories[i] == category && categories[i - 1] != null)
                            {
                                categories[i].Priority = categories[i - 1].Priority;
                                categories[i - 1].Priority = categories[i].Priority + 1;
                            }
                        }
                    }
                    db.SaveChanges();
                    return new BaseResponse(EnumResponseStatus.Success, "Приоритет категории успешно повышен");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Понизить приоритет категории
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public BaseResponse DownCategoryPriority(int? parentId, int categoryId)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var category = db.Categories.FirstOrDefault(x => x.Id == categoryId);
                    if (category == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Категория не найдена");
                    var categories = db.Categories.Where(x => x.ParentId == parentId && x.Priority.HasValue)
                        .OrderBy(x => x.Priority).ToList();
                    for (var i = 0; i < categories.Count - 1; i++)
                    {
                        if (categories[i] != null)
                        {
                            if (categories[i] == category && categories[i + 1] != null)
                            {
                                categories[i].Priority = categories[i + 1].Priority;
                                categories[i + 1].Priority = categories[i].Priority - 1;
                            }
                        }
                    }
                    db.SaveChanges();
                    return new BaseResponse(EnumResponseStatus.Success, "Приоритет категории успешно понижен");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }
        #endregion
    }
}
