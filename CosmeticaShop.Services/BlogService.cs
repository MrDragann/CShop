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
using CosmeticaShop.IServices.Models.Blog;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.Services.Static;

namespace CosmeticaShop.Services
{
    public class BlogService : IBlogService
    {
        #region [ Публичная ]

        /// <summary>
        /// Получить список постов блога
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PaginationResponse<BlogModel> GetBlogPostList(PaginationRequest request)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var query = db.Blogs.AsNoTracking().Where(x => x.IsActive && !x.IsDelete.HasValue)
                        .OrderByDescending(x => x.DateCreate).AsQueryable();

                    var model = new PaginationResponse<BlogModel>
                    {
                        Count = query.Count()
                    };
                    query = request.Load(query);
                    model.Items = query.Select(x => new BlogModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        PreviewContent = x.PreviewContent,
                        PhotoUrl = x.PhotoUrl,
                        KeyUrl = x.KeyUrl
                    }).ToList();
                    return model;
                }
            }
            catch (Exception ex)
            {
                return new PaginationResponse<BlogModel>(EnumResponseStatus.Exception,ex.Message);
            }
        }

        /// <summary>
        /// Получить пост из блога
        /// </summary>
        /// <param name="keyUrl"></param>
        /// <returns></returns>
        public BaseResponse<BlogModel> GetBlogPostDetail(string keyUrl)
        {
            try
            {
                using (var db=new DataContext())
                {
                    var blog = db.Blogs.AsNoTracking()
                        .FirstOrDefault(x => x.IsActive && !x.IsDelete.HasValue && x.KeyUrl == keyUrl);
                    if(blog==null)
                        return new BaseResponse<BlogModel>(EnumResponseStatus.Error,"Пост не найден");
                    var model = new BlogModel
                    {
                        Id = blog.Id,
                        Title = blog.Title,
                        Content = blog.Content,
                        SeoDescription = blog.SeoDescription,
                        SeoKeywords = blog.SeoKeywords
                    };
                    return new BaseResponse<BlogModel>(model);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<BlogModel>(EnumResponseStatus.Exception);
            }
        }

        /// <summary>
        /// Получить послединие посты
        /// </summary>
        /// <param name="currentId"></param>
        /// <returns></returns>
        public List<BlogModel> GetRecentBlogs(int? currentId = null)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var query = db.Blogs.AsNoTracking().Where(x => x.IsActive && !x.IsDelete.HasValue);
                    if (currentId.HasValue)
                        query = query.Where(x => x.Id != currentId);
                    var recentBlogs = query
                        .OrderByDescending(x => x.DateCreate).Take(10)
                        .Select(x => new BlogModel
                        {
                            Id = x.Id,
                            Title = x.Title,
                            PhotoUrl = x.PhotoUrl,
                            PreviewContent = x.PreviewContent,
                            KeyUrl = x.KeyUrl
                        }).ToList();
                    var randomRecentBlogs = CalculationService.GetRandomBlogs(recentBlogs, 3);
                    return randomRecentBlogs;
                }
            }
            catch (Exception ex)
            {
                return new List<BlogModel>();
            }
        }

        #endregion

        #region [ Административная ]

        /// <summary>
        /// Получить список брендов для постранички
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PaginationResponse<BlogModel> GetFilteredBlogs(PaginationRequest<BaseFilter> request)
        {
            using (var db = new DataContext())
            {
                var query = db.Blogs.AsNoTracking().Where(x=>!x.IsDelete.HasValue)
                    .OrderByDescending(x => x.DateCreate).AsQueryable();

                if (!string.IsNullOrEmpty(request.Filter.Term))
                    query = query.Where(x => x.Title.Contains(request.Filter.Term));

                var model = new PaginationResponse<BlogModel> { Count = query.Count() };

                query = request.Load(query);

                model.Items = query.Select(x => new BlogModel
                {
                    Id = x.Id,
                    DateCreate = x.DateCreate,
                    Title = x.Title,
                    KeyUrl = x.KeyUrl,
                    IsActive = x.IsActive,
                    PhotoUrl = x.PhotoUrl
                }).ToList();
                return model;
            }
        }

        /// <summary>
        /// Получить модель поста в блоге
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse<BlogModel> GetBlogPostModel(int id)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var blog = db.Blogs.AsNoTracking().FirstOrDefault(x => x.Id == id);
                    if (blog == null)
                        return new BaseResponse<BlogModel>(EnumResponseStatus.Error,"Пост не найден");

                    var model = new BlogModel
                    {
                        Id = blog.Id,
                        Title = blog.Title,
                        PhotoUrl = blog.PhotoUrl,
                        PreviewContent = blog.PreviewContent,
                        Content = blog.Content,
                        SeoKeywords = blog.SeoKeywords,
                        SeoDescription = blog.SeoDescription,
                        IsActive = blog.IsActive
                    };
                    return new BaseResponse<BlogModel>(model);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<BlogModel>(EnumResponseStatus.Exception);
            }
        }

        /// <summary>
        /// Добавить пост в блог
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<int> BlogPostAdd(BlogModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var newBlog = ConvertToBlog(model);
                    var allKeyUrls = db.Blogs.AsNoTracking().Select(x => x.KeyUrl).ToList();

                    newBlog.DateCreate = DateTime.Now;
                    newBlog.KeyUrl = StringHelper.GetUrl(StringHelper.FormKeyUrl(newBlog.Title),allKeyUrls);
                    db.Blogs.Add(newBlog);
                    db.SaveChanges();
                    if (model.PhotoFile != null)
                    {
                        newBlog.PhotoUrl = FileManager.SaveImage(model.PhotoFile, EnumDirectoryType.Blog,
                            Guid.NewGuid().ToString());
                        db.SaveChanges();
                    }
                    return new BaseResponse<int>(EnumResponseStatus.Success,newBlog.Id);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>(EnumResponseStatus.Exception);
            }
        }

        /// <summary>
        /// Редактировать пост в блога
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse<int> BlogPostEdit(BlogModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var blog = db.Blogs.FirstOrDefault(x => x.Id == model.Id);
                    if(blog==null)
                        return new BaseResponse<int>(EnumResponseStatus.Error,"Пост не найден");

                    var allKeyUrls = db.Blogs.AsNoTracking().Where(x=>x.Id!=model.Id)
                        .Select(x => x.KeyUrl).ToList();

                    blog.Title = model.Title;
                    blog.PreviewContent = model.PreviewContent;
                    blog.Content = model.Content;
                    blog.IsActive = model.IsActive;

                    blog.KeyUrl = StringHelper.GetUrl(StringHelper.FormKeyUrl(blog.Title),allKeyUrls);

                    db.SaveChanges();
                    if (model.PhotoFile != null)
                    {
                        blog.PhotoUrl = FileManager.SaveImage(model.PhotoFile, EnumDirectoryType.Blog,
                            Guid.NewGuid().ToString());
                        db.SaveChanges();
                    }
                    return new BaseResponse<int>(EnumResponseStatus.Success, blog.Id);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>(EnumResponseStatus.Exception);
            }
        }

        /// <summary>
        /// Удалить пост в блоге
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponse BlogPostDelete(int id)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var blog = db.Blogs.FirstOrDefault(x => x.Id == id);
                    if(blog==null)
                        return new BaseResponse(EnumResponseStatus.Error,"Пост не найден");
                    blog.IsDelete = DateTime.Now;
                    db.SaveChanges();
                    return new BaseResponse(EnumResponseStatus.Success);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception);
            }
        }

        private Blog ConvertToBlog(BlogModel m)
        {
            return new Blog
            {
                Id = m.Id,
                Title = m.Title,
                Content = m.Content,
                PreviewContent = m.PreviewContent,
                SeoDescription = m.SeoDescription,
                SeoKeywords = m.SeoKeywords,
                IsActive = m.IsActive
            };
        }

        #endregion
    }
}
