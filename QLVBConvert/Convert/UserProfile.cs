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
        Logging _logger = new Logging();

        public event ProgressBarHandler ReportProgress;

        public void Canbo(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblcanbo";
            string tableD = "canbo";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

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

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "truncate table " + tableD;

            sqlTruncate = "delete from doituongxuly;delete from canbodonvi;delete from vanbandencanbo;delete from vanbandicanbo;";
            sqlTruncate += "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);
            // FK doituongxuly, vanbandicanbo,vanbandencanbo

            // mo ket noi toi qlvb2
            SqlConnection sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            int count = 0;
            while ((reader != null) && (reader.Read()))
            {
                count++;
                ReportProgress(countrows, count);

                string colname = string.Empty;

                colname = "intid";
                int? intid = Utils.GetIntNullCheck(reader, colname);

                colname = "strmacanbo";
                string strmacanbo = Utils.GetStringNullCheck(reader, colname);

                colname = "strhoten";
                string strhoten = Utils.GetStringNullCheck(reader, colname);

                colname = "strngaysinh";
                DateTime? strngaysinh = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "intgioitinh";
                int? intgioitinh = (Utils.GetBitNullCheck(reader, colname) == true) ?
                    (int)enumcanbo.intgioitinh.Nam : (int)enumcanbo.intgioitinh.Nu;

                colname = "intchucvu";
                int? intchucvu = Utils.GetIntNullCheck(reader, colname);

                colname = "intnhomquyen";
                int? intnhomquyen = Utils.GetIntNullCheck(reader, colname);
                //if ((intnhomquyen == null) || (intnhomquyen == 0))
                //{     user bi xoa (bitloai=2) co intnhomquyen=null
                //    _logger.Error("tblcanbo: Lỗi nhóm quyền của cán bộ: " + strhoten);
                //}

                colname = "strusername";
                string strusername = Utils.GetStringNullCheck(reader, colname);

                colname = "strpassword";
                string strpassword = Utils.GetStringNullCheck(reader, colname);

                colname = "bitkivb";
                int? bitkivb = (Utils.GetBitNullCheck(reader, colname) == true) ?
                    (int)enumcanbo.intkivb.Co : (int)enumcanbo.intkivb.Khong;

                colname = "bitloai";
                int? bitloai = Utils.GetIntNullCheck(reader, colname);

                int inttrangthai = (bitloai == 0) ? (int)enumcanbo.inttrangthai.IsActive : (int)enumcanbo.inttrangthai.NotActive;

                colname = "strRight";
                string strRight = Utils.GetStringNullCheck(reader, colname);

                int intnguoixuly = (int)enumcanbo.intnguoixuly.NotActive;
                if (strRight == "1000")
                {
                    intnguoixuly = (int)enumcanbo.intnguoixuly.IsActive;
                }
                strRight = "0000"; // reset strRight

                colname = "intiddonvi";
                int? intiddonvi = Utils.GetIntNullCheck(reader, colname);

                string strkyhieu = string.Empty;

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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
                            + "'" + intid + "', "
                            + "N'" + strmacanbo + "', "
                            + "N'" + strkyhieu + "', "
                            + "N'" + strhoten + "', "
                            + "N'" + strusername + "', "
                            + "N'" + strpassword + "', "
                            + "'" + inttrangthai + "', "
                            + "'" + intgioitinh + "', "
                            + "'" + strngaysinh + "', "
                            + "'" + intchucvu + "', "
                            + "'2', " //+ "'" + intnhomquyen + "', "  default la nhom quyen chuyen vien
                            + "'" + intiddonvi + "', "
                            + "'" + bitkivb + "', "
                            + "'" + intnguoixuly + "', "
                            + "'" + strRight + "' "
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

            // set nhom quyen cua admin la quan tri he thong
            string sqlFixQuyen = "update canbo set intnhomquyen=1, strusername='admin' , inttrangthai=1, "
                + " strpassword='21232f297a57a5a743894a0e4a801fc3' where intid=1";
            Utils.RunQuery(sqlFixQuyen, strconnectdich);

            string sqlFixDate = SQLQuery.FixDateTime(tableD, "strngaysinh");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            _logger.Info(tableS + " -- Done");

            reader.Close();
            sqlConnectionDich.Close();
            sqlConnectionnguon.Close();
        }

        public void Donvitructhuoc(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tbldonvitructhuoc";
            string tableD = "donvitructhuoc";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

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

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            // mo ket noi toi qlvb2
            SqlConnection sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            int count = 0;
            while ((reader != null) && (reader.Read()))
            {
                count++;
                ReportProgress(countrows, count);

                string colname = string.Empty;

                colname = "intid";
                int? intid = Utils.GetIntNullCheck(reader, colname);

                colname = "strparentid";
                string strparentid = Utils.GetStringNullCheck(reader, colname);

                colname = "intlevel";
                int? intlevel = Utils.GetIntNullCheck(reader, colname);

                colname = "strmadonvi";
                string strmadonvi = Utils.GetStringNullCheck(reader, colname);

                colname = "strtendonvi";
                string strtendonvi = Utils.GetStringNullCheck(reader, colname);

                int inttrangthai = (int)enumDiachiluutru.inttrangthai.IsActive;

                try
                {
                    if (intid == 1)
                    {   // root
                        string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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
                        string[] id = strparentid.Split(new Char[] { '_' });
                        int parentid = 0;
                        foreach (var p in id)
                        {
                            if (!string.IsNullOrEmpty(p))
                            {
                                // lay id cuoi cung
                                parentid = System.Convert.ToInt32(p);
                            }
                        }

                        string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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

        public void Nhomquyen(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblnhomquyen";
            string tableD = "nhomquyen";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

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

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            // mo ket noi toi qlvb2
            SqlConnection sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            int count = 0;
            while ((reader != null) && (reader.Read()))
            {
                count++;
                ReportProgress(countrows, count);

                string colname = string.Empty;

                colname = "intid";
                int? intid = Utils.GetIntNullCheck(reader, colname);

                colname = "strtennhom";
                string strtennhom = Utils.GetStringNullCheck(reader, colname);

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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

        public void Uyquyen(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tbluquyen";
            string tableD = "uyquyen";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

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

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            // mo ket noi toi qlvb2
            SqlConnection sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            int count = 0;
            while ((reader != null) && (reader.Read()))
            {
                count++;
                ReportProgress(countrows, count);

                string colname = string.Empty;

                colname = "intPersSend";
                int? intPersSend = Utils.GetIntNullCheck(reader, colname);

                colname = "intPersRec";
                int? intPersRec = Utils.GetIntNullCheck(reader, colname);

                try
                {
                    string sInsert = "";// " Set IDENTITY_INSERT " + tableD + " ON;";

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
