using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Category;
using CosmeticaShop.IServices.Models.Responses;

namespace CosmeticaShop.IServices.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получить список категорий товаров
        /// </summary>
        /// <returns></returns>
        List<BaseModel> GetBaseProductCategories();

        /// <summary>
        /// Создать новую категорию
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse AddCategory(CategoryModel model);

        /// <summary>
        /// Получить категорию для редактирования
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        CategoryModel GetCategoryModel(int categoryId);

        /// <summary>
        /// Редактировать категорию
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse EditCategory(CategoryModel model);

        /// <summary>
        /// Получить список всех категорий
        /// </summary>
        /// <param name="parentId">Ид родителя</param>
        /// <param name="parentsId"></param>
        /// <returns></returns>
        List<CategoryModel> GetAllCategories(int? parentId = null, List<int> parentsId = null);

        /// <summary>
        /// Удалить категорию
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        BaseResponse DeleteCategory(int categoryId);

        /// <summary>
        /// Повысить приоритет категории
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        BaseResponse UpCategoryPriority(int? parentId, int categoryId);

        /// <summary>
        /// Понизить приоритет категории
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        BaseResponse DownCategoryPriority(int? parentId, int categoryId);





        /// <summary>
        /// Получить список категорий товаров
        /// </summary>
        /// <returns></returns>
        List<CategoryModel> GetCategories();
    }
}
