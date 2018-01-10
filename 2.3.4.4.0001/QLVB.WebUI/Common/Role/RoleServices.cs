using QLVB.Common.Sessions;
using QLVB.Common.Utilities;
using QLVB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLVB.WebUI.Common.Role
{
    /// <summary>
    /// Kiem tra cac quyen cua user
    /// </summary>
    public class RoleServices
    {
        /// <summary>
        /// Gets the user role for fluentsecurity.
        /// HIEN KHONG SU DUNG
        /// </summary>
        /// <returns>array user roles</returns>
        public static object[] GetUserRole()
        {
            Int32 userId = QLVB.WebUI.Common.Session.UserStatus.GetOwinCookie().UserId;
            if (userId != 0)
            {
                var userRoles = GetRole(userId);
                return userRoles;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// dung cho owin cookie
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static object[] GetUserRole(int userId)
        {
            if (userId != 0)
            {
                var userRoles = GetUserRolesInSession();
                if (string.IsNullOrEmpty(userRoles))
                {
                    return GetRole(userId);
                }
                else
                {
                    string[] role = userRoles.Split(new Char[] { ';' });
                    foreach (var p in role)
                    {
                        //if (p == strRole) flag = true;
                    }
                    object[] objectRoles = (object[])role;
                    return objectRoles.ToArray();
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Lay quyen cua user tu database
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// array user role
        /// add user role in session
        /// </returns>
        private static object[] GetRole(Int32 userId)
        {
            QLVBDatabase context = new QLVBDatabase();
            //var roles = (from p in context.QuyenNhomQuyens
            //             join m in context.Quyens on p.intidquyen equals m.intid
            //             join n in context.NhomQuyens on p.intidnhomquyen equals n.intid
            //             join u in context.CanboNhomquyens on n.intid equals u.intidcanbo
            //             where u.intidcanbo == userId
            //             select m.strquyen
            //            );
            // da bo table canbonhomquyen roi

            //var roles = context.QuyenNhomQuyens
            //            .Join(
            //                context.Canbos.Where(p => p.intid == userId),
            //                q => q.intidnhomquyen,
            //                cb => cb.intnhomquyen,
            //                (q, cb) => q
            //            )
            //            .Join(
            //                context.Quyens,
            //                g => g.intidquyen,
            //                q => q.intid,
            //                (q, g) => g
            //            )
            //            .Select(p => p.strquyen);

            if (userId == 0) { return null; }

            int? idnhomquyen = context.Canbos
                                .FirstOrDefault(p => p.intid == userId)
                                .intnhomquyen;

            //int? idnhomquyen = null;
            //var canbo = context.Canbos.FirstOrDefault(p => p.intid == userId);
            //if (canbo != null) idnhomquyen = canbo.intnhomquyen;
            var roles = context.Quyens
                        .Where(p => p.inttrangthai == 1)    // chi lay nhung quyen dang duoc active
                        .Join(
                            context.QuyenNhomQuyens.Where(p => p.intidnhomquyen == idnhomquyen),
                            quyen => quyen.intid,
                            nhom => nhom.intidquyen,
                            (quyen, nhom) => quyen
                        ).Select(p => p.strquyen);


            List<object> userRoles = new List<object>();
            string Roles = "";
            foreach (var p in roles)
            {
                if (!userRoles.Contains(p))
                {
                    userRoles.Add(p);
                    Roles = Roles + p + ";";
                }
            }
            //SessionServices.InsertObject(AppConts.SessionUserRoles, Roles);
            HttpContext.Current.Session[AppConts.SessionUserRoles] = Roles;

            return userRoles.ToArray();
        }
        /// <summary>
        /// Gets the user role in session.
        /// </summary>
        /// <returns>role in string</returns>
        private static string GetUserRolesInSession()
        {
            var roles = HttpContext.Current.Session[AppConts.SessionUserRoles];
            return roles != null ? roles.ToString() : string.Empty;
            //return _session.GetObject(AppConts.SessionUserRoles).ToString();
        }

    }
}