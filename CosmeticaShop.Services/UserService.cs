using CosmeticaShop.Data;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using CosmeticaShop.Data.Models;
using CosmeticaShop.Services.Static;

namespace CosmeticaShop.Services
{
    public class UserService : IUserService
    {
        /// <summary>
        /// Получить информацию о пользователе 
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <returns></returns>
        public UserDetailModel GetUser(Guid userId)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.Include(x => x.UserAddress).FirstOrDefault(x => x.Id == userId);
                if (user == null)
                    return new UserDetailModel();
                return ConvertUserDetailModel(user);
            }
        }
        /// <summary>
        /// Установить пользователя в куки
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <returns></returns>
        public void SetUserCookie(Guid userId)
        {
            HttpCookie cookieReq = HttpContext.Current.Request.Cookies["User"];
            if (string.IsNullOrWhiteSpace(cookieReq?.Value))
            {
                HttpCookie aCookie = new HttpCookie("User")
                {
                    Value = userId.ToString(),
                    Expires = DateTime.Now.AddDays(30)
                };
                HttpContext.Current.Response.Cookies.Add(aCookie);
            }
            else
            {
                var cookieUser = Guid.Parse(cookieReq.Value);
                if (cookieUser != userId)
                {
                    cookieReq.Value = userId.ToString();
                    HttpContext.Current.Response.Cookies.Add(cookieReq);
                }
            }
        }
        /// <summary>
        /// Полчить пользователя в куки
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <returns></returns>
        public BaseResponse<UserBaseModel> GetUserCookie()
        {
            using (var db = new DataContext())
            {
                HttpCookie cookieReq = HttpContext.Current.Request.Cookies["User"];
                if (string.IsNullOrWhiteSpace(cookieReq?.Value))
                {
                    return new BaseResponse<UserBaseModel>(EnumResponseStatus.Error, "Пользователь не найден");
                }
                var cookieUser = Guid.Parse(cookieReq.Value);
                var user = db.Users.FirstOrDefault(x => x.Id == cookieUser && x.Status != (int)EnumStatusUser.Unauthorized);
                if (user == null)
                    return new BaseResponse<UserBaseModel>(EnumResponseStatus.Error,"Пользователь не найден");
                return new BaseResponse<UserBaseModel>(EnumResponseStatus.Success, "Успешно", new UserBaseModel()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Id = user.Id,
                    LastName = user.LastName
                });

            }

        }
        /// <summary>
        /// Изменить личные данные пользователя
        /// </summary>
        /// <param name="model">Модель пользователя</param>
        /// <returns></returns>
        public BaseResponse EditPersonData(UserDetailModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var user = db.Users.Include(x => x.UserAddress).FirstOrDefault(x => x.Id == model.Id);
                    if (user == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Пользователь не найден");
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.UserAddress.Address = model.Address;
                    user.UserAddress.City = model.City;
                    user.UserAddress.Country = model.Country;
                    user.UserAddress.Phone = model.Phone;
                    user.DateBirth = model.DateBirth;
                    if (model.Password != null)
                    {
                        user.PasswordHash = model.Password.GetHashString();
                    }
                    db.SaveChanges();
                    return new BaseResponse(EnumResponseStatus.Success, "Личные данные успешно изменены");

                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }
        #region Конвертация
        public UserDetailModel ConvertUserDetailModel(User m)
        {
            return new UserDetailModel
            {
                Id = m.Id,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Email = m.Email,
                Country = m.UserAddress?.Country,
                City = m.UserAddress?.City,
                Address = m.UserAddress?.Address,
                Phone = m.UserAddress?.Phone,
                DateBirth = m.DateBirth,
                DateDay = m.DateBirth?.Day,
                DateMonth = m.DateBirth?.Month,
                DateYear = m.DateBirth?.Year
            };
        }
        #endregion
    }
}
