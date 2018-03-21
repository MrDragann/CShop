using System;
using System.Linq;
using System.Net.Mail;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.User;
using Resources;

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
                    return new BaseResponse(EnumResponseStatus.Success, "Пароль успешно изменен");
                }
                return new BaseResponse(EnumResponseStatus.Error);
            }
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="model">Модель пользователя</param>
        /// <param name="token">Токен для потверждения</param>
        /// <returns></returns>
        public BaseResponse Register(UserDetailModel model, Guid token)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var users = db.Users.Include(x=>x.UserAddress);
                    if (string.IsNullOrEmpty(model.Email))
                        return new BaseResponse(EnumResponseStatus.ValidationError, "Email не может быть пустым");


                    if (users.Any(x => x.Email == model.Email))
                        return new BaseResponse(EnumResponseStatus.ValidationError, "Пользователь с таким адресом электронной почты уже существует");

                    if (string.IsNullOrEmpty(model.Password))
                        return new BaseResponse(EnumResponseStatus.ValidationError, "Пароль не может быть пустым");
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["User"];
                    Guid userId;
                    if (string.IsNullOrWhiteSpace(cookie?.Value))
                    {

                    }
                    else
                    {
                        userId = Guid.Parse(cookie.Value);
                        var cookieUser = users.FirstOrDefault(x => x.Id == userId && x.Status == (int)EnumStatusUser.Unauthorized);
                        if (cookieUser != null)
                        {
                            cookieUser.Email = model.Email;
                            cookieUser.FirstName = model.FirstName;
                            cookieUser.LastName = model.LastName;
                            cookieUser.PasswordHash = model.Password;
                            cookieUser.DateBirth = model.DateBirth;
                            cookieUser.ConfirmationToken = token;
                            cookieUser.Status = (int)EnumStatusUser.New;
                            cookieUser.UserAddress.Address = model.Address;
                            cookieUser.UserAddress.City = model.City;
                            cookieUser.UserAddress.Country = "România";
                            cookieUser.UserAddress.Phone = model.Phone;
                            db.SaveChanges();
                            return new BaseResponse(EnumResponseStatus.Success, "Регистрация успешно выполнена");
                        }
                    }
                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PasswordHash = model.Password,
                        DateBirth = model.DateBirth,
                        ConfirmationToken = token,
                        Status = (int)EnumStatusUser.New,
                        UserAddress = new UserAddress
                        {
                            Country = "România",
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
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Метод выполняющий авторизацю пользователя
        /// </summary>
        /// <param name="email">Почта</param>
        /// <param name="passwordHash">Пароль пользователя</param>
        public BaseResponse<UserBaseModel> Login(string email, string passwordHash)
        {
            using (var db = new DataContext())
            {
                var user = db.Users.Include(x => x.Roles).FirstOrDefault(_ => _.Email == email && _.PasswordHash == passwordHash);
                if (user != null)
                {
                    if (user.Status != (int)EnumStatusUser.Active)
                        return new BaseResponse<UserBaseModel>(EnumResponseStatus.ValidationError, Resource.ConfirmRegistrationSite);
                    var webUser = new UserBaseModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Roles = user.Roles.Select(x => x.Name).ToList()
                    };

                    return new BaseResponse<UserBaseModel>(webUser);
                }
                return new BaseResponse<UserBaseModel>(EnumResponseStatus.ValidationError, Resource.WrongEmailOrPassword);
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
                    return new BaseResponse(EnumResponseStatus.Success);
                }
                return new BaseResponse(EnumResponseStatus.Error);
            }
        }

        /// <summary>
        /// Подтверждение аккаунта пользователя
        /// </summary>
        /// <param name="token">Токен</param>
        /// <param name="email">Почта</param>
        /// <returns></returns>
        public BaseResponse<Guid> ConfrimUser(Guid token, string email)
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
                    return new BaseResponse<Guid>(EnumResponseStatus.Success, "Успешно", user.Id);
                }
                return new BaseResponse<Guid>(EnumResponseStatus.Error);
            }
        }
        /// <summary>
        /// Подтверждение регистрации аккаунта пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponse ConfrimRegisterUser(UserDetailModel model)
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
                        return new BaseResponse(EnumResponseStatus.Success, "Успешно");
                    }
                    return new BaseResponse(EnumResponseStatus.Error);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }

        /// <summary>
        /// Сгенерировать токен пользователю
        /// </summary>
        /// <param name="email">Почта</param>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="isExpireDate">Установить токену срок действия</param>
        /// <returns></returns>
        public BaseResponse<Guid?> GenerateToken(string email, Guid? userId = null, bool isExpireDate = true)
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
                return new BaseResponse<Guid?>(EnumResponseStatus.Error);
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
                return new BaseResponse(EnumResponseStatus.Success, "Письмо успешно отправлено");
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }
        /// <summary>
        /// Проверка электроной почты
        /// </summary>
        /// <param name="email">Эл.адрес</param>
        public BaseResponse CheckExistEmail(string email)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var user = db.Users.FirstOrDefault(x => x.Email == email);
                    if (user == null)
                        return new BaseResponse(EnumResponseStatus.Error, "Пользователь с такой почтой не найден");
                    return new BaseResponse(EnumResponseStatus.Success, "Пользователь существует");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse(EnumResponseStatus.Exception, ex.Message);
            }
        }
        #endregion

        #region Криптография

        static byte[] key = Encoding.UTF8.GetBytes("Some salt value0Some salt value0");

        static byte[] IV = Encoding.UTF8.GetBytes("Some salt value0");


        public static string Encrypt(LoginModel model)
        {
            using (Aes myAes = Aes.Create())
            {
                myAes.Key = key;
                myAes.IV = IV;
                byte[] encrypted = EncryptStringToBytes_Aes($"{model.Email},{model.Password}", myAes.Key, myAes.IV);
                return new BigInteger(encrypted).ToString("x2");
            }
        }

        public static LoginModel Decrypt(string encrypted)
        {
            using (Aes myAes = Aes.Create())
            {
                try
                {
                    myAes.Key = key;
                    myAes.IV = IV;
                    var number = BigInteger.Parse(encrypted, NumberStyles.HexNumber);
                    string roundtrip = DecryptStringFromBytes_Aes(number.ToByteArray(), myAes.Key, myAes.IV);
                    var mas = roundtrip.Split(',');
                    var model = new LoginModel() { Email = mas[0], Password = mas[1] };
                    return model;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }


        #endregion
    }
}
