using System.Collections.Generic;
using CosmeticaShop.IServices.Enums;

namespace CosmeticaShop.IServices.Models.Responses
{
    /// <summary>
    /// Ответ для постранички
    /// </summary>
    /// <typeparam name="T">Тип записей</typeparam>
    public class PaginationResponse<T> : BaseResponse
    {
        public PaginationResponse() { }
        public PaginationResponse(EnumResponseStatus status) : base(status) { }
        public PaginationResponse(EnumResponseStatus status, string message) : base(status, message) { }

        /// <summary>
        /// Элементы постранички
        /// </summary>
        public List<T> Items { get; set; }
        /// <summary>
        /// Количество записей всего
        /// </summary>
        public int Count { get; set; }
    }
}
