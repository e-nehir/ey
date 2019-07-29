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
    public class ProductController : Controller
    {

        ProductRepository repository = new ProductRepository( new Models.goldstoreEntities());
        // GET: Panel/Product
        public ActionResult Index()
        {
            return View(repository.GetAll());
        }
        public ActionResult Create()
        {
            ViewBag.Categories = repository.GetCategories();
            ViewBag.Brands = repository.GetBrands();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(product product,IEnumerable<HttpPostedFileBase> image1)
        {
            if (product != null)
            {
                repository.Save(product);
            }
            if(image1.First()!=null)
            {
                productImage newImg = new productImage();
                newImg.productId = product.productId;

                foreach (var item in image1)
                {
                    using (var br = new BinaryReader(item.InputStream))
                    {
                        var data = br.ReadBytes(item.ContentLength);
                        newImg.image = data;
                    }
                    repository.Save(newImg);
                }
            }
            return RedirectToAction("Index");
            
        }
        public ActionResult Edit(int id)
        {
            var selectedBrand = repository.Get(id).brandId;
            var selectedCategory= repository.Get(id).categoryId;

            ViewBag.Categories = new SelectList(repository.GetCategories(), "categoryId", "categoryName", selectedCategory);
            ViewBag.Brands = new SelectList(repository.GetBrands(), "brandId", "brandName", selectedBrand);

            return View(repository.Get(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(product product,IEnumerable<HttpPostedFileBase> image1)
        {
            if (product!=null)
            {
                repository.Update(product);
            }
            if (image1.First() != null)
            {
                productImage newImg = new productImage();
                newImg.productId = product.productId;

                foreach (var item in image1)
                {
                    using (var br = new BinaryReader(item.InputStream))
                    {
                        var data = br.ReadBytes(item.ContentLength);
                        newImg.image = data;
                    }
                    repository.Save(newImg);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void deleteImage(int imageId)
        {
            // resim sil
             repository.Delete(imageId);
        }

        public ActionResult Delete(int id)
        {
            return View(repository.Get(id));
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteProduct(int id)
        {
            repository.Delete(repository.Get(id));
            return RedirectToAction("Index");
        }

    }
}