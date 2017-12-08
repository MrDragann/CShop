using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.Data.Models
{
    /// <summary>
    /// Таблица слайдера
    /// </summary>
    public class Slider
    {
        #region [ Свойства ]

        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Страница (EnumSitePage)
        /// </summary>
        public int SitePage { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Приоритет
        /// </summary>
        public int? Priority { get; set; }

        /// <summary>
        /// Url изображения
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Видимость
        /// </summary>
        public bool IsActive { get; set; }

        #endregion
    }
}
