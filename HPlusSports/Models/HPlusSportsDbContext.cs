using System.Data.Entity;

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
            Database.SetInitializer(new HPlusSportsDbContextInitializer());
        }

        public static void Initialize()
        {
            var context = new HPlusSportsDbContext();
            if(!context.Database.Exists())
            {
                context.Database.Create();
                new SeedData().Populate(context);
            }
        }
    }
}