using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.Abstract
{
    public interface IStoreAttachVanbanRepository
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
