using System;
using HPlusSports.Models;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace HPlusSports
{
  internal class HPlusSportsDbContextInitializer
      : DropCreateDatabaseAlways<HPlusSportsDbContext>
  {
    protected override void Seed(HPlusSportsDbContext context)
    {
      new SeedData().Populate(context);
    }
  }

  internal class SeedData
  {
    static readonly Random Random = new Random(1);

    public const string TestUserId = "testuser";

    static readonly IList<string> UserIds =
      Enumerable.Range(0, 10)
        .Select(x => $"user{x}")
        .Concat(new[] { TestUserId })
        .ToArray();

    static readonly IList<string> ClothingColors = new List<string> { "Red", "Blue", "Green", "Orange", "Brown" };
    static readonly IList<string> ClothingSizes = new List<string> { "S", "M", "L", "XL" };

    static long categoryId = 0;
    static readonly Category ActiveWearMen = new Category { Id = ++categoryId, Key = "active-wear-men", Name = "Active Wear - Men" };
    static readonly Category ActiveWearWomen = new Category { Id = ++categoryId, Key = "active-wear-women", Name = "Active Wear - Women" };
    static readonly Category MineralWater = new Category { Id = ++categoryId, Key = "mineral-water", Name = "Mineral Water" };
    static readonly Category Supplements = new Category { Id = ++categoryId, Key = "supplements", Name = "Supplements" };

    static readonly IList<Category> categories = new[] {
            ActiveWearMen, ActiveWearWomen, MineralWater, Supplements
        };

    public void Populate(HPlusSportsDbContext context)
    {
      context.Categories.AddRange(categories);
      context.SaveChanges();

      context.Products.AddRange(CreateSupplementProducts("Calcium 400 IU"));
      context.Products.AddRange(CreateSupplementProducts("Flaxseed Oil 1000 mg"));
      context.Products.AddRange(CreateSupplementProducts("Iron 65 mg"));
      context.Products.AddRange(CreateSupplementProducts("Magnesium 250 mg"));
      context.Products.AddRange(CreateSupplementProducts("Multivitamin"));

      context.Products.Add(CreateActiveWear("Running Jacket (Men)", ActiveWearMen));
      context.Products.Add(CreateActiveWear("Running Jacket (Women)", ActiveWearWomen));

      context.SaveChanges();

      var reviews = context.Products.SelectMany(GenerateReviews);
      context.Reviews.AddRange(reviews);
      context.SaveChanges();
    }

    private IEnumerable<Product> CreateSupplementProducts(string name)
    {
      var description = $"An amazing new type of supplement that helps make you healthier and happier!";
      var msrpPerCapsule = Random.NextDouble() * Random.Next(1, 5);
      var discount = msrpPerCapsule * ((double)Random.Next(0, 60) / 100);
      var pricePerCapsule = msrpPerCapsule - discount;

      return new[] { 50, 100, 250, 500 }.Select(count =>
          new Product
          {
            SKU = GenerateSKU(),
            Category = Supplements,
            Name = $"{name} ({count} capsules)",
            Description = description,
            Summary = description,
            MSRP = ToPrice(msrpPerCapsule * count),
            Price = ToPrice(pricePerCapsule * count),
          }
      );
    }

    private Product CreateActiveWear(string name, Category category)
    {
      var description = $"An amazing new type of activewear that helps make you healthier and happier!";
      double msrp = Random.Next(30, 150);
      var discount = msrp * ((double)Random.Next(0, 60) / 100);

      var product = new Product
      {
        SKU = GenerateSKU(),
        Category = category,
        Name = name,
        Description = description,
        Summary = description,
        MSRP = ToPrice(msrp),
        Price = ToPrice(msrp - discount),
      };

      return product;
    }

    IEnumerable<Review> GenerateReviews(Product product)
    {
      return Enumerable.Range(0, Random.Next(10))
        .Select(i =>
          new Review
          {
            ProductSku = product.SKU,
            UserId = UserIds[Random.Next(0, UserIds.Count)],
            Rating = Random.Next(3, 5),
          });
    }

    string GenerateSKU()
    {
      return Random.Next(100000, int.MaxValue).ToString();
    }

    static double ToPrice(double price)
    {
      return (double)Decimal.Round((decimal)price, 2);
    }
  }
}