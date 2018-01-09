using System.Collections.Generic;

namespace CosmeticaShop.IServices.Models.Navigation
{
    public class NavigationItemModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string KeyUrl { get; set; }

        public List<NavigationItemModel> ChildItems { get; set; }
    }
}
