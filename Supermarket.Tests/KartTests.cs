using Shouldly;
using Supermarket.Entities;
using Xunit;

namespace Supermarket.Tests
{
    public class KartTests
    {
        [Fact]
        public void ShouldBeZeroItemsIfSkuNotInCart()
        {
            var stock = new Skus();
            stock.AddOrUpdate(new Sku { ItemName = "A", Price = 5, Offer = new Pricing { Units = 3, OfferPrice = 13 } });
            stock.AddOrUpdate(new Sku { ItemName = "B", Price = 3, Offer = new Pricing { Units = 2, OfferPrice = 4.5m } });
            var sut = new Kart();

            var items = sut.GetItems(stock.Find("A"));
            items.ShouldBe(0);
        }

        [Fact]
        public void ShouldAddItemsIfSkuNotInCart()
        {
            var stock = new Skus();
            stock.AddOrUpdate(new Sku { ItemName = "A", Price = 5, Offer = new Pricing { Units = 3, OfferPrice = 13 } });
            stock.AddOrUpdate(new Sku { ItemName = "B", Price = 3, Offer = new Pricing { Units = 2, OfferPrice = 4.5m } });
            var sut = new Kart();
            var items = sut.GetItems(stock.Find("A"));
            items.ShouldBe(0);

            sut.AddOrUpdate(stock.Find("A"), 2);

            sut.GetItems(stock.Find("A")).ShouldBe(2);
        }

        [Fact]
        public void ShouldUpdateItemsIfSkuInCart()
        {
            var stock = new Skus();
            stock.AddOrUpdate(new Sku { ItemName = "A", Price = 5, Offer = new Pricing { Units = 3, OfferPrice = 13 } });
            stock.AddOrUpdate(new Sku { ItemName = "B", Price = 3, Offer = new Pricing { Units = 2, OfferPrice = 4.5m } });

            var sut = new Kart();
            var items = sut.GetItems(stock.Find("A"));
            items.ShouldBe(0);
            sut.AddOrUpdate(stock.Find("A"), 2);
            sut.GetItems(stock.Find("A")).ShouldBe(2);

            sut.AddOrUpdate(stock.Find("A"), 4);
            sut.GetItems(stock.Find("A")).ShouldBe(4);
        }
    }
}
