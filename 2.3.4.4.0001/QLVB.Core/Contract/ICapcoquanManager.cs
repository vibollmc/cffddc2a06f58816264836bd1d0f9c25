using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Capcoquan;


namespace QLVB.Core.Contract
{
    public interface ICapcoquanManager
    {
        IEnumerable<Khoiphathanh> GetListKhoiph();

        IEnumerable<Tochucdoitac> GetListTochuc(int idkhoiph);

        EditKhoiphViewModel GetEditKhoiph(int id);

        ResultFunction SaveKhoiph(EditKhoiphViewModel model);

        ResultFunction DeleteKhoiph(EditKhoiphViewModel model);

        EditTochucViewModel GetEditTochuc(int id);

        ResultFunction SaveTochuc(EditTochucViewModel model);

        ResultFunction DeleteTochuc(EditTochucViewModel model);

        CapnhatDanhmucDonviViewModel GetDanhmucDonvi();

        int UpdateDanhmucDonvi(string listid);

        int AddNewDanhmucDonvi(string listemail, int idkhoiph);

    }
}
