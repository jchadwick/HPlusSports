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

        public ActionResult Index()
        {
            return null;
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

            ViewData["Category"] = category;

            return View(products);
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
                _context.Products
                    .Where(x => x.SKU == id)
                    .SelectMany(x => x.Images.Select(img => img.Id))
                    .ToArray();

            ViewData["Category"] = product.Category;
            ViewData["Images"] = images;

            return View(product);
        }
    }
}