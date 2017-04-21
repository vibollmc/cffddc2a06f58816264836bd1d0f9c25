using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Luutru;
using QLVB.DTO.TracuuLuutru;

namespace QLVB.Core.Contract
{
    public interface ILuutruManager
    {
        IList<Diachiluutru> GetRootDonvi();

        EditDonviViewModel GetDonvi(int id);

        int SaveDonvi(EditDonviViewModel model);

        int DeleteDonvi(DeleteDonviViewModel model);

        CapnhatLuutruViewModel GetFormCapnhatLuutru(int idvanban, int intloai);

        int SaveLuutruVanban(LuutruVanban luutru);
    }
}
