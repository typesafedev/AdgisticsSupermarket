namespace Supermarket.Entities
{
    public class Sku
    {
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public Offer Offer { get; set; }

    }
}
