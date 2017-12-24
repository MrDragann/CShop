using System;
using System.Linq;
using System.Net.Mail;
using System.Data.Entity;
using System.Web;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.User;

namespace CosmeticaShop.Services
{
    /// <summary>
    /// Сервис авторизации
    /// </summary>
    public class AuthCommonService : IAuthCommonService
    {
        #region Авторизация

        /// <summary>
        /// Обновление пароля пользователя
        /// </summary>
        /// <param name="email">Почта</param>
        /// <param name="token">Токен пользователя</param>
        /// <param name="passwordHash">Хешированный пароль</param>
        public BaseResponse RestorePassword(string email, Guid token, string passwordHash)
        {
            var tokenStatus = CheckUserToken(email, token);
            if (!tokenStatus.IsSuccess)
            {
                return tokenStatus;
            }
            using (var db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(_ => _.Email == email);
                if (user != null)
                {
                    user.ConfirmationToken = null;
                    user.TokenExpireDate = null;
                    user.PasswordHash = passwordHash;
                    db.SaveChanges();
                    return new BaseResponse(EnumResponseStatus.Success, "Пароль успешно изменен");
                }
                return new BaseResponse(EnumResponseStatus.Error);
            }
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="model">Модель пользователя</param>
        /// <param name="token">Токен для потверждения</param>
        /// <returns></returns>
        public BaseResponse Register(UserDetailModel model, Guid token)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var users = db.Users.Include(x=>x.UserAddress);
                    if (string.IsNullOrEmpty(model.Email))
                        return new BaseResponse(EnumResponseStatus.ValidationError, "Email не может быть пустым");


                    if (users.Any(x => x.Email == model.Email))
                        return new BaseResponse(EnumResponseStatus.ValidationError, "Пользователь с таким адресом электронной почты уже существует");

                    if (string.IsNullOrEmpty(model.Password))
                        return new BaseResponse(EnumResponseStatus.ValidationError, "Пароль не может быть пустым");
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["User"];
                    Guid userId;
                    if (string.IsNullOrWhiteSpace(cookie?.Value))
                    {

                    }
                    else
                    {
                        userId = Guid.Parse(cookie.Value);
                        var cookieUser = users.FirstOrDefault(x => x.Id == userId && x.Status == (int)EnumStatusUser.Unauthorized);
                        if (cookieUser != null)
                        {
                            cookieUser.Email = model.Email;
                            cookieUser.FirstName = model.FirstName;
                            cookieUser.LastName = model.LastName;
                            cookieUser.PasswordHash = model.Password;
                            cookieUser.DateBirth = model.DateBirth;
                            cookieUser.ConfirmationToken = token;
                            cookieUser.Status = (int)EnumStatusUser.New;
                            cookieUser.UserAddress.Address = model.Address;
                            cookieUser.UserAddress.City = model.City;
                            cookieUser.UserAddress.Country = model.Country;
                            cookieUser.UserAddress.Phone = model.Phone;
                            db.SaveChanges();
                            return new BaseResponse(EnumResponseStatus.Success, "Регистрация успешно выполнена");
                        }
                    }
                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PasswordHash = model.Password,
                        DateBirth = model.DateBirth,
                        ConfirmationToken = token,
                        Status = (int)EnumStatusUser.New,
                        UserAddress = new UserAddress
                        {
                            Country = model.Country,
                            City = model.City,
                            Address = model.Address,
                            Phone = model.Phone
                        }
                    };

                    db.Users.Add(user);
                    db.SaveChanges();
                    return new BaseResponse(0, "Регистрация успешно выполнена");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Метод выполняющий авторизацю пользователя
        /// </summary>
        /// <param name="email">Почта</param>
        /// <param name="passwordHash">Пароль пользователя</param>
        public BaseResponse<UserBaseModel> Login(string email, string passwordHash)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.Include(x => x.Roles).FirstOrDefault(_ => _.Email == email && _.PasswordHash == passwordHash);
                if (user != null)
                {
                    if (user.Status != (int)EnumStatusUser.Active)
                        return new BaseResponse<UserBaseModel>(EnumResponseStatus.ValidationError, "Пользователь не активирован");
                    var webUser = new UserBaseModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Roles = user.Roles.Select(x => x.Name).ToList()
                    };

