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
                    user.PasswordHash = model.Password.GetHashString();
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
                DateMonth =  m.DateBirth?.Month,
                DateYear = m.DateBirth?.Year
            };
        }
        #endregion
    }
}
