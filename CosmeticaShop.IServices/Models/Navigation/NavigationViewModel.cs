using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.IServices.Models.Navigation
{
    public class NavigationViewModel
    {
        public NavigationModel Brand { get; set; }

        public List<NavigationModel> Categories { get; set; }
    }
}
