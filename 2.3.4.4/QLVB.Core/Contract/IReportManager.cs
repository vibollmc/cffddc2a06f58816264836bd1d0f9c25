using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;

namespace QLVB.Core.Contract
{
    public interface IReportManager
    {
        string GetTenSovanban(int idsovb);


        IEnumerable<Vanbanden> GetRptSovanbanden(DateTime? dtengaybd, DateTime? dtengaykt, int idsovb);

    }
}
