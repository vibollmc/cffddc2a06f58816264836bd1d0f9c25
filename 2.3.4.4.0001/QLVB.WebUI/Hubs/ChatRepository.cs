using QLVB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLVB.Domain.Entities;
using QLVB.DTO.Chat;

namespace QLVB.WebUI.Hubs
{
    public class ChatRepository : QLVB.WebUI.Hubs.IChatRepository
    {

        public UserInfo GetUserConnection(int iduser, string idconnection, string UserAgent)
        {
            QLVBDatabase context = new QLVBDatabase();

            var alluserInfo = context.Connections
                   .Where(p => p.UserId == iduser)
                   .Join(
                       context.Canbos.Where(p => p.inttrangthai == (int)enumcanbo.inttrangthai.IsActive),
                       c => c.UserId,
                       u => u.intid,
                       (c, u) => new { c, u }
                   )
                   .Select(p => new UserInfo
                   {
                       UserID = p.c.UserId,
                       UserName = p.u.strhoten,
                       UserImage = (p.u.strImageProfile == null) ? "avatar_default.png" : p.u.strImageProfile,
                       IsConnected = p.c.Connected,
                       ConnectionId = p.c.ConnectionId,
                       UserAgent = p.c.UserAgent,
                       LastActivity = p.c.LastActivity
                   });
            var canbo = context.Canbos.FirstOrDefault(p => p.intid == iduser);

            // current connected user 
            UserInfo userConnect = new UserInfo();
            userConnect.UserID = iduser;
            userConnect.UserName = canbo.strhoten;
            userConnect.UserImage = (canbo.strImageProfile == null) ? "avatar_default.png" : canbo.strImageProfile;
            userConnect.ConnectionId = idconnection;
            userConnect.IsConnected = true;

            if (alluserInfo.Count() == 0)
            {
                // lan dau connect
                AddNewConnection(iduser, idconnection, UserAgent);
            }
            else
            {
                // kiem tra dang nhap tren nhieu thiet bi
                var _user = alluserInfo.FirstOrDefault(p => p.UserAgent == UserAgent);
                if (_user == null)
                {
                    // khac browser
                    AddNewConnection(iduser, idconnection, UserAgent);
                }
                else
                {
                    // cung 1 browser
                    // update connectionId , connected                   
                    UpdateConnectionState(userConnect.UserID, true, idconnection, UserAgent);
                }
            }
            return userConnect;
        }

        public void AddNewConnection(int iduser, string id, string UserAgent)
        {
            try
            {
                QLVBDatabase context = new QLVBDatabase();

                Connection user = new Connection();
                user.UserId = iduser;
                user.Connected = true;
                user.ConnectionId = id;
                user.UserAgent = UserAgent;
                user.LastActivity = DateTime.Now;

                context.Connections.Add(user);
                context.SaveChanges();
            }
            catch
            {

            }
        }
        /// <summary>
        /// update connectionid , isconnected
        /// theo userid va useragent
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="IsConnected"></param>
        /// <param name="id"></param>
        /// <param name="UserAgent"></param>
        public void UpdateConnectionState(int UserId, bool IsConnected, string id, string UserAgent)
        {
            try
            {
                QLVBDatabase context = new QLVBDatabase();
                var user = context.Connections
                    .Where(p => p.UserAgent == UserAgent)
                    .FirstOrDefault(p => p.UserId == UserId);

                user.ConnectionId = id;
                user.Connected = IsConnected;
                user.LastActivity = DateTime.Now;
                user.UserAgent = UserAgent;

                context.SaveChanges();
            }
            catch
            {

            }
        }
        public IEnumerable<UserInfo> GetListUserOnline()
        {
            QLVBDatabase context = new QLVBDatabase();

            var listuser = context.Connections.Where(p => p.Connected == true)
                    .Join(
                        context.Canbos.Where(p => p.inttrangthai == (int)enumcanbo.inttrangthai.IsActive),
                        c => c.UserId,
                        u => u.intid,
                        (c, u) => new { c, u }
                    )
                    .Select(p => new UserInfo
                    {
                        ConnectionId = p.c.ConnectionId,
                        UserID = p.c.UserId,
                        UserName = p.u.strhoten,
                        UserImage = (p.u.strImageProfile == null) ? "avatar_default.png" : p.u.strImageProfile,
                    }).ToList();

            return listuser;
        }


    }
}