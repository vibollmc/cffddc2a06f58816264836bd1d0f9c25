using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Common.Utilities
{
    //======================================================
    // cac quyen su dung trong phan mem QLVB
    // tương ứng với các quyền trong table quyen
    //======================================================

    #region HETHONG

    public class RoleCauhinhhethong
    {
        public const string Truycap = "Config";
        public const string Capnhat = "UpdateConfig";
        public const string SaoluuDB = "Backup";
    }

    public class RoleNhomquyen
    {
        public const string Truycap = "RoleGroup";
        public const string CapnhatNhomquyen = "UpdateRoleGroup";
        public const string Themcanbovaonhomquyen = "UpdateUserGroup";
    }

    public class RoleNhatkysudung
    {
        public const string Truycap = "Log";
    }

    public class RoleNhatkyLoi
    {
        public const string Truycap = "Elmah";
    }

    #endregion HETHONG

    #region DANHMUC

    public class RoleChucdanh
    {
        public const string Truycap = "Chucdanh";
        public const string Capnhat = "UpdateChucdanh";
    }

    public class RoleDonvitructhuoc
    {
        public const string Truycap = "Donvi";
        public const string CapnhatDonvi = "UpdateDonvi";
        public const string CapnhatCanbo = "UpdateCanbo";
        public const string ChuyenCanbo = "MoveCanbo";
        public const string Phanvaitroxuly = "RoleCanbo";
    }

    public class RoleCapcoquanbenngoai
    {
        public const string Truycap = "Capcoquan";
        public const string Capnhat = "UpdateCapcoquan";
    }

    public class RoleTinhchatvanban
    {
        public const string Truycap = "Tinhchatvanban";
        public const string Capnhat = "UpdateTinhchatvanban";
    }

    public class RoleLinhvuc
    {
        public const string Truycap = "Linhvuc";
        public const string Capnhat = "UpdateLinhvuc";
    }

    public class RoleDangVanbanlienquan
    {
        public const string Truycap = "Vblq";
        public const string Capnhat = "UpdateVblq";
    }

    public class RoleVaitroVanbanlienquan
    {
        public const string Truycap = "RoleVblq";
        public const string Capnhat = "UpdateRoleVblq";
    }

    public class RoleLoaivanbanden
    {
        public const string Truycap = "Loaivbden";
        public const string Capnhat = "UpdateLoaivbden";
    }

    public class RoleLoaivanbandi
    {
        public const string Truycap = "Loaivbdi";
        public const string Capnhat = "UpdateLoaivbdi";
    }

    public class RoleLoaivanbanduthao
    {
        public const string Truycap = "Loaivbduthao";
        public const string Capnhat = "UpdateLoaivbduthao";
    }

    public class RoleLuutru
    {
        public const string Truycap = "Luutru";
        public const string Capnhat = "UpdateLuutru";
    }

    public class RoleSovanban
    {
        public const string Truycap = "Sovb";
        public const string Capnhat = "UpdateSovb";
    }

    public class RoleDMQuytrinhxuly
    {
        public const string Truycap = "Quytrinh";
        public const string Capnhat = "UpdateQuytrinh";
    }

    public class RoleYKCDDonvi
    {
        public const string Truycap = "YKCDDonvi";
        public const string Capnhat = "UpdateYKCDDonvi";
    }

    #endregion DANHMUC

    #region CONGCU

    public class RoleBaocao
    {
        public const string Truycap = "Baocao";
        // xem tung loai bao cao
    }

    public class RoleDoimatkhau
    {
        public const string Truycap = "Doimatkhau";
    }

    public class RoleUyquyen
    {
        public const string Truycap = "Uyquyen";
    }
    public class RoleTuychoncanhan
    {
        public const string Truycap = "Option";
    }

    #endregion CONGCU

    #region HOSOCONGVIEC

    public class RoleHosocongviec
    {
        public const string TruycapHoso = "Hoso";
        //public const string TruycapTonghopxuly = "Tinhhinhxuly";
    }

    public class RoleTinhhinhxulyVBDen
    {
        public const string Truycap = "Tinhhinhxuly";
        public const string Xemtatca = "AllTinhhinhxuly";
    }
    public class RoleTinhhinhxulyQuytrinh
    {
        public const string Truycap = "TinhhinhxulyQuytrinh";
        public const string Xemtatca = "AllTinhhinhxulyQuytrinh";
    }

    public class RoleTinhhinhxulyVBDi
    {
        public const string Truycap = "TinhhinhxulyVbdi";
        public const string Xemtatca = "AllTinhhinhxulyVbdi";
    }

    #endregion HOSOCONGVIEC

    #region VANBAN

    public class RoleVanbanden
    {
        public const string Truycap = "Vanbanden";
        public const string Capnhatvb = "UpdateVanbanden";

        // -----------   quyen moi
        public const string Xoavb = "DeleteVanbanden";

        public const string Duyetvb = "DuyetVanbanden";
        public const string Xulyvb = "XulyVanbanden";
        public const string PhanXLvb = "PhanxulyVanbanden";
        public const string GuiEmail = "EmailVanbanden";
        public const string Xemtatcavb = "AllVanbanden";
        public const string XemtatcaVBLQ = "AllVBLQVanbanden";
        // thieu cap quyen xem van ban den
        public const string Capquyenxem = "PhanxemVanbanden";

        public const string PhanQuytrinh = "PhanQuytrinhVanbanden";

        public const string YKCDVanbanden = "YKCDVanbanden";

        public const string Luutruvanban = "LuutruVanbanden";

    }

    public class RoleVanbandi
    {
        public const string Truycap = "Vanbandi";
        public const string Capnhatvb = "UpdateVanbandi";

        //-------- quyen moi
        public const string Xoavb = "DeleteVanbandi";

        public const string Capquyenxem = "PhanxemVanbandi";
        public const string Duyetvb = "DuyetVanbandi";
        public const string GuiEmail = "EmailVanbandi";
        public const string Hoibaovb = "HoibaoVanbandi";
        public const string Xemtatcavb = "AllVanbandi";
        public const string XemtatcaVBLQ = "AllVBLQVanbandi";

        public const string YKCDVanbandi = "YKCDVanbandi";

        public const string Luutruvanban = "LuutruVanbandi";
    }

    public class RoleVanbandendientu
    {
        public const string Truycap = "Vanbandendientu";
        public const string Capnhat = "UpdateVanbandendientu";
    }

    public class RoleVanbandidientu
    {
        public const string Truycap = "Vanbandidientu";
    }

    public class RoleYKCDXuly
    {
        public const string Truycap = "YKCDXuly";
    }

    public class RoleTracuuVanbanden
    {
        public const string Truycap = "TracuuVanbanden";
        public const string Xemtatcavb = "AllTracuuVanbanden";
        public const string Luutruvanban = "LuutruTracuuVanbanden";
    }

    public class RoleTracuuVanbandi
    {
        public const string Truycap = "TracuuVanbandi";
        public const string Xemtatcavb = "AllTracuuVanbandi";
        public const string Luutruvanban = "LuutruTracuuVanbandi";
    }


    #endregion VANBAN
}
