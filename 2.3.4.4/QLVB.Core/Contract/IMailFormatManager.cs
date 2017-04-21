using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.DTO;
using QLVB.Domain.Entities;
using QLVB.DTO.Mail;
using QLVB.DTO.Vanbandientu;
using QLVB.DTO.Vanbandientu.EdXML;

namespace QLVB.Core.Contract
{
    public interface IMailFormatManager
    {
        #region Config
        /// <summary>
        /// cau hinh mail
        /// </summary>
        /// <returns></returns>
        AccountSettingViewModel GetAccountSetting();

        /// <summary>
        /// ten don vi gui mail (dll donvi)
        /// </summary>
        /// <returns></returns>
        string GetTendonvi();

        /// <summary>
        /// ma dinh danh cua don vi 
        /// </summary>
        /// <returns></returns>
        string GetMaDinhdanh();

        /// <summary>
        /// thong tin van ban gui mail
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        ThongtinVanbanViewModel GetThongtinVanban(int idvanban);

        List<string> GetFileAttach(int idvanban, int intloai);

        #endregion Config

        #region WriteFile

        /// <summary>
        /// tao file ebxml theo quy dinh cua BTTTT
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns>file path</returns>
        string CreateEdXML(int idvanban);

        #endregion WriteFile

        #region QLVB1

        /// <summary>
        /// lay gia tri truong can tim
        /// </summary>
        /// <param name="strmessage"></param>
        /// <param name="strtruong"></param>
        /// <returns></returns>
        string _GetTruong(string strmessage, string strtruong);

        /// <summary>
        /// giai ma va luu vao database
        /// </summary>
        /// <param name="strmessage"></param>
        /// <returns>idvanbandenmail</returns>
        int DecodeQLVB1(string strmessage, bool isAttach);

        /// <summary>
        /// thong tin bao cao theo doi cua van ban dien tu
        /// </summary>
        /// <param name="strmessage"></param>
        /// <returns></returns>
        int DecodeBaocaoQLVB1(string strmessage);

        /// <summary>
        /// lay thong tin yeu cau bao cao phan hoi da nhan vbdt
        /// </summary>
        /// <param name="strmessage"></param>
        /// <returns></returns>
        TTTHBaocaoViewModel GetTTTHBaocao(string strmessage);

        /// <summary>
        /// lay duong dan de luu file dinh kem
        /// </summary>
        /// <param name="idmail"></param>
        /// <param name="strmota"></param>
        /// <returns>physicalPath de luu file</returns>
        string SaveAttachment(int idmail, string strmota, string folderPath);

        /// <summary>
        /// insert vao table attachmail
        /// </summary>
        /// <param name="idmail"></param>
        /// <param name="strtenfile"></param>
        /// <param name="strmota"></param>
        /// <param name="intloai"></param>
        /// <returns>id attachmail</returns>
        int InsertAttachment(int idmail, string strtenfile, string strmota, int intloai);

        /// <summary>
        /// save vao table mailinbox
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="strheader"></param>
        /// <param name="strcontent"></param>
        /// <param name="strfrom"></param>
        /// <param name="intloai"></param>
        /// <returns></returns>
        int SaveMailInbox(string subject, string strheader, string strcontent, string strfrom, int intloai);

        /// <summary>
        /// ma hoa theo qlvb1 "TTTHmail"
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="intloaivanban"></param>
        /// <returns></returns>
        string EncodeQLVB1(int idvanban, int intloaivanban);

        string EncodeQLVB1(string strmessage);

        /// <summary>
        /// ma hoa bao cao them qlvb1 [TTTHBCmail]
        /// </summary>
        /// <param name="strFrom"></param>
        /// <param name="idvanban"></param>
        /// <param name="iddonvi"></param>
        /// <returns></returns>
        string EncodeBaocaoQLVB1(string strFrom, int idvanban, int iddonvi);



        /// <summary>
        /// lay duong dan file de attach vao mail
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="intloai"></param>
        /// <returns></returns>
        List<ListFileToAttach> GetFileVanbanToAttach(int idvanban, int intloai);

        /// <summary>
        /// luu nhung van ban da gui cho don vi
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="iddonvi"></param>
        /// <param name="intloai"></param>
        /// <returns>intid</returns>
        int SaveGuiVanban(int idvanban, int iddonvi, int intloai);

        /// <summary>
        /// save vao table MailOubox
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="strcontent"></param>
        /// <param name="strTo"></param>
        /// <param name="intloai"></param>
        /// <returns></returns>
        int SaveMailOutbox(string subject, string strcontent, string strTo, int intloai);

        /// <summary>
        /// cap nhat van ban da gui dien tu
        /// </summary>
        /// <param name="idvanban"></param>
        /// <returns></returns>
        int UpdateVBDT(int idvanban, int intloai);

        #endregion QLVB1

        #region 512

        edXMLMessage CreateEdXMLMessage_512(int idvanban, List<int> listiddonvi);

        string edXMLMessageToString_512(edXMLMessage msg);

        edXMLMessage StringToEdXMLMessage_512(string xml);

        string edXMLDocumentToString_512(int idvanban, int intloai);

        #endregion 512

        #region 2803

        edXMLMessage CreateEdXMLMessage_2803(int idvanban, List<int> listiddonvi, int intloaivanban);

        string edXMLMessageToString_2803(edXMLMessage msg);

        edXMLMessage StringToEdXMLMessage_2803(string xml);

        edXMLDocument CreateEdXMLDocument_2803(int idvanban, int intloaivanban);

        string edXMLDocumentToString_2803(edXMLDocument doc);

        edXMLDocument StringToEdXMLDocument_2803(string xml);

        /// <summary>
        /// luu edxml thanh vbdt
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="IsAttach"></param>
        /// <returns>idvanban</returns>
        int SaveEdXML(string xml, bool IsAttach);

        /// <summary>
        /// lay don vi hoi bao nhan vbdt
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        QLVB.DTO.Vanbandientu.EdXMLBC.edXMLBaocao GetBTTTT_2803_Baocao(string xml);

        string edXMLBaocaoToString(QLVB.DTO.Vanbandientu.EdXMLBC.edXMLBaocao edBaocao);

        int UpdateEdXMLBaocao(string xml);

        #endregion 2803

        #region MailInbox

        #endregion MailInbox

        #region AutoSend

        /// <summary>
        /// luu cac don vi gui vbdt de tu dong gui 
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="listSendDonvi"></param>
        /// <returns></returns>
        ResultFunction SaveAutoSendMail(int idvanban, List<ListSendDonviViewModel> listSendDonvi, int intloaivanban);
        /// <summary>
        /// lay id van ban tu dong gui
        /// </summary>
        /// <returns></returns>
        GuiVanban GetIdVanbanAutoSend();

        List<ListSendDonviViewModel> GetListSendDonviAutoSend(GuiVanban vanban);

        /// <summary>
        /// cap nhat trang thai da gui vb khi thuc hien tu dong gui vb
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="iddonvi"></param>
        /// <param name="intloai"></param>
        /// <returns></returns>
        int UpdateGuiVanban(int idvanban, int iddonvi, int intloaivanban);

        /// <summary>
        /// lay noi dung van ban gui binh thuong
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="intloai"></param>
        /// <returns></returns>
        string GetNoidungVanbanSendNormal(int idvanban, int intloai);

        string GetTrichyeuVanbanSendNormal(int idvanban, int intloai);

        #endregion AutoSend
    }
}
