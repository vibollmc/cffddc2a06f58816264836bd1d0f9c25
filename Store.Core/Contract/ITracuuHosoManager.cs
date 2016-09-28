using QLVB.DTO.Hoso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Contract
{
    public interface ITracuuHosoManager
    {
        #region ViewDetailHosocongviec

        /// <summary>
        /// hien thi thong thi chi tiet va qua trinh xu ly cua ho so
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <returns></returns>
        DetailHosoViewModel GetDetailHoso(int idhosocongviec);

        #endregion ViewDetailHosocongviec

    }
}
