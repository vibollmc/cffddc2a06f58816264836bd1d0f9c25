using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IHosovanbanRepository
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