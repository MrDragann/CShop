using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CosmeticaShop.IServices.Models;
using CosmeticaShop.IServices.Models.User;

namespace CosmeticaShop.Web.Models
{
    public class CabinetViewModel
    {
        public UserDetailModel User { get; set; }
        public List<DictionaryModel> Cities { get; set; }
    }
}