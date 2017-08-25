using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.DTO.TracuuLuutru;
using QLVB.Domain.Entities;

namespace Store.Core.Contract
{
    public interface ITracuuLuutruManager
    {

        CapnhatLuutruViewModel GetFormCapnhatLuutru(int idvanban, int intloai);

        int SaveLuutruVanban(LuutruVanban luutru);

    }
}
