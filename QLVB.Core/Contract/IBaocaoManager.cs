using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Baocao;

namespace QLVB.Core.Contract
{
    public interface IBaocaoManager
    {
        CategoryBaocaoViewModel GetCategory();


        IEnumerable<SoVanban> GetSovanban(int idloai);

        IEnumerable<PhanloaiVanban> GetLoaivanban(int idloai);

        ListdonviViewModel GetListDonvi();


        string GetTenDonvi();

        string GetTenPhong(int idphong, bool isxuly);

        List<Vanbanden> GetSovanbanden(SoVanbandenViewModel model);

        string GetTensovanban(int idsovb);

        string GetTensovanban(string listidsovb);

        List<Vanbandi> GetSovanbandi(SovanbandiViewModel model);

        List<Vanbanden> GetLoaivanbanden(LoaivanbandenViewModel model);

        string GetTenLoaivanban(int idloai);

        List<Vanbandi> GetLoaivanbandi(LoaivanbandiViewModel model);

        PhieunhanVBDenViewModels GetNoidungPhieuVBDen(int idvanban);

    }
}
