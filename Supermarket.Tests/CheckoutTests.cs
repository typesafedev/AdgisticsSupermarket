using Moq;
using Shouldly;
using Supermarket.Entities;
using System;
using System.Collections.Generic;
using Xunit;

namespace Supermarket.Tests
{
    public class CheckoutTests
    {
        [Fact]
        public void ShouldCheckoutEmptyBasket()
        {
            var skus = new Skus();
            var basket = new List<(Sku sku, int items)>();
            var pricer = new Pricer();
            var kart = new Kart();
            var sut = new Checkout(pricer, skus);

            foreach(var b in basket)
            {
                kart = sut.Scan(kart, b.sku, b.items);
            }

            kart.Total.ShouldBe(0);
        }

        [Fact]
        public void ShouldCheckoutSingleItemInBasket()
        {
            var skus = new Skus();
            skus.AddOrUpdate(new Sku { ItemName = "A", Price = 5, Offer = new Pricing { Units = 3, OfferPrice = 13 } });
            skus.AddOrUpdate(new Sku { ItemName = "B", Price = 3, Offer = new Pricing { Units = 3, OfferPrice = 4.5m } });
            skus.AddOrUpdate(new Sku { ItemName = "C", Price = 2 });
            skus.AddOrUpdate(new Sku { ItemName = "D", Price = 1.5m });
            var basket = new List<(Sku sku, int items)>();
            basket.AddRange(new[] 
            { 
                (skus.Find("A"), 1), 
                (skus.Find("B"), 1), 
                (skus.Find("C"), 1), 
                (skus.Find("D"), 1) 
            });
            var pricer = new Pricer();
            var kart = new Kart();
            var sut = new Checkout(pricer, skus);

            foreach (var b in basket)
            {
                kart = sut.Scan(kart, b.sku, b.items);
            }

            kart.Total.ShouldBe(11.5m);
        }

        [Fact]
        public void ShouldCheckoutMultipleItemsInBasketNoOffer()
        {
            var skus = new Skus();
            skus.AddOrUpdate(new Sku { ItemName = "A", Price = 5, Offer = new Pricing { Units = 3, OfferPrice = 13 } });
            skus.AddOrUpdate(new Sku { ItemName = "B", Price = 3, Offer = new Pricing { Units = 3, OfferPrice = 4.5m } });
            skus.AddOrUpdate(new Sku { ItemName = "C", Price = 2 });
            skus.AddOrUpdate(new Sku { ItemName = "D", Price = 1.5m });
            var basket = new List<(Sku sku, int items)>();
            basket.AddRange(new[] 
            { 
                (skus.Find("A"), 2), 
                (skus.Find("B"), 2), 
                (skus.Find("C"), 2), 
                (skus.Find("D"), 2),
            });
            var pricer = new Pricer();
            var kart = new Kart();

            var sut = new Checkout(pricer, skus);

            foreach (var b in basket)
            {
                kart = sut.Scan(kart, b.sku, b.items);
            }

            kart.Total.ShouldBe(23m);
        }

        [Fact]
        public void ShouldCheckoutMultipleItemsInBasketWithOffer()
        {
            var skus = new Skus();
            skus.AddOrUpdate(new Sku { ItemName = "A", Price = 5, Offer = new Pricing { Units = 3, OfferPrice = 13 } });
            skus.AddOrUpdate(new Sku { ItemName = "B", Price = 3, Offer = new Pricing { Units = 3, OfferPrice = 4.5m } });
            skus.AddOrUpdate(new Sku { ItemName = "C", Price = 2 });
            skus.AddOrUpdate(new Sku { ItemName = "D", Price = 1.5m });
            var basket = new List<(Sku sku, int items)>();
            basket.AddRange(new[]
            {
                (skus.Find("A"), 3),
                (skus.Find("B"), 3),
                (skus.Find("C"), 2),
                (skus.Find("D"), 2),
            });
            var pricer = new Pricer();
            var kart = new Kart();
            var sut = new Checkout(pricer, skus);

            foreach (var b in basket)
            {
                kart = sut.Scan(kart, b.sku, b.items);
            }

            kart.Total.ShouldBe(24.5m);
        }

        [Fact]
        public void ShouldCheckoutMultipleItemsInBasketSplitOffer()
        {
            var skus = new Skus();
            skus.AddOrUpdate(new Sku { ItemName = "A", Price = 5, Offer = new Pricing { Units = 3, OfferPrice = 13 } });
            skus.AddOrUpdate(new Sku { ItemName = "B", Price = 3, Offer = new Pricing { Units = 3, OfferPrice = 4.5m } });
            skus.AddOrUpdate(new Sku { ItemName = "C", Price = 2 });
            skus.AddOrUpdate(new Sku { ItemName = "D", Price = 1.5m });
            var basket = new List<(Sku sku, int items)>();
            basket.AddRange(new[]
            {
                (skus.Find("A"), 3),
                (skus.Find("B"), 4),
                (skus.Find("C"), 2),
                (skus.Find("D"), 2),
                (skus.Find("B"), 2), // Some B's checked out previously. Offer should still be applied
                (skus.Find("A"), 3), // Some A's checked out previously. Offer should still be applied
                (skus.Find("D"), 2), // Some D's checked out previously. D has no offers
            });
            var pricer = new Pricer();
            var kart = new Kart();
            var sut = new Checkout(pricer, skus);

            foreach (var b in basket)
            {
                kart = sut.Scan(kart, b.sku, b.items);
            }

            kart.Total.ShouldBe(45m);
        }

        [Fact]
        public void ShouldThrowIfSkuNotFound()
        {
            var skus = new Skus();
            var basket = new List<(Sku sku, int items)>();
            basket.Add((new Sku { ItemName = "A", Price = 5, Offer = new Pricing { Units = 3, OfferPrice = 13 } }, 1));
            var pricer = new Mock<IPricer>();
            pricer.Setup(p => p.DifferencePrice(It.IsAny<Sku>(), It.IsAny<int>(), It.IsAny<int>())).Returns(1);
            var kart = new Kart();
            var sut = new Checkout(pricer.Object, skus);
            Exception ex = null;

            ex = Assert.Throws<KeyNotFoundException>(() => kart = sut.Scan(kart, basket[0].sku, basket[0].items));
        }
    }
}
