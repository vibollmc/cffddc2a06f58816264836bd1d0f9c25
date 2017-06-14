using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using INet;
using INet.SecurityToken;
using INet.SecurityToken.Model;

using INet.Mercury;
using INet.Mercury.Model;

using QLVB.DTO.Edxml;
using QLVB.DTO;

namespace QLVB.Core.Contract
{
    public interface IEdxmlManager
    {
        DonviEdxmlViewModel getalldonvi(int idvanban);

        string WriteEdxml(int idvanban, int intloaivanban, DonviEdxmlViewModel donvi);

        string Sender(int idvanban, int intloaivanban, DonviEdxmlViewModel donvi);

        ResultFunction Receiver();

        int ReadEdxml(string fileEdxml);

        string SendStatus(int idvanban, string status, string statusDescription, string nguoigui, string phongban);

        //string WriteStatusEdxml();
    }
}
