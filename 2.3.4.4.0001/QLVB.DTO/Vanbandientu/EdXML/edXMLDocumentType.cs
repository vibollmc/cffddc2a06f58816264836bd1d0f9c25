using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Vanbandientu.EdXML
{
    public class edXMLDocumentType
    {
        /// <summary>
        /// mac dinh = 2
        /// </summary>
        public int Type { get; set; }
        public string TypeName { get; set; }
    }

    public class enumedXMLDocumentType
    {
        public enum TypeQPPL
        {
            HienPhap = 1,
            Luat = 2,
            Phaplenh = 3,
            Lenh = 4,
            Nghiquyet = 5,
            NghiquyetLientich = 6,
            Nghidinh = 7,
            Quyetdinh = 8,
            Thongtu = 9,
            ThongtuLientich = 10,
            Chithi = 11,

        }
        public enum TypeVBHanhchinh
        {
            Nghiquyet = 1,
            Quyetdinh = 2,
            Chithi = 3,
            Quyche = 4,
            Quydinh = 5,
            Thongcao = 6
            //..........
            //..............
        }
    }
}
