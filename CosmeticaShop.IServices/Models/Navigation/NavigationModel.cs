using System.Collections.Generic;

namespace CosmeticaShop.IServices.Models.Navigation
{
    public class NavigationModel
    {
        public string Title { get; set; }

        public List<NavigationItemModel> Items { get; set; }
    }
}
