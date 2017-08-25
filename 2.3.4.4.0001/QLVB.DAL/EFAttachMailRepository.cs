using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class EFAttachMailRepository : IAttachMailRepository
    {
        private QLVBDatabase context;

        public EFAttachMailRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<AttachMail> AttachMails
        {
            get { return context.AttachMails; }
        }

        public int Them(AttachMail mail)
        {
            try
            {
                // cac truong mac dinh
                mail.inttrangthai = (int)enumAttachMail.inttrangthai.IsActive;
                mail.strngaycapnhat = DateTime.Now;

                context.AttachMails.Add(mail);
                context.SaveChanges();
                return mail.intid;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Xoa(int intid, int idnguoixoa)
        {
            try
            {
                var mail = context.AttachMails.FirstOrDefault(p => p.intid == intid);
                if (mail != null)
                {
                    mail.intidnguoixoa = idnguoixoa;
                    mail.strngayxoa = DateTime.Now;
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
