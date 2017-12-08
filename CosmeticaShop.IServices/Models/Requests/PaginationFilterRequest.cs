namespace CosmeticaShop.IServices.Models.Requests
{
    public class PaginationFilterRequest : PaginationRequest
    {
        /// <summary>
        /// Поисковый запрос
        /// </summary>
        public string Term { get; set; }
        /// <summary>
        /// Целое Ид
        /// </summary>
        public int? IntId { get; set; }
        /// <summary>
        /// Номер категории
        /// </summary>
        public int? CategoryId { get; set; }
        /// <summary>
        /// Номер подкатегории
        /// </summary>
        public int? ChildCategoryId { get; set; }
    }
}
