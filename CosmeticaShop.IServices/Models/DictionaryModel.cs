using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.IServices.Models
{
    public class DictionaryModel
    {
        public DictionaryModel() { }

        public DictionaryModel(int id, string value)
        {
            Id = id;
            Name = value;
        }
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
