using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFMenuRepository : IMenuRepository
    {
        private QLVBDatabase context;

        public EFMenuRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Menu> Menus
        {
            get { return context.Menus.Where(p => p.inttrangthai == (int)enummenu.inttrangthai.IsActive); }
        }
        /// <summary>
        /// cap nhat trang thai cua menu ykcd
        /// </summary>
        /// <param name="inttrangthai"></param>
        /// <returns></returns>
        public int UpdateYkcd(int inttrangthai)
        {
            try
            {
                // id menu ykcd:36
                var danhmuc = context.Menus.FirstOrDefault(p => p.Id == 36);
                danhmuc.inttrangthai = inttrangthai;
                context.SaveChanges();

                var ykcd = context.Menus.FirstOrDefault(p => p.Id == 37);
                ykcd.inttrangthai = inttrangthai;
                context.SaveChanges();

                return danhmuc.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
