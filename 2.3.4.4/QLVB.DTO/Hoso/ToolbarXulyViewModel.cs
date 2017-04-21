using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO.Hoso
{
    public class ToolbarXulyViewModel
    {
        public bool IsDisable { get; set; }
        public bool IsXulychinhDongHoso { get; set; }
        public bool IsDongHoso { get; set; }


        // can bo dang xu ly/xem ho so 
        public int IdCurrentUser { get; set; }

        // so phieu trinh can cho y kien chi dao
        public int intsophieutrinh { get; set; }

        public bool IsQuytrinh { get; set; }

        // kiem tra xem user con trong buoc xu ly quy trinh khong
        public bool IsXulyQuytrinh { get; set; }

        // kiem tra dieu kien re nhanh tai buoc xu ly 
        public bool IsDieukienXuly { get; set; }

        // kiem tra XLVB (phan biet voi quy trinh, va hscv)
        public bool IsXLVB { get; set; }

        // 
        public bool IsTamngungQuytrinh { get; set; }
        // kiem tra ghi nhan y kien xu ly cua nguoi phoi hop tai buoc xu ly
        public bool IsPhoihopXLQuytrinh { get; set; }

        // kiem tra xem tai buoc ke tiep co de nguoi dung chon buoc tiep theo khong 
        public bool IsChonBuocXLQuytrinh { get; set; }

        // quay ve trang nao
        public int? intBack { get; set; }


    }
}
