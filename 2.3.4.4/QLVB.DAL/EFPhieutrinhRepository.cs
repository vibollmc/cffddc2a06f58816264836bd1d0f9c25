using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFPhieutrinhRepository : IPhieutrinhRepository
    {
        private QLVBDatabase context;

        public EFPhieutrinhRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Phieutrinh> Phieutrinhs
        {
            get { return context.Phieutrinhs; }
        }

        public int ThemNoidungtrinh(int idhoso, int idcanbotrinh, int idlanhdaotrinh, string strnoidungtrinh)
        {
            Phieutrinh hs = new Phieutrinh();
            hs.intidhosocongviec = idhoso;
            hs.intidcanbotrinh = idcanbotrinh;
            hs.strnoidungtrinh = strnoidungtrinh;
            hs.intidlanhdao = idlanhdaotrinh;

            // default
            hs.strngaytrinh = DateTime.Now;
            hs.inttrangthaitrinh = (int)enumphieutrinh.inttrangthaitrinh.Datrinh;
            hs.inttrangthaichidao = (int)enumphieutrinh.inttrangthaichidao.Chuachidao;
            try
            {
                context.Phieutrinhs.Add(hs);
                context.SaveChanges();
                return hs.intid;
            }
            catch
            {
                return 0;
            }
        }

        public int ThemYkienchidao(int idphieutrinh, int idlanhdao, string strykienchidao)
        {
            try
            {
                var hs = context.Phieutrinhs.FirstOrDefault(p => p.intid == idphieutrinh);
                //hs.intidlanhdao = idlanhdao;
                hs.strykienchidao = strykienchidao;
                hs.strngaychidao = DateTime.Now;
                hs.inttrangthaichidao = (int)enumphieutrinh.inttrangthaichidao.Dachidao;

                context.SaveChanges();
                return idphieutrinh;
            }
            catch
            {
                return 0;
            }
        }
    }
}
