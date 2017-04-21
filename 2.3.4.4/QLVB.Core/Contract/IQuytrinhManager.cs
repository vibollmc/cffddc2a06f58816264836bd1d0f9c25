using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO.Quytrinh;
using QLVB.DTO;

namespace QLVB.Core.Contract
{
    public interface IQuytrinhManager
    {
        /// <summary>
        /// form them moi/cap nhat Loai quy trinh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EditLoaiQuytrinhViewModel GetEditLoaiQuytrinh(int id);

        ResultFunction SaveLoaiQuytrinh(int id, string strten);

        ListLoaiQuytrinhViewModel GetListLoaiQuytrinh();


        /// <summary>
        /// form them moi/cap nhat ten quy trinh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EditQuytrinhViewModel GetEditQuytrinh(int id);

        ResultFunction SaveQuytrinh(EditQuytrinhViewModel model);
        /// <summary>
        /// luu js flowchart        
        /// </summary>
        /// <param name="strjson"></param>
        /// <returns></returns>
        ResultFunction SaveFlowChart(int idquytrinh, string strjson);

        /// <summary>
        /// doc quy trinh
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <returns>json</returns>
        string ReadFlowChart(int idquytrinh);

        /// <summary>
        /// lay ten quy trinh
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <returns></returns>
        string GetFlowChartName(int idquytrinh);

        /// <summary>
        /// xoa toan bo quy trinh
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <returns></returns>
        ResultFunction DeleteFlowChart(int idquytrinh);

        /// <summary>
        /// lay cac thong tin xu ly cua Node
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <param name="NodeId"></param>
        /// <returns></returns>
        EditThongtinXulyViewModel GetThongtinXuly(int idquytrinh, string NodeId);

        /// <summary>
        /// lay thong tin cac user thuoc don vi : iddonvi
        /// </summary>
        /// <param name="iddonvi"></param>
        /// <returns></returns>
        IEnumerable<QLVB.DTO.Donvi.EditUserViewModel> GetListCanbo(int iddonvi);

        /// <summary>
        /// luu cac thong tin xu ly tai node
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultFunction SaveThongtinXuly(EditThongtinXulyViewModel model);

        /// <summary>
        /// lay toan bo quy trinh xu ly
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <returns></returns>
        QuytrinhXulyViewModels GetQuytrinhXuly(int idquytrinh);


        ResultFunction SaveVersion(int idquytrinh);

        CategoryNgayViewModel GetCategoryNgay(int idquytrinh);

        /// <summary>
        ///  doc quy trinh trong phien ban
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <param name="strngay"></param>
        /// <returns></returns>
        string ReadFlowChartVersion(int idquytrinh, string strngay);

        /// <summary>
        /// hien thi thong tin xu ly tai node 
        /// </summary>
        /// <param name="idquytrinh"></param>
        /// <param name="NodeId"></param>
        /// <returns></returns>
        ViewThongtinXulyViewModel LoadThongtinXuly(int idquytrinh, string strngay, string NodeId);



    }
}
