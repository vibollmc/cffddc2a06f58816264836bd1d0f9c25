using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFNhomQuyenRepository : INhomQuyenRepository
    {
        private QLVBDatabase context;

        public EFNhomQuyenRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<NhomQuyen> GetActiveNhomQuyens
        {
            get { return context.NhomQuyens.Where(p => p.inttrangthai == (int)enumNhomquyen.inttrangthai.IsActive); }
        }

        public IQueryable<NhomQuyen> GetAllNhomQuyens
        {
            get { return context.NhomQuyens; }
        }

        public void SaveGroup(NhomQuyen _group)
        {
            context.NhomQuyens.Add(_group);
            context.SaveChanges();
        }

        public void SaveGroup(string strtennhom)
        {
            NhomQuyen group = new NhomQuyen();
            group.strtennhom = strtennhom;
            group.inttrangthai = 1;
            context.NhomQuyens.Add(group);
            context.SaveChanges();
        }

        public void EditGroup(Int32 intidgroup, string strtennhom)
        {
            NhomQuyen group = context.NhomQuyens.FirstOrDefault(p => p.intid == intidgroup);
            group.strtennhom = strtennhom;
            context.SaveChanges();
        }

        public void DeleteGroup(Int32 intidgroup)
        {
            NhomQuyen group = context.NhomQuyens.FirstOrDefault(p => p.intid == intidgroup);
            group.inttrangthai = 0;
            //context.NhomQuyens.Remove(group);
            context.SaveChanges();
        }

        public string GetTenNhom(Int32 intidgroup)
        {
            var strtennhom = context.NhomQuyens.FirstOrDefault(p => p.intid == intidgroup).strtennhom;
            return strtennhom;
        }
    }
}
