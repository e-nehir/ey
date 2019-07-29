using goldStore.Areas.Panel.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace goldStore.Areas.Panel.Models.Repository
{
    public class CategoryRepository : IRepository<category>
    {
        private goldstoreEntities _context;

        public CategoryRepository(goldstoreEntities Context)
        {
            this._context = Context;
        }

        public void Delete(category model)
        {
            _context.category.Remove(model);
            _context.SaveChanges();
        }

        public category Get(int id)
        {
            return _context.category.Find(id);
        }

        public List<category> GetAll()
        {
            return _context.category.ToList();
        }

        public void Save(category model)
        {
            _context.category.Add(model);
            _context.SaveChanges();
        }

        public void Update(category model)
        {
            category old = Get(model.categoryId);
            _context.Entry(old).State = EntityState.Detached;
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }
        
    }
}