using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.User;

namespace CosmeticaShop.IServices.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса авторизации
    /// </summary>
    public interface IAuthCommonService
    {
        #region Авторизация
        /// <summary>
        /// Проверить токен пользователя 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        BaseResponse CheckUserToken(string email, Guid token);

        /// <summary>
        /// Метод выполняющий авторизацю пользователя
        /// </summary>
        /// <param name="email">Почта</param>
        /// <param name="passwordHash">Пароль пользователя</param>
        BaseResponse<UserBaseModel> Login(string email, string passwordHash);

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="model">Модель пользователя</param>
        /// <param name="token">Токен для потверждения</param>
        /// <returns></returns>
        BaseResponse Register(UserDetailModel model, Guid token);

        /// <summary>
        /// Подтверждение аккаунта пользователя
        /// </summary>
        /// <param name="token">Токен</param>
        /// <param name="email">Почта</param>
        /// <returns></returns>
        BaseResponse<int> ConfrimUser(Guid token, string email);

        /// <summary>
        /// Сгенерировать токен пользователю
        /// </summary>
        /// <param name="email">Почта</param>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="isExpireDate">Установить токену срок действия</param>
        /// <returns></returns>
        BaseResponse<Guid?> GenerateToken(string email, int? userId = null, bool isExpireDate = true);

        /// <summary>
        /// Обновление пароля пользователя
        /// </summary>
        /// <param name="email">Почта</param>
        /// <param name="token">Токен пользователя</param>
        /// <param name="passwordHash">Хешированный пароль</param>
        BaseResponse RestorePassword(string email, Guid token, string passwordHash);

        /// <summary>
        /// Отправка письма на эл.адрес пользователя
        /// </summary>
        /// <param name="subject">Тема письма</param>
        /// <param name="email">Эл.адрес</param>
        /// <param name="body">Текст письма</param>
        BaseResponse SendMail(string subject, string email, string body);

        /// <summary>
        /// Подтверждение регистрации аккаунта пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse ConfrimRegisterUser(UserDetailModel model);

        #endregion
    }
}
