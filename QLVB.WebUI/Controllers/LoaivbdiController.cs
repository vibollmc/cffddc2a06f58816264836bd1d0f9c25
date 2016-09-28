using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using QLVB.DTO.Loaivanban;

namespace QLVB.WebUI.Controllers
{
    public class LoaivbdiController : Controller
    {
        #region Constructor

        private ILoaivanbanManager _loaivb;
        public LoaivbdiController(ILoaivanbanManager loaivb)
        {
            _loaivb = loaivb;
        }
        #endregion Constructor


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Loaivb_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetLoaivbs().ToDataSourceResult(request));
        }

        private IEnumerable<PhanloaiVanban> GetLoaivbs()
        {
            return _loaivb.GetLoaivanban((int)enumPhanloaiVanban.intloai.vanbandi);
        }

        public ActionResult _AjaxTruongvb(int id)
        {
            var loaitruongvb = _loaivb.GetLoaitruongvanban(id);
            return PartialView(loaitruongvb);
        }

        public ActionResult _AjaxEditLoaivb(int id)
        {
            EditLoaivanbanViewModel model = new EditLoaivanbanViewModel
            {
                intid = id,
                strghichu = string.Empty,
                strmavanban = string.Empty,
                strtenvanban = string.Empty,
                IsDefault = false
            };
            if (id != 0)
            {   //cap nhat
                PhanloaiVanban loaivb = _loaivb.GetIdLoaivanban(id);
                model.intid = loaivb.intid;
                model.IsDefault = loaivb.IsDefault;
                model.strghichu = loaivb.strghichu;
                model.strmavanban = loaivb.strmavanban;
                model.strtenvanban = loaivb.strtenvanban;
            }
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult SaveLoaivb(EditLoaivanbanViewModel model)
        {
            int kq = _loaivb.SaveLoaivanban(model, (int)enumPhanloaiVanban.intloai.vanbandi);
            if (kq == (int)ResultViewModels.Success)
            {
                return Json((int)ResultViewModels.Success);
            }
            else
            {
                return Json((int)ResultViewModels.Error);
            }
        }

        public ActionResult _AjaxDeleteLoaivb(int id)
        {
            var loaivb = _loaivb.GetIdLoaivanban(id);
            DeleteLoaivanbanViewModel model = new DeleteLoaivanbanViewModel
            {
                intid = loaivb.intid,
                strtenvanban = loaivb.strtenvanban
            };
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult DeleteLoaivb(DeleteLoaivanbanViewModel model)
        {
            int kq = _loaivb.DeleteLoaivanban(model);
            if (kq == (int)ResultViewModels.Success)
            {
                return Json((int)ResultViewModels.Success);
            }
            else
            {
                return Json((int)ResultViewModels.Error);
            }
        }

        [HttpPost]
        public ActionResult SaveTruongvb(FormCollection collection)
        {
            try
            {
                foreach (string p in collection)
                {
                    string col = collection[p];
                    if (col.Contains("false") || col.Contains("true"))
                    {
                        string strgiatri = collection[p].ToLower();
                        // kiem tra dung cac gia tri request

                        // cat chuoi gia tri ra bang dau ,
                        // isRequire,  isDisplay, intorder
                        // true, false, 1
                        // false, true, false, 10
                        // end collection

                        int flag = strgiatri.IndexOf(',', 0);
                        string isRequire = strgiatri.Substring(0, flag);
                        //  cat chuoi require ra khoi chuoi collection

                        strgiatri = strgiatri.Substring(flag + 1);

                        flag = strgiatri.LastIndexOf(',');
                        // lay gia tri intorder o cuoi cung
                        int intorder = Convert.ToInt32(strgiatri.Substring(flag + 1));
                        //  lay gia tri cua isdisplay
                        string isDisplay = strgiatri.Substring(0, flag);

                        if (isRequire.Contains("true"))
                        {   // truong bat buoc phai co nen chi cap nhat intorder
                            // isDisplay = true
                            //_truongvbRepo.EditOrder(Convert.ToInt32(p), intorder);
                            _loaivb.EditPhanloaiTruong(Convert.ToInt32(p), true, intorder);
                        }
                        else
                        {   // truong khong bat buoc phai co , cho phep cap nhat isDisplay va intorder
                            if (isDisplay.Contains("true"))
                            {
                                _loaivb.EditPhanloaiTruong(Convert.ToInt32(p), true, intorder);
                            }
                            else
                            {
                                _loaivb.EditPhanloaiTruong(Convert.ToInt32(p), false, intorder);
                            }
                        }
                    }
                }
                return Json((int)ResultViewModels.Success);
            }
            catch
            {
                return Json((int)ResultViewModels.Error);
            }

        }
    }
}