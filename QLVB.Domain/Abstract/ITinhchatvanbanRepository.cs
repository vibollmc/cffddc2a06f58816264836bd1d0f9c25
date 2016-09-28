using QLVB.Domain.Entities;
using System;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface ITinhchatvanbanRepository
    {
        IQueryable<Tinhchatvanban> GetActiveTinhchatvanbans { get; }

        IQueryable<Tinhchatvanban> GetAllTinhchatvanbans { get; }

        void AddTinhchatvb(Tinhchatvanban vb);

        void EditTinhchatvb(Int32 intid, Tinhchatvanban vb);

        void DeleteTinhchatvb(Int32 intid);
    }
}