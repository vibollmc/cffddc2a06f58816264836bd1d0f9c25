using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class EFChucdanhRepository : IChucdanhRepository
    {
        private QLVBDatabase context;

        public EFChucdanhRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Chucdanh> Chucdanhs
        {
            get { return context.Chucdanhs.AsNoTracking(); }
        }

        public Chucdanh GetChucdanh(Int32 intid)
        {
            return context.Chucdanhs.FirstOrDefault(p => p.intid == intid);
        }

        public void AddChucdanh(Chucdanh chucdanh)
        {
            context.Chucdanhs.Add(chucdanh);
            context.SaveChanges();
        }

        public void EditChucdanh(Int32 intid, Chucdanh chucdanh)
        {
            Chucdanh cd = context.Chucdanhs.FirstOrDefault(p => p.intid == intid);
            cd.strtenchucdanh = chucdanh.strtenchucdanh;
            cd.strmachucdanh = chucdanh.strmachucdanh;
            cd.strghichu = chucdanh.strghichu;
            cd.intloai = chucdanh.intloai;
            context.SaveChanges();
        }

        public void DeleteChucdanh(Int32 intid)
        {
            //var emp = new Employee { ID = empID };
            //db.Employees.Attach(emp);
            //db.Employees.Remove(emp);
            //Chucdanh cd = context.Chucdanhs.FirstOrDefault(p => p.intid == intid);
            Chucdanh cd = new Chucdanh { intid = intid };
            context.Chucdanhs.Attach(cd);
            context.Chucdanhs.Remove(cd);
            context.SaveChanges();
        }
    }
}
