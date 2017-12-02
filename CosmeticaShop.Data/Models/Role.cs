using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.Data.Models
{
    public class Role
    {
        #region [ Свойства ]

        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region [ Связанные объекты ]

        /// <summary>
        /// Связь с пользователями
        /// </summary>
        public List<User> Users { get; set; }

        #endregion
    }
}
