using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IAttachHosoRepository
    {
        IQueryable<AttachHoso> AttachHosos { get; }

        int Them(AttachHoso hs);

        void Xoa(int intid);

        int CapnhatPhathanh(int intid);

    }
}
