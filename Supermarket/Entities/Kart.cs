﻿using System.Collections.Generic;

namespace Supermarket.Entities
{
    /// <summary>
    /// Represents a checked out cart
    /// </summary>
    public class Kart
    {
        public Dictionary<Sku, int> Cart { get; set; } = new Dictionary<Sku, int>();
        public decimal Total { get; set; } = 0;

        public void AddOrUpdate(Sku sku, int items)
        {
            if (Cart.ContainsKey(sku))
            {
                Cart[sku] = items;
                return;
            }

            Cart.Add(sku, items);
        }

        public int GetItems(Sku sku)
        {
            int items;
            if (Cart.TryGetValue(sku, out items))
            {
                return items;
            }

            return items;
        }

        public SkuUnits GetSkuUnits(Sku sku)
        {
            if (Cart.ContainsKey(sku))
            {
                return new SkuUnits { Sku = sku, Units = Cart[sku] };
            }

            return null;
        }

    }
}
