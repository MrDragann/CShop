using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Base;
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
                    return new BaseResponse(0, "Пароль успешно изменен");
                }
                return new BaseResponse(1);
            }
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="model">Модель пользователя</param>
        /// <param name="token">Токен для потверждения</param>
        /// <returns></returns>
        public BaseResponse Register(ModelUserDetail model, Guid token)
        {
            try
            {
                using (var db = new DataContext())
                {
                    if (string.IsNullOrEmpty(model.Email))
                        return new BaseResponse(1, "Email не может быть пустым");


                    if (db.Users.Any(x => x.Email == model.Email))
                        return new BaseResponse(1, "Пользователь с таким адресом электронной почты уже существует");

                    if (string.IsNullOrEmpty(model.Password))
                        return new BaseResponse(1, "Пароль не может быть пустым");

                    var user = new User
                    {
                        Email = model.Email,
                        PasswordHash = model.Password,
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
                return new BaseResponse(1, ex.Message);
            }
        }

        /// <summary>
        /// Метод выполняющий авторизацю пользователя
        /// </summary>
        /// <param name="email">Почта</param>
        /// <param name="passwordHash">Пароль пользователя</param>
        public BaseResponse<ModelUserBase> Login(string email, string passwordHash)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.FirstOrDefault(_ => _.Email == email && _.PasswordHash == passwordHash);
                if (user != null)
                {
                    if (user.Status != (int)EnumStatusUser.Active)
                        return new BaseResponse<ModelUserBase>(1, "Пользователь не активирован");
                    var webUser = new ModelUserBase
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    };

                    return new BaseResponse<ModelUserBase>(webUser);
                }
                return new BaseResponse<ModelUserBase>(2, "Введен неверный Email или пароль");
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
                    return new BaseResponse(0);
                }
                return new BaseResponse(1);
            }
        }

        /// <summary>
        /// Подтверждение аккаунта пользователя
        /// </summary>
        /// <param name="token">Токен</param>
        /// <param name="email">Почта</param>
        /// <returns></returns>
        public BaseResponse<int> ConfrimUser(Guid token, string email)
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
                    return new BaseResponse<int>(0, "Успешно", user.Id);
                }
                return new BaseResponse<int>(1);
            }
        }
        /// <summary>
        /// Подтверждение регистрации аккаунта пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse ConfrimRegisterUser(ModelUserDetail model)
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
                        return new BaseResponse(0, "Успешно");
                    }
                    return new BaseResponse(1);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(2, ex.Message);
            }
        }

        /// <summary>
        /// Сгенерировать токен пользователю
        /// </summary>
        /// <param name="email">Почта</param>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="isExpireDate">Установить токену срок действия</param>
        /// <returns></returns>
        public BaseResponse<Guid?> GenerateToken(string email, int? userId = null, bool isExpireDate = true)
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
                return new BaseResponse<Guid?>(1);
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
                return new BaseResponse(0, "Письмо успешно отправлено");
            }
            catch (Exception ex)
            {
                return new BaseResponse(1, ex.Message);
            }
        }
        #endregion
    }
}
