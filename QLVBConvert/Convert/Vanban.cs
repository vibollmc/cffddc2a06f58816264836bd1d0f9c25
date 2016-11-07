using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using QLVB.Domain.Entities;

namespace Convert
{


    public class Vanban
    {
        readonly Logging _logger = new Logging();

        public event ProgressBarHandler ReportProgress;

        // truong moi o vpub
        public void Vanbanden(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblcongvanden";
            const string tableD = "vanbanden";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            var reader = cmd.ExecuteReader();

            // them cot intvbdt de luu intid cua qlvb 1 (nhung don vi chua co - tru UBT)
            var sqlAddColumn = SQLQuery.AddColumn(tableS, "intvbdt", "int");
            Utils.RunQuery(sqlAddColumn, strconnectnguon);

            // truncate table truoc khi convert
            var sqlTruncate = "delete from vanbandencanbo; ";
            sqlTruncate += "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress(countrows, count);

                var colname = "intid";
                var intid = reader.GetIntNullCheck(colname);

                colname = "intsoden";
                var intsoden = reader.GetIntNullCheck(colname);

                colname = "strkyhieu";
                var strkyhieu = reader.GetStringNullCheck(colname);

                colname = "strngayden";
                var dtengayden = reader.GetDateTimeNullCheck(colname);

                colname = "strngayky";
                var dtengayky = reader.GetDateTimeNullCheck(colname);

                colname = "intkhoiphathanh";
                var intkhoiphathanh = reader.GetIntNullCheck(colname);

                colname = "strnoiphathanh";
                var strnoiphathanh = reader.GetStringNullCheck(colname);

                colname = "strnoigui";
                var strnoigui = reader.GetStringNullCheck(colname);

                colname = "strtrichyeu";
                var strtrichyeu = reader.GetStringNullCheck(colname);

                colname = "intnguoiduyet";
                var intnguoiduyet = reader.GetIntNullCheck(colname);

                colname = "strnoinhan";
                var strnoinhan = reader.GetStringNullCheck(colname);

                colname = "strtomtatnoidung";
                var strtomtatnoidung = reader.GetStringNullCheck(colname);

                colname = "strnguoiky";
                var strnguoiky = reader.GetStringNullCheck(colname);

                colname = "strtukhoa";
                var strtukhoa = reader.GetStringNullCheck(colname);

                colname = "intkhan";
                var intkhan = reader.GetIntNullCheck(colname);

                colname = "intmat";
                var intmat = reader.GetIntNullCheck(colname);

                colname = "intiddiachiluutru";
                var intiddiachiluutru = reader.GetIntNullCheck(colname);

                colname = "intidphanloaicongvanden";
                var intidphanloaicongvanden = reader.GetIntNullCheck(colname);

                colname = "bittrangthaiduyet";
                var bittrangthaiduyet = reader.GetIntNullCheck(colname);

                colname = "intiddonvinhap";
                var intiddonvinhap = reader.GetIntNullCheck(colname);

                colname = "strnoidung";
                var strnoidung = reader.GetStringNullCheck(colname);

                colname = "intidnguoitao";
                var intidnguoitao = reader.GetIntNullCheck(colname);

                colname = "strngaytao";
                var dtengaytao = reader.GetDateTimeNullCheck(colname);

                colname = "intidnguoisua";
                var intidnguoisua = reader.GetIntNullCheck(colname);

                colname = "strngaysua";
                var dtengaysua = reader.GetDateTimeNullCheck(colname);

                colname = "intlinhvuc";
                var intlinhvuc = reader.GetIntNullCheck(colname);

                colname = "intmucquantrong";
                var intmucquantrong = reader.GetIntNullCheck(colname);

                colname = "intidsovanban";
                var intidsovanban = reader.GetIntNullCheck(colname);

                colname = "bitquyphamphapluat";
                var bitquyphamphapluat = reader.GetBitNullCheck(colname);
                var intquyphamphapluat = (bitquyphamphapluat == true) ?
                        (int)enumVanbanden.intquyphamphapluat.Co : (int)enumVanbanden.intquyphamphapluat.Khong;

                colname = "intidldvp";
                var intidldvp = reader.GetIntNullCheck(colname);

                colname = "bitguivbdt";
                var intguivbdt = reader.GetIntNullCheck(colname);

                colname = "strhanxuly";
                var dtehanxuly = reader.GetDateTimeNullCheck(colname);

                colname = "strtraloicongvanso";
                var strtraloicongvanso = reader.GetStringNullCheck(colname);

                colname = "intvbdt"; // truong moi o vpub
                var intvbdt = reader.GetIntNullCheck_NotLog(colname);
                //(int)enumVanbanden.intvbdt.VBGiay;

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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
                            + "@intid,"
                            + "@intsoden,"
                            + "@strkyhieu,"
                            + "@dtengayden, "
                            + "@dtengayky, "
                            + "@intkhoiphathanh, "
                            + "@strnoiphathanh, "
                            + "@strnoigui, "
                            + "@strtrichyeu, "
                            + "@intnguoiduyet, "
                            + "@intidsovanban, "
                            + "@intidphanloaicongvanden, "
                            + "@strnoinhan, "
                            + "@strnguoiky, "
                            + "@strtomtatnoidung, "
                            + "@intkhan, "
                            + "@strtukhoa, "
                            + "@intmat, "
                            + "@intiddiachiluutru, "
                            + "@bittrangthaiduyet, "
                            + "@intiddonvinhap, "
                            + "@strnoidung, "
                            + "@intidnguoitao, "
                            + "@dtengaytao, "
                            + "@intidnguoisua, "
                            + "@dtengaysua, "
                            + "@intlinhvuc, "
                            + "@intmucquantrong, "
                            + "@intquyphamphapluat, "
                            + "@intidldvp, "
                            + "@intguivbdt, "
                            + "@dtehanxuly, "
                            + "@strtraloicongvanso, "
                            + "@intvbdt " // intvbdt truong moi, chi co tai vpub 
                            + " ); ";

                    sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    var lstParams = new List<SqlParameter>
                    {
                        new SqlParameter("@intid", intid),
                        new SqlParameter("@intsoden", intsoden),
                        new SqlParameter("@strkyhieu", strkyhieu),
                        new SqlParameter("@dtengayden", dtengayden),
                        new SqlParameter("@dtengayky", dtengayky),
                        new SqlParameter("@intkhoiphathanh", intkhoiphathanh),
                        new SqlParameter("@strnoiphathanh", strnoiphathanh),
                        new SqlParameter("@strnoigui", strnoigui),
                        new SqlParameter("@strtrichyeu", strtrichyeu),
                        new SqlParameter("@intnguoiduyet", intnguoiduyet),
                        new SqlParameter("@intidsovanban", intidsovanban),
                        new SqlParameter("@intidphanloaicongvanden", intidphanloaicongvanden),
                        new SqlParameter("@strnoinhan", strnoinhan),
                        new SqlParameter("@strnguoiky", strnguoiky),
                        new SqlParameter("@strtomtatnoidung", strtomtatnoidung),
                        new SqlParameter("@intkhan", intkhan),
                        new SqlParameter("@strtukhoa", strtukhoa),
                        new SqlParameter("@intmat", intmat),
                        new SqlParameter("@intiddiachiluutru", intiddiachiluutru),
                        new SqlParameter("@bittrangthaiduyet", bittrangthaiduyet),
                        new SqlParameter("@intiddonvinhap", intiddonvinhap),
                        new SqlParameter("@strnoidung", strnoidung),
                        new SqlParameter("@intidnguoitao", intidnguoitao),
                        new SqlParameter("@dtengaytao", dtengaytao),
                        new SqlParameter("@intidnguoisua", intidnguoisua),
                        new SqlParameter("@dtengaysua", dtengaysua),
                        new SqlParameter("@intlinhvuc", intlinhvuc),
                        new SqlParameter("@intmucquantrong", intmucquantrong),
                        new SqlParameter("@intquyphamphapluat", intquyphamphapluat),
                        new SqlParameter("@intidldvp", intidldvp),
                        new SqlParameter("@intguivbdt", intguivbdt),
                        new SqlParameter("@dtehanxuly", dtehanxuly),
                        new SqlParameter("@strtraloicongvanso", strtraloicongvanso),
                        new SqlParameter("@intvbdt", intvbdt)
                    };

                    Utils.RunQuery(sInsert, strconnectdich, lstParams.ToArray());

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    return;
                }
            }
            var sqlFixDate = SQLQuery.FixDateTime(tableD, "strhanxuly");
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

