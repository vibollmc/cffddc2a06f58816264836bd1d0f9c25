using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface ITonghopCanboRepository
    {
        IQueryable<TonghopCanbo> TonghopCanbos { get; }

        int Them(TonghopCanbo th);

        /// <summary>
        /// chi cap nhat trang thai da xem cua nhung vbden
        /// </summary>
        /// <param name="idcanbo"></param>
        /// <param name="idtailieu"></param>        
        /// <returns>0/1</returns>
        int CapnhatTrangthaiVBDen(int idcanbo, int idtailieu);

        /// <summary>
        /// cap nhat trang thai cua hoso xlvb den:phieu trinh, y kien
        /// </summary>
        /// <param name="idcanbo"></param>
        /// <param name="idtailieu"></param>        
        /// <returns></returns>
        int CapnhatTrangthaiHosoVBDen(int idcanbo, int idtailieu);
    }
}
