using goldStore.Areas.Panel.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace goldStore.Areas.Panel.Models.Repository
{
    public class ProductRepository : IRepository<product>
    {
        private goldstoreEntities _context;
        public ProductRepository(goldstoreEntities Context)
        {
            _context = Context;
        }
        public void Delete(int id)
        {
            // delete içerisine bir id geldiğinde resim silecek
            productImage img = _context.productImage.Find(id);
            if (img != null)
            {
                _context.productImage.Remove(img);
                _context.SaveChanges();
            }

        }
        public void Delete(product model)
        {
            // delete içerisine bir product geldiğinde product silecek
        
        _context.product.Remove(model);
            _context.SaveChanges();
        }

        public product Get(int id)
        {
            return _context.product.Find(id);
        }

        public List<product> GetAll()
        {
            return _context.product.ToList();
        }
        public void Save(product model)
        {
            _context.product.Add(model);
            _context.SaveChanges();
        }
        public void Save(productImage model)
        {
            _context.productImage.Add(model);
            _context.SaveChanges();
        }

        public void Update(product model)
        {
            product old = Get(model.productId);
            _context.Entry(old).State = EntityState.Detached;
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }
        //Kategori Listesi
        public List<category> GetCategories()
        {
            return _context.category.ToList() ;
        }
        //Marka Listesi
        public List<brand> GetBrands()
        {
            return _context.brand.ToList();
        }
    }
}