using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HPlusSports.Models
{
  public class HPlusSportsDbContext : DbContext
  {
    public DbSet<Category> Categories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

    public HPlusSportsDbContext()
        : this("HPlusSports")
    {
    }

    public HPlusSportsDbContext(string connectionName)
        : base(connectionName)
    {
    }

    public Product FindProductBySku(string sku) 
      => Products.FirstOrDefault(x => x.SKU == sku);

    public ProductRating GetProductRating(string sku)
    {
      var reviews = Reviews.Where(x => x.SKU == sku);

      return new ProductRating
      {
        SKU = sku,
        Rating = reviews.Average(x => x.Rating),
        ReviewCount = reviews.Count(),
      };
    }

    public IQueryable<ProductRating> GetProductRatings(IEnumerable<string> skus)
    {
      var uniqueSkus = skus.Distinct().ToArray();

      return
        Reviews
          .Where(x => uniqueSkus.Contains(x.SKU))
          .GroupBy(x => x.SKU)
          .Select(reviews => new ProductRating
          {
            SKU = reviews.Key,
            Rating = reviews.Average(x => x.Rating),
            ReviewCount = reviews.Count(),
          });
    }
  }
}