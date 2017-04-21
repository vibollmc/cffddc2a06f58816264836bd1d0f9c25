using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IQuytrinhRepository
    {
        IQueryable<Quytrinh> AllQuytrinhs { get; }

        IQueryable<Quytrinh> ActiveQuytrinhs { get; }

        int Them(Quytrinh hs);

        int Them(string strten, int idloai, int intsongay, DateTime? dteThoigianApdung, int inttrangthai);

        int Sua(int intid, string strten, int intsongay, DateTime? dteThoigianApdung, int inttrangthai);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intid"></param>
        /// <param name="numberOfElements"></param>
        /// <returns>tra ve ten quy trinh</returns>
        string SaveNumberOfElements(int intid, int? numberOfElements);

        int? GetNumberOfElements(int idquytrinh);



    }
}
