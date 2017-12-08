namespace CosmeticaShop.IServices.Models.User
{
    public class UserLoginModel
    {
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Запомнить
        /// </summary>
        public bool IsRemember { get; set; }
    }
}
