using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFMailContentRepositoy : IMailContentRepository
    {
        private QLVBDatabase context;

        public EFMailContentRepositoy(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<MailContent> MailContents
        {
            get { return context.MailContents.AsNoTracking(); }
        }

        public int Them(MailContent mail)
        {
            mail.strngay = DateTime.Now;
            context.MailContents.Add(mail);
            context.SaveChanges();
            return mail.intid;
        }
    }
}
