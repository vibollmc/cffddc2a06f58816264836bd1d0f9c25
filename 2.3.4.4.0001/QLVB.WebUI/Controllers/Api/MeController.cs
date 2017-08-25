using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using QLVB.DAL;

namespace QLVB.WebUI.Controllers.Api
{
    [Authorize]
    public class MeController : ApiController
    {
        private readonly QLVBDatabase _qlvbDatabase;
        public MeController()
        {
            _qlvbDatabase = new QLVBDatabase();
        }

        [AllowAnonymous]
        public string Get()
        {

            if (!HttpContext.Current.User.Identity.IsAuthenticated) return string.Empty;

            var cb = _qlvbDatabase.Canbos.FirstOrDefault(o => o.strusername == this.User.Identity.Name);
            if (cb == null) return string.Empty;

            var results = new List<string>
            {
                cb.strusername,
                cb.strmacanbo,
                cb.strhoten,
                cb.stremail,
                cb.strdienthoai,
                string.Format("{0:dd/MM/yyyy}",cb.strngaysinh),
                cb.strImageProfile,
                string.Format("{0}", cb.intdonvi),
                string.Format("{0}", cb.intgioitinh),
                string.Format("{0}", cb.intnhomquyen)
            };

            return string.Join("$", results);
        }
	}
}