using System.ComponentModel;
using System.Data.SqlTypes;
using System.Reflection.Metadata.Ecma335;

namespace InternetShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string Info { get; set; }
        public int Amount { get; set; }
        public int Price { get; set; }

    }
}
