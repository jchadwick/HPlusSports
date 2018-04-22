using HPlusSports.Models;
using System.Linq;
using System.Web.Mvc;

namespace HPlusSports.Controllers
{
  public class ProductsController : Controller
  {
    private readonly HPlusSportsDbContext _context;

    public ProductsController()
        : this(new HPlusSportsDbContext())
    {
    }

    public ProductsController(HPlusSportsDbContext context)
    {
      _context = context;
    }

    public ActionResult Index(int page = 0, int count = 25)
    {
      var products =
          _context.Products
              .OrderBy(x => x.Name)
              .Skip(page * count)
              .Take(count);

      var skus = products.Select(x => x.SKU);
      var ratings = _context.GetProductRatings(skus);

      ViewData["Ratings"] = ratings;
      ViewData["Category"] = "All Products";

      return View("ProductList", products);
    }

    public ActionResult Category(string id, int page = 0, int count = 25)
    {
      var category = _context.Categories.FirstOrDefault(x => x.Key == id);

      if (category == null)
        return HttpNotFound();

      var products =
          _context.Products
              .Where(x => x.CategoryId == category.Id)
              .OrderBy(x => x.Name)
              .Skip(page * count)
              .Take(count);

      var skus = products.Select(x => x.SKU);
      var ratings = _context.GetProductRatings(skus);

      ViewData["Ratings"] = ratings;
      ViewData["Category"] = category;

      return View("ProductList", products);
    }

    public ActionResult Product(string id)
    {
      var product =
          _context.Products
              .Include("Category") // For use below
              .FirstOrDefault(x => x.SKU == id);

      if (product == null)
        return HttpNotFound();

      var images =
          product
            .Images
              .Select(img => img.Id)
              .ToArray();

      ViewData["Rating"] = _context.GetProductRating(product.SKU);
      ViewData["Category"] = product.Category;
      ViewData["Images"] = images;

      return View(product);
    }
  }
}