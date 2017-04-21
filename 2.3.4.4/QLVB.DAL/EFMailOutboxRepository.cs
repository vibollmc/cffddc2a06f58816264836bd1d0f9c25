using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFMailOutboxRepository : IMailOutboxRepository
    {
        private QLVBDatabase context;

        public EFMailOutboxRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<MailOutbox> MailOutboxs
        {
            get { return context.MailOutboxs; }
        }

        public int Them(MailOutbox mail)
        {
            try
            {
                // cac truong mac dinh
                mail.strngaygui = DateTime.Now;
                mail.inttrangthai = (int)enumMailOutbox.inttrangthai.IsActive;

                context.MailOutboxs.Add(mail);
                context.SaveChanges();
                return mail.intid;
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
