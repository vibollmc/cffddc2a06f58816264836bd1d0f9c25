using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using QLVB.DTO.Chat;
using QLVB.DAL;
using Microsoft.AspNet.SignalR.Hubs;
using QLVB.Common.Sessions;
using QLVB.Common.Date;
using QLVB.Domain.Entities;

namespace QLVB.WebUI.Hubs
{
    [Authorize]
    [HubName("chatRoomHub")]
    public class ChatRoomHub : Hub
    {

        private IChatRepository _chatRepo;

        public ChatRoomHub(IChatRepository chatRepo)
        {
            _chatRepo = chatRepo;
        }

        //http://www.codeproject.com/Articles/562023/Asp-Net-SignalR-Chat-Room

        //http://syfuhs.net/2013/03/24/real-time-user-notification-and-session-management-with-signalr-part-2/


        // co the luu vao database cac user online
        //static List<UserInfo> ConnectedUsers = new List<UserInfo>();

        static List<MessageInfo> CurrentMessage = new List<MessageInfo>();

        public void Connect()
        {
            var id = Context.ConnectionId;
            string UserAgent = Context.Request.Headers["User-Agent"];
            int iduser = QLVB.WebUI.Common.Session.UserStatus.GetOwinCookie().UserId;

            //QLVBDatabase context = new QLVBDatabase();

            //var alluserInfo = context.Connections
            //        .Where(p => p.UserId == iduser)
            //        .Join(
            //            context.Canbos.Where(p => p.inttrangthai == (int)enumcanbo.inttrangthai.IsActive),
            //            c => c.UserId,
            //            u => u.intid,
            //            (c, u) => new { c, u }
            //        )
            //        .Select(p => new UserInfo
            //        {
            //            UserID = p.c.UserId,
            //            UserName = p.u.strhoten,
            //            IsConnected = p.c.Connected,
            //            ConnectionId = p.c.ConnectionId,
            //            UserAgent = p.c.UserAgent,
            //            LastActivity = p.c.LastActivity
            //        })
            //        ;//.FirstOrDefault();

            //// current connected user 
            //UserInfo userInfo = new UserInfo();
            //userInfo.UserID = iduser;
            //userInfo.UserName = context.Canbos.FirstOrDefault(p => p.intid == iduser).strhoten;
            //userInfo.ConnectionId = id;
            //userInfo.IsConnected = true;

            //if (alluserInfo.Count() == 0)
            //{
            //    // lan dau connect
            //    _AddNewConnection(iduser, id, UserAgent);
            //}
            //else
            //{
            //    // kiem tra dang nhap tren nhieu thiet bi
            //    var _user = alluserInfo.FirstOrDefault(p => p.UserAgent == UserAgent);
            //    if (_user == null)
            //    {
            //        // khac browser
            //        _AddNewConnection(iduser, id, UserAgent);
            //    }
            //    else
            //    {
            //        // cung 1 browser
            //        // update connectionId , connected                   
            //        _UpdateConnectionState(userInfo.UserID, true, id, UserAgent);
            //    }
            //}

            var userInfo = _chatRepo.GetUserConnection(iduser, id, UserAgent);

            var ConnectedUsers = _chatRepo.GetListUserOnline(); //_GetListUserOnline();

            // send to caller
            Clients.Caller.onConnected(id, userInfo.UserID, userInfo.UserName, userInfo.UserImage, ConnectedUsers.OrderBy(p => p.UserName), CurrentMessage);

            // send to all except caller client
            Clients.AllExcept(id).onNewUserConnected(id, userInfo.UserID, userInfo.UserName, userInfo.UserImage, ConnectedUsers.OrderBy(p => p.UserName));

        }

        public void SendMessageToAll(string userName, string message)
        {
            // store last 100 messages in cache
            //AddMessageinCache(userName, message);

            // Broad cast message
            Clients.All.messageReceived(userName, message);
        }

        public void SendPrivateMessage(string toUserId, string message)
        {

            string fromUserId = Context.ConnectionId;

            var ConnectedUsers = _chatRepo.GetListUserOnline();// _GetListUserOnline();

            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
            {
                // send to 
                Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

                // send to caller user
                Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
            }

        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            //http://stackoverflow.com/questions/24878187/signalr-detecting-alive-connection-in-c-sharp-clients
            //http://forums.asp.net/t/1998802.aspx?SignalR+on+mobile+browser+doesn+t+disconnect+on+close
            if (stopCalled)
            {
                // We know that Stop() was called on the client,
                // and the connection shut down gracefully.
            }
            else
            {
                // This server hasn't heard from the client in the last ~35 seconds.
                // If SignalR is behind a load balancer with scaleout configured, 
                // the client may still be connected to another SignalR server.

            }

            var id = Context.ConnectionId;
            string UserAgent = Context.Request.Headers["User-Agent"];

            var ConnectedUsers = _chatRepo.GetListUserOnline();// _GetListUserOnline();
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == id);
            if (item != null)
            {
                _chatRepo.UpdateConnectionState(item.UserID, false, id, UserAgent);

                Clients.All.onUserDisconnected(id, item.UserID, item.UserName, ConnectedUsers.Count() - 1);
            }
            else
            {
                // khong co trong list useronline
                string UserName = string.Empty;
                int UserID = 0;
                Clients.All.onUserDisconnected(id, UserID, UserName, ConnectedUsers.Count() - 1);
            }

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// HIEN KHONG SU DUNG. CHUYEN SANG DUNG INTERFACE
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="message"></param>

        #region private Messages

        private void AddMessageinCache(string userName, string message)
        {
            CurrentMessage.Add(new MessageInfo
            {
                UserName = userName,
                Message = message,
                MsgDate = DateServices.FormatDateTimeVN(DateTime.Now)
            });

            if (CurrentMessage.Count > 100)
                CurrentMessage.RemoveAt(0);
        }

        private IEnumerable<UserInfo> _GetListUserOnline()
        {
            QLVBDatabase context = new QLVBDatabase();
            return context.Connections.Where(p => p.Connected == true)
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
                        UserName = p.u.strhoten
                    }).ToList();
        }

        /// <summary>
        /// update connectionid , isconnected
        /// theo userid va useragent
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="IsConnected"></param>
        /// <param name="id"></param>
        /// <param name="UserAgent"></param>
        private void _UpdateConnectionState(int UserId, bool IsConnected, string id, string UserAgent)
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

        private void _AddNewConnection(int iduser, string id, string UserAgent)
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
        #endregion

    }
}