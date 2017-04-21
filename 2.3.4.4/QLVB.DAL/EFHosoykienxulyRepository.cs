using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFHosoykienxulyRepository : IHosoykienxulyRepository
    {
        private QLVBDatabase context;

        public EFHosoykienxulyRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Hosoykienxuly> Hosoykienxulys
        {
            get { return context.Hosoykienxulys; }
        }

        public int Them(Hosoykienxuly hs)
        {
            try
            {
                // cac truong mac dinh khi them moi
                hs.strthoigian = DateTime.Now;
                //hs.inttrangthai = (int)enumHosoykienxuly.inttrangthai.Dachoykien;

                context.Hosoykienxulys.Add(hs);
                context.SaveChanges();
                return hs.intid;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// chi update trangthai va y kien
        /// </summary>
        /// <param name="intid"></param>
        /// <param name="hs"></param>
        public void Sua(int intid, Hosoykienxuly hs)
        {
            var _hs = context.Hosoykienxulys.SingleOrDefault(p => p.intid == intid);

            _hs.strthoigian = DateTime.Now;
            //_hs.intiddoituongxuly = hs.intiddoituongxuly;
            //_hs.intnguoilap = hs.intnguoilap;
            _hs.inttrangthai = hs.inttrangthai;
            _hs.strykien = hs.strykien;

            context.SaveChanges();
        }

        public void Xoa(int intid)
        {
            var hs = context.Hosoykienxulys.SingleOrDefault(p => p.intid == intid);
            hs.inttrangthai = (int)enumHosoykienxuly.inttrangthai.DaXoaYkien;
            //context.Hosoykienxulys.Remove(hs);
            context.SaveChanges();
        }
    }
}
