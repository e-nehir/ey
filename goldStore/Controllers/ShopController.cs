using goldStore.Areas.Panel.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace goldStore.Controllers
{
    public class ShopController : Controller
    {
        ProductRepository repoProduct = new ProductRepository(new Areas.Panel.Models.goldstoreEntities());
        CategoryRepository repoCategory = new CategoryRepository(new Areas.Panel.Models.goldstoreEntities());
        BrandRepository repoBrand = new BrandRepository(new Areas.Panel.Models.goldstoreEntities());
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Products");
        }
        public PartialViewResult PartialNewArrivals ()
        {

            return PartialView();
        }
        public PartialViewResult PartialBrands()
        {

            return PartialView(repoBrand.GetAll());
        }
        public ActionResult PartialCategory()
        {
            return PartialView(repoCategory.GetAll());

        }

        public ActionResult PartialPrice()
        {
            return PartialView();

        }

      
        public ActionResult Products (int?brandId,  int? categoryId, decimal? max, decimal? min, int?  page, int? PageSize, int? orderBy)
        {
            ViewBag.orderBy = new List<SelectListItem>()
            {


                new SelectListItem{ Text ="Fiyat", Value="1",Selected=true},
                 new SelectListItem{ Text ="İsim", Value="2"}

            };
            ViewBag.PageSize = new List<SelectListItem>()
            {


                new SelectListItem{ Text ="20", Value="20",Selected=true},
                 new SelectListItem{ Text ="10", Value="10"},
                   new SelectListItem{ Text ="5", Value="5"},
                     new SelectListItem{ Text ="2", Value="2"},
                       new SelectListItem{ Text ="1", Value="1"}

            };



            int _page = page ?? 1;
            int _pageSize = PageSize ?? 6;
            TempData["PageSize"] = _pageSize;
            TempData.Keep("PageSize");

            var result = repoProduct.GetAll();


            {
              if (brandId != null)
                {
                    result = result.Where(x => x.brandId == brandId).ToList();
                }

                else if (categoryId != null)
                {
                    result = result.Where(x => x.categoryId == categoryId).ToList();
                }
                else if (max != null && min != null)
                {
                    result = result.Where(x => x.price >= min && x.price <= max).ToList();
                }


                else if (orderBy ==1)
                {

                    result = result.OrderBy(x => x.price).ToList();

                }
                else if (orderBy == 2)
                {

                    result = result.OrderBy(x => x.productName).ToList();

                }

                return View(result.ToPagedList(_page, _pageSize));
            }

           
        }
           
        public ActionResult ProductDetail (int productId)
        {

            return View(repoProduct.Get(productId));
        }
        public ActionResult Thumbnail(int width, int height, int Id, int _imageId)
        {
            byte[] photo = repoProduct.Get(Id).productImage.FirstOrDefault(x=>x.imageId==_imageId).image;
            var base64 = Convert.ToBase64String(photo);
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);

            using (var newImage = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(newImage))
            using (var stream = new MemoryStream())
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(image, new Rectangle(0, 0, width, height));
                newImage.Save(stream, ImageFormat.Png);
                return File(stream.ToArray(), "image/png");
            }

        }

        public ActionResult BrandThumbnail(int width, int height, int Id, int _imageId)
        {
            byte[] photo = repoBrand.Get(Id).image;
            var base64 = Convert.ToBase64String(photo);
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);

            using (var newImage = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(newImage))
            using (var stream = new MemoryStream())
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(image, new Rectangle(0, 0, width, height));
                newImage.Save(stream, ImageFormat.Png);
                return File(stream.ToArray(), "image/png");
            }

        }

    }


       
           
        }
   