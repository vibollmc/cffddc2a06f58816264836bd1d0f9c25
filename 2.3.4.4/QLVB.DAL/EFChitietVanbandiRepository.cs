using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFChitietVanbandiRepository : IChitietVanbandiRepository
    {
        private QLVBDatabase context;

        public EFChitietVanbandiRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<ChitietVanbandi> ChitietVanbandis
        {
            get { return context.Chitietvanbandis; }
        }

        public int Them(ChitietVanbandi ct)
        {
            // mac dinh khi them moi
            ct.strngaytao = DateTime.Now;

            context.Chitietvanbandis.Add(ct);
            context.SaveChanges();
            return ct.intid;
        }
        public void Sua(int intid, ChitietVanbandi ct)
        {
            var _ct = context.Chitietvanbandis.FirstOrDefault(p => p.intid == intid);

            _ct.intnguoichuyen = ct.intnguoichuyen;
            _ct.intvaitro = ct.intvaitro;
            _ct.intvaitrocu = ct.intvaitrocu;
            _ct.strngaychuyen = ct.strngaychuyen;

            context.SaveChanges();
        }

        public void Xoa(int intid)
        {
            var ct = context.Chitietvanbandis.FirstOrDefault(p => p.intid == intid);
            context.Chitietvanbandis.Remove(ct);
            context.SaveChanges();
        }
    }
}
