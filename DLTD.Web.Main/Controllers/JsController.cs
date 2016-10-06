using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using DLTD.Web.Main.Common;
using DLTD.Web.Main.DAL;
using Yahoo.Yui.Compressor;

namespace DLTD.Web.Main.Controllers
{
    [AllowAnonymous]
    public class JsController : BaseController
    {
        public async Task<ActionResult> QlvbScripts(string v)
        {
            var qlvbUri = WebConfigurationManager.AppSettings["ServerQLVBOauthPath"];
            var donVi = await DonViManagement.Go.GetDonViByKhoi(6, 7, 9);
            var khoi = await DonViManagement.Go.GetNguonChiDaoByKhoi();
            var htmlOption = new StringBuilder();
            var htmlOptionkhoi = new StringBuilder();
            foreach (var item in donVi.OrderBy(x => x.Ten))
            {
                htmlOption.AppendFormat("<option value=\'{0}\'>{1}</option>", item.Id, Server.HtmlEncode(item.Ten));
            }
            foreach (var item in khoi.OrderBy(x => x.Ten))
            {
                htmlOptionkhoi.AppendFormat("<option value=\'{0}\'>{1}</option>", item.Id, Server.HtmlEncode(item.Ten));
            }

            var js = System.IO.File.ReadAllText(Server.MapPath("~/Scripts/api/qlvbScripts.js"));

            js = js.Replace("{{DONVI}}", htmlOption.ToString())
                .Replace("{{URI}}", Request.Url.AbsoluteUri.Replace(Request.RawUrl, ""))
                .Replace("{{KHOI}}", htmlOptionkhoi.ToString())
                .Replace("{{QLVBURI}}", qlvbUri);

            htmlOption = new StringBuilder();
            var nguoiChiDao = await DangNhapManagement.Go.GetNguoiChiDao(WebConfigurationManager.AppSettings["IDDonViTrucThuocLanhDao"].ToIntExt());
            if (nguoiChiDao != null)
                foreach (var item in nguoiChiDao)
                {
                    htmlOption.AppendFormat("<option value=\'{0}\'>{1}</option>", item.Id, Server.HtmlEncode(item.Ten));
                }

            js = js.Replace("{{NGUOICHIDAO}}", htmlOption.ToString());

            htmlOption = new StringBuilder();
            var nguoiTheoDoi = await DangNhapManagement.Go.GetNguoiTheoDoi();
            if (nguoiTheoDoi != null)
                foreach (var item in nguoiTheoDoi)
                {
                    htmlOption.AppendFormat("<option value=\'{0}\'>{1}</option>", item.Id, Server.HtmlEncode(item.Ten));
                }

            js = js.Replace("{{NGUOITHEODOI}}", htmlOption.ToString());

            var jsCompressor = new JavaScriptCompressor {ObfuscateJavascript = true, Encoding = Encoding.Unicode};

            ViewBag.Js = jsCompressor.Compress(js);
            //ViewBag.Js = js;
            return View();

        }
    }
}