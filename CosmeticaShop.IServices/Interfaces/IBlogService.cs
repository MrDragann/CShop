using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Blog;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.IServices.Models.Responses;

namespace CosmeticaShop.IServices.Interfaces
{
    public interface IBlogService
    {
        #region [ Административная ]

        /// <summary>
        /// Получить список брендов для постранички
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PaginationResponse<BlogModel> GetFilteredBlogs(PaginationRequest<BaseFilter> request);

        /// <summary>
        /// Получить модель поста в блоге
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<BlogModel> GetBlogPostModel(int id);

        /// <summary>
        /// Добавить пост в блог
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<int> BlogPostAdd(BlogModel model);

        /// <summary>
        /// Редактировать пост в блога
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<int> BlogPostEdit(BlogModel model);

        /// <summary>
        /// Удалить пост в блоге
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse BlogPostDelete(int id);

        #endregion

        #region [ Публичная ]

        /// <summary>
        /// Получить список постов блога
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PaginationResponse<BlogModel> GetBlogPostList(PaginationRequest request);

        /// <summary>
        /// Получить пост из блога
        /// </summary>
        /// <param name="keyUrl"></param>
        /// <returns></returns>
        BaseResponse<BlogModel> GetBlogPostDetail(string keyUrl);

        /// <summary>
        /// Получить послединие посты
        /// </summary>
        /// <param name="currentId"></param>
        /// <returns></returns>
        List<BlogModel> GetRecentBlogs(int? currentId = null);

        #endregion
    }
}
