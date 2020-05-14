namespace Supermarket.Entities
{
    public class Sku
    {
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public Pricing Offer { get; set; }
    }
}
