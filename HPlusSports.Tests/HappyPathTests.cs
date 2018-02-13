using HPlusSports.Controllers;
using HPlusSports.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Xunit;

namespace HPlusSports.Tests
{
    public class HappyPathTests
    {
        private static volatile bool ContextInitialized = false;
        private const string TestUserId = SeedData.TestUserId;

        private HPlusSportsDbContext _context;
        private ProductsController productsController;
        private CartController cartController;

        public HappyPathTests()
        {
            EnsureInitialized();
            ResetContext();
        }

        [Fact]
        public void CanBrowseTheSiteAndAddAndRemoveProductsInTheCart()
        {
            // View the product categories
            var indexResult = productsController.Index();
            var categories = (indexResult as ViewResult)?.Model as IDictionary<Category, int>;
            Assert.Equal(
                _context.Categories.Count(),
                categories?.Count
            );

            // Pick a category
            var categoryKey = categories.Last().Key.Key;
            var categoryResult = productsController.Category(categoryKey);
            var products = (categoryResult as ViewResult)?.Model as IEnumerable<Product>;
            Assert.Equal(
                _context.Products.Count(x => x.Category.Key == categoryKey),
                products?.Count()
            );

            // Pick a product
            var productSKU = products.Last().SKU;
            var productResult = productsController.Product(productSKU);
            var product = (productResult as ViewResult)?.Model as Product;
            Assert.Equal(
                productSKU,
                product?.SKU
            );

            // Add the product to the cart
            ClearCart();
            ResetContext();
            var expectedQuantity = 3;
            var addToCartResult = cartController.Add(productSKU, expectedQuantity);
            Assert.IsType<RedirectToRouteResult>(addToCartResult);
            Assert.Equal("Index", (addToCartResult as RedirectToRouteResult).RouteValues["Action"]);
            ResetContext();
            var addedItem = GetCart()?.Items?.FirstOrDefault();
            Assert.Equal(expectedQuantity, addedItem.Quantity);
            Assert.Equal(expectedQuantity * addedItem.Price, addedItem.Total);

            // Remove the product from the cart
            ResetContext();
            var removeFromCartResult = cartController.Remove(addedItem.Id);
            Assert.IsType<RedirectToRouteResult>(removeFromCartResult);
            Assert.Equal("Index", (removeFromCartResult as RedirectToRouteResult).RouteValues["Action"]);
            ResetContext();
            var cartWithRemovedItem = GetCart();
            Assert.NotNull(cartWithRemovedItem?.Items);
            Assert.DoesNotContain(
                cartWithRemovedItem.Items,
                x => x.SKU == productSKU
            );
        }

        private ShoppingCart GetCart()
        {
            var cartResult = cartController.Index();
            return (cartResult as ViewResult)?.Model as ShoppingCart ?? new ShoppingCart();
        }

        private void ClearCart()
        {
            var existingCart = _context.ShoppingCarts.SingleOrDefault(x => x.UserId == TestUserId);
            if (existingCart != null)
            {
                existingCart.Items.Clear();
                _context.SaveChanges();
            }
        }

        private void ResetContext()
        {
            _context = CreateContext();

            productsController = new ProductsController(_context);

            cartController = new CartController(_context)
            {
                GetUserId = x => TestUserId
            };
        }

        static void EnsureInitialized()
        {
            if (!ContextInitialized)
            {
                var context = CreateContext();
                context.Database.Delete();
                context.Database.Create();
                new SeedData().Populate(context);

                ContextInitialized = true;
            }
        }

        static HPlusSportsDbContext CreateContext()
        {
            return new HPlusSportsDbContext("HPlusSports_Test");
        }
    }
}