        public void VanbandenCanbo(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblcongvandencanbo";
            const string tableD = "vanbandencanbo";

            // chuan hoa truoc khi convert
            var sDelete = "delete from " + tableS + " where intidvanbanden=0;";
            sDelete += "delete from " + tableS + " where intidcanbo=0;";
            Utils.RunQuery(sDelete, strconnectnguon);

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intidvanbanden";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            var reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            var sqlTruncate = "truncate table " + tableD;
            //Utils.RunQuery(sqlTruncate, strconnectdich);

            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress?.Invoke(countrows, count);

                var colname = "intidcanbo";
                var intidcanbo = reader.GetIntNullCheck(colname);

                colname = "intidvanbanden";
                var intidvanbanden = reader.GetIntNullCheck(colname);

                try
                {
                    var sInsert = "";//" Set IDENTITY_INSERT " + tableD + " ON;";

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

        public void Vanbandi(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblcongvanphathanh";
            const string tableD = "vanbandi";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            var reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            var sqlTruncate = "delete from vanbandicanbo; ";
            sqlTruncate += "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress(countrows, count);

                var colname = "intid";
                var intid = reader.GetIntNullCheck(colname);

                colname = "intso";
                var intso = reader.GetIntNullCheck(colname);

                colname = "strkyhieu";
                var strkyhieu = reader.GetStringNullCheck(colname);

                colname = "strngayky";
                var dtengayky = reader.GetDateTimeNullCheck(colname);

                colname = "strdonvisoan";
                var strdonvisoan = reader.GetStringNullCheck(colname);

                colname = "strtrichyeu";
                var strtrichyeu = reader.GetStringNullCheck(colname);

                colname = "strnoinhan";
                var strnoinhan = reader.GetStringNullCheck(colname);

                colname = "strnguoiky";
                var strnguoiky = reader.GetStringNullCheck(colname);

                colname = "strnguoisoan";
                var strnguoisoan = reader.GetStringNullCheck(colname);

                colname = "strnguoiduyet";
                var strnguoiduyet = reader.GetStringNullCheck(colname);

                colname = "intnguoiduyet";
                var intnguoiduyet = reader.GetIntNullCheck(colname);

                colname = "strtomtat";
                var strtomtat = reader.GetStringNullCheck(colname);

                colname = "intkhan";
                var intkhan = reader.GetIntNullCheck(colname);

                colname = "intmat";
                var intmat = reader.GetIntNullCheck(colname);

                colname = "intsoban";
                var intsoban = reader.GetIntNullCheck(colname);

                colname = "intsoto";
                var intsoto = reader.GetIntNullCheck(colname);

                colname = "strtukhoa";
                var strtukhoa = reader.GetStringNullCheck(colname);

                colname = "intiddiachiluutru";
                var intiddiachiluutru = reader.GetIntNullCheck(colname);

                colname = "intidphanloaicongvanphathanh";
                var intidphanloaicongvanphathanh = reader.GetIntNullCheck(colname);

                colname = "bittrangthaiduyet";
                var bittrangthaiduyet = reader.GetIntNullCheck(colname);

                colname = "intiddonvinhap";
                var intiddonvinhap = reader.GetIntNullCheck(colname);

                colname = "strnoidung";
                var strnoidung = reader.GetStringNullCheck(colname);

                colname = "intidnguoitao";
                var intidnguoitao = reader.GetIntNullCheck(colname);

                colname = "strngaytao";
                var dtengaytao = reader.GetDateTimeNullCheck(colname);

                colname = "intidnguoisua";
                var intidnguoisua = reader.GetIntNullCheck(colname);

                colname = "strngaysua";
                var dtengaysua = reader.GetDateTimeNullCheck(colname);

                colname = "intlinhvuc";
                var intlinhvuc = reader.GetIntNullCheck(colname);

                colname = "intmucquantrong";
                var intmucquantrong = reader.GetIntNullCheck(colname);

                colname = "bitguivbdt";
                int? intguivbdt = (reader.GetBitNullCheck(colname) == true) ?
                    (int)enumVanbandi.intguivbdt.Dagui : (int)enumVanbandi.intguivbdt.Chuagui;

                colname = "intidsovanban";
                var intidsovanban = reader.GetIntNullCheck(colname);

                colname = "bitquyphamphapluat";
                var bitquyphamphapluat = reader.GetBitNullCheck(colname);
                var intquyphamphapluat = (bitquyphamphapluat == true) ?
                        (int)enumVanbandi.intquyphamphapluat.Co : (int)enumVanbandi.intquyphamphapluat.Khong;

                colname = "strmorong";
                var strmorong = reader.GetStringNullCheck(colname);

                colname = "strhanxuly";
                var dtehanxuly = reader.GetDateTimeNullCheck(colname);

                colname = "intsosao";
                var intsosao = reader.GetIntNullCheck(colname);

                colname = "strngaysao";
                var dtengaysao = reader.GetDateTimeNullCheck(colname);

                colname = "strtraloicongvanso";
                var strtraloicongvanso = reader.GetStringNullCheck(colname);

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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
                            + "@intid,"
                            + "@intso,"
                            + "@strkyhieu,"
                            + "@dtengayky, "
                            + "@strdonvisoan, "
                            + "@strtrichyeu, "
                            + "@strnoinhan, "
                            + "@strnguoiky, "
                            + "@strnguoisoan, "
                            + "@strnguoiduyet, "
                            + "@intnguoiduyet, "
                            + "@strtomtat, "
                            + "@intkhan, "
                            + "@intmat, "
                            + "@intsoban, "
                            + "@intsoto, "
                            + "@strtukhoa, "
                            + "@intiddiachiluutru, "
                            + "@intidphanloaicongvanphathanh, "
                            + "@bittrangthaiduyet, "
                            + "@intiddonvinhap, "
                            + "@strnoidung, "
                            + "@intidnguoitao, "
                            + "@dtengaytao, "
                            + "@intidnguoisua, "
                            + "@dtengaysua, "
                            + "@intlinhvuc, "
                            + "@intmucquantrong, "
                            + "@intguivbdt, "
                            + "@intidsovanban, "
                            + "@intquyphamphapluat, "
                            + "@strmorong, "
                            + "@dtehanxuly, "
                            + "@intsosao, "
                            + "@dtengaysao, "
                            + "@strtraloicongvanso "
                            + " ); ";

                    sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    var lstParams = new List<SqlParameter>
                    {
                        new SqlParameter("@intid", intid),
                        new SqlParameter("@intso", intso),
                        new SqlParameter("@strkyhieu", strkyhieu),
                        new SqlParameter("@dtengayky", dtengayky),
                        new SqlParameter("@strdonvisoan", strdonvisoan),
                        new SqlParameter("@strtrichyeu", strtrichyeu),
                        new SqlParameter("@strnoinhan", strnoinhan),
                        new SqlParameter("@strnguoiky", strnguoiky),
                        new SqlParameter("@strnguoisoan", strnguoisoan),
                        new SqlParameter("@strnguoiduyet", strnguoiduyet),
                        new SqlParameter("@intnguoiduyet", intnguoiduyet),
                        new SqlParameter("@strtomtat", strtomtat),
                        new SqlParameter("@intkhan", intkhan),
                        new SqlParameter("@intmat", intmat),
                        new SqlParameter("@intsoban", intsoban),
                        new SqlParameter("@intsoto", intsoto),
                        new SqlParameter("@strtukhoa", strtukhoa),
                        new SqlParameter("@intiddiachiluutru", intiddiachiluutru),
                        new SqlParameter("@intidphanloaicongvanphathanh", intidphanloaicongvanphathanh),
                        new SqlParameter("@bittrangthaiduyet", bittrangthaiduyet),
                        new SqlParameter("@intiddonvinhap", intiddonvinhap),
                        new SqlParameter("@strnoidung", strnoidung),
                        new SqlParameter("@intidnguoitao", intidnguoitao),
                        new SqlParameter("@dtengaytao", dtengaytao),
                        new SqlParameter("@intidnguoisua", intidnguoisua),
                        new SqlParameter("@dtengaysua", dtengaysua),
                        new SqlParameter("@intlinhvuc", intlinhvuc),
                        new SqlParameter("@intmucquantrong", intmucquantrong),
                        new SqlParameter("@intguivbdt", intguivbdt),
                        new SqlParameter("@intidsovanban", intidsovanban),
                        new SqlParameter("@intquyphamphapluat", intquyphamphapluat),
                        new SqlParameter("@strmorong", strmorong),
                        new SqlParameter("@dtehanxuly", dtehanxuly),
                        new SqlParameter("@intsosao", intsosao),
                        new SqlParameter("@dtengaysao", dtengaysao),
                        new SqlParameter("@strtraloicongvanso", strtraloicongvanso)
                    };

                    Utils.RunQuery(sInsert, strconnectdich, lstParams.ToArray());

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    return;
                }
            }
            var sqlFixDate = SQLQuery.FixDateTime(tableD, "strhanxuly");
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

        public void VanbandiCanbo(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblcongvanphathanhcanbo";
            const string tableD = "vanbandicanbo";

            // chuan hoa truoc khi convert
            var sDelete = "delete from " + tableS + " where intidvanbanphathanh=0;";
            sDelete += "delete from " + tableS + " where intidcanbo=0;";
            Utils.RunQuery(sDelete, strconnectnguon);

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intidvanbanphathanh";
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

                var colname = "intidcanbo";
                var intidcanbo = reader.GetIntNullCheck(colname);

                colname = "intidvanbanphathanh";
                var intidvanbandi = reader.GetIntNullCheck(colname);

                try
                {
                    var sInsert = "";//" Set IDENTITY_INSERT " + tableD + " ON;";

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

        public void Guicongvan(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblguicongvan";
            const string tableD = "guivanban";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            var reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            var sqlTruncate = "truncate table " + tableD;
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

                colname = "intidvanban";
                var intidvanban = reader.GetIntNullCheck(colname);

                colname = "intiddonvitgtdcv";
                var intiddonvitgtdcv = reader.GetIntNullCheck(colname);

                colname = "intloaicongvan";
                var intloaicongvan = reader.GetIntNullCheck(colname);

                colname = "inttrangthai";
                var inttrangthai = reader.GetIntNullCheck(colname);

                colname = "strngaygui";
                var dtengaygui = reader.GetDateTimeNullCheck(colname);

                colname = "inttinhtrangnhanvb";
                var inttinhtrangnhanvb = reader.GetIntNullCheck(colname);

                colname = "strngaynhan";
                var dtengaynhan = reader.GetDateTimeNullCheck(colname);

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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
                            + "@intid, "
                            + "@intidvanban, "
                            + "@intloaicongvan, "
                            + "@intiddonvitgtdcv, "
                            + "@inttrangthai, "
                            + "@dtengaygui, "
                            + "@inttinhtrangnhanvb, "
                            + "@dtengaynhan "
                            + " ); ";

                    sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    var lstParams = new List<SqlParameter>
                    {
                        new SqlParameter("@intid", intid),
                        new SqlParameter("@intidvanban", intidvanban),
                        new SqlParameter("@intloaicongvan", intloaicongvan),
                        new SqlParameter("@intiddonvitgtdcv", intiddonvitgtdcv),
                        new SqlParameter("@inttrangthai", inttrangthai),
                        new SqlParameter("@dtengaygui", dtengaygui),
                        new SqlParameter("@inttinhtrangnhanvb", inttinhtrangnhanvb),
                        new SqlParameter("@dtengaynhan", dtengaynhan)
                    };


                    Utils.RunQuery(sInsert, strconnectdich, lstParams.ToArray());
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    return;
                }
            }
            var sqlFixDate = SQLQuery.FixDateTime(tableD, "strngaygui");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            sqlFixDate = SQLQuery.FixDateTime(tableD, "strngaynhan");
            Utils.RunQuery(sqlFixDate, strconnectdich);

            _logger.Info(tableS + " -- Done");

            sqlConnectionDich.Close();
            reader.Close();
            sqlConnectionnguon.Close();

        }

        public void Hoibaovanban(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblhoibaovanban";
            const string tableD = "hoibaovanban";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS;
            //+ " order by intid ";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            var reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            const string sqlTruncate = "truncate table " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress?.Invoke(countrows, count);

                var colname = string.Empty;

                colname = "intTransID";
                var intTransID = reader.GetIntNullCheck(colname);

                colname = "intRecID";
                var intRecID = reader.GetIntNullCheck(colname);

                colname = "intType";
                var intType = reader.GetIntNullCheck(colname);

                try
                {
                    var sInsert = ""; //" Set IDENTITY_INSERT " + tableD + " ON;";

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

        public void Vanbandenmail(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblvanbandenmail";
            const string tableD = "vanbandenmail";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS
                        + " order by intid";
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
                ReportProgress(countrows, count);

                var colname = "intid";
                var intid = reader.GetIntNullCheck(colname);

                colname = "strngayky";
                var dtengayky = reader.GetDateTimeNullCheck(colname);

                colname = "strkyhieu";
                var strkyhieu = reader.GetStringNullCheck(colname);

                colname = "intidphanloaicongvanden";
                var intidphanloaicongvanden = reader.GetIntNullCheck(colname);

                colname = "intkhan";
                var intkhan = reader.GetIntNullCheck(colname);

                colname = "intmat";
                var intmat = reader.GetIntNullCheck(colname);

                colname = "strtrichyeu";
                var strtrichyeu = reader.GetStringNullCheck(colname);

                colname = "strnguoiky";
                var strnguoiky = reader.GetStringNullCheck(colname);

                colname = "strnoigui";
                var strnoigui = reader.GetStringNullCheck(colname);

                colname = "strloaivanban";
                var strloaivanban = reader.GetStringNullCheck(colname);

                colname = "bittrangthai";
                var bittrangthai = reader.GetBitNullCheck(colname);
                int? inttrangthai = (bittrangthai == true) ?
                    1 : 0;

                colname = "bitattach";
                var bitattach = reader.GetBitNullCheck(colname);
                int? intattach = (bitattach == true) ?
                   (int)enumVanbandenmail.intattach.Co : (int)enumVanbandenmail.intattach.Khong;

                colname = "strAddressSend";
                var strAddressSend = reader.GetStringNullCheck(colname);

                colname = "strNoiguiVB";
                var strNoiguiVb = reader.GetStringNullCheck(colname);

                colname = "strngayguivb";
                var dtengayguivb = reader.GetDateTimeNullCheck(colname);

                colname = "strngaynhanvb";
                var dtengaynhanvb = reader.GetDateTimeNullCheck(colname);

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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
                            + "@intid,"
                            + "@dtengayky,"
                            + "@strkyhieu,"
                            + "@intidphanloaicongvanden,"
                            + "@intkhan,"

                            + "@intmat, "

                            + "@strtrichyeu, "
                            + "@strnguoiky, "
                            + "@strnoigui, "
                            + "@strloaivanban, "
                            + "@inttrangthai, "
                            + "@intattach, "
                            + "@strAddressSend, "
                            + "@strNoiguiVb, "
                            + "@dtengayguivb, "
                            + "@dtengaynhanvb "
                            + " ); ";

                    sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    var lstParams = new List<SqlParameter>()
                    {
                        new SqlParameter("@intid", intid),
                        new SqlParameter("@dtengayky", dtengayky),
                        new SqlParameter("@strkyhieu", strkyhieu),
                        new SqlParameter("@intidphanloaicongvanden", intidphanloaicongvanden),
                        new SqlParameter("@intkhan", intkhan),

                        new SqlParameter("@intmat", intmat),

                        new SqlParameter("@strtrichyeu", strtrichyeu),
                        new SqlParameter("@strnguoiky", strnguoiky),
                        new SqlParameter("@strnoigui", strnoigui),
                        new SqlParameter("@strloaivanban", strloaivanban),
                        new SqlParameter("@inttrangthai", inttrangthai),
                        new SqlParameter("@intattach", intattach),
                        new SqlParameter("@strAddressSend", strAddressSend),
                        new SqlParameter("@strNoiguiVb", strNoiguiVb),
                        new SqlParameter("@dtengayguivb", dtengayguivb),
                        new SqlParameter("@dtengaynhanvb", dtengaynhanvb)
                    };

                    Utils.RunQuery(sInsert, strconnectdich, lstParams.ToArray());

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    return;
                }
            }

            var sqlFixDate = SQLQuery.FixDateTime(tableD, "strngayky");
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
