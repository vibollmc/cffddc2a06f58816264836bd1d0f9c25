using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IAttachVanbanRepository
    {
        IQueryable<AttachVanban> AttachVanbans { get; }

        int Them(AttachVanban vb);

        /// <summary>
        /// DeActive file trong database, chua xoa file vat ly
        /// </summary>
        /// <param name="idfile"></param>
        /// <returns>ten file</returns>
        string Xoa(int idfile, int idcanbo);


    }
}