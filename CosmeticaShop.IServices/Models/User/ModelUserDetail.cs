using System;
using System.Collections.Generic;

namespace CosmeticaShop.IServices.Models.User
{
	/// <summary>
	/// Детальная модель пользователя
	/// </summary>
	public class ModelUserDetail :ModelUserBase
	{
		/// <summary>
		/// Дата регистрации
		/// </summary>
		public DateTime RegistrationDate { get; set; }
		/// <summary>
		/// Дата строкой
		/// </summary>
		public string StringDate => RegistrationDate.ToShortDateString();
		/// <summary>
		/// Имя статуса
		/// </summary>
		public string StatusName { get; set; }
		/// <summary>
		/// Статус (EnumStatusUser)
		/// </summary>
		public int Status { get; set; }
		/// <summary>
		/// Список Id ролей пользователя
		/// </summary>
		public List<Guid> RolesId { get; set; }
		/// <summary>
		/// Телефон
		/// </summary>
		public string Phone { get; set; }
		/// <summary>
		/// Адрес доставки
		/// </summary>
		public string DeliveryAddress { get; set; }
		/// <summary>
		/// Дата рождения
		/// </summary>
		public DateTime DateBirth { get; set; }
	}
}
