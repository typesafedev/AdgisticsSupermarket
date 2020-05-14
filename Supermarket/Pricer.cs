using Supermarket.Entities;
using System;

namespace Supermarket
{
    public interface IPricer
    {
        decimal Price(Sku sku, int units);
        decimal DifferencePrice(Sku sku, int pricedUnits, int newUnits);
    }

    public class Pricer : IPricer
    {
        public decimal Price(Sku sku, int units)
        {
            if (sku == null)
                throw new ArgumentNullException(nameof(Sku));

            var hasOffer = sku.Offer != null;
            if (hasOffer)
            {
                var discountedUnits = units / sku.Offer.Units;
                var fullPricedUnits = units % sku.Offer.Units;
                var discountedPrice = discountedUnits * sku.Offer.Price;
                var fullPrice = fullPricedUnits * sku.Price;
                return discountedPrice + fullPrice;
            }
            return units * sku.Price;
        }

        public decimal DifferencePrice(Sku sku, int checkedoutUnits, int newUnits)
        {
            var checkedoutSubTotal = this.Price(sku, checkedoutUnits);
            var newSubTotal = this.Price(sku, newUnits + checkedoutUnits);
            return newSubTotal - checkedoutSubTotal;
        }
    }
}
