using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFTochucdoitacRepository : ITochucdoitacRepository
    {
        private QLVBDatabase context;

        public EFTochucdoitacRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Tochucdoitac> GetActiveTochucdoitacs
        {
            get
            {
                return context.Tochucdoitacs
                    .Where(p => p.inttrangthai == (int)enumTochucdoitac.inttrangthai.IsActive);
            }
        }

        public IQueryable<Tochucdoitac> GetAllTochucdoitacs
        {
            get
            {
                return context.Tochucdoitacs;
            }
        }

        public void AddTochuc(Tochucdoitac tochuc)
        {
            tochuc.inttrangthai = (int)enumTochucdoitac.inttrangthai.IsActive;
            context.Tochucdoitacs.Add(tochuc);
            context.SaveChanges();
        }

        public void EditTochuc(Int32 intid, Tochucdoitac tochuc)
        {
            Tochucdoitac _tochuc = context.Tochucdoitacs.FirstOrDefault(p => p.intid == intid);
            _tochuc.inthoibao = tochuc.inthoibao;
            _tochuc.Isvbdt = tochuc.Isvbdt;
            _tochuc.strdiachi = tochuc.strdiachi;
            _tochuc.stremail = tochuc.stremail;
            _tochuc.stremailvbdt = tochuc.stremailvbdt;
            _tochuc.strfax = tochuc.strfax;
            _tochuc.strmatochucdoitac = tochuc.strmatochucdoitac;
            _tochuc.strmadinhdanh = tochuc.strmadinhdanh;
            _tochuc.strphone = tochuc.strphone;
            _tochuc.strtentochucdoitac = tochuc.strtentochucdoitac;
            context.SaveChanges();
        }

        public void DeleteTochuc(Int32 intid)
        {
            Tochucdoitac _tochuc = context.Tochucdoitacs.FirstOrDefault(p => p.intid == intid);
            _tochuc.inttrangthai = (int)enumTochucdoitac.inttrangthai.NotActive;
            context.SaveChanges();
        }

        public void XoaTochuc(int id)
        {
            Tochucdoitac _tochuc = context.Tochucdoitacs.FirstOrDefault(p => p.intid == id);
            context.Tochucdoitacs.Remove(_tochuc);
            context.SaveChanges();
        }
        public void UpdateTochuc(int id, string madinhdanh)
        {
            Tochucdoitac _tochuc = context.Tochucdoitacs.FirstOrDefault(p => p.intid == id);
            _tochuc.strmadinhdanh = madinhdanh;
            context.SaveChanges();
        }
    }
}
