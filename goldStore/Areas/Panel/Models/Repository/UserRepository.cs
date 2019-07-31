using goldStore.Areas.Panel.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace goldStore.Areas.Panel.Models.Repository
{
    public class UserRepository : IRepository<user>
    {
        goldstoreEntities _context;

        public UserRepository(goldstoreEntities Context)
        {
            _context = Context;

        }
        public void Delete(user model)
        {
            if(model !=null)
            {
                _context.user.Remove(model);
                _context.SaveChanges();

            }
        }

        public user Get(int id)
        {
            return _context.user.Find(id);
        }

        public List<user> GetAll()
        {
            return _context.user.ToList();
        }

        public void Save(user model)
        {
            if(model !=null)
            {
                _context.user.Add(model);
                _context.SaveChanges();
            }
        }

        public void Update(user model)
        {
            if(model !=null)
            {

                user old = Get(model.userId);
                _context.Entry(old).State = EntityState.Detached;
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

            }
        }
    }
}