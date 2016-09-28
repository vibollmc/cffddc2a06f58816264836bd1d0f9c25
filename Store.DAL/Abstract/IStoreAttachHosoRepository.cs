using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.Abstract
{
    public interface IStoreAttachHosoRepository
    {
        IQueryable<AttachHoso> AttachHosos { get; }

        int Them(AttachHoso hs);

        void Xoa(int intid);

        int CapnhatPhathanh(int intid);
    }
}
