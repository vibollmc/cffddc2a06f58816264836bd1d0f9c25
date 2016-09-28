using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFMailInboxRepository : IMailInboxRepository
    {
        private QLVBDatabase context;

        public EFMailInboxRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<MailInbox> MailInboxs
        {
            get { return context.MailInboxs; }
        }

        public int Them(MailInbox mail)
        {
            try
            {
                // cac truong mac dinh
                mail.strngaynhan = DateTime.Now;
                mail.inttrangthai = (int)enumMailInbox.inttrangthai.IsActive;

                context.MailInboxs.Add(mail);
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
                var mail = context.MailInboxs.FirstOrDefault(p => p.intid == intid);
                if (mail != null)
                {
                    mail.inttrangthai = (int)enumMailInbox.inttrangthai.NotActive;
                    mail.intidnguoixoa = idnguoixoa;
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
