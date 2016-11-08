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
    public class Hoso
    {
        Logging _logger = new Logging();

        public event ProgressBarHandler ReportProgress;

        public void Doituongxuly(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tbldoituongxuly";
            string tableD = "doituongxuly";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            SqlConnection sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            int count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress(countrows, count);

                string colname = string.Empty;

                colname = "intid";
                int? intid = Utils.GetIntNullCheck(reader, colname);

                colname = "intidhoso";
                int? intidhoso = Utils.GetIntNullCheck(reader, colname);

                colname = "intidcanbo";
                int? intidcanbo = Utils.GetIntNullCheck(reader, colname);

                colname = "intvaitro";
                int? intvaitro = Utils.GetIntNullCheck(reader, colname);

                colname = "intidnguoitao";
                int? intidnguoitao = Utils.GetIntNullCheck(reader, colname);

                colname = "strngaythang";
                DateTime? dtengaythang = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "intvaitrocu";
                int? intvaitrocu = Utils.GetIntNullCheck(reader, colname);

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "intidhosocongviec, "
                            + "intidcanbo, "
                            + "intvaitro, "
                            + "intnguoitao, "
                            + "strngaytao, "
                            + "intvaitrocu "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "',"
                            + "'" + intidhoso + "', "
                            + "'" + intidcanbo + "', "
                            + "'" + intvaitro + "', "
                            + "'" + intidnguoitao + "', "
                            + "'" + dtengaythang + "', "
                            + "'" + intvaitrocu + "' "
                            + " ); ";

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

            sqlConnectionDich.Close();
            reader.Close();
            sqlConnectionnguon.Close();

        }

        public void Hosocongviec(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblhosocongviec";
            string tableD = "hosocongviec";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            SqlConnection sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            int count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress(countrows, count);

                string colname = string.Empty;

                colname = "intid";
                int? intid = Utils.GetIntNullCheck(reader, colname);

                colname = "intsotudong";
                int? intsotudong = Utils.GetIntNullCheck(reader, colname);

                colname = "strsohieuht";
                string strsohieuht = Utils.GetStringNullCheck(reader, colname);

                colname = "intloaihoso";
                int? intloaihoso = Utils.GetIntNullCheck(reader, colname);

                colname = "intmucdo";
                int? intmucdo = Utils.GetIntNullCheck(reader, colname);

                colname = "intlinhvuc";
                int? intlinhvuc = Utils.GetIntNullCheck(reader, colname);

                colname = "intiddonvi";
                int? intiddonvi = Utils.GetIntNullCheck(reader, colname);

                colname = "strtieude";
                string strtieude = Utils.GetStringNullCheck(reader, colname);

                colname = "strngaymohoso";
                DateTime? dtengaymohoso = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "strthoihanxuly";
                DateTime? dtethoihanxuly = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "strnoidung";
                string strnoidung = Utils.GetStringNullCheck(reader, colname);

                colname = "bittinhtrang";
                bool? bittinhtrang = Utils.GetBitNullCheck(reader, colname);
                int? inttrangthai = (bittinhtrang == true) ?
                    (int)enumHosocongviec.inttrangthai.Dahoanthanh : (int)enumHosocongviec.inttrangthai.Dangxuly;

                colname = "strngayketthuc";
                DateTime? dtengayketthuc = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "strketqua";
                string strketqua = Utils.GetStringNullCheck(reader, colname);

                colname = "intidnguoinhap";
                int? intidnguoinhap = Utils.GetIntNullCheck(reader, colname);

                colname = "strnguoihoanthanh";
                string strnguoihoanthanh = Utils.GetStringNullCheck(reader, colname);

                colname = "intsoden";
                int? intsoden = Utils.GetIntNullCheck(reader, colname);

                colname = "intkhan";
                int? intkhan = Utils.GetIntNullCheck(reader, colname);

                colname = "intmat";
                int? intmat = Utils.GetIntNullCheck(reader, colname);

                colname = "bitluuhoso";
                // vpub : int
                int? intluuhoso = Utils.GetIntNullCheck(reader, colname);

                // cac don vi : bit


                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "intsotudong, "
                            + "strsohieuht, "
                            + "intloai, "
                            + "intmucdo, "
                            + "intlinhvuc, "
                            + "intiddonvi, "
                            + "strtieude, "
                            + "strngaymohoso, "
                            + "strthoihanxuly, "
                            + "strnoidung, "
                            + "inttrangthai, "
                            + "strngayketthuc, "
                            + "strketqua, "
                            + "intidnguoinhap, "
                        //+ "intidnguoihoanthanh, "
                            + "intsoden, "
                            + "intkhan, "
                            + "intmat, "
                            + "intluuhoso "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "',"
                            + "'" + intsotudong + "',"
                            + "N'" + strsohieuht + "',"
                            + "'" + intloaihoso + "',"
                            + "'" + intmucdo + "',"
                            + "'" + intlinhvuc + "',"
                            + "'" + intiddonvi + "',"
                            + "N'" + strtieude + "',"
                            + "'" + dtengaymohoso + "',"
                            + "'" + dtethoihanxuly + "',"
                            + "N'" + strnoidung + "',"
                            + "'" + inttrangthai + "',"
                            + "'" + dtengayketthuc + "',"
                            + "N'" + strketqua + "',"
                            + "'" + intidnguoinhap + "',"
                        //+ "'" + intidnguoihoanthanh + "',"
                            + "'" + intsoden + "',"
                            + "'" + intkhan + "',"
                            + "'" + intmat + "',"
                            + "'" + intluuhoso + "' "
                            + " ); ";

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

            sqlConnectionDich.Close();
            reader.Close();
            sqlConnectionnguon.Close();

        }

        public void Hosovanban(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblhosovanban";
            string tableD = "hosovanban";

            // xoa trung ho so truoc khi convert

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            SqlConnection sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            int count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress(countrows, count);

                string colname = string.Empty;

                colname = "intid";
                int? intid = Utils.GetIntNullCheck(reader, colname);

                colname = "intidhoso";
                int? intidhoso = Utils.GetIntNullCheck(reader, colname);

                colname = "intloai";
                int? intloai = Utils.GetIntNullCheck(reader, colname);

                colname = "intidvanban";
                int? intidvanban = Utils.GetIntNullCheck(reader, colname);

                colname = "inttrangthai";
                int? inttrangthai = Utils.GetIntNullCheck(reader, colname);

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "intidhosocongviec, "
                            + "intloai, "
                            + "intidvanban, "
                            + "inttrangthai "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "',"
                            + "'" + intidhoso + "', "
                            + "'" + intloai + "', "
                            + "'" + intidvanban + "', "
                            + "'" + inttrangthai + "' "
                            + " ); ";

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

            sqlConnectionDich.Close();
            reader.Close();
            sqlConnectionnguon.Close();

        }

        /// <summary>
        /// chua lam
        /// </summary>
        /// <param name="strconnectnguon"></param>
        /// <param name="strconnectdich"></param>
        public void Hosovanbanlienquan(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblhosovanbanlq";
            string tableD = "hosovanban";

            // xoa trung ho so truoc khi convert

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            SqlConnection sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            int count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress(countrows, count);

                string colname = string.Empty;

                colname = "intid";
                int? intid = Utils.GetIntNullCheck(reader, colname);

                colname = "intidhoso";
                int? intidhoso = Utils.GetIntNullCheck(reader, colname);

                colname = "intloai";
                int? intloai = Utils.GetIntNullCheck(reader, colname);

                colname = "intidvanban";
                int? intidvanban = Utils.GetIntNullCheck(reader, colname);

                colname = "inttrangthai";
                int? inttrangthai = Utils.GetIntNullCheck(reader, colname);

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "intidhosocongviec, "
                            + "intloai, "
                            + "intidvanban, "
                            + "inttrangthai "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "',"
                            + "'" + intidhoso + "', "
                            + "'" + intloai + "', "
                            + "'" + intidvanban + "', "
                            + "'" + inttrangthai + "' "
                            + " ); ";

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

            sqlConnectionDich.Close();
            reader.Close();
            sqlConnectionnguon.Close();

        }

        public void Hosoykienxuly(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblhosoykienxuly";
            string tableD = "hosoykienxuly";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            SqlConnection sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            int count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress(countrows, count);

                string colname = string.Empty;

                colname = "intid";
                int? intid = Utils.GetIntNullCheck(reader, colname);

                colname = "intiddoituongxuly";
                int? intiddoituongxuly = Utils.GetIntNullCheck(reader, colname);

                colname = "strthoigian";
                DateTime? dtethoigian = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "strykien";
                string strykien = Utils.GetStringNullCheck(reader, colname);

                colname = "bittrangthai";
                bool? bittrangthai = Utils.GetBitNullCheck(reader, colname);
                int? inttrangthai = (bittrangthai == true) ?
                    (int)enumHosoykienxuly.inttrangthai.Dachoykien : (int)enumHosoykienxuly.inttrangthai.DangchoYkien;

                colname = "intidnguoilap";
                int? intidnguoilap = Utils.GetIntNullCheck(reader, colname);

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "intiddoituongxuly, "
                            + "strthoigian, "
                            + "strykien, "
                            + "inttrangthai, "
                            + "intidnguoilap "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "',"
                            + "'" + intiddoituongxuly + "', "
                            + "'" + dtethoigian + "', "
                            + "N'" + strykien + "', "
                            + "'" + inttrangthai + "', "
                            + "'" + intidnguoilap + "' "
                            + " ); ";

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

            sqlConnectionDich.Close();
            reader.Close();
            sqlConnectionnguon.Close();

        }

        public void Phieutrinh(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblphieutrinh";
            string tableD = "phieutrinh";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            SqlConnection sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            int count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress(countrows, count);

                string colname = string.Empty;

                colname = "intid";
                int? intid = Utils.GetIntNullCheck(reader, colname);

                colname = "strsohieuht";
                string strsohieuht = Utils.GetStringNullCheck(reader, colname);

                colname = "intidhoso";
                int? intidhoso = Utils.GetIntNullCheck(reader, colname);

                colname = "strnguoinhan";
                string strnguoinhan = Utils.GetStringNullCheck(reader, colname);

                colname = "intiddoituongxuly";
                int? intiddoituongxuly = Utils.GetIntNullCheck(reader, colname);

                colname = "strykienxuly";
                string strykienxuly = Utils.GetStringNullCheck(reader, colname);

                colname = "strthoigian";
                DateTime? dtethoigian = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "inttrangthaidtxl";
                int? inttrangthaidtxl = Utils.GetIntNullCheck(reader, colname);

                colname = "intidlanhdao";
                int? intidlanhdao = Utils.GetIntNullCheck(reader, colname);

                colname = "strykienlanhdao";
                string strykienlanhdao = Utils.GetStringNullCheck(reader, colname);

                colname = "strthoigianduanx";
                DateTime? dtethoigianduanx = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "inttrangthaildnx";
                int? inttrangthaildnx = Utils.GetIntNullCheck(reader, colname);

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "intidhosocongviec, "
                            + "intidcanbotrinh, "
                            + "strnoidungtrinh, "
                            + "strngaytrinh, "
                            + "inttrangthaitrinh, "

                            + "intidlanhdao, "
                            + "strykienchidao, "
                            + "strngaychidao, "
                            + "inttrangthaichidao "

                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "',"
                            + "'" + intidhoso + "', "
                            + "'" + intiddoituongxuly + "', "
                            + "N'" + strykienxuly + "', "
                            + "'" + dtethoigian + "', "
                            + "'" + inttrangthaidtxl + "', "

                            + "'" + intidlanhdao + "', "
                            + "N'" + strykienlanhdao + "', "
                            + "'" + dtethoigianduanx + "', "
                            + "'" + inttrangthaildnx + "' "

                            + " ); ";

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

            sqlConnectionDich.Close();
            reader.Close();
            sqlConnectionnguon.Close();

        }


    }
}
