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
    {
      return Products.FirstOrDefault(x => x.SKU == sku);
    }

    public double? GetProductRating(string sku)
    {
      return Reviews
        .Where(x => x.ProductSku == sku)
        .Average(x => x.Rating);
    }

    public IDictionary<string, double?> GetProductRatings(IEnumerable<string> skus)
    {
      var uniqueSkus = skus.Distinct().ToArray();

      var ratings = 
        Reviews
          .Where(x => uniqueSkus.Contains(x.ProductSku))
          .GroupBy(x => x.ProductSku)
          .ToDictionary(
            x => x.Key,
            x => (double?)x.Average(y => y.Rating)
          );

      var missingRatings =
        uniqueSkus
          .Except(ratings.Keys)
          .Select(sku => new KeyValuePair<string, double?>(sku, null));

      return 
        ratings
          .Concat(missingRatings)
          .ToDictionary(x => x.Key, x => x.Value);
    }
  }
}