using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFLuutruVanbanRepository : ILuutruVanbanRepository
    {
        private QLVBDatabase context;

        public EFLuutruVanbanRepository(QLVBDatabase _context)
        {
            context = _context;
        }
        public IQueryable<LuutruVanban> LuutruVanbans
        {
            get { return context.LuutruVanbans; }
        }

        public int Them(LuutruVanban vb)
        {
            try
            {
                vb.strngaycapnhat = DateTime.Now;
                context.LuutruVanbans.Add(vb);
                context.SaveChanges();
                return vb.intid;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Sua(int intid, LuutruVanban vb)
        {
            try
            {
                var lt = context.LuutruVanbans.FirstOrDefault(p => p.intid == intid);
                if (lt != null)
                {
                    lt.inthopso = vb.inthopso;
                    lt.intdonvibaoquan = vb.intdonvibaoquan;
                    lt.strthoihanbaoquan = vb.strthoihanbaoquan;
                    lt.strnoidung = vb.strnoidung;
                    lt.intidnguoicapnhat = vb.intidnguoicapnhat;
                    lt.strngaycapnhat = DateTime.Now;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
