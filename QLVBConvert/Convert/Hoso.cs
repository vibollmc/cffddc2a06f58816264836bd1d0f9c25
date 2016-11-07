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
        readonly Logging _logger = new Logging();

        public event ProgressBarHandler ReportProgress;

        public void Doituongxuly(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tbldoituongxuly";
            const string tableD = "doituongxuly";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            var reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            const string sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

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

                colname = "intidhoso";
                var intidhoso = reader.GetIntNullCheck(colname);

                colname = "intidcanbo";
                var intidcanbo = reader.GetIntNullCheck(colname);

                colname = "intvaitro";
                var intvaitro = reader.GetIntNullCheck(colname);

                colname = "intidnguoitao";
                var intidnguoitao = reader.GetIntNullCheck(colname);

                colname = "strngaythang";
                var dtengaythang = reader.GetDateTimeNullCheck(colname);

                colname = "intvaitrocu";
                var intvaitrocu = reader.GetIntNullCheck(colname);

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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
                            + "@intid,"
                            + "@intidhoso, "
                            + "@intidcanbo, "
                            + "@intvaitro, "
                            + "@intidnguoitao, "
                            + "@dtengaythang, "
                            + "@intvaitrocu "
                            + " ); ";

                    sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    var lstParams = new List<SqlParameter>
                    {
                        new SqlParameter("@intid", intid),
                        new SqlParameter("@intidhoso", intidhoso),
                        new SqlParameter("@intidcanbo", intidcanbo),
                        new SqlParameter("@intvaitro", intvaitro),
                        new SqlParameter("@intidnguoitao", intidnguoitao),
                        new SqlParameter("@dtengaythang", dtengaythang),
                        new SqlParameter("@intvaitrocu", intvaitrocu)
                    };

                    Utils.RunQuery(sInsert, strconnectdich, lstParams.ToArray());
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

        public void Hosocongviec(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();
            SqlDataReader reader;

            var tableS = "tblhosocongviec";
            var tableD = "hosocongviec";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            var sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress(countrows, count);

                var colname = string.Empty;

                colname = "intid";
                var intid = reader.GetIntNullCheck(colname);

                colname = "intsotudong";
                var intsotudong = reader.GetIntNullCheck(colname);

                colname = "strsohieuht";
                var strsohieuht = reader.GetStringNullCheck(colname);

                colname = "intloaihoso";
                var intloaihoso = reader.GetIntNullCheck(colname);

                colname = "intmucdo";
                var intmucdo = reader.GetIntNullCheck(colname);

                colname = "intlinhvuc";
                var intlinhvuc = reader.GetIntNullCheck(colname);

                colname = "intiddonvi";
                var intiddonvi = reader.GetIntNullCheck(colname);

                colname = "strtieude";
                var strtieude = reader.GetStringNullCheck(colname);

                colname = "strngaymohoso";
                var dtengaymohoso = reader.GetDateTimeNullCheck(colname);

                colname = "strthoihanxuly";
                var dtethoihanxuly = reader.GetDateTimeNullCheck(colname);

                colname = "strnoidung";
                var strnoidung = reader.GetStringNullCheck(colname);

                colname = "bittinhtrang";
                var bittinhtrang = reader.GetBitNullCheck(colname);
                int? inttrangthai = (bittinhtrang == true) ?
                    (int)enumHosocongviec.inttrangthai.Dahoanthanh : (int)enumHosocongviec.inttrangthai.Dangxuly;

                colname = "strngayketthuc";
                var dtengayketthuc = reader.GetDateTimeNullCheck(colname);

                colname = "strketqua";
                var strketqua = reader.GetStringNullCheck(colname);

                colname = "intidnguoinhap";
                var intidnguoinhap = reader.GetIntNullCheck(colname);

                colname = "strnguoihoanthanh";
                var strnguoihoanthanh = reader.GetStringNullCheck(colname);

                colname = "intsoden";
                var intsoden = reader.GetIntNullCheck(colname);

                colname = "intkhan";
                var intkhan = reader.GetIntNullCheck(colname);

                colname = "intmat";
                var intmat = reader.GetIntNullCheck(colname);

                colname = "bitluuhoso";
                // vpub : int
                var intluuhoso = reader.GetIntNullCheck(colname);

                // cac don vi : bit


                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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
                            + "@intid,"
                            + "@intsotudong,"
                            + "@strsohieuht,"
                            + "@intloaihoso,"
                            + "@intmucdo,"
                            + "@intlinhvuc,"
                            + "@intiddonvi,"
                            + "@strtieude,"
                            + "@dtengaymohoso,"
                            + "@dtethoihanxuly,"
                            + "@strnoidung,"
                            + "@inttrangthai,"
                            + "@dtengayketthuc,"
                            + "@strketqua,"
                            + "@intidnguoinhap,"
                        //+ "@intidnguoihoanthanh,"
                            + "@intsoden,"
                            + "@intkhan,"
                            + "@intmat,"
                            + "@intluuhoso"
                            + " ); ";

                    sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    var lstParam = new List<SqlParameter>()
                    {
                        new SqlParameter("@intid", intid),
                        new SqlParameter("@intsotudong", intsotudong),
                        new SqlParameter("@strsohieuht", strsohieuht),
                        new SqlParameter("@intloaihoso", intloaihoso),
                        new SqlParameter("@intmucdo", intmucdo),
                        new SqlParameter("@intlinhvuc", intlinhvuc),
                        new SqlParameter("@intiddonvi", intiddonvi),
                        new SqlParameter("@strtieude", strtieude),
                        new SqlParameter("@dtengaymohoso", dtengaymohoso),
                        new SqlParameter("@dtethoihanxuly", dtethoihanxuly),
                        new SqlParameter("@strnoidung", strnoidung),
                        new SqlParameter("@inttrangthai", inttrangthai),
                        new SqlParameter("@dtengayketthuc", dtengayketthuc),
                        new SqlParameter("@strketqua", strketqua),
                        new SqlParameter("@intidnguoinhap", intidnguoinhap),
                        //new SqlParameter("@intidnguoihoanthanh", intidnguoihoanthanh),
                        new SqlParameter("@intsoden", intsoden),
                        new SqlParameter("@intkhan", intkhan),
                        new SqlParameter("@intmat", intmat),
                        new SqlParameter("@intluuhoso", intluuhoso)
                    };

                    Utils.RunQuery(sInsert, strconnectdich, lstParam.ToArray());
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

        public void Hosovanban(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblhosovanban";
            const string tableD = "hosovanban";

            // xoa trung ho so truoc khi convert

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            var reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            const string sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

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

                colname = "intidhoso";
                var intidhoso = reader.GetIntNullCheck(colname);

                colname = "intloai";
                var intloai = reader.GetIntNullCheck(colname);

                colname = "intidvanban";
                var intidvanban = reader.GetIntNullCheck(colname);

                colname = "inttrangthai";
                var inttrangthai = reader.GetIntNullCheck(colname);

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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
        public void Hosovanbanlienquan(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblhosovanbanlq";
            const string tableD = "hosovanban";

            // xoa trung ho so truoc khi convert

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            var reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            var sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

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

                colname = "intidhoso";
                var intidhoso = reader.GetIntNullCheck(colname);

                colname = "intloai";
                var intloai = reader.GetIntNullCheck(colname);

                colname = "intidvanban";
                var intidvanban = reader.GetIntNullCheck(colname);

                colname = "inttrangthai";
                var inttrangthai = reader.GetIntNullCheck(colname);

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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

        public void Hosoykienxuly(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblhosoykienxuly";
            const string tableD = "hosoykienxuly";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            var reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            var sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

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

                colname = "intiddoituongxuly";
                var intiddoituongxuly = reader.GetIntNullCheck(colname);

                colname = "strthoigian";
                var dtethoigian = reader.GetDateTimeNullCheck(colname);

                colname = "strykien";
                var strykien = reader.GetStringNullCheck(colname);

                colname = "bittrangthai";
                var bittrangthai = reader.GetBitNullCheck(colname);
                int? inttrangthai = (bittrangthai == true) ?
                    (int)enumHosoykienxuly.inttrangthai.Dachoykien : (int)enumHosoykienxuly.inttrangthai.DangchoYkien;

                colname = "intidnguoilap";
                var intidnguoilap = reader.GetIntNullCheck(colname);

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "intiddoituongxuly, "
                            + "strthoigian, "
                            + "strykien, "
                            + "inttrangthai, "
                            + "intidnguoilap "
                            + " ) ";

                    sInsert += "values("
                            + "@intid,"
                            + "@intiddoituongxuly, "
                            + "@dtethoigian, "
                            + "@strykien, "
                            + "@inttrangthai, "
                            + "@intidnguoilap"
                            + " ); ";

                    sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    var lstParams = new List<SqlParameter>()
                    {
                        new SqlParameter("@intid", intid),
                        new SqlParameter("@intiddoituongxuly", intiddoituongxuly),
                        new SqlParameter("@dtethoigian", dtethoigian),
                        new SqlParameter("@strykien", strykien),
                        new SqlParameter("@inttrangthai", inttrangthai),
                        new SqlParameter("@intidnguoilap", intidnguoilap)
                    };


                    Utils.RunQuery(sInsert, strconnectdich, lstParams.ToArray());
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

        public void Phieutrinh(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblphieutrinh";
            const string tableD = "phieutrinh";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            var reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            const string sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

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

                colname = "strsohieuht";
                var strsohieuht = reader.GetStringNullCheck(colname);

                colname = "intidhoso";
                var intidhoso = reader.GetIntNullCheck(colname);

                colname = "strnguoinhan";
                var strnguoinhan = reader.GetStringNullCheck(colname);

                colname = "intiddoituongxuly";
                var intiddoituongxuly = reader.GetIntNullCheck(colname);

                colname = "strykienxuly";
                var strykienxuly = reader.GetStringNullCheck(colname);

                colname = "strthoigian";
                var dtethoigian = reader.GetDateTimeNullCheck(colname);

                colname = "inttrangthaidtxl";
                var inttrangthaidtxl = reader.GetIntNullCheck(colname);

                colname = "intidlanhdao";
                var intidlanhdao = reader.GetIntNullCheck(colname);

                colname = "strykienlanhdao";
                var strykienlanhdao = reader.GetStringNullCheck(colname);

                colname = "strthoigianduanx";
                var dtethoigianduanx = reader.GetDateTimeNullCheck(colname);

                colname = "inttrangthaildnx";
                var inttrangthaildnx = reader.GetIntNullCheck(colname);

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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
                            + "@intid,"
                            + "@intidhoso, "
                            + "@intiddoituongxuly, "
                            + "@strykienxuly, "
                            + "@dtethoigian, "
                            + "@inttrangthaidtxl, "

                            + "@intidlanhdao, "
                            + "@strykienlanhdao, "
                            + "@dtethoigianduanx, "
                            + "@inttrangthaildnx "

                            + " ); ";

                    sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    var lstParams = new List<SqlParameter>()
                    {
                        new SqlParameter("@intid", intid),
                        new SqlParameter("@intidhoso", intidhoso),
                        new SqlParameter("@intiddoituongxuly", intiddoituongxuly),
                        new SqlParameter("@strykienxuly", strykienxuly),
                        new SqlParameter("@dtethoigian", dtethoigian),
                        new SqlParameter("@inttrangthaidtxl", inttrangthaidtxl),

                        new SqlParameter("@intidlanhdao", intidlanhdao),
                        new SqlParameter("@strykienlanhdao", strykienlanhdao),
                        new SqlParameter("@dtethoigianduanx", dtethoigianduanx),
                        new SqlParameter("@inttrangthaildnx", inttrangthaildnx)
                    };


                    Utils.RunQuery(sInsert, strconnectdich, lstParams.ToArray());
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
