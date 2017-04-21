using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.DTO.Truclienthongtinh;
using QLVB.Core.WebServiceTruclienthongTinh;

namespace QLVB.Core.Contract
{
    public interface ITrucLienthongTinhManager
    {
        NSSGatewayServiceSoapService ConnectGateway();
        List<OrganizationVM> GetAllOrganization();


    }
}
