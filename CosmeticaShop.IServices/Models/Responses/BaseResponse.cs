using CosmeticaShop.IServices.Enums;

namespace CosmeticaShop.IServices.Models.Responses
{
    /// <summary>
    /// Базовый ответ сервера
    /// </summary>
    public class BaseResponse
    {
        public BaseResponse() { }

        public BaseResponse(EnumResponseStatus status, string message)
        {
            // подозреваю, что в JS вернется строка если не привести к целому
            Status = (int)status;
            Message = message;
        }

        public BaseResponse(EnumResponseStatus status)
        {
            Status = (int)status;
            Message = status == 0
                ? "Запрос успешно выполнен."
                : "Ошибка при выполнении запроса.";
        }
        public int Status { get; set; }

        public string Message { get; set; }

        public bool IsSuccess => Status == (int)EnumResponseStatus.Success;
    }

    public class BaseResponse<T> : BaseResponse
    {
        public BaseResponse() { }

        public BaseResponse(EnumResponseStatus status, string message) : base(status, message) { }

        public BaseResponse(EnumResponseStatus status) : base(status) { }

        public BaseResponse(T value)
        {
            Status = 0;
            Message = "Запрос успешно выполнен.";
            Value = value;
        }

        public BaseResponse(EnumResponseStatus status, T value) : base(status)
        {
            Value = value;
        }

        public BaseResponse(EnumResponseStatus status, string message, T value) : base(status)
        {
            Message = message;
            Value = value;
        }
        
        public T Value { get; set; }
    }
}
