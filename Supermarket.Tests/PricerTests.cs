using Shouldly;
using Supermarket.Entities;
using System;
using Xunit;

namespace Supermarket.Tests
{
    public class PricerTests
    {
        [Fact]
        public void ShouldNotPriceSkuIfNullSku()
        {
            var sut = new Pricer();

            var ex = Assert.Throws<ArgumentNullException>(() => sut.Price(null, 1));

            ex.ParamName.ShouldBe(nameof(Sku));
        }

        [Theory]
        [InlineData("A", 1, 5)]
        [InlineData("A", 2, 10)]
        [InlineData("B", 1, 3)]
        [InlineData("C", 1, 2)]
        [InlineData("C", 2, 4)]
        [InlineData("D", 1, 1.5)]
        [InlineData("D", 2, 3)]
        public void ShouldPriceSkusNotOnOffer(string itemName, int units, decimal expectedPrice)
        {
            var skus = new Skus();
            skus.AddOrUpdate(new Sku { ItemName = "A", Price = 5, Offer = new Pricing { Units = 3, OfferPrice = 13 } });
            skus.AddOrUpdate(new Sku { ItemName = "B", Price = 3, Offer = new Pricing { Units = 2, OfferPrice = 4.5m } });
            skus.AddOrUpdate(new Sku { ItemName = "C", Price = 2 });
            skus.AddOrUpdate(new Sku { ItemName = "D", Price = 1.5m });
            var sut = new Pricer();

            sut.Price(skus.Find(itemName), units).ShouldBe(expectedPrice);
        }

        [Theory]
        [InlineData("A", 3, 13)]
        [InlineData("A", 4, 18)]
        [InlineData("A", 5, 23)]
        [InlineData("A", 6, 26)]
        [InlineData("B", 2, 4.5)]
        [InlineData("B", 3, 7.5)]
        [InlineData("B", 4, 9)]
        public void ShouldPriceSkusOnOffer(string itemName, int units, decimal expectedPrice)
        {
            var skus = new Skus();
            skus.AddOrUpdate(new Sku { ItemName = "A", Price = 5, Offer = new Pricing { Units = 3, OfferPrice = 13 } });
            skus.AddOrUpdate(new Sku { ItemName = "B", Price = 3, Offer = new Pricing { Units = 2, OfferPrice = 4.5m } });
            skus.AddOrUpdate(new Sku { ItemName = "C", Price = 2 });
            skus.AddOrUpdate(new Sku { ItemName = "D", Price = 1.5m });
            var sut = new Pricer();

            sut.Price(skus.Find(itemName), units).ShouldBe(expectedPrice);
        }

        [Fact]
        public void ShouldKeepRunningSubtotalForSingleSkus()
        {
            var skus = new Skus();
            skus.AddOrUpdate(new Sku { ItemName = "A", Price = 5, Offer = new Pricing { Units = 3, OfferPrice = 13 } });
            skus.AddOrUpdate(new Sku { ItemName = "B", Price = 3, Offer = new Pricing { Units = 2, OfferPrice = 4.5m } });

            var kart = new Kart();
            var sut = new Pricer();

            int items;
            items = kart.GetItems(skus.Find("B"));
            sut.DifferencePrice(skus.Find("B"), items, 1).ShouldBe(3);
            kart.AddOrUpdate(skus.Find("B"), 1);

            items = kart.GetItems(skus.Find("A"));
            sut.DifferencePrice(skus.Find("A"), items, 1).ShouldBe(5);
            kart.AddOrUpdate(skus.Find("A"), items + 1);

            items = kart.GetItems(skus.Find("B"));
            sut.DifferencePrice(skus.Find("B"), items, 1).ShouldBe(1.5m);
            kart.AddOrUpdate(skus.Find("B"), items + 1);

            items = kart.GetItems(skus.Find("A"));
            sut.DifferencePrice(skus.Find("A"), items, 1).ShouldBe(5);
            kart.AddOrUpdate(skus.Find("A"), items + 1);

            items = kart.GetItems(skus.Find("B"));
            sut.DifferencePrice(skus.Find("B"), items, 1).ShouldBe(3);
            kart.AddOrUpdate(skus.Find("B"), items + 1);

            items = kart.GetItems(skus.Find("A"));
            sut.DifferencePrice(skus.Find("A"), items, 1).ShouldBe(3);
            kart.AddOrUpdate(skus.Find("A"), items + 1);
        }

        [Fact]
        public void ShouldKeepRunningSubtotalForMultipleSkus()
        {
            var skus = new Skus();
            var kart = new Kart();
            var sut = new Pricer();

            skus.AddOrUpdate(new Sku { ItemName = "A", Price = 5, Offer = new Pricing { Units = 3, OfferPrice = 13 } });
            skus.AddOrUpdate(new Sku { ItemName = "B", Price = 3, Offer = new Pricing { Units = 2, OfferPrice = 4.5m } });

            int items;
            items = kart.GetItems(skus.Find("B"));
            sut.DifferencePrice(skus.Find("B"), items, 2).ShouldBe(4.5m);
            kart.AddOrUpdate(skus.Find("B"), items + 2);

            items = kart.GetItems(skus.Find("A"));
            sut.DifferencePrice(skus.Find("A"), items, 2).ShouldBe(10);
            kart.AddOrUpdate(skus.Find("A"), items + 2);

            items = kart.GetItems(skus.Find("B"));
            sut.DifferencePrice(skus.Find("B"), items, 2).ShouldBe(4.5m);
            kart.AddOrUpdate(skus.Find("B"), items + 2);

            items = kart.GetItems(skus.Find("A"));
            sut.DifferencePrice(skus.Find("A"), items, 2).ShouldBe(8);
            kart.AddOrUpdate(skus.Find("A"), items + 2);

            items = kart.GetItems(skus.Find("B"));
            sut.DifferencePrice(skus.Find("B"), items, 2).ShouldBe(4.5m);
            kart.AddOrUpdate(skus.Find("B"), items + 2);

            items = kart.GetItems(skus.Find("A"));
            sut.DifferencePrice(skus.Find("A"), items, 2).ShouldBe(8);
            kart.AddOrUpdate(skus.Find("A"), items + 2);
        }
    }
}
