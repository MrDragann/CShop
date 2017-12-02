using System;
using System.Collections.Generic;

namespace CosmeticaShop.Data.Models
{
    /// <summary>
    /// Таблица пользователей
    /// </summary>
    public class User
    {
        #region [ Свойства ]

        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string PasswordHash { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Статус пользователя (EnumStatusUser)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Дата роджения
        /// </summary>
        public DateTime? DateBirth { get; set; }
        
        /// <summary>
        /// Токен для восстановления пароля или активации пользователя
        /// </summary>
        public Guid? ConfirmationToken { get; set; }
        /// <summary>
        /// Время истечения срока действия токена
        /// </summary>
        public DateTime? TokenExpireDate { get; set; }

        #endregion

        #region [ Связанные объекты ]

        /// <summary>
        /// Связь с ролями
        /// </summary>
        public List<Role> Roles { get; set; }

        /// <summary>
        /// Связь с заказами
        /// </summary>
        public List<OrderHeader> OrderHeaders { get; set; } 

        /// <summary>
        /// Связь со списком желаемого
        /// </summary>
        public List<WishList> WishLists { get; set; }

        #endregion
    }
}
