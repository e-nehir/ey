using goldStore.Areas.Panel.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace goldStore.Controllers
{
    public class ShopController : Controller
    {
        ProductRepository repoProduct = new ProductRepository(new Areas.Panel.Models.goldstoreEntities());
        CategoryRepository repoCategory = new CategoryRepository(new Areas.Panel.Models.goldstoreEntities());
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Products");
        }
        public ActionResult PartialCategory()
        {
            return PartialView(repoCategory.GetAll());

        }

        public ActionResult Products()

        {

            return View(repoProduct.GetAll());
        }

        public ActionResult Product(int ? categoryId)
        {

            var result = repoProduct.GetAll();
            if(categoryId!=null)
            {

                result = result.Where(x => x.categoryId == categoryId).ToList();
            }

            return View(result);

        }
    }
}