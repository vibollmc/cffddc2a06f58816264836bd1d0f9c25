using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface ILogGuiTonghopVBRepository
    {
        IQueryable<LogGuiTonghopVB>  LogGuiTonghopVBs { get; }

        LogGuiTonghopVB GetLogTonghopByID();

        void Themmoi(DateTime ngaygui, int intTrangthai);

        int Capnhat( int intTrangthai);

        int Capnhat( DateTime ngaygui, int intTrangthai);
    }
}
