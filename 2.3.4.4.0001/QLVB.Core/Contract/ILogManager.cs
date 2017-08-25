using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Log;

namespace QLVB.Core.Contract
{
    public interface ILogManager
    {
        IEnumerable<LogViewModel> GetNhatkysudung(int intloai);
    }
}
