using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.IServices.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Изменить личные данные пользователя
        /// </summary>
        /// <param name="model">Модель пользователя</param>
        /// <returns></returns>
        BaseResponse EditPersonData(UserDetailModel model);
        /// <summary>
        /// Получить информацию о пользователе 
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <returns></returns>
        UserDetailModel GetUser(Guid userId);

        /// <summary>
        /// Установить пользователя в куки
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <returns></returns>
        void SetUserCookie(Guid userId);

        /// <summary>
        /// Полчить пользователя в куки
        /// </summary>
        /// <returns></returns>
        BaseResponse<UserBaseModel> GetUserCookie();
    }
}
