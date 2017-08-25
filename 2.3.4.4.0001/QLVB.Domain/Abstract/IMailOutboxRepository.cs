using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IMailOutboxRepository
    {
        IQueryable<MailOutbox> MailOutboxs { get; }

        int Them(MailOutbox mail);

    }
}
