using System.Web;
using FluentSecurity;
using QLVB.WebUI.Controllers;
using QLVB.Common.Utilities;
using Elmah.Mvc;
using QLVB.WebUI.Common.Session;
using QLVB.WebUI.Controllers.Store;


namespace QLVB.WebUI.Common.Security
{
    /// <summary>
    /// kiem tra quyen cua tung action
    /// </summary>
    public class SecurityServices
    {
        public static ISecurityConfiguration SetupFluentSecurity()
        {
            SecurityConfigurator.Configure(configuration =>
            {
                // Let Fluent Security know how to get the authentication status of the current user
                configuration.GetAuthenticationStatusFrom(() => HttpContext.Current.User.Identity.IsAuthenticated);
                // Let Fluent Security know how to get the roles for the current user

                //configuration.GetRolesFrom(() => RoleServices.GetUserRole());
                configuration.GetRolesFrom(() => UserStatus.GetUserRole());

                // This is where you set up the policies you want Fluent Security to enforce on your controllers and actions

                //================TỪ CHỐI TRUY CẬP CHƯA ĐĂNG NHẬP====================================
                configuration.ForAllControllers().DenyAnonymousAccess();

                //=================XỬ LÝ KHI KHÔNG CÓ QUYỀN TRUY CẬP===================================
                configuration.DefaultPolicyViolationHandlerIs(() => new DefaultPolicyViolationHandler());

                //=================CÁC MỤC DÙNG CHUNG==========================================
                configuration.For<OAuthController>().Ignore();
                configuration.For<HomeController>().DenyAnonymousAccess();
                configuration.For<AccountController>().Ignore();
                configuration.For<FileController>().Ignore();

                //configuration.For<SessionController>().DenyAnonymousAccess();
                //// dung de test chuc nang khi debug
                configuration.For<TestController>().Ignore();

                var request = new Kendo.Mvc.UI.DataSourceRequest();
                var collection = new System.Web.Mvc.FormCollection();

                #region Hethong
                //========================QUẢN TRỊ HỆ THỐNG==================================
                configuration.For<HethongController>(x => x.Config()).RequireAnyRole(RoleCauhinhhethong.Truycap);
                configuration.For<HethongController>(x => x.SaveConfig(collection)).RequireAnyRole(RoleCauhinhhethong.Truycap);

                //========================SAO LƯU DỮ LIỆU==================================
                configuration.For<HethongController>(x => x._SaveBackup(new QLVB.DTO.Hethong.BackupViewModel())).RequireAnyRole(RoleCauhinhhethong.SaoluuDB);
                configuration.For<HethongController>(x => x.Backup()).RequireAnyRole(RoleCauhinhhethong.SaoluuDB);

                ////===========PHÂN QUYỀN SỬ DỤNG THEO NHÓM============================
                configuration.For<HethongController>(x => x.RoleGroup()).RequireAnyRole(RoleNhomquyen.Truycap); // xem
                configuration.For<HethongController>(x => x.Group_Read(request)).RequireAnyRole(RoleNhomquyen.Truycap);
                configuration.For<HethongController>(x => x._AjaxUser(1)).RequireAnyRole(RoleNhomquyen.Truycap);
                configuration.For<HethongController>(x => x.User_Read(request, 1)).RequireAnyRole(RoleNhomquyen.Truycap);
                configuration.For<HethongController>(x => x._AjaxQuyen(1)).RequireAnyRole(RoleNhomquyen.Truycap);

                configuration.For<HethongController>(x => x.SaveQuyen(collection)).RequireAnyRole(RoleNhomquyen.CapnhatNhomquyen);
                configuration.For<HethongController>(x => x._AjaxEditGroup(1)).RequireAnyRole(RoleNhomquyen.CapnhatNhomquyen);
                configuration.For<HethongController>(x => x.SaveGroup()).RequireAnyRole(RoleNhomquyen.CapnhatNhomquyen);
                configuration.For<HethongController>(x => x.DeleteGroup(1)).RequireAnyRole(RoleNhomquyen.CapnhatNhomquyen); // update


                ////===========NHẬT KÝ ======================================
                configuration.For<LogController>().RequireAnyRole(RoleNhatkysudung.Truycap);
                configuration.For<ElmahController>().RequireAnyRole(RoleNhatkyLoi.Truycap);

                #endregion Hethong

                #region Danhmuc

                //=============DANH MỤC CHỨC DANH====================================
                configuration.For<ChucdanhController>(x => x.Index()).RequireAnyRole(RoleChucdanh.Truycap);
                configuration.For<ChucdanhController>(x => x.Chucdanh_Read(request)).RequireAnyRole(RoleChucdanh.Truycap);

                configuration.For<ChucdanhController>(x => x._AjaxEditChucdanh(1)).RequireAnyRole(RoleChucdanh.Capnhat);
                configuration.For<ChucdanhController>(x => x.Save(new QLVB.DTO.Chucdanh.EditChucdanhViewModel()))
                                .RequireAnyRole(RoleChucdanh.Capnhat);
                configuration.For<ChucdanhController>(x => x._AjaxDeleteChucdanh(1)).RequireAnyRole(RoleChucdanh.Capnhat);
                configuration.For<ChucdanhController>(x => x.Delete(new QLVB.DTO.Chucdanh.DeleteChucdanhViewModel()))
                                .RequireAnyRole(RoleChucdanh.Capnhat);

                //==============DANH MỤC ĐƠN VỊ TRỰC THUỘC========================================
                configuration.For<DonviController>().RequireAnyRole(RoleDonvitructhuoc.Truycap);
                configuration.For<DonviController>(x => x.Index()).RequireAnyRole(RoleDonvitructhuoc.Truycap);
                configuration.For<DonviController>(x => x._Listdonvi()).RequireAnyRole(RoleDonvitructhuoc.Truycap);
                configuration.For<DonviController>(x => x._ListUser(1)).RequireAnyRole(RoleDonvitructhuoc.Truycap);
                configuration.For<DonviController>(x => x.User_Read(request, 1)).RequireAnyRole(RoleDonvitructhuoc.Truycap);


                configuration.For<DonviController>(x => x._formEditDonvi(1, "")).RequireAnyRole(RoleDonvitructhuoc.CapnhatDonvi);
                configuration.For<DonviController>(x => x.SaveDonvi(default(QLVB.DTO.Donvi.EditDonviViewModel)))
                                .RequireAnyRole(RoleDonvitructhuoc.CapnhatDonvi);
                configuration.For<DonviController>(x => x.SaveDonvi2(default(QLVB.DTO.Donvi.EditDonviViewModel)))
                                .RequireAnyRole(RoleDonvitructhuoc.CapnhatDonvi);
                configuration.For<DonviController>(x => x._formDeleteDonvi(1)).RequireAnyRole(RoleDonvitructhuoc.CapnhatDonvi);
                configuration.For<DonviController>(x => x.DeleteDonvi(default(QLVB.DTO.Donvi.DeleteDonviViewModel)))
                                .RequireAnyRole(RoleDonvitructhuoc.CapnhatDonvi);

                configuration.For<DonviController>(x => x._formEditUser(1, 1)).RequireAnyRole(RoleDonvitructhuoc.CapnhatCanbo);
                configuration.For<DonviController>(x => x.SaveUser(collection)).RequireAnyRole(RoleDonvitructhuoc.CapnhatCanbo);
                configuration.For<DonviController>(x => x._formDeleteUser(1)).RequireAnyRole(RoleDonvitructhuoc.CapnhatCanbo);
                configuration.For<DonviController>(x => x.DeleteUser(1)).RequireAnyRole(RoleDonvitructhuoc.CapnhatCanbo);

                configuration.For<DonviController>(x => x._formMoveUser(1, 1)).RequireAnyRole(RoleDonvitructhuoc.ChuyenCanbo);
                configuration.For<DonviController>(x => x.SaveMoveUser("", 1)).RequireAnyRole(RoleDonvitructhuoc.ChuyenCanbo);

                //==============CẤP CƠ QUAN BÊN NGOÀI==============================
                configuration.For<CapcoquanController>().RequireAnyRole(RoleCapcoquanbenngoai.Truycap);

                configuration.For<CapcoquanController>(x => x._formEditKhoiph(1)).RequireAnyRole(RoleCapcoquanbenngoai.Capnhat);
                configuration.For<CapcoquanController>(x => x.SaveKhoiph(collection)).RequireAnyRole(RoleCapcoquanbenngoai.Capnhat);
                configuration.For<CapcoquanController>(x => x.DeleteKhoiph(1, "")).RequireAnyRole(RoleCapcoquanbenngoai.Capnhat);

                configuration.For<CapcoquanController>(x => x.SaveTochuc(collection)).RequireAnyRole(RoleCapcoquanbenngoai.Capnhat);
                configuration.For<CapcoquanController>(x => x.DeleteTochuc(1, "")).RequireAnyRole(RoleCapcoquanbenngoai.Capnhat);
                configuration.For<CapcoquanController>(x => x.CapnhatDanhmucDonvi("")).RequireAnyRole(RoleCapcoquanbenngoai.Capnhat);
                configuration.For<CapcoquanController>(x => x._AddNewDonvi("", 1)).RequireAnyRole(RoleCapcoquanbenngoai.Capnhat);

                //==============TÍNH CHẤT VĂN BẢN===================================
                configuration.For<TinhchatvbController>().RequireAnyRole(RoleTinhchatvanban.Truycap);

                configuration.For<TinhchatvbController>(x => x.Save(collection)).RequireAnyRole(RoleTinhchatvanban.Capnhat);
                configuration.For<TinhchatvbController>(x => x.Delete(1, "")).RequireAnyRole(RoleTinhchatvanban.Capnhat);

                //==============DANH MỤC LĨNH VỰC ===============================
                configuration.For<LinhvucController>().RequireAnyRole(RoleLinhvuc.Truycap);

                configuration.For<LinhvucController>(x => x.Save(new QLVB.DTO.Linhvuc.EditLinhvucViewModel())).RequireAnyRole(RoleLinhvuc.Capnhat);
                configuration.For<LinhvucController>(x => x.Delete(1)).RequireAnyRole(RoleLinhvuc.Capnhat);

                //=============DANH MỤC LOẠI VĂN BẢN ĐẾN============================
                configuration.For<LoaivbdenController>().RequireAnyRole(RoleLoaivanbanden.Truycap);

                configuration.For<LoaivbdenController>(x => x.SaveLoaivb(new QLVB.DTO.Loaivanban.EditLoaivanbanViewModel())).RequireAnyRole(RoleLoaivanbanden.Capnhat);
                configuration.For<LoaivbdenController>(x => x.DeleteLoaivb(new QLVB.DTO.Loaivanban.DeleteLoaivanbanViewModel())).RequireAnyRole(RoleLoaivanbanden.Capnhat);
                configuration.For<LoaivbdenController>(x => x.SaveTruongvb(collection)).RequireAnyRole(RoleLoaivanbanden.Capnhat);

                //=============DANH MỤC LOẠI VĂN BẢN Đi============================
                configuration.For<LoaivbdiController>().RequireAnyRole(RoleLoaivanbandi.Truycap);

                configuration.For<LoaivbdiController>(x => x.SaveLoaivb(new QLVB.DTO.Loaivanban.EditLoaivanbanViewModel())).RequireAnyRole(RoleLoaivanbandi.Capnhat);
                configuration.For<LoaivbdiController>(x => x.DeleteLoaivb(new QLVB.DTO.Loaivanban.DeleteLoaivanbanViewModel())).RequireAnyRole(RoleLoaivanbandi.Capnhat);
                configuration.For<LoaivbdiController>(x => x.SaveTruongvb(collection)).RequireAnyRole(RoleLoaivanbandi.Capnhat);

                //=============NƠI LƯU TRỮ============================
                configuration.For<LuutruController>().RequireAnyRole(RoleLuutru.Truycap);

                configuration.For<LuutruController>(x => x.SaveDonvi(new QLVB.DTO.Luutru.EditDonviViewModel())).RequireAnyRole(RoleLuutru.Capnhat);
                configuration.For<LuutruController>(x => x.DeleteDonvi(new QLVB.DTO.Luutru.DeleteDonviViewModel())).RequireAnyRole(RoleLuutru.Capnhat);

                //==============SỔ VĂN BẢN =====================================
                configuration.For<SovbController>().RequireAnyRole(RoleSovanban.Truycap);

                configuration.For<SovbController>(x => x.SaveKhoiphathanh(1, collection)).RequireAnyRole(RoleSovanban.Capnhat);
                configuration.For<SovbController>(x => x.SaveLoaivb(1, collection)).RequireAnyRole(RoleSovanban.Capnhat);
                configuration.For<SovbController>(x => x.SaveSovb(collection)).RequireAnyRole(RoleSovanban.Capnhat);
                configuration.For<SovbController>(x => x.Delete(1, "")).RequireAnyRole(RoleSovanban.Capnhat);

                //===============DANH MỤC QUY TRÌNH XỬ LÝ ======================
                configuration.For<QuytrinhController>().RequireAnyRole(RoleDMQuytrinhxuly.Truycap);

                // bo qua muc nay de user co the truy cap trong menu xulyhosoquytrinh/thay doi buoc xu ly
                // (thay doi can bo xu ly cua ho so tai node chua xu ly )
                configuration.For<QuytrinhController>(x => x.GetCanbothuchien(1)).DenyAnonymousAccess();

                configuration.For<QuytrinhController>(x => x.SaveLoaiquytrinh(1, "")).RequireAnyRole(RoleDMQuytrinhxuly.Capnhat);
                configuration.For<QuytrinhController>(x => x.Savequytrinh(new QLVB.DTO.Quytrinh.EditQuytrinhViewModel(), "")).RequireAnyRole(RoleDMQuytrinhxuly.Capnhat);
                configuration.For<QuytrinhController>(x => x.SaveFlowChart(1, "")).RequireAnyRole(RoleDMQuytrinhxuly.Capnhat);
                configuration.For<QuytrinhController>(x => x.DeleteFlowChart(1)).RequireAnyRole(RoleDMQuytrinhxuly.Capnhat);
                configuration.For<QuytrinhController>(x => x.SaveVersion(1)).RequireAnyRole(RoleDMQuytrinhxuly.Capnhat);
                configuration.For<QuytrinhController>(x => x._SaveThongtinXuly(new QLVB.DTO.Quytrinh.EditThongtinXulyViewModel())).RequireAnyRole(RoleDMQuytrinhxuly.Capnhat);
                configuration.For<QuytrinhController>(x => x._LoadThongtinXuly(1, "", "")).RequireAnyRole(RoleDMQuytrinhxuly.Capnhat);

                //===============DANH MỤC Ý KIẾN CHỈ ĐẠO ĐƠN VỊ ======================
                configuration.For<YKCDDonviController>().RequireAnyRole(RoleYKCDDonvi.Truycap);

                #endregion Danhmuc

                #region Congcu

                //==============THỐNG KÊ BÁO CÁO========================================
                configuration.For<BaocaoController>().RequireAnyRole(RoleBaocao.Truycap);

                //==============TÙY CHỌN CÁ NHÂN========================================
                configuration.For<AccountController>(x => x.Doimatkhau()).RequireAnyRole(RoleDoimatkhau.Truycap);
                configuration.For<AccountController>(x => x.ChangePass(collection)).RequireAnyRole(RoleDoimatkhau.Truycap);

                configuration.For<AccountController>(x => x.Uyquyen()).RequireAnyRole(RoleUyquyen.Truycap);
                configuration.For<AccountController>(x => x._SaveUyquyen("")).RequireAnyRole(RoleUyquyen.Truycap);

                configuration.For<AccountController>(x => x.Option()).RequireAnyRole(RoleTuychoncanhan.Truycap);
                configuration.For<AccountController>(x => x._SaveOption(collection)).RequireAnyRole(RoleTuychoncanhan.Truycap);

                #endregion Congcu

                #region HosoCongviec

                //==============HỒ SƠ CÔNG VIỆC ========================================
                configuration.For<HosoController>(x => x.Index(false)).RequireAnyRole(RoleHosocongviec.TruycapHoso);
                configuration.For<HosoController>(x => x.ListHoso_Read(request, "", "", 1, 1, "", "", "", "", 1)).RequireAnyRole(RoleHosocongviec.TruycapHoso);
                configuration.For<HosoController>(x => x.DeleteHoso(1)).RequireAnyRole(RoleHosocongviec.TruycapHoso);
                configuration.For<HosoController>(x => x.ThemHoso(1)).RequireAnyRole(RoleHosocongviec.TruycapHoso);

                //==============TÌNH HÌNH XỬ LÝ VB ĐẾN ========================================                
                configuration.For<TinhhinhxulyController>(x => x.Vanbanden(false, 1, "", "", 1, 1)).RequireAnyRole(RoleTinhhinhxulyVBDen.Truycap);
                configuration.For<TinhhinhxulyController>(x => x.ListVanbanDen(1, 1, 1, "", "", 1, 1)).RequireAnyRole(RoleTinhhinhxulyVBDen.Truycap);
                configuration.For<TinhhinhxulyController>(x => x._TonghopVBDen(1, "", "", 1, 1)).RequireAnyRole(RoleTinhhinhxulyVBDen.Truycap);
                configuration.For<TinhhinhxulyController>(x => x.Vanbanden_Read(request, 1, 1, 1, "", "", 1, 1)).RequireAnyRole(RoleTinhhinhxulyVBDen.Truycap);
                configuration.For<TinhhinhxulyController>(x => x.ExportVBDen(request, 1, 1, 1, "", "", 1, 1)).RequireAnyRole(RoleTinhhinhxulyVBDen.Truycap);

                //==============TÌNH HÌNH XỬ LÝ VB ĐẾN QUY TRÌNH========================================                
                configuration.For<TinhhinhxulyController>(x => x.Quytrinh(false, 1, 1, 1, "", "")).RequireAnyRole(RoleTinhhinhxulyQuytrinh.Truycap);
                configuration.For<TinhhinhxulyController>(x => x.ListQuytrinh(1, 1, 1, "", "")).RequireAnyRole(RoleTinhhinhxulyQuytrinh.Truycap);
                configuration.For<TinhhinhxulyController>(x => x._TonghopQuytrinh(1, 1, 1, "", "")).RequireAnyRole(RoleTinhhinhxulyQuytrinh.Truycap);
                configuration.For<TinhhinhxulyController>(x => x.QuytrinhVBDen_Read(request, 1, "", 1, 1, "", "")).RequireAnyRole(RoleTinhhinhxulyQuytrinh.Truycap);
                configuration.For<TinhhinhxulyController>(x => x._XemQuytrinh(1, 1, "", "")).RequireAnyRole(RoleTinhhinhxulyQuytrinh.Truycap);
                configuration.For<TinhhinhxulyController>(x => x.ExportVBDenQuytrinh(request, 1, "", 1, 1, "", "")).RequireAnyRole(RoleTinhhinhxulyQuytrinh.Truycap);

                #endregion HosoCongviec

                #region Vanbanden

                //==============VĂN BẢN ĐẾN ========================================
                configuration.For<VanbandenController>().RequireAnyRole(RoleVanbanden.Truycap);

                // duyet van ban
                configuration.For<VanbandenController>(x => x._AjaxDuyetVanban(1)).RequireAnyRole(RoleVanbanden.Duyetvb);
                configuration.For<VanbandenController>(x => x._AjaxHuyDuyetVanban(1)).RequireAnyRole(RoleVanbanden.Duyetvb);

                // update vanban
                configuration.For<VanbandenController>(x => x.Themvanban(1, 1)).RequireAnyRole(RoleVanbanden.Capnhatvb);
                configuration.For<VanbandenController>(x => x._CapnhatThoigianXulyVBDT(1)).RequireAnyRole(RoleVanbanden.Capnhatvb);
                configuration.For<VanbandenController>(x => x._UploadVBDen(1)).RequireAnyRole(RoleVanbanden.Capnhatvb);
                configuration.For<VanbandenController>(x => x.DeleteVanban(1)).RequireAnyRole(RoleVanbanden.Xoavb);

                // cap quyen xem
                // Error: configuration.For<VanbandenController>(x => x._Capquyenxem()).RequireAnyRole(RoleVanbanden.Capquyenxem);  
                configuration.For<VanbandenController>(x => x._ListUserCapquyenxem(1)).RequireAnyRole(RoleVanbanden.Capquyenxem);
                configuration.For<VanbandenController>(x => x._SaveCapquyenxem(1, collection)).RequireAnyRole(RoleVanbanden.Capquyenxem);
                configuration.For<VanbandenController>(x => x._CapquyenxemPublic(1, 1)).RequireAnyRole(RoleVanbanden.Capquyenxem);

                // phan xu ly van ban
                configuration.For<HosoController>(x => x.PhanXLVB(1)).RequireAnyRole(RoleVanbanden.PhanXLvb);

                // phan xu ly nhieu van ban
                configuration.For<HosoController>(x => x.PhanXLNhieuVB("", collection)).RequireAnyRole(RoleVanbanden.PhanXLvb);
                configuration.For<VanbandenController>(x => x.PhanXLNhieuVB()).RequireAnyRole(RoleVanbanden.PhanXLvb);
                configuration.For<VanbandenController>(x => x._ListPhanXLNhieuVB()).RequireAnyRole(RoleVanbanden.PhanXLvb);
                configuration.For<VanbandenController>(x => x._FormPhanXLNhieuVB()).RequireAnyRole(RoleVanbanden.PhanXLvb);

                // van ban den lien quan
                //-------------------------


                // phan xu ly quy trinh
                configuration.For<HosoController>(x => x.PhanQuytrinh(1)).RequireAnyRole(RoleVanbanden.PhanQuytrinh);

                // xu ly van ban
                configuration.For<HosoController>(x => x.XulyHoso(1)).RequireAnyRole(RoleVanbanden.Xulyvb);
                configuration.For<HosoController>(x => x._DanhsachNguoixuly(1)).RequireAnyRole(RoleVanbanden.Xulyvb);
                configuration.For<HosoController>(x => x._ThongtinHoso(1)).RequireAnyRole(RoleVanbanden.Xulyvb);
                configuration.For<HosoController>(x => x._ThongtinXuly(1)).RequireAnyRole(RoleVanbanden.Xulyvb);
                configuration.For<HosoController>(x => x._HosoVBDenLQ(1)).RequireAnyRole(RoleVanbanden.Xulyvb);
                configuration.For<HosoController>(x => x._HosoVBDiLQ(1)).RequireAnyRole(RoleVanbanden.Xulyvb);

                configuration.For<HosoController>(x => x._Phoihopxuly(1)).RequireAnyRole(RoleVanbanden.Xulyvb);
                configuration.For<HosoController>(x => x._SavePhoihopxuly(1, collection)).RequireAnyRole(RoleVanbanden.Xulyvb);

                configuration.For<HosoController>(x => x._AddYkien()).RequireAnyRole(RoleVanbanden.Xulyvb);
                configuration.For<HosoController>(x => x._SaveYkien(1, "")).RequireAnyRole(RoleVanbanden.Xulyvb);
                configuration.For<HosoController>(x => x._YkienXulyNhanh(1)).RequireAnyRole(RoleVanbanden.Xulyvb);

                configuration.For<HosoController>(x => x._DuthaoVB(new QLVB.DTO.Hoso.PhathanhVBViewModel())).RequireAnyRole(RoleVanbanden.Xulyvb);

                #endregion Vanbanden

                #region Vanbandi

                //==============VĂN BẢN Đi ========================================
                configuration.For<VanbandiController>().RequireAnyRole(RoleVanbandi.Truycap);

                // duyet vb
                configuration.For<VanbandiController>(x => x._AjaxDuyetVanban(1)).RequireAnyRole(RoleVanbandi.Duyetvb);
                configuration.For<VanbandiController>(x => x._AjaxHuyDuyetVanban(1)).RequireAnyRole(RoleVanbandi.Duyetvb);

                // cap nhat vb
                configuration.For<VanbandiController>(x => x.Themvanban(1)).RequireAnyRole(RoleVanbandi.Capnhatvb);
                configuration.For<VanbandiController>(x => x.DeleteVanban(1)).RequireAnyRole(RoleVanbandi.Xoavb);
                configuration.For<VanbandiController>(x => x._UploadVBDi(1)).RequireAnyRole(RoleVanbandi.Capnhatvb);

                // cap quyen xem
                configuration.For<VanbandiController>(x => x._SaveCapquyenxem(1, collection)).RequireAnyRole(RoleVanbandi.Capquyenxem);
                configuration.For<VanbandiController>(x => x._CapquyenxemPublic(1, 1)).RequireAnyRole(RoleVanbandi.Capquyenxem);
                configuration.For<VanbandiController>(x => x._ListUserCapquyenxem(1)).RequireAnyRole(RoleVanbandi.Capquyenxem);

                // email
                configuration.For<VanbandiController>(x => x.Email(1)).RequireAnyRole(RoleVanbandi.GuiEmail);
                configuration.For<VanbandiController>(x => x._SendDonvi(1, collection)).RequireAnyRole(RoleVanbandi.GuiEmail);

                // van ban di lien quan

                // edxml
                configuration.For<VanbandiController>(x => x._GetDonviEdxml(1)).RequireAnyRole(RoleVanbandi.GuiEmail);
                configuration.For<VanbandiController>(x => x._SendEdxmlChinhphu(1, collection)).RequireAnyRole(RoleVanbandi.GuiEmail);

                #endregion Vanbandi

                #region Vanbandendientu

                //==============VĂN BẢN ĐẾN ĐIỆN TỬ ========================================
                configuration.For<VanbandendientuController>().RequireAnyRole(RoleVanbandendientu.Truycap);

                configuration.For<VanbandendientuController>(x => x._NhanEmail()).RequireAnyRole(RoleVanbandendientu.Capnhat);
                configuration.For<VanbandendientuController>(x => x._AutoReceiveMail()).RequireAnyRole(RoleVanbandendientu.Capnhat);
                configuration.For<VanbandendientuController>(x => x.DeleteVanban(1)).RequireAnyRole(RoleVanbandendientu.Capnhat);

                configuration.For<VanbandendientuController>(x => x._NhanEdxml()).RequireAnyRole(RoleVanbandendientu.Capnhat);

                #endregion Vanbandendientu

                #region Vanbandidientu

                //==============VĂN BẢN ĐI ĐIỆN TỬ ========================================
                configuration.For<VanbandidientuController>().RequireAnyRole(RoleVanbandidientu.Truycap);

                configuration.For<VanbandidientuController>(x => x._AutoSendMail()).RequireAnyRole(RoleVanbandi.GuiEmail);


                #endregion Vanbandidientu

                #region YKCD

                //===============XỬ LÝ Ý KIẾN CHỈ ĐẠO ======================
                configuration.For<YKCDController>().RequireAnyRole(RoleYKCDXuly.Truycap);

                #endregion YKCD

                #region WebApi

                //configuration.For<AttachFileController>(x=>x.Post).RequireAnyRole(RoleVanbanden.Capnhatvb);

                #endregion WebApi

                #region TracuuVanban

                configuration.For<TracuuVanbandenController>().RequireAnyRole(RoleTracuuVanbanden.Truycap);

                configuration.For<TracuuVanbandiController>().RequireAnyRole(RoleTracuuVanbandi.Truycap);
                #endregion TracuuVanban

            });
            //GlobalFilters.Filters.Add(new HandleSecurityAttribute(), 0);

            return SecurityConfiguration.Current;
        }

        public static bool UserIsAuthenticated()
        {
            var currentUser = HttpContext.Current.User;

            return !string.IsNullOrEmpty(currentUser.Identity.Name);
        }
    }
}