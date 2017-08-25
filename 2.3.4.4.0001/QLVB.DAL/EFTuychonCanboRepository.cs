using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class EFTuychonCanboRepository : ITuychonCanboRepository
    {
        private QLVBDatabase context;

        public EFTuychonCanboRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<TuychonCanbo> TuychonCanbos
        {
            get
            {
                return context.TuychonCanbos.AsNoTracking();
            }
        }

        public int Save(string strthamso, string strgiatri, int idcanbo)
        {
            try
            {
                var opt = context.Tuychons.Where(p => p.strthamso == strthamso)
                    .GroupJoin(
                        context.TuychonCanbos.Where(p => p.intidcanbo == idcanbo),
                        t => t.intid,
                        c => c.intidoption,
                        (t, c) => new { t, c }
                    );
                var optDefault = opt.FirstOrDefault();
                if (optDefault.c.FirstOrDefault() != null)
                {
                    // da co thi cap nhat
                    int idoption = optDefault.c.FirstOrDefault().intid;
                    var optUser = context.TuychonCanbos.FirstOrDefault(p => p.intid == idoption);
                    optUser.strgiatri = strgiatri;
                    context.SaveChanges();
                }
                else
                {
                    // chua co thi them moi
                    TuychonCanbo cb = new TuychonCanbo();
                    cb.intidcanbo = idcanbo;
                    cb.intidoption = opt.FirstOrDefault().t.intid;
                    cb.strgiatri = strgiatri;
                    context.TuychonCanbos.Add(cb);
                    context.SaveChanges();
                }

                return 1;
            }
            catch
            {
                return 0;
            }
        }
        //public string GetTuychon(int idcanbo, int idoption)
        //{

        //}

        //int GetTuychonToInt(int idcanbo, int idoption);

        //bool GetTuychonToBool(int idcanbo, int idoption);
    }
}
