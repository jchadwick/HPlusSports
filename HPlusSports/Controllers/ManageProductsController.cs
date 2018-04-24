﻿using HPlusSports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPlusSports.Controllers
{
  [Authorize(Roles = UserRoles.Admin)]
  public class ManageProductsController : Controller
  {
    private HPlusSportsDbContext _context;

    public ManageProductsController()
      : this(new HPlusSportsDbContext())
    {
    }

    public ManageProductsController(HPlusSportsDbContext context)
    {
      _context = context;
    }

    public ActionResult Index()
    {
      var products =
        _context.Products
          .OrderBy(x => x.Category.Name)
          .ThenBy(x => x.Name);

      return View(products);
    }

    [HttpGet]
    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Product product)
    {
      if (!ModelState.IsValid)
      {
        return View();
      }

      product.LastUpdated = DateTime.UtcNow;
      product.LastUpdatedUserId = GetUserId(this);

      _context.Products.Add(product);
      _context.SaveChanges();

      TempData["SuccessMessage"] =
        $"Successfully created \"{product.Name}\"";

      return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public ActionResult Update(long id)
    {
      var existing = _context.Products.Find(id);

      if (existing == null)
      {
        return ProductListError(
          $"Couldn't update product #\"{id}\": product not found!"
        );
      }

      return View(existing);
    }

    [HttpPost]
    public ActionResult Update(long id, Product product)
    {
      if (!ModelState.IsValid)
      {
        return View();
      }

      var existing = _context.Products.Find(id);

      if (existing == null)
      {
        return ProductListError(
          $"Couldn't update product #\"{id}\": product not found!"
        );
      }

      existing.CategoryId = product.CategoryId;
      existing.Description = product.Description;
      existing.MSRP = product.MSRP;
      existing.Name = product.Name;
      existing.Price = product.Price;
      existing.SKU = product.SKU;
      existing.Summary = product.Summary;
      existing.Tags = product.Tags;

      existing.LastUpdated = DateTime.UtcNow;
      existing.LastUpdatedUserId = GetUserId(this);

      _context.SaveChanges();

      TempData["SuccessMessage"] =
        $"Successfully updated \"{product.Name}\"";

      return RedirectToAction(nameof(Index));
    }

    public ActionResult Delete(long id)
    {
      var product = _context.Products.Find(id);

      if (product == null)
      {
        return ProductListError(
          $"Couldn't delete \"{product.Name}\": product not found!"
        );
      }

      _context.Products.Remove(product);
      _context.SaveChanges();

      TempData["SuccessMessage"] =
        $"Successfully deleted \"{product.Name}\"";

      return RedirectToAction(nameof(Index));
    }

    private ActionResult ProductListError(string error)
    {
      TempData["ErrorMessage"] = error;
      return RedirectToAction(nameof(Index));
    }

    protected override void OnResultExecuting(ResultExecutingContext filterContext)
    {
      ViewData["CategoryId"] =
        _context.Categories
            .Select(x => new SelectListItem
            {
              Text = x.Name,
              Value = x.Id.ToString(),
            })
            .ToArray();
    }

    // Overwriteable function for unit testing
    internal Func<Controller, string> GetUserId =
        (controller) => controller.User.Identity.Name;
  }
}