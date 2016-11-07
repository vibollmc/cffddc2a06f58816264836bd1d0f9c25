using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace Convert
{

    public class UserProfile
    {
        readonly Logging _logger = new Logging();

        public event ProgressBarHandler ReportProgress;

        public void Canbo(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblcanbo";
            const string tableD = "canbo";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            //cmd.CommandText = "select * from " + tableS
            //        + " left join tblcanbodonvi on tblcanbo.intid = tblcanbodonvi.intidcanbo "
            //        + " where bitloai=0 or bitloai=2 order by intid";
            cmd.CommandText = "select * from " + tableS
                    + " left join tblcanbodonvi on tblcanbo.intid = tblcanbodonvi.intidcanbo "
                    + " order by intid";

            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            try
            {
                sqlConnectionnguon.Open();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return;
            }

            var reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            var sqlTruncate = "truncate table " + tableD;

            sqlTruncate = "delete from doituongxuly;delete from canbodonvi;delete from vanbandencanbo;delete from vanbandicanbo;";
            sqlTruncate += "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);
            // FK doituongxuly, vanbandicanbo,vanbandencanbo

            // mo ket noi toi qlvb2
            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress?.Invoke(countrows, count);

                var colname = "intid";
                var intid = reader.GetIntNullCheck(colname);

                colname = "strmacanbo";
                var strmacanbo = reader.GetStringNullCheck(colname);

                colname = "strhoten";
                var strhoten = reader.GetStringNullCheck(colname);

                colname = "strngaysinh";
                var strngaysinh = reader.GetDateTimeNullCheck(colname);

                colname = "intgioitinh";
                int? intgioitinh = (reader.GetBitNullCheck(colname) == true) ?
                    (int)enumcanbo.intgioitinh.Nam : (int)enumcanbo.intgioitinh.Nu;

                colname = "intchucvu";
                var intchucvu = reader.GetIntNullCheck(colname);

                colname = "intnhomquyen";
                var intnhomquyen = reader.GetIntNullCheck(colname);
                //if ((intnhomquyen == null) || (intnhomquyen == 0))
                //{     user bi xoa (bitloai=2) co intnhomquyen=null
                //    _logger.Error("tblcanbo: Lỗi nhóm quyền của cán bộ: " + strhoten);
                //}

                colname = "strusername";
                var strusername = reader.GetStringNullCheck(colname);

                colname = "strpassword";
                var strpassword = reader.GetStringNullCheck(colname);

                colname = "bitkivb";
                int? bitkivb = (reader.GetBitNullCheck(colname) == true) ?
                    (int)enumcanbo.intkivb.Co : (int)enumcanbo.intkivb.Khong;

                colname = "bitloai";
                var bitloai = reader.GetIntNullCheck(colname);

                var inttrangthai = (bitloai == 0) ? (int)enumcanbo.inttrangthai.IsActive : (int)enumcanbo.inttrangthai.NotActive;

                colname = "strRight";
                var strRight = reader.GetStringNullCheck(colname);

                var intnguoixuly = (int)enumcanbo.intnguoixuly.NotActive;
                if (strRight == "1000")
                {
                    intnguoixuly = (int)enumcanbo.intnguoixuly.IsActive;
                }
                strRight = "0000"; // reset strRight

                colname = "intiddonvi";
                var intiddonvi = reader.GetIntNullCheck(colname);

                var strkyhieu = string.Empty;

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "strmacanbo, "
                            + "strkyhieu,"  // truong moi
                            + "strhoten, "
                            + "strusername, "
                            + "strpassword, "
                            + "inttrangthai, "
                            + "intgioitinh, "
                            + "strngaysinh, "
                            + "intchucvu, "
                            + "intnhomquyen, "
                            + "intdonvi, "  // truong moi
                            + "intkivb, "
                            + "intnguoixuly, " // truong moi
                            + "strRight "
                            + " ) ";

                    sInsert += "values("
                            + "@intid, "
                            + "@strmacanbo, "
                            + "@strkyhieu, "
                            + "@strhoten, "
                            + "@strusername, "
                            + "@strpassword, "
                            + "@inttrangthai, "
                            + "@intgioitinh, "
                            + "@strngaysinh, "
                            + "@intchucvu, "
                            + "2, " //+ "'" + intnhomquyen + "', "  default la nhom quyen chuyen vien
                            + "@intiddonvi, "
                            + "@bitkivb, "
                            + "@intnguoixuly, "
                            + "@strRight);";

                    sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";


                    var lstParams = new List<SqlParameter>
                    {
                        new SqlParameter("@intid", intid),
                        new SqlParameter("@strmacanbo", strmacanbo),
                        new SqlParameter("@strkyhieu", strkyhieu),
                        new SqlParameter("@strhoten", strhoten),
                        new SqlParameter("@strusername", strusername),
                        new SqlParameter("@strpassword", strpassword),
                        new SqlParameter("@inttrangthai", inttrangthai),
                        new SqlParameter("@intgioitinh", intgioitinh),
                        new SqlParameter("@strngaysinh", strngaysinh),
                        new SqlParameter("@intchucvu", intchucvu),
                        new SqlParameter("@intiddonvi", intiddonvi),
                        new SqlParameter("@bitkivb", bitkivb),
                        new SqlParameter("@intnguoixuly", intnguoixuly),
                        new SqlParameter("@strRight", strRight)
                    };

                    Utils.RunQuery(sInsert, strconnectdich, lstParams.ToArray());

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    return;
                }
            }

            // set nhom quyen cua admin la quan tri he thong
            var sqlFixQuyen = "update canbo set intnhomquyen=1, strusername='admin' , inttrangthai=1, "
                + " strpassword='21232f297a57a5a743894a0e4a801fc3' where intid=1";
            Utils.RunQuery(sqlFixQuyen, strconnectdich);

            var sqlFixDate = SQLQuery.FixDateTime(tableD, "strngaysinh");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            _logger.Info(tableS + " -- Done");

            reader.Close();
            sqlConnectionDich.Close();
            sqlConnectionnguon.Close();
        }

        public void Donvitructhuoc(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tbldonvitructhuoc";
            const string tableD = "donvitructhuoc";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                    + "  order by intid";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            try
            {
                sqlConnectionnguon.Open();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return;
            }

            var reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            const string sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            // mo ket noi toi qlvb2
            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress?.Invoke(countrows, count);

                var colname = "intid";
                var intid = reader.GetIntNullCheck(colname);

                colname = "strparentid";
                var strparentid = reader.GetStringNullCheck(colname);

                colname = "intlevel";
                var intlevel = reader.GetIntNullCheck(colname);

                colname = "strmadonvi";
                var strmadonvi = reader.GetStringNullCheck(colname);

                colname = "strtendonvi";
                var strtendonvi = reader.GetStringNullCheck(colname);

                const int inttrangthai = (int)enumDiachiluutru.inttrangthai.IsActive;

                try
                {
                    if (intid == 1)
                    {   // root
                        var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                        sInsert += "insert into  " + tableD
                                + "(Id, "
                                + "ParentId, "
                                + "intlevel, "
                                + "strmadonvi,"
                                + "strtendonvi,"
                                + "inttrangthai "
                                + " ) ";

                        sInsert += "values("
                                + "'" + intid + "', "
                                + "null, "
                                + "'0', "
                                + "N'" + strmadonvi + "', "
                                + "N'" + strtendonvi + "', "
                                + "'" + inttrangthai + "' "
                                + " ) ;";

                        sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                        Utils.RunQuery(sInsert, strconnectdich);
                    }
                    else
                    {   // child
                        var id = strparentid.Split(new char[] { '_' });
                        var parentid = 0;
                        foreach (var p in id)
                        {
                            if (!string.IsNullOrEmpty(p))
                            {
                                // lay id cuoi cung
                                parentid = System.Convert.ToInt32(p);
                            }
                        }

                        var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                        sInsert += "insert into  " + tableD
                                + "(Id, "
                                + "ParentId, "
                                + "intlevel, "
                                + "strmadonvi,"
                                + "strtendonvi,"
                                + "inttrangthai "
                                + " ) ";

                        sInsert += "values("
                                + "'" + intid + "', "
                                + "'" + parentid + "', "
                                + "'" + intlevel + "', "
                                + "N'" + strmadonvi + "', "
                                + "N'" + strtendonvi + "', "
                                + "'" + inttrangthai + "' "
                                + " ) ;";

                        sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                        Utils.RunQuery(sInsert, strconnectdich);
                    }


                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    return;
                }
            }
            _logger.Info(tableS + " -- Done");

            reader.Close();
            sqlConnectionDich.Close();
            sqlConnectionnguon.Close();
        }

        public void Nhomquyen(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblnhomquyen";
            const string tableD = "nhomquyen";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                    + "  order by intid";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            try
            {
                sqlConnectionnguon.Open();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return;
            }

            var reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            const string sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            // mo ket noi toi qlvb2
            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress?.Invoke(countrows, count);

                var colname = "intid";
                var intid = reader.GetIntNullCheck(colname);

                colname = "strtennhom";
                var strtennhom = reader.GetStringNullCheck(colname);

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "strtennhom, "
                            + "inttrangthai "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "', "
                            + "N'" + strtennhom + "', "
                            + "'1' "
                            + " ) ;";

                    sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    Utils.RunQuery(sInsert, strconnectdich);

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    return;
                }
            }
            _logger.Info(tableS + " -- Done");

            reader.Close();
            sqlConnectionDich.Close();
            sqlConnectionnguon.Close();
        }

        public void Uyquyen(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tbluquyen";
            const string tableD = "uyquyen";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS;
            //+ "  order by intid";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            try
            {
                sqlConnectionnguon.Open();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return;
            }

            var reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            var sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            // mo ket noi toi qlvb2
            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress?.Invoke(countrows, count);

                var colname = "intPersSend";
                var intPersSend = reader.GetIntNullCheck(colname);

                colname = "intPersRec";
                var intPersRec = reader.GetIntNullCheck(colname);

                try
                {
                    var sInsert = "";// " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intPersSend, "
                            + "intPersRec "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intPersSend + "', "
                            + "'" + intPersRec + "' "
                            + " ) ;";

                    //sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    Utils.RunQuery(sInsert, strconnectdich);

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    return;
                }
            }
            _logger.Info(tableS + " -- Done");

            reader.Close();
            sqlConnectionDich.Close();
            sqlConnectionnguon.Close();
        }

    }
}
