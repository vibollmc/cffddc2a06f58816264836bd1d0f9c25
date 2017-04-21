using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFAttachHosoRepository : IAttachHosoRepository
    {
        private QLVBDatabase context;

        public EFAttachHosoRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<AttachHoso> AttachHosos
        {
            get { return context.AttachHosos; }
        }

        public int Them(AttachHoso hs)
        {
            try
            {
                // cac truong mac dinh
                hs.inttrangthai = (int)enumAttachHoso.inttrangthai.IsActive;
                hs.intphathanh = (int)enumAttachHoso.intphathanh.Khong;
                hs.strngaycapnhat = DateTime.Now;

                context.AttachHosos.Add(hs);
                context.SaveChanges();
                return hs.intid;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Xoa(int intid)
        {
            try
            {
                var hs = context.AttachHosos.FirstOrDefault(p => p.intid == intid);
                if (hs != null)
                {
                    context.AttachHosos.Remove(hs);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CapnhatPhathanh(int intid)
        {
            try
            {
                var hs = context.AttachHosos.FirstOrDefault(p => p.intid == intid);
                if (hs != null)
                {
                    hs.intphathanh = (int)enumAttachHoso.intphathanh.Co;
                    context.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
