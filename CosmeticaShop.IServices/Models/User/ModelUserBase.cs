using System;

namespace CosmeticaShop.IServices.Models.User
{
	/// <summary>
	/// Базовая модель пользователя
	/// </summary>
	public class ModelUserBase
	{
		/// <summary>
		/// Ид пользователя
		/// </summary>
		public int Id { get; set; }
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
    }
}
