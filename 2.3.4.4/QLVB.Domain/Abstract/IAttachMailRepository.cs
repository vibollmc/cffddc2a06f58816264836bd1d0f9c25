using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IAttachMailRepository
    {
        IQueryable<AttachMail> AttachMails { get; }

        int Them(AttachMail mail);

        void Xoa(int intid, int idnguoixoa);
    }
}
