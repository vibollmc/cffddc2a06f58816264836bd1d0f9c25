using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using DLTD.Web.Main.Common;
using DLTD.Web.Main.Models.Enum;

namespace DLTD.Web.Main.Controllers
{
    public class BaseController : Controller
    {
        public int? UserIdLogin
        {
            get
            {
                if (!User.Identity.IsAuthenticated) return null;

                var identity = (ClaimsIdentity)User.Identity;

                var nameId = identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (nameId == null) return null;
                var userId = nameId.Value;

                return userId.ToIntExt();
            }
        }

        public NhomNguoiDung GroupUserLogin
        {
            get
            {
                if (!User.Identity.IsAuthenticated) return NhomNguoiDung.Undefined;

                var indentity = (ClaimsIdentity) User.Identity;

                var primaryGroup = indentity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.PrimaryGroupSid);
                if (primaryGroup == null) return NhomNguoiDung.Undefined;
                var group = primaryGroup.Value;

                return group.ParseEnum<NhomNguoiDung>();
            }
        }
    }
}