using System;
namespace QLVB.WebUI.Hubs
{
    public interface IChatRepository
    {
        void AddNewConnection(int iduser, string id, string UserAgent);
        System.Collections.Generic.IEnumerable<QLVB.DTO.Chat.UserInfo> GetListUserOnline();
        QLVB.DTO.Chat.UserInfo GetUserConnection(int iduser, string idconnection, string UserAgent);
        void UpdateConnectionState(int UserId, bool IsConnected, string id, string UserAgent);
    }
}
