using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Linhvuc;

namespace QLVB.Core.Contract
{
    public interface ILinhvucManager
    {
        IEnumerable<ListLinhvucViewModel> GetListLinhvuc();

        EditLinhvucViewModel GetEditLinhvuc(int id);

        ResultFunction Save(EditLinhvucViewModel model);

        ResultFunction Delete(int id);
    }
}
