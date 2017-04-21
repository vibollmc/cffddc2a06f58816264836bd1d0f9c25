using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFTinhchatvanbanRepository : ITinhchatvanbanRepository
    {
        private QLVBDatabase context;

        public EFTinhchatvanbanRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Tinhchatvanban> GetActiveTinhchatvanbans
        {
            get
            {
                return context.Tinhchatvanbans
                  .Where(p => p.inttrangthai == (int)enumTinhchatvanban.inttrangthai.IsActive);
            }
        }

        public IQueryable<Tinhchatvanban> GetAllTinhchatvanbans
        {
            get
            {
                return context.Tinhchatvanbans;
            }
        }

        public void AddTinhchatvb(Tinhchatvanban vb)
        {
            vb.inttrangthai = (int)enumTinhchatvanban.inttrangthai.IsActive;
            context.Tinhchatvanbans.Add(vb);
            context.SaveChanges();
        }

        public void EditTinhchatvb(Int32 intid, Tinhchatvanban vb)
        {
            Tinhchatvanban vanban = context.Tinhchatvanbans.FirstOrDefault(p => p.intid == intid);
            vanban.intloai = vb.intloai;
            vanban.strkyhieu = vb.strkyhieu;
            vanban.strmota = vb.strmota;
            vanban.strtentinhchatvb = vb.strtentinhchatvb;
            context.SaveChanges();
        }

        public void DeleteTinhchatvb(Int32 intid)
        {
            Tinhchatvanban vanban = context.Tinhchatvanbans.FirstOrDefault(p => p.intid == intid);
            //context.Tinhchatvanbans.Remove(vanban);
            vanban.inttrangthai = (int)enumTinhchatvanban.inttrangthai.NotActive;
            context.SaveChanges();
        }
    }
}
