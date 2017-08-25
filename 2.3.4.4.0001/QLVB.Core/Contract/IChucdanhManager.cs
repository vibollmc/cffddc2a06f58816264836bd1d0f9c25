using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.DTO.Chucdanh;

namespace QLVB.Core.Contract
{
    public interface IChucdanhManager
    {
        IEnumerable<Chucdanh> GetChucdanh();

        IEnumerable<ListChucdanhViewModel> GetListChucdanh();

        Chucdanh GetChucdanh(int id);

        int Save(EditChucdanhViewModel model);

        int Delete(DeleteChucdanhViewModel model);
    }
}
