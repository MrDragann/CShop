using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Responses;

namespace CosmeticaShop.IServices.Models.Category
{
    public class CategoryEditModel
    {
        public CategoryModel Category { get; set; }

        public List<BaseModel> Categories { get; set; }

        public List<CategoryModel> AllCategories { get; set; }
    }
}
