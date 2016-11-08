using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using QLVB.Domain.Entities;

namespace Convert
{


    public class Vanban
    {
        Logging _logger = new Logging();

        public event ProgressBarHandler ReportProgress;

        // truong moi o vpub
        public void Vanbanden(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblcongvanden";
            string tableD = "vanbanden";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            reader = cmd.ExecuteReader();

            // them cot intvbdt de luu intid cua qlvb 1 (nhung don vi chua co - tru UBT)
            string sqlAddColumn = SQLQuery.AddColumn(tableS, "intvbdt", "int");
            Utils.RunQuery(sqlAddColumn, strconnectnguon);

            // truncate table truoc khi convert
            string sqlTruncate = "delete from vanbandencanbo; ";
            sqlTruncate += "delete from " + tableD;
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

                colname = "intsoden";
                int? intsoden = Utils.GetIntNullCheck(reader, colname);

                colname = "strkyhieu";
                string strkyhieu = Utils.GetStringNullCheck(reader, colname);

                colname = "strngayden";
                DateTime? dtengayden = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "strngayky";
                DateTime? dtengayky = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "intkhoiphathanh";
                int? intkhoiphathanh = Utils.GetIntNullCheck(reader, colname);

                colname = "strnoiphathanh";
                string strnoiphathanh = Utils.GetStringNullCheck(reader, colname);

                colname = "strnoigui";
                string strnoigui = Utils.GetStringNullCheck(reader, colname);

                colname = "strtrichyeu";
                string strtrichyeu = Utils.GetStringNullCheck(reader, colname);

                colname = "intnguoiduyet";
                int? intnguoiduyet = Utils.GetIntNullCheck(reader, colname);

                colname = "strnoinhan";
                string strnoinhan = Utils.GetStringNullCheck(reader, colname);

                colname = "strtomtatnoidung";
                string strtomtatnoidung = Utils.GetStringNullCheck(reader, colname);

                colname = "strnguoiky";
                string strnguoiky = Utils.GetStringNullCheck(reader, colname);

                colname = "strtukhoa";
                string strtukhoa = Utils.GetStringNullCheck(reader, colname);

                colname = "intkhan";
                int? intkhan = Utils.GetIntNullCheck(reader, colname);

                colname = "intmat";
                int? intmat = Utils.GetIntNullCheck(reader, colname);

                colname = "intiddiachiluutru";
                int? intiddiachiluutru = Utils.GetIntNullCheck(reader, colname);

                colname = "intidphanloaicongvanden";
                int? intidphanloaicongvanden = Utils.GetIntNullCheck(reader, colname);

                colname = "bittrangthaiduyet";
                int? bittrangthaiduyet = Utils.GetIntNullCheck(reader, colname);

                colname = "intiddonvinhap";
                int? intiddonvinhap = Utils.GetIntNullCheck(reader, colname);

                colname = "strnoidung";
                string strnoidung = Utils.GetStringNullCheck(reader, colname);

                colname = "intidnguoitao";
                int? intidnguoitao = Utils.GetIntNullCheck(reader, colname);

                colname = "strngaytao";
                DateTime? dtengaytao = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "intidnguoisua";
                int? intidnguoisua = Utils.GetIntNullCheck(reader, colname);

                colname = "strngaysua";
                DateTime? dtengaysua = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "intlinhvuc";
                int? intlinhvuc = Utils.GetIntNullCheck(reader, colname);

                colname = "intmucquantrong";
                int? intmucquantrong = Utils.GetIntNullCheck(reader, colname);

                colname = "intidsovanban";
                int? intidsovanban = Utils.GetIntNullCheck(reader, colname);

                colname = "bitquyphamphapluat";
                bool? bitquyphamphapluat = Utils.GetBitNullCheck(reader, colname);
                int intquyphamphapluat = (bitquyphamphapluat == true) ?
                        (int)enumVanbanden.intquyphamphapluat.Co : (int)enumVanbanden.intquyphamphapluat.Khong;

                colname = "intidldvp";
                int? intidldvp = Utils.GetIntNullCheck(reader, colname);

                colname = "bitguivbdt";
                int? intguivbdt = Utils.GetIntNullCheck(reader, colname);

                colname = "strhanxuly";
                DateTime? dtehanxuly = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "strtraloicongvanso";
                string strtraloicongvanso = Utils.GetStringNullCheck(reader, colname);

                colname = "intvbdt"; // truong moi o vpub
                int? intvbdt = Utils.GetIntNullCheck_NotLog(reader, colname);
                //(int)enumVanbanden.intvbdt.VBGiay;

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "intsoden, "
                            + "strkyhieu,"
                            + "strngayden, "
                            + "strngayky, "
                            + "intidkhoiphathanh, "
                            + "strnoiphathanh, "
                            + "strnoigui, "
                            + "strtrichyeu, "
                            + "intidnguoiduyet, "
                            + "intidsovanban, "
                            + "intidphanloaivanbanden, "
                            + "strnoinhan, "
                            + "strnguoiky, "
                            + "strtomtatnoidung, "
                            + "intidkhan, "
                            + "strtukhoa, "
                            + "intidmat, "
                            + "intiddiachiluutru, "
                            + "inttrangthai, "
                            + "intiddonvinhap, "
                            + "strnoidung, "
                            + "intidnguoitao, "
                            + "strngaytao, "
                            + "intidnguoisua, "
                            + "strngaysua, "
                            + "intidlinhvuc, "
                            + "intmucquantrong, "
                            + "intquyphamphapluat, "
                            + "intidldvp, "
                            + "bitguivbdt, "
                            + "strhanxuly, "
                            + "strtraloivanbanso, "
                            + "intdangvb " // intvbdt truong moi, chi co tai vpub 
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "',"
                            + "N'" + intsoden + "',"
                            + "N'" + strkyhieu + "',"
                            + "'" + dtengayden + "', "
                            + "'" + dtengayky + "', "
                            + "'" + intkhoiphathanh + "', "
                            + "N'" + strnoiphathanh + "', "
                            + "N'" + strnoigui + "', "
                            + "N'" + strtrichyeu + "', "
                            + "'" + intnguoiduyet + "', "
                            + "'" + intidsovanban + "', "
                            + "'" + intidphanloaicongvanden + "', "
                            + "N'" + strnoinhan + "', "
                            + "N'" + strnguoiky + "', "
                            + "N'" + strtomtatnoidung + "', "
                            + "'" + intkhan + "', "
                            + "N'" + strtukhoa + "', "
                            + "'" + intmat + "', "
                            + "'" + intiddiachiluutru + "', "
                            + "'" + bittrangthaiduyet + "', "
                            + "'" + intiddonvinhap + "', "
                            + "N'" + strnoidung + "', "
                            + "'" + intidnguoitao + "', "
                            + "'" + dtengaytao + "', "
                            + "'" + intidnguoisua + "', "
                            + "'" + dtengaysua + "', "
                            + "'" + intlinhvuc + "', "
                            + "'" + intmucquantrong + "', "
                            + "'" + intquyphamphapluat + "', "
                            + "'" + intidldvp + "', "
                            + "'" + intguivbdt + "', "
                            + "'" + dtehanxuly + "', "
                            + "N'" + strtraloicongvanso + "', "
                            + "'" + intvbdt + "' " // intvbdt truong moi, chi co tai vpub 
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
            string sqlFixDate = SQLQuery.FixDateTime(tableD, "strhanxuly");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            sqlFixDate = SQLQuery.FixDateTime(tableD, "strngayky");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            sqlFixDate = SQLQuery.FixDateTime(tableD, "strngaytao");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            sqlFixDate = SQLQuery.FixDateTime(tableD, "strngaysua");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            _logger.Info(tableS + " -- Done");

            sqlConnectionDich.Close();
            reader.Close();
            sqlConnectionnguon.Close();

        }

