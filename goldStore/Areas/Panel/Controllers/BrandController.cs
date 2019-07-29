using goldStore.Areas.Panel.Models;
using goldStore.Areas.Panel.Models.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace goldStore.Areas.Panel.Controllers
{
    public class BrandController : Controller
    {
        // GET: Panel/Brand
        BrandRepository repository = new BrandRepository(new goldstoreEntities());

        public ActionResult Index()
        {
            return View(repository.GetAll());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(brand brand, HttpPostedFileBase image1)
        {
            if (image1 != null)
            {
                using (var br = new BinaryReader(image1.InputStream))
                {
                    var data = br.ReadBytes(image1.ContentLength);
                    brand.image = data;
                }

            }
            repository.Save(brand);
            return RedirectToAction("/");
        }

        public ActionResult Edit(int id)
        {
            return View(repository.Get(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(brand brand, HttpPostedFileBase image1)
        {
            if (brand != null)
            {
                if (image1 != null)
                {
                    using (var br = new BinaryReader(image1.InputStream))
                    {
                        var data = br.ReadBytes(image1.ContentLength);
                        brand.image = data;
                    }

                }
                repository.Update(brand);
            }
            return RedirectToAction("/");
        }
        public ActionResult Delete(int id)
        {
            return View(repository.Get(id));
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(int id)
        {
            repository.Delete(repository.Get(id));
            return RedirectToAction("/");
        }
    }
}