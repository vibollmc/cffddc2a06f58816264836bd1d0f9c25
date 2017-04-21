using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class EFChitietVanbandenRepository : IChitietVanbandenRepository
    {
        private QLVBDatabase context;

        public EFChitietVanbandenRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<ChitietVanbanden> Chitietvanbandens
        {
            get { return context.Chitietvanbandens; }
        }

        public int Them(ChitietVanbanden vb)
        {
            try
            {
                //cac truong mac dinh khi them moi
                vb.strngaytao = DateTime.Now;

                context.Chitietvanbandens.Add(vb);
                context.SaveChanges();
                return vb.intid;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Sua(int intid, ChitietVanbanden ct)
        {
            try
            {
                var _ct = context.Chitietvanbandens.FirstOrDefault(p => p.intid == intid);
                _ct.intnguoichuyen = ct.intnguoichuyen;
                _ct.intvaitro = ct.intvaitro;
                _ct.intvaitrocu = ct.intvaitrocu;
                _ct.strngaychuyen = ct.strngaychuyen;
                context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void Xoa(int intid)
        {
            try
            {
                var vb = context.Chitietvanbandens.SingleOrDefault(p => p.intid == intid);
                context.Chitietvanbandens.Remove(vb);
                context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