        public void VanbandenCanbo(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblcongvandencanbo";
            string tableD = "vanbandencanbo";

            // chuan hoa truoc khi convert
            string sDelete = "delete from " + tableS + " where intidvanbanden=0;";
            sDelete += "delete from " + tableS + " where intidcanbo=0;";
            Utils.RunQuery(sDelete, strconnectnguon);

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intidvanbanden";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "truncate table " + tableD;
            //Utils.RunQuery(sqlTruncate, strconnectdich);

            SqlConnection sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            int count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress(countrows, count);

                string colname = string.Empty;

                colname = "intidcanbo";
                int? intidcanbo = Utils.GetIntNullCheck(reader, colname);

                colname = "intidvanbanden";
                int? intidvanbanden = Utils.GetIntNullCheck(reader, colname);

                try
                {
                    string sInsert = "";//" Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intidcanbo, "
                            + "intidvanban "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intidcanbo + "',"
                            + "'" + intidvanbanden + "' "
                            + " ); ";

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

            sqlConnectionDich.Close();
            reader.Close();
            sqlConnectionnguon.Close();

        }

        public void Vanbandi(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblcongvanphathanh";
            string tableD = "vanbandi";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "delete from vanbandicanbo; ";
            sqlTruncate += "delete from " + tableD;
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

                colname = "intso";
                int? intso = Utils.GetIntNullCheck(reader, colname);

                colname = "strkyhieu";
                string strkyhieu = Utils.GetStringNullCheck(reader, colname);

                colname = "strngayky";
                DateTime? dtengayky = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "strdonvisoan";
                string strdonvisoan = Utils.GetStringNullCheck(reader, colname);

                colname = "strtrichyeu";
                string strtrichyeu = Utils.GetStringNullCheck(reader, colname);

                colname = "strnoinhan";
                string strnoinhan = Utils.GetStringNullCheck(reader, colname);

                colname = "strnguoiky";
                string strnguoiky = Utils.GetStringNullCheck(reader, colname);

                colname = "strnguoisoan";
                string strnguoisoan = Utils.GetStringNullCheck(reader, colname);

                colname = "strnguoiduyet";
                string strnguoiduyet = Utils.GetStringNullCheck(reader, colname);

                colname = "intnguoiduyet";
                int? intnguoiduyet = Utils.GetIntNullCheck(reader, colname);

                colname = "strtomtat";
                string strtomtat = Utils.GetStringNullCheck(reader, colname);

                colname = "intkhan";
                int? intkhan = Utils.GetIntNullCheck(reader, colname);

                colname = "intmat";
                int? intmat = Utils.GetIntNullCheck(reader, colname);

                colname = "intsoban";
                int? intsoban = Utils.GetIntNullCheck(reader, colname);

                colname = "intsoto";
                int? intsoto = Utils.GetIntNullCheck(reader, colname);

                colname = "strtukhoa";
                string strtukhoa = Utils.GetStringNullCheck(reader, colname);

                colname = "intiddiachiluutru";
                int? intiddiachiluutru = Utils.GetIntNullCheck(reader, colname);

                colname = "intidphanloaicongvanphathanh";
                int? intidphanloaicongvanphathanh = Utils.GetIntNullCheck(reader, colname);

                colname = "bittrangthaiduyet";
                int? bittrangthaiduyet = Utils.GetIntNullCheck(reader, colname);

                colname = "intiddonvinhap";
                int? intiddonvinhap = Utils.GetIntNullCheck(reader, colname);

                colname = "strnoidung";
                string strnoidung = Utils.GetStringNullCheck(reader, colname);

                colname = "intidnguoitao";
                int? intidnguoitao = Utils.GetIntNullCheck(reader, colname);

                colname = "strngaytao";
                DateTime? dtengaytao = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "intidnguoisua";
                int? intidnguoisua = Utils.GetIntNullCheck(reader, colname);

                colname = "strngaysua";
                DateTime? dtengaysua = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "intlinhvuc";
                int? intlinhvuc = Utils.GetIntNullCheck(reader, colname);

                colname = "intmucquantrong";
                int? intmucquantrong = Utils.GetIntNullCheck(reader, colname);

                colname = "bitguivbdt";
                int? intguivbdt = (Utils.GetBitNullCheck(reader, colname) == true) ?
                    (int)enumVanbandi.intguivbdt.Dagui : (int)enumVanbandi.intguivbdt.Chuagui;

                colname = "intidsovanban";
                int? intidsovanban = Utils.GetIntNullCheck(reader, colname);

                colname = "bitquyphamphapluat";
                bool? bitquyphamphapluat = Utils.GetBitNullCheck(reader, colname);
                int intquyphamphapluat = (bitquyphamphapluat == true) ?
                        (int)enumVanbandi.intquyphamphapluat.Co : (int)enumVanbandi.intquyphamphapluat.Khong;

                colname = "strmorong";
                string strmorong = Utils.GetStringNullCheck(reader, colname);

                colname = "strhanxuly";
                DateTime? dtehanxuly = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "intsosao";
                int? intsosao = Utils.GetIntNullCheck(reader, colname);

                colname = "strngaysao";
                DateTime? dtengaysao = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "strtraloicongvanso";
                string strtraloicongvanso = Utils.GetStringNullCheck(reader, colname);

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "intso, "
                            + "strkyhieu,"
                            + "strngayky, "
                            + "strdonvisoan, "
                            + "strtrichyeu, "
                            + "strnoinhan, "
                            + "strnguoiky, "
                            + "strnguoisoan, "
                            + "strnguoiduyet, "
                            + "intidnguoiduyet, "
                            + "strtomtat, "
                            + "intidkhan, "
                            + "intidmat, "
                            + "intsoban, "
                            + "intsoto, "
                            + "strtukhoa, "
                            + "intiddiachiluutru, "
                            + "intidphanloaivanbandi, "
                            + "inttrangthai, "
                            + "intiddonvinhap, "
                            + "strnoidung, "
                            + "intidnguoitao, "
                            + "strngaytao, "
                            + "intidnguoisua, "
                            + "strngaysua, "
                            + "intidlinhvuc, "
                            + "intmucquantrong, "
                            + "intguivbdt, "
                            + "intidsovanban, "
                            + "intquyphamphapluat, "
                            + "strmorong, "
                            + "strhanxuly, "
                            + "intsosao, "
                            + "strngaysao, "
                            + "strtraloivanbanso "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "',"
                            + "N'" + intso + "',"
                            + "N'" + strkyhieu + "',"
                            + "'" + dtengayky + "', "
                            + "N'" + strdonvisoan + "', "
                            + "N'" + strtrichyeu + "', "
                            + "N'" + strnoinhan + "', "
                            + "N'" + strnguoiky + "', "
                            + "N'" + strnguoisoan + "', "
                            + "N'" + strnguoiduyet + "', "
                            + "'" + intnguoiduyet + "', "
                            + "N'" + strtomtat + "', "
                            + "'" + intkhan + "', "
                            + "'" + intmat + "', "
                            + "'" + intsoban + "', "
                            + "'" + intsoto + "', "
                            + "N'" + strtukhoa + "', "
                            + "'" + intiddiachiluutru + "', "
                            + "'" + intidphanloaicongvanphathanh + "', "
                            + "'" + bittrangthaiduyet + "', "
                            + "'" + intiddonvinhap + "', "
                            + "N'" + strnoidung + "', "
                            + "'" + intidnguoitao + "', "
                            + "'" + dtengaytao + "', "
                            + "'" + intidnguoisua + "', "
                            + "'" + dtengaysua + "', "
                            + "'" + intlinhvuc + "', "
                            + "'" + intmucquantrong + "', "
                            + "'" + intguivbdt + "', "
                            + "'" + intidsovanban + "', "
                            + "'" + intquyphamphapluat + "', "
                            + "N'" + strmorong + "', "
                            + "'" + dtehanxuly + "', "
                            + "'" + intsosao + "', "
                            + "'" + dtengaysao + "', "
                            + "N'" + strtraloicongvanso + "' "
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
            string sqlFixDate = SQLQuery.FixDateTime(tableD, "strhanxuly");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            sqlFixDate = SQLQuery.FixDateTime(tableD, "strngaytao");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            sqlFixDate = SQLQuery.FixDateTime(tableD, "strngaysua");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            sqlFixDate = SQLQuery.FixDateTime(tableD, "strngaysao");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            _logger.Info(tableS + " -- Done");

            sqlConnectionDich.Close();
            reader.Close();
            sqlConnectionnguon.Close();

        }

        public void VanbandiCanbo(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblcongvanphathanhcanbo";
            string tableD = "vanbandicanbo";

            // chuan hoa truoc khi convert
            string sDelete = "delete from " + tableS + " where intidvanbanphathanh=0;";
            sDelete += "delete from " + tableS + " where intidcanbo=0;";
            Utils.RunQuery(sDelete, strconnectnguon);

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intidvanbanphathanh";
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

                colname = "intidcanbo";
                int? intidcanbo = Utils.GetIntNullCheck(reader, colname);

                colname = "intidvanbanphathanh";
                int? intidvanbandi = Utils.GetIntNullCheck(reader, colname);

                try
                {
                    string sInsert = "";//" Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intidcanbo, "
                            + "intidvanban "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intidcanbo + "',"
                            + "'" + intidvanbandi + "' "
                            + " ); ";

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

            sqlConnectionDich.Close();
            reader.Close();
            sqlConnectionnguon.Close();

        }

        public void Guicongvan(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblguicongvan";
            string tableD = "guivanban";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "truncate table " + tableD;
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

                colname = "intidvanban";
                int? intidvanban = Utils.GetIntNullCheck(reader, colname);

                colname = "intiddonvitgtdcv";
                int? intiddonvitgtdcv = Utils.GetIntNullCheck(reader, colname);

                colname = "intloaicongvan";
                int? intloaicongvan = Utils.GetIntNullCheck(reader, colname);

                colname = "inttrangthai";
                int? inttrangthai = Utils.GetIntNullCheck(reader, colname);

                colname = "strngaygui";
                DateTime? dtengaygui = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "inttinhtrangnhanvb";
                int? inttinhtrangnhanvb = Utils.GetIntNullCheck(reader, colname);

                colname = "strngaynhan";
                DateTime? dtengaynhan = Utils.GetDateTimeNullCheck(reader, colname);

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "intidvanban, "
                            + "intloaivanban, "
                            + "intiddonvi, "
                            + "inttrangthaigui, "
                            + "strngaygui, "
                            + "inttrangthainhan, "
                            + "strngaynhan "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "', "
                            + "'" + intidvanban + "', "
                            + "'" + intloaicongvan + "', "
                            + "'" + intiddonvitgtdcv + "', "
                            + "'" + inttrangthai + "', "
                            + "'" + dtengaygui + "', "
                            + "'" + inttinhtrangnhanvb + "', "
                            + "'" + dtengaynhan + "' "
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
            string sqlFixDate = SQLQuery.FixDateTime(tableD, "strngaygui");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            sqlFixDate = SQLQuery.FixDateTime(tableD, "strngaynhan");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            _logger.Info(tableS + " -- Done");

            sqlConnectionDich.Close();
            reader.Close();
            sqlConnectionnguon.Close();

        }

        public void Hoibaovanban(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblhoibaovanban";
            string tableD = "hoibaovanban";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS;
            //+ " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "truncate table " + tableD;
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

                colname = "intTransID";
                int? intTransID = Utils.GetIntNullCheck(reader, colname);

                colname = "intRecID";
                int? intRecID = Utils.GetIntNullCheck(reader, colname);

                colname = "intType";
                int? intType = Utils.GetIntNullCheck(reader, colname);

                try
                {
                    string sInsert = ""; //" Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intTransID, "
                            + "intRecID, "
                            + "intloai "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intTransID + "', "
                            + "'" + intRecID + "', "
                            + "'" + intType + "' "
                            + " ); ";

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

            sqlConnectionDich.Close();
            reader.Close();
            sqlConnectionnguon.Close();

        }

        public void Vanbandenmail(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblvanbandenmail";
            string tableD = "vanbandenmail";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid";
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

                colname = "strngayky";
                DateTime? dtengayky = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "strkyhieu";
                string strkyhieu = Utils.GetStringNullCheck(reader, colname);

                colname = "intidphanloaicongvanden";
                int? intidphanloaicongvanden = Utils.GetIntNullCheck(reader, colname);

                colname = "intkhan";
                int? intkhan = Utils.GetIntNullCheck(reader, colname);

                colname = "intmat";
                int? intmat = Utils.GetIntNullCheck(reader, colname);

                colname = "strtrichyeu";
                string strtrichyeu = Utils.GetStringNullCheck(reader, colname);

                colname = "strnguoiky";
                string strnguoiky = Utils.GetStringNullCheck(reader, colname);

                colname = "strnoigui";
                string strnoigui = Utils.GetStringNullCheck(reader, colname);

                colname = "strloaivanban";
                string strloaivanban = Utils.GetStringNullCheck(reader, colname);

                colname = "bittrangthai";
                bool? bittrangthai = Utils.GetBitNullCheck(reader, colname);
                int? inttrangthai = (bittrangthai == true) ?
                    1 : 0;

                colname = "bitattach";
                bool? bitattach = Utils.GetBitNullCheck(reader, colname);
                int? intattach = (bitattach == true) ?
                   (int)enumVanbandenmail.intattach.Co : (int)enumVanbandenmail.intattach.Khong;

                colname = "strAddressSend";
                string strAddressSend = Utils.GetStringNullCheck(reader, colname);

                colname = "strNoiguiVB";
                string strNoiguiVB = Utils.GetStringNullCheck(reader, colname);

                colname = "strngayguivb";
                DateTime? dtengayguivb = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "strngaynhanvb";
                DateTime? dtengaynhanvb = Utils.GetDateTimeNullCheck(reader, colname);

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "strngayky, "
                            + "strkyhieu,"
                            + "intidphanloaivanbanden,"
                            + "intkhan, "

                        //+ "intmat, "
                            + "intso, "

                            + "strtrichyeu, "
                            + "strnguoiky, "
                            + "strnoigui, "
                            + "strloaivanban, "
                            + "inttrangthai, "
                             + "intattach, "
                            + "strAddressSend, "
                            + "strNoiguiVB, "
                            + "strngayguivb, "
                            + "strngaynhanvb "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "',"
                            + "'" + dtengayky + "',"
                            + "N'" + strkyhieu + "',"
                            + "'" + intidphanloaicongvanden + "',"
                            + "'" + intkhan + "',"

                            + "'" + intmat + "', "

                            + "N'" + strtrichyeu + "', "
                            + "N'" + strnguoiky + "', "
                            + "N'" + strnoigui + "', "
                            + "N'" + strloaivanban + "', "
                            + "'" + inttrangthai + "', "
                            + "'" + intattach + "', "
                            + "N'" + strAddressSend + "', "
                            + "N'" + strNoiguiVB + "', "
                            + "'" + dtengayguivb + "', "
                            + "'" + dtengaynhanvb + "' "
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

            string sqlFixDate = SQLQuery.FixDateTime(tableD, "strngayky");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            sqlFixDate = SQLQuery.FixDateTime(tableD, "strngayguivb");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            sqlFixDate = SQLQuery.FixDateTime(tableD, "strngaynhanvb");
            Utils.RunQuery(sqlFixDate, strconnectdich);


            _logger.Info(tableS + " -- Done");

            sqlConnectionDich.Close();
            reader.Close();
            sqlConnectionnguon.Close();

        }

    }
}
