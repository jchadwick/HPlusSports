using System;
using HPlusSports.Models;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace HPlusSports
{
    internal class HPlusSportsDbContextInitializer 
        : DropCreateDatabaseIfModelChanges<HPlusSportsDbContext>
    {
        public override void InitializeDatabase(HPlusSportsDbContext context)
        {
            context.Database.CreateIfNotExists();

            if (context.Categories.Any())
                return;

            new SeedData().Populate(context);
        }
    }

    internal class SeedData
    {
        static readonly Random Random = new Random(1);

        public const string TestUserId = "testuser";

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
        }

        private IEnumerable<Product> CreateSupplementProducts(string name)
        {
            var description = $"An amazing new type of supplement that helps make you healthier and happier!";
            var msrpPerCapsule = Random.NextDouble() * Random.Next(1, 5);
            var discount = Random.Next(0, 1) * Random.NextDouble();
            var pricePerCapsule = msrpPerCapsule - msrpPerCapsule * discount;

            return new[] { 50, 100, 250, 500 }.Select(count =>
                new Product
                {
                    SKU = GenerateSKU(),
                    Category = Supplements,
                    Name = $"{name} ({count} capsules)",
                    Description = description,
                    Rating = GenerateRating(),
                    Summary = description,
                    MSRP = ToPrice(msrpPerCapsule * count),
                    Price = ToPrice(pricePerCapsule * count),
                }
            );
        }

        private Product CreateActiveWear(string name, Category category)
        {
            var description = $"An amazing new type of activewear that helps make you healthier and happier!";
            var msrp = Random.Next(30, 150);
            var discount = Random.Next(0, 1) * Random.NextDouble();

            var product = new Product
            {
                SKU = GenerateSKU(),
                Category = category,
                Name = name,
                Description = description,
                Rating = GenerateRating(),
                Summary = description,
                MSRP = ToPrice(msrp),
                Price = ToPrice(msrp - msrp * discount),
            };

            return product;
        }

        float? GenerateRating()
        {
            return Random.Next(1, 5);
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