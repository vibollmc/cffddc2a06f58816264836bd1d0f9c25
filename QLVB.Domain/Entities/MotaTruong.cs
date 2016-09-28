using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLVB.Domain.Entities
{
    
    public class MotaTruong
    {
    
        public Int32 intid { get; set; }

        public Int32 intloai { get; set; }

        public string strten { get; set; }

        //--- truong bat buoc phai co trong loai van ban (*)
        public bool IsRequire { get; set; }

        public Int32? intorder { get; set; }
    }

    public class enumMotatruong
    {
        public enum intloai
        {
            // giong nhau cho 3 table
            //  phanloaivanban
            //  motatruong
            // phanloaitruong
            vanbanden = 1,
            vanbandi = 2,
            vanbanduthao = 3
        }
    }
}