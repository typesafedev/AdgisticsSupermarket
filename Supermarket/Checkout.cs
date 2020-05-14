using Supermarket.Entities;

namespace Supermarket
{
    public class Checkout
    {
        private readonly IPricer pricer;
        private readonly Skus skus;
        
        public Checkout(IPricer pricer, Skus skus)
        {
            this.pricer = pricer;
            this.skus = skus;
        }

        public Kart Scan(Kart kart, Sku sku, int items)
        {
            var checkedoutItems = kart.GetItems(sku);
            var skuInStock = skus.Find(sku.ItemName);
            var price = pricer.DifferencePrice(skuInStock, checkedoutItems, items);
            kart.AddOrUpdate(sku, items);
            kart.Total += price;

            return kart;
        }
    }
}
