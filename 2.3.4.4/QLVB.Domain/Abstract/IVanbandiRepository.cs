using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IVanbandiRepository
    {
        IQueryable<Vanbandi> Vanbandis { get; }

        int Them(Vanbandi vb);

        void Sua(int intid, Vanbandi vb);

        void Xoa(int intid);

        void Duyet(int intid, int inttrangthai);

        void CapquyenxemPublic(int intid, int intpublic);


        /// <summary>
        /// cap nhat truong van ban dien tu sau khi da gui email
        /// </summary>
        /// <param name="intid"></param>
        /// <param name="intguivbdt"></param>
        void CapnhatVBDT(int intid, int intguivbdt);
    }
}