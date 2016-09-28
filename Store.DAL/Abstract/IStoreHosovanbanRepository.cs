using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.Abstract
{
    public interface IStoreHosovanbanRepository
    {
        IQueryable<Hosovanban> Hosovanbans { get; }

        int Them(Hosovanban hoso);

        void Sua(int intid, Hosovanban hoso);

        void Xoa(int intid);

        /// <summary>
        /// xoa hosovanban
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="intloai"></param>
        /// <returns>idhosocongviec</returns>
        int Xoa(int idvanban, int intloai);
    }
}
