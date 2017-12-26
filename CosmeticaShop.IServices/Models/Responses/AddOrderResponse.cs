using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.IServices.Models.Responses
{
   public class AddOrderResponse
    {
        public bool IsNewUser { get; set; }
        public Guid? Token { get; set; }
        public  string Password { get; set; }
    }
}
