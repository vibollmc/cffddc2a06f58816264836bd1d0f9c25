using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IMailInboxRepository
    {
        IQueryable<MailInbox> MailInboxs { get; }

        int Them(MailInbox mail);

        void Xoa(int intid, int idnguoixoa);

    }
}
