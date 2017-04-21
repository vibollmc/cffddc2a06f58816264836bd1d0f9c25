using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class EFLogGuiTonghopVBRepository:ILogGuiTonghopVBRepository
    {
        private QLVBDatabase context;

        public EFLogGuiTonghopVBRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<LogGuiTonghopVB> LogGuiTonghopVBs
        {
            get { return context.LogGuiTonghopVBs; }
        }
        public LogGuiTonghopVB GetLogTonghopByID()
        {
            try
            {
                //return context.LogGuiTonghopVBs.FirstOrDefault(p => p.intid == 1);
                return context.LogGuiTonghopVBs.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Themmoi(DateTime ngaygui, int intTrangthai)
        {
            LogGuiTonghopVB log = new LogGuiTonghopVB
            {
                Ngaygui = ngaygui,
                intTrangthai = intTrangthai
            };

            try
            {
                context.LogGuiTonghopVBs.Add(log);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Capnhat( int intTrangthai)
        {
            try
            {
                var hs = context.LogGuiTonghopVBs.FirstOrDefault(p => p.intid == 1);
                if (hs != null)
                {
                    hs.intTrangthai = intTrangthai;
                    context.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Capnhat( DateTime ngaygui, int intTrangthai)
        {
            try
            {
                var hs = context.LogGuiTonghopVBs.FirstOrDefault(p => p.intid == 1);
                if (hs != null)
                {
                    hs.Ngaygui = ngaygui;
                    hs.intTrangthai = intTrangthai;
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