                    return new BaseResponse<UserBaseModel>(webUser);
                }
                return new BaseResponse<UserBaseModel>(EnumResponseStatus.ValidationError, "Введен неверный Email или пароль");
            }
        }


        /// <summary>
        /// Проверить токен пользователя 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public BaseResponse CheckUserToken(string email, Guid token)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(_ => _.Email == email && _.ConfirmationToken == token && _.TokenExpireDate > DateTime.Now);
                if (user != null)
                {
                    return new BaseResponse(EnumResponseStatus.Success);
                }
                return new BaseResponse(EnumResponseStatus.Error);
            }
        }

        /// <summary>
        /// Подтверждение аккаунта пользователя
        /// </summary>
        /// <param name="token">Токен</param>
        /// <param name="email">Почта</param>
        /// <returns></returns>
        public BaseResponse<Guid> ConfrimUser(Guid token, string email)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(_ => _.Email == email && _.ConfirmationToken == token);
                if (user != null)
                {
                    user.Status = (int)EnumStatusUser.Active;
                    user.ConfirmationToken = null;
                    user.TokenExpireDate = null;
                    db.SaveChanges();
                    return new BaseResponse<Guid>(EnumResponseStatus.Success, "Успешно", user.Id);
                }
                return new BaseResponse<Guid>(EnumResponseStatus.Error);
            }
        }
        /// <summary>
        /// Подтверждение регистрации аккаунта пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse ConfrimRegisterUser(UserDetailModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var user = db.Users.FirstOrDefault(_ => _.Id == model.Id);
                    if (user != null)
                    {
                        //user.Phone = model.Phone;
                        //user.DeliveryAddress = model.DeliveryAddress;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.DateBirth = model.DateBirth;
                        db.SaveChanges();
                        return new BaseResponse(EnumResponseStatus.Success, "Успешно");
                    }
                    return new BaseResponse(EnumResponseStatus.Error);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Сгенерировать токен пользователю
        /// </summary>
        /// <param name="email">Почта</param>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="isExpireDate">Установить токену срок действия</param>
        /// <returns></returns>
        public BaseResponse<Guid?> GenerateToken(string email, Guid? userId = null, bool isExpireDate = true)
        {
            using (var db = new DataContext())
            {
                var user = userId.HasValue ? db.Users.FirstOrDefault(x => x.Id == userId) : db.Users.FirstOrDefault(x => x.Email == email);
                if (user != null)
                {
                    user.ConfirmationToken = Guid.NewGuid();

                    if (isExpireDate)
                        user.TokenExpireDate = DateTime.Now.AddHours(1);
                    db.SaveChanges();
                    return new BaseResponse<Guid?>(user.ConfirmationToken);
                }
                return new BaseResponse<Guid?>(EnumResponseStatus.Error);
            }
        }

        /// <summary>
        /// Отправка письма на эл.адрес пользователя
        /// </summary>
        /// <param name="subject">Тема письма</param>
        /// <param name="email">Эл.адрес</param>
        /// <param name="body">Текст письма</param>
        public BaseResponse SendMail(string subject, string email, string body)
        {
            try
            {
                var msg = new MailMessage();
                msg.To.Add(email);
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;
                var smtp = new SmtpClient();
                smtp.Send(msg);
                return new BaseResponse(EnumResponseStatus.Success, "Письмо успешно отправлено");
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }
        /// <summary>
        /// Проверка электроной почты
        /// </summary>
        /// <param name="email">Эл.адрес</param>
        public BaseResponse CheckExistEmail(string email)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var user = db.Users.FirstOrDefault(x => x.Email == email);
                    if (user == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Пользователь с такой почтой не найден");
                    return new BaseResponse(EnumResponseStatus.Success, "Пользователь существует");
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
