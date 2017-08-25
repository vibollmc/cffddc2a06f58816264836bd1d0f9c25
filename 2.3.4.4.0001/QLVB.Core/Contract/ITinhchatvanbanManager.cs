using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Tinhchatvb;

namespace QLVB.Core.Contract
{
    public interface ITinhchatvanbanManager
    {

        IEnumerable<Tinhchatvanban> GetTinhchatvb(int intloai);

        EditTinhchatViewModel GetEdit(int id);

        /// <summary>
        /// them moi/cap nhat tinh chat vb
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultFunction Save(EditTinhchatViewModel model);

        ResultFunction Delete(EditTinhchatViewModel model);
    }
}
