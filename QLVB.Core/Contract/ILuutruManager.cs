using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Luutru;

namespace QLVB.Core.Contract
{
    public interface ILuutruManager
    {
        IList<Diachiluutru> GetRootDonvi();

        EditDonviViewModel GetDonvi(int id);

        int SaveDonvi(EditDonviViewModel model);

        int DeleteDonvi(DeleteDonviViewModel model);
    }
}
