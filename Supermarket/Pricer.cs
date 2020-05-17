using Supermarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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

            var pricings = new List<Pricing>();
            pricings.Add(new Pricing { OfferPrice = sku.Price, Units = 1 });
            pricings.AddRange(sku.Offers);

            var sortedPricing = pricings.OrderBy(p => p.UnitPrice);

            int remainingUnits = units;
            decimal price = 0;
            foreach (var p in sortedPricing)
            {
                var pricedUnits = remainingUnits / p.Units;
                remainingUnits = remainingUnits % p.Units;
                price += pricedUnits * p.OfferPrice;
            }

            return price;
        }

        public decimal DifferencePrice(Sku sku, int checkedoutUnits, int newUnits)
        {
            var checkedoutSubTotal = this.Price(sku, checkedoutUnits);
            var newSubTotal = this.Price(sku, newUnits + checkedoutUnits);
            return newSubTotal - checkedoutSubTotal;
        }
    }
}
