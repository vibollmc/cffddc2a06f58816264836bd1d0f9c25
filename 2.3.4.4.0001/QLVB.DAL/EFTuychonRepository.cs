using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;


namespace QLVB.DAL
{
    public class EFTuychonRepository : ITuychonRepository
    {
        private QLVBDatabase context;

        public EFTuychonRepository(QLVBDatabase _context)
        {
            context = _context;
        }
        public IQueryable<Tuychon> Tuychons
        {
            get
            {
                return context.Tuychons.AsNoTracking()
                    .Where(p => p.inttrangthai == (int)enumTuychon.inttrangthai.IsActive);
            }
        }

        public string GetTuychon(string strthamso)
        {
            try
            {
                return context.Tuychons
                .FirstOrDefault(p => p.strthamso == strthamso)
                .strgiatri;
            }
            catch
            {
                return string.Empty;
            }
        }

        public int GetTuychonToInt(string strthamso)
        {
            try
            {
                string strgiatri = context.Tuychons
                    .FirstOrDefault(p => p.strthamso == strthamso)
                    .strgiatri;
                int intgiatri = Convert.ToInt32(strgiatri);
                return intgiatri;
            }
            catch
            {
                return 0;
            }
        }

        public bool GetTuychonToBool(string strthamso)
        {
            try
            {
                string strgiatri = context.Tuychons
                   .FirstOrDefault(p => p.strthamso == strthamso)
                   .strgiatri;
                bool isgiatri = (strgiatri.ToLower() == "true") ? true : false;
                return isgiatri;
            }
            catch
            {
                return false;
            }
        }
    }
}
