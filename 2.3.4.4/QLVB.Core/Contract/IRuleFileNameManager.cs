using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Core.Contract
{
    /// <summary>
    /// các quy định đặt tên file để đính kèm tự dộng
    /// </summary>
    public interface IRuleFileNameManager
    {
        /// <summary>
        /// lay tên sổ văn bản
        /// </summary>
        /// <param name="strquytac"></param>
        /// <param name="strtenvanban"></param>
        /// <returns>vd: CV</returns>
        string GetMaSo(string strquytac, string strtenvanban);

        /// <summary>
        /// lấy số văn bản
        /// </summary>
        /// <param name="strquytac"></param>
        /// <param name="strtenvanban"></param>
        /// <returns>vd: 234</returns>
        int GetSo(string strquytac, string strtenvanban);

        /// <summary>
        /// Lấy năm văn bản
        /// </summary>
        /// <param name="strquytac"></param>
        /// <param name="strtenvanban"></param>
        /// <returns>vd: 2013</returns>
        int GetYear(string strquytac, string strtenvanban);

        /// <summary>
        /// lay idvanban theo quy tac dat ten 
        /// tu dong lien ket/dong hoso/ vb den/di 
        /// </summary>
        /// <param name="strquytac"></param>
        /// <param name="strtenvanban"></param>
        /// <returns>
        /// 0: loi khong tim thay id vanban
        /// !=0: id van ban
        /// </returns>
        int GetIdVanban(string strquytac, string strtenvanban);

        /// <summary>
        /// lấy tên sổ vb và số đến/đi của văn bản theo quy tắc đã có
        /// </summary>
        /// <param name="strquytac"></param>
        /// <param name="strtenvanban"></param>
        /// <returns> trả về
        /// loaivb: loại sổ văn bản (đến / đi)
        /// idvanban: id văn bản
        /// </returns>
        Dictionary<string, string> GetFromFileName(string strquytac, string strtenvanban);

        /// <summary>
        /// lien ket van ban den/di
        /// </summary>
        /// <param name="intLoai"></param>
        /// <param name="idvanbanden"></param>
        /// <param name="idvanbandi"></param>
        /// <returns></returns>
        int LienketVanban(int intLoai, int idvanbanden, int idvanbandi);

        /// <summary>
        /// tra ve cac danh sach quy tac dat ten file
        /// </summary>
        /// <returns></returns>
        string GetQuytacTenFile(string strquytac);

    }
}
