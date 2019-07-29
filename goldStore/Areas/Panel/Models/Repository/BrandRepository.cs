using goldStore.Areas.Panel.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;



namespace goldStore.Areas.Panel.Models.Repository
{
    public class BrandRepository : IRepository<brand>
    {

        private goldstoreEntities _context;
      // private goldstoreEntities1 goldstoreEntities1;

        public BrandRepository(goldstoreEntities Context)
        {
            this._context = Context;
        }

       

        public void Delete(brand model)
        {
            _context.brand.Remove(model);
            _context.SaveChanges();
        }

        public brand Get(int id)
        {
            return _context.brand.Find(id);
        }

        public List<brand> GetAll()
        {
            return _context.brand.ToList();
        }

        public void Save(brand model)
        {
            _context.brand.Add(model);
            _context.SaveChanges();
        }

        public void Update(brand model)
        {
            brand old = Get(model.brandId);
            _context.Entry(old).State = EntityState.Detached;
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}