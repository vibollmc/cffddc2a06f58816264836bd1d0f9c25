using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFPhanloaiTruongRepository : IPhanloaiTruongRepository
    {
        private QLVBDatabase context;

        public EFPhanloaiTruongRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<PhanloaiTruong> PhanloaiTruongs
        {
            get { return context.PhanloaiTruongs.AsNoTracking(); }
        }

        public void AddPhanloaiTruong(PhanloaiTruong loaitruong)
        {
            context.PhanloaiTruongs.Add(loaitruong);
            context.SaveChanges();
        }

        public void EditPhanloaiTruong(PhanloaiTruong loaitruong)
        {
        }

        /// <summary>
        /// cap nhat IsDisplay va intorder
        /// </summary>
        /// <param name="intid"></param>
        /// <param name="IsDisplay"></param>
        /// <param name="intorder"></param>
        public void EditPhanloaiTruong(Int32 intid, bool IsDisplay, Int32 intorder)
        {
            var _loaitruong = context.PhanloaiTruongs.FirstOrDefault(p => p.intid == intid);
            _loaitruong.IsDisplay = IsDisplay;
            _loaitruong.intorder = intorder;
            context.SaveChanges();
        }

        /// <summary>
        /// chi edit field IsDisplay
        /// </summary>
        /// <param name="intid">The intid.</param>
        /// <param name="IsDisplay">The is display.</param>
        public void EditDisplay(Int32 intid, bool IsDisplay)
        {
            var _loaitruong = context.PhanloaiTruongs.FirstOrDefault(p => p.intid == intid);
            _loaitruong.IsDisplay = IsDisplay;
            context.SaveChanges();
        }

        /// <summary>
        /// cap nhat intorder cua cac truong IsRequire
        /// mac dinh IsDisplay = true
        /// </summary>
        /// <param name="intid"></param>
        /// <param name="intorder"></param>
        public void EditOrder(Int32 intid, Int32 intorder)
        {
            var _loaitruong = context.PhanloaiTruongs.FirstOrDefault(p => p.intid == intid);
            _loaitruong.IsDisplay = true;
            _loaitruong.intorder = intorder;
            context.SaveChanges();
        }
    }
}
