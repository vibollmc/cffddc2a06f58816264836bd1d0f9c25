using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface ITuychonCanboRepository
    {
        IQueryable<TuychonCanbo> TuychonCanbos { get; }

        int Save(string strthamso, string strgiatri, int idcanbo);

        //string GetTuychon(int idcanbo, int idoption);

        //int GetTuychonToInt(int idcanbo, int idoption);

        //bool GetTuychonToBool(int idcanbo, int idoption);
    }
}
