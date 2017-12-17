using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.IServices.Models.Pagination
{
    public class PaginationModel
    {
        /// <summary>
        /// Количество записей
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Пропуск записей
        /// </summary>
        public int Skip { get; set; }
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Количество страниц
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Взять записей
        /// </summary>
        public int Take { get; set; }

        public PaginationModel(int take)
        {
            Take = take;
        }
    }
}
