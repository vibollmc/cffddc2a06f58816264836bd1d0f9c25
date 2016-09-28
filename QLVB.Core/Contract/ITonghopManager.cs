using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO.Tonghop;
using QLVB.DTO;

namespace QLVB.Core.Contract
{
    public interface ITonghopManager
    {
        IEnumerable<ListTonghopVBDenViewModel> GetTonghopVanbanden();

        ResultFunction UpdateStatusVanbanden(int idtailieu);

        IEnumerable<ListTonghopVBDenViewModel> GetTonghopHosoXLVBDen();
    }
}
