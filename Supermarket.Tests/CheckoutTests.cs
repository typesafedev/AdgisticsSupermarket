using Shouldly;
using Supermarket.Entities;
using System.Collections.Generic;
using Xunit;

namespace Supermarket.Tests
{
    public class CheckoutTests
    {
        [Fact]
        public void ShouldCheckoutNothing()
        {
            var a = new Sku { ItemName = "A", Price = 5 };
            var b = new Sku { ItemName = "B", Price = 3 };
            var offers = new List<Pricing> { new Pricing { Condition = new List<SkuUnits> { new SkuUnits { Sku = a, Units = 2 }, new SkuUnits { Sku = b, Units = 1 } } } };
            var skus = new Skus();
            skus.AddOrUpdate(a);
            skus.AddOrUpdate(b);
            var emptyBasket = new List<(Sku sku, int items)>();
            var kart = new Kart();
            var sut = new Checkout(skus, offers);

            foreach (var i in emptyBasket)
            {
                kart = sut.Scan(kart, i.sku, i.items);
            }

            kart.Total.ShouldBe(0m);
        }

        [Fact]
        public void ShouldCheckout1A()
        {
            var a = new Sku { ItemName = "A", Price = 5 };
            var b = new Sku { ItemName = "B", Price = 3 };
            var offers = new List<Pricing> { new Pricing { Condition = new List<SkuUnits> { new SkuUnits { Sku = a, Units = 2 }, new SkuUnits { Sku = b, Units = 1 } } } };
            var skus = new Skus();
            skus.AddOrUpdate(a);
            skus.AddOrUpdate(b);
            var basket = new List<(Sku sku, int items)>();
            basket.AddRange(new[]
            {
                (skus.Find("A"), 1),
            });
            var kart = new Kart();
            var sut = new Checkout(skus, offers);

            foreach (var i in basket)
            {
                kart = sut.Scan(kart, i.sku, i.items);
            }

            kart.Total.ShouldBe(5m);
        }

        [Fact]
        public void ShouldCheckout2A()
        {
            var a = new Sku { ItemName = "A", Price = 5 };
            var b = new Sku { ItemName = "B", Price = 3 };
            var offers = new List<Pricing> { new Pricing { Condition = new List<SkuUnits> { new SkuUnits { Sku = a, Units = 2 }, new SkuUnits { Sku = b, Units = 1 } } } };
            var skus = new Skus();
            skus.AddOrUpdate(a);
            skus.AddOrUpdate(b);
            var basket = new List<(Sku sku, int items)>();
            basket.AddRange(new[]
            {
                (skus.Find("A"), 2),
            });
            var kart = new Kart();
            var sut = new Checkout(skus, offers);

            foreach (var i in basket)
            {
                kart = sut.Scan(kart, i.sku, i.items);
            }

            kart.Total.ShouldBe(10m);
        }

        [Fact]
        public void ShouldCheckout3A()
        {
            var a = new Sku { ItemName = "A", Price = 5 };
            var b = new Sku { ItemName = "B", Price = 3 };
            var offers = new List<Pricing> { new Pricing { Condition = new List<SkuUnits> { new SkuUnits { Sku = a, Units = 2 }, new SkuUnits { Sku = b, Units = 1 } } } };
            var skus = new Skus();
            skus.AddOrUpdate(a);
            skus.AddOrUpdate(b);
            var basket = new List<(Sku sku, int items)>();
            basket.AddRange(new[]
            {
                (skus.Find("A"), 3),
            });
            var kart = new Kart();
            var sut = new Checkout(skus, offers);

            foreach (var i in basket)
            {
                kart = sut.Scan(kart, i.sku, i.items);
            }

            kart.Total.ShouldBe(15m);
        }

        [Fact]
        public void ShouldCheckout2A1B()
        {
            var a = new Sku { ItemName = "A", Price = 5 };
            var b = new Sku { ItemName = "B", Price = 3 };
            var offers = new List<Pricing> { new Pricing { Condition = new List<SkuUnits> { new SkuUnits { Sku = a, Units = 2 }, new SkuUnits { Sku = b, Units = 1 } } } };
            var skus = new Skus();
            skus.AddOrUpdate(a);
            skus.AddOrUpdate(b);
            var basket = new List<(Sku sku, int items)>();
            basket.AddRange(new[]
            {
                (skus.Find("A"), 2),
                (skus.Find("B"), 1),
            });
            var kart = new Kart();
            var sut = new Checkout(skus, offers);

            foreach (var i in basket)
            {
                kart = sut.Scan(kart, i.sku, i.items);
            }

            kart.Total.ShouldBe(10m);
        }
    }
}
