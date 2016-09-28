using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class EFQuyenRepository : IQuyenRepository
    {
        private QLVBDatabase context;

        public EFQuyenRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Quyen> Quyens
        {
            get
            {
                return context.Quyens
                    .Where(p => p.inttrangthai == (int)enumQuyen.inttrangthai.IsActive);
            }
        }
        /// <summary>
        /// cap nhat trang thai quyen cua module ykcd
        /// </summary>
        /// <param name="inttrangthai"></param>
        /// <returns></returns>
        public int UpdateYKCD(int inttrangthai)
        {
            try
            {
                _UpdateMenuYKCD(inttrangthai);

                _UpdateMenuDonviYKCD(inttrangthai);

                _UpdateMenuVanbandenYKCD(inttrangthai);
                _UpdateMenuVanbandiYKCD(inttrangthai);

                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void _UpdateMenuDonviYKCD(int inttrangthai)
        {
            try
            {
                var quyen = context.Quyens.Where(p => p.intidmenu == 36);
                foreach (var q in quyen)
                {
                    q.inttrangthai = inttrangthai;
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void _UpdateMenuVanbandenYKCD(int inttrangthai)
        {
            try
            {
                var quyen = context.Quyens.Where(p => p.intidmenu == 27)
                    .Where(p => p.strquyen == "YKCDVanbanden");
                foreach (var q in quyen)
                {
                    q.inttrangthai = inttrangthai;
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void _UpdateMenuVanbandiYKCD(int inttrangthai)
        {
            try
            {
                var quyen = context.Quyens.Where(p => p.intidmenu == 28)
                    .Where(p => p.strquyen == "YKCDVanbandi");
                foreach (var q in quyen)
                {
                    q.inttrangthai = inttrangthai;
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void _UpdateMenuYKCD(int inttrangthai)
        {
            try
            {
                var quyen = context.Quyens.Where(p => p.intidmenu == 37);
                foreach (var q in quyen)
                {
                    q.inttrangthai = inttrangthai;
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
