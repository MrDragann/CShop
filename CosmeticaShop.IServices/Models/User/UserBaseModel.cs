using System;
using System.Collections.Generic;

namespace CosmeticaShop.IServices.Models.User
{
	/// <summary>
	/// Базовая модель пользователя
	/// </summary>
	public class UserBaseModel
	{
		/// <summary>
		/// Ид пользователя
		/// </summary>
		public Guid Id { get; set; }
		/// <summary>
		/// Почта пользователя
		/// </summary>
		public string Email { get; set; }
		/// <summary>
		/// Пароль пользователя
		/// </summary>
		public string Password { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName { get; set; }
	    /// <summary>
	    /// Список имен ролей пользователя
	    /// </summary>
	    public List<string> Roles { get; set; }
    }
}
