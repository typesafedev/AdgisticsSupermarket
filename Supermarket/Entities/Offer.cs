using System.Runtime.InteropServices;

namespace Supermarket.Entities
{
    public class Pricing
    {
        public int Units { get; set; }
        public decimal OfferPrice { get; set; }
        public decimal UnitPrice => OfferPrice / Units;
    }
}
