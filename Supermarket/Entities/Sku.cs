using System.Collections.Generic;

namespace Supermarket.Entities
{
    public class Sku
    {
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public ICollection<Pricing> Offers { get; set; } = new List<Pricing>();
    }
}
