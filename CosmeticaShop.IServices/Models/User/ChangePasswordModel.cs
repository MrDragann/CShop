namespace CosmeticaShop.IServices.Models.User
{
    /// <summary>
    /// Модель смены пароля
    /// </summary>
    public class ChangePasswordModel
    {
        /// <summary>
        /// Старый пароль
        /// </summary>
        public string OldPassword { get; set; }
        /// <summary>
        /// Новый пароль
        /// </summary>
        public string NewPassword { get; set; }
    }
}
