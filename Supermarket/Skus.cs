using Supermarket.Entities;
using System.Collections.Generic;

namespace Supermarket
{
    /// <summary>
    /// ISkus is a collection of sku from some kind of store, normally db. 
    /// </summary>
    public interface ISkus
    {
        void AddOrUpdate(Sku sku);
        Sku Find(string itemName);
    }

    public class Skus : ISkus
    {
        private readonly Dictionary<string, Sku> skus = new Dictionary<string, Sku>();

        public Sku Find(string itemName) => skus[itemName];

        public void AddOrUpdate(Sku sku)
        {
            if (skus.ContainsKey(sku.ItemName))
            {
                skus[sku.ItemName] = sku;
                return;
            }

            skus.Add(sku.ItemName, sku);
        }
    }
}
