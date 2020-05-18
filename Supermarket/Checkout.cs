using Supermarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Supermarket
{
    public class Checkout
    {
        private readonly Skus skus;
        private readonly List<Pricing> offers;
        
        public Checkout(Skus skus, List<Pricing> offers)
        {
            this.skus = skus;
            this.offers = offers;
        }

        public Kart Scan(Kart kart, Sku sku, int items)
        {
            var skuInStock = skus.Find(sku.ItemName);
            var checkedOutSkuUnits = kart.GetSkuUnits(sku);
            var noOfferPrice = sku.Price * items;

            decimal offerPrice = 0;
            bool offerApplies = false;
            int existingSkuItemsInKart = 0;
            kart.Cart.TryGetValue(sku, out existingSkuItemsInKart);

            // Get all offers that contains current added sku
            var applicableOffers = offers.Where(o => o.Condition.FirstOrDefault(su => su.Sku == sku) != null);

            foreach (var offer in applicableOffers)
            {
                var applicableOfferExisting = offer.Condition.SingleOrDefault(su => su.Sku == sku);
                // Check if adding current sku items triggers offer
                if (applicableOfferExisting == null)
                {
                    continue; // Offer does not apply
                }
                if (applicableOfferExisting.Units > existingSkuItemsInKart + items)
                {
                    continue; // Offer does not applu
                }

                //Current added sku can trigger offer, now check other skus that make up offer
                var applicableOfferOther = offer.Condition.Where(su => su.Sku != sku);

                // Check other skus already in kart will trigger offer.
                if (applicableOfferOther.All(o => kart.Cart.ContainsKey(o.Sku)) == false)
                {
                    continue;
                }

                // Check other skus already in kart has enough items to trigger offer.
                foreach(var other in applicableOfferOther)
                {
                    if (other.Units > kart.Cart[other.Sku])
                    {
                        continue;
                    }
                }

                // Finally, we know offer applies
                if (offerPrice > offer.OfferPrice)
                {
                    offerPrice = offer.OfferPrice;
                }
                offerApplies = true;
            }

            kart.AddOrUpdate(sku, items);
            if (offerApplies)
                kart.Total += Math.Min(noOfferPrice, offerPrice);
            else
                kart.Total += noOfferPrice;
            return kart;
        }
    }
}
