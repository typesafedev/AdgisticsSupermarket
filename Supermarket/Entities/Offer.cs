using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Supermarket.Entities
{
    public class Pricing
    {
        public decimal OfferPrice { get; set; }
        public List<SkuUnits> Condition { get; set; } = new List<SkuUnits>();
    }

    public class SkuUnits
    {
        public Sku Sku { get; set; }
        public int Units { get; set; }
    }
}
