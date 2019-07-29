using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goldStore.Areas.Panel.Models.Interface
{
    interface IRepository<T> where T:class
    {
        void Save(T model);
        void Update(T model);
        void Delete(T model);
        T Get(int id);
        List<T> GetAll();
          
    }
}
