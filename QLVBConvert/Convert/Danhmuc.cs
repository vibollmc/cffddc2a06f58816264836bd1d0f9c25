using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using QLVB.Domain.Entities;
//using System.Data.Entity;
//using System.Data.EntityClient;

namespace Convert
{
    public class Danhmuc
    {
        readonly Logging _logger = new Logging();

        public event ProgressBarHandler ReportProgress;

        public void SoVanban(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            //EntityConnection encon = Utils.GetEntityConnection(strconnectdich);
            //string strencon = Utils.GetStrConnectCSDL(strconnectdich);

            //QLVBDatabase context = new QLVBDatabase(); //new QLVBDatabase(strencon);

            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon); //new SqlConnection(strcon);
            var cmd = new SqlCommand();

            const string tableS = "tblsovanban";
            const string tableD = "sovanban";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from tblsovanban left join tblsovbkhoiph on tblsovanban.intid = tblsovbkhoiph.intidsovb order by intid";
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

                colname = "strten";
                var strten = reader.GetStringNullCheck(colname);

                colname = "strkyhieu";
                var strkyhieu = reader.GetStringNullCheck(colname);

                colname = "strghichu";
                var strghichu = reader.GetStringNullCheck(colname);

                colname = "bitloai";
                var intloai = reader.GetSmallIntNullCheck(colname);

                colname = "intidkhoiph";
                var intidkhoiph = reader.GetIntNullCheck(colname);

                colname = "intidloaivb";
                var intidloaivb = reader.GetIntNullCheck(colname);

                //int idsovb = 0;
                try
                {
                    //SoVanban sovb = new SoVanban();
                    //sovb.intidkhoiph = intidkhoiph;
                    //sovb.intidloaivb = intidloaivb;
                    //sovb.intloai = intloai;
                    //sovb.inttrangthai = (int)enumSovanban.inttrangthai.IsActive;
                    //sovb.strghichu = strghichu;
                    //sovb.strkyhieu = strkyhieu;
                    //sovb.strten = strten;
                    //context.SoVanbans.Add(sovb);
                    //context.SaveChanges();
                    //idsovb = sovb.intid;
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "strten, "
                            + "strkyhieu,"
                            + "strghichu, "
                            + "intidkhoiph, "
                            + "intidloaivb, "
                            + "intloai, "
                            + "isdefault, "
                            + "inttrangthai "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "', "
                            + "N'" + strten + "', "
                            + "N'" + strkyhieu + "', "
                            + "N'" + strghichu + "', "
                            + "'" + intidkhoiph + "', "
                            + "'" + intidloaivb + "', "
                            + "'" + intloai + "', "
                            + "'0', "
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

                //string sqlUpdate = SQLQuery.UpdateId1(tableD, intid.ToString(), idsovb.ToString());
                //Utils.UpdateQuery(sqlUpdate, sqlConnectionDich);

            }
            _logger.Info(tableS + " -- Done");

            reader.Close();
            sqlConnectionDich.Close();
            sqlConnectionnguon.Close();

        }

        public void Cauhinh(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblcauhinh";
            //string tableD = "Config";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS;
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

            // mo ket noi toi qlvb2
            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress?.Invoke(countrows, count);

                var colname = "domainname";
                var domainname = reader.GetStringNullCheck(colname);

                colname = "smtpport";
                var smtpport = reader.GetStringNullCheck(colname);

                colname = "domainip";
                var domainip = reader.GetStringNullCheck(colname);

                colname = "pop3port";
                var pop3Port = reader.GetStringNullCheck(colname);

                colname = "usernamemail";
                var usernamemail = reader.GetStringNullCheck(colname);

                colname = "passwordmail";
                var passwordmail = reader.GetStringNullCheck(colname);

                colname = "thoigian";
                var thoigian = reader.GetIntNullCheck(colname);

                try
                {
                    var sqlUpdate = "update config set strgiatri='" + thoigian + "' where strthamso='SoNgayHienThi' ;";

                    sqlUpdate += "update config set strgiatri='" + domainname + "' where strthamso='SMTPServer' ;";
                    sqlUpdate += "update config set strgiatri='" + smtpport + "' where strthamso='SMTPPort' ;";

                    sqlUpdate += "update config set strgiatri='" + domainip + "' where strthamso='POP3Server' ;";
                    sqlUpdate += "update config set strgiatri='" + pop3Port + "' where strthamso='POP3Port' ;";

                    sqlUpdate += "update config set strgiatri='" + usernamemail + "' where strthamso='UsernameMail' ;";
                    sqlUpdate += "update config set strgiatri='" + passwordmail + "' where strthamso='PasswordMail' ;";

                    Utils.UpdateQuery(sqlUpdate, sqlConnectionDich);

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

        public void Chucdanh(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblchucdanh";
            const string tableD = "chucdanh";

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
            const string sqlTruncate = "truncate table " + tableD;
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

                colname = "strmachucdanh";
                var strmachucdanh = reader.GetStringNullCheck(colname);

                colname = "strtenchucdanh";
                var strtenchucdanh = reader.GetStringNullCheck(colname);

                colname = "strghichu";
                var strghichu = reader.GetStringNullCheck(colname);

                colname = "intloai";
                var intloai = reader.GetIntNullCheck(colname);

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "strmachucdanh, "
                            + "strtenchucdanh,"
                            + "strghichu, "
                            + "intloai "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "', "
                            + "N'" + strmachucdanh + "', "
                            + "N'" + strtenchucdanh + "', "
                            + "N'" + strghichu + "', "
                            + "'" + intloai + "' "
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

        public void Diachiluutru(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tbldiachiluutrutree";
            const string tableD = "diachiluutru";

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
            var sqlTruncate = "truncate table " + tableD;
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

                colname = "strparent";
                var strparent = reader.GetStringNullCheck(colname);

                colname = "strname";
                var strname = reader.GetStringNullCheck(colname);

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
                                + "strtendonvi,"
                                + "inttrangthai "
                                + " ) ";

                        sInsert += "values("
                                + "'" + intid + "', "
                                + "null, "
                                + "'0', "
                                + "N'" + strname + "', "
                                + "'" + inttrangthai + "' "
                                + " ) ;";

                        sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                        Utils.RunQuery(sInsert, strconnectdich);
                    }
                    else
                    {   // child
                        var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                        sInsert += "insert into  " + tableD
                                + "(Id, "
                                + "ParentId, "
                                + "intlevel, "
                                + "strtendonvi,"
                                + "inttrangthai "
                                + " ) ";

                        sInsert += "values("
                                + "'" + intid + "', "
                                + "'1', "
                                + "'0', "
                                + "N'" + strname + "', "
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

        public void Khoiphathanh(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblkhoiphathanh";
            const string tableD = "khoiphathanh";

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

                colname = "strtenkhoi";
                var strtenkhoi = reader.GetStringNullCheck(colname);

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "strtenkhoi, "
                            + "isdefault,"
                            + "inttrangthai "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "', "
                            + "N'" + strtenkhoi + "', "
                            + "'0', "
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

        public void Linhvuc(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tbllinhvuc";
            const string tableD = "linhvuc";

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

                var colname = "intid";
                var intid = reader.GetIntNullCheck(colname);

                colname = "strtenlinhvuc";
                var strtenlinhvuc = reader.GetStringNullCheck(colname);

                colname = "strkyhieu";
                var strkyhieu = reader.GetStringNullCheck(colname);

                colname = "strghichu";
                var strghichu = reader.GetStringNullCheck(colname);

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "strtenlinhvuc, "
                            + "strkyhieu, "
                            + "strghichu, "
                            + "inttrangthai "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "', "
                            + "N'" + strtenlinhvuc + "', "
                            + "N'" + strkyhieu + "', "
                            + "N'" + strghichu + "', "
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

        /// <summary>
        /// chua co
        /// </summary>
        /// <param name="strconnectnguon"></param>
        /// <param name="strconnectdich"></param>
        public void Mucquantrong(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            var tableS = "tblmucquantrong";
            var tableD = "linhvuc";

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

                var colname = "intid";
                var intid = reader.GetIntNullCheck(colname);

                colname = "strtenlinhvuc";
                var strtenlinhvuc = reader.GetStringNullCheck(colname);

                colname = "strkyhieu";
                var strkyhieu = reader.GetStringNullCheck(colname);

                colname = "strghichu";
                var strghichu = reader.GetStringNullCheck(colname);

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "strtenlinhvuc, "
                            + "strkyhieu, "
                            + "strghichu, "
                            + "inttrangthai "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "', "
                            + "N'" + strtenlinhvuc + "', "
                            + "N'" + strkyhieu + "', "
                            + "N'" + strghichu + "', "
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

        public void Phanloaivanbanden(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();
            SqlDataReader reader;

            var tableS = "tblplcongvanden";
            var tableD = "phanloaivanban";

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

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            var sqlTruncate = "delete from " + tableD + " where intloai=" + (int)enumPhanloaiVanban.intloai.vanbanden;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            // them cot id1 de luu intid cua qlvb 1 
            var sqlAddColumn = SQLQuery.AddColumn(tableD, "id1", "int");
            Utils.RunQuery(sqlAddColumn, strconnectdich);

            // mo ket noi toi qlvb2
            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while ((reader.Read()))
            {
                count++;
                ReportProgress?.Invoke(countrows, count);

                var colname = string.Empty;

                colname = "intid";
                var id1 = reader.GetIntNullCheck(colname);

                colname = "strmacongvan";
                var strmacongvan = reader.GetStringNullCheck(colname);

                colname = "strtenhinhthuc";
                var strtenhinhthuc = reader.GetStringNullCheck(colname);

                colname = "strghichu";
                var strghichu = reader.GetStringNullCheck(colname);

                colname = "bitmacdinh";
                var intmacdinh = reader.GetIntNullCheck(colname);

                var intloai = (int)enumPhanloaiVanban.intloai.vanbanden;
                try
                {
                    var sInsert = "";// " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(id1, "
                            + "intloai, "
                            + "strmavanban, "
                        //+ "strkyhieu, "
                            + "strtenvanban, "
                            + "strghichu, "
                            + "isdefault, "
                            + "inttrangthai "
                            + " ) ";

                    sInsert += "values("
                            + "'" + id1 + "', "
                            + "'" + intloai + "', "
                            + "N'" + strmacongvan + "', "
                            + "N'" + strtenhinhthuc + "', "
                            + "N'" + strghichu + "', "
                            + "'" + intmacdinh + "', "
                            + "'1' "
                            + " ) ;";

                    //sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    Utils.RunQuery(sInsert, strconnectdich);
                    //==============================================
                    // lay idloaivanban vua moi them vao
                    cmd.CommandText = "select * from " + tableD
                    + " where intloai='" + (int)enumPhanloaiVanban.intloai.vanbanden + "' and id1=" + id1;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnectionDich;
                    var reader11 = cmd.ExecuteReader();
                    var idloaivb = 0;
                    while ((reader11 != null) && (reader11.Read()))
                    {
                        colname = string.Empty;
                        colname = "intid";
                        idloaivb = (int)reader11.GetIntNullCheck(colname);
                    }
                    reader11.Close();

                    if (idloaivb != 0)
                    {
                        //================================================
                        // them motatruong vao phanloaitruong
                        cmd.CommandText = "select * from motatruong "
                        + " where intloai='" + (int)enumPhanloaiVanban.intloai.vanbanden + "'  order by intid";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = sqlConnectionDich;

                        var reader3 = cmd.ExecuteReader();
                        while ((reader3 != null) && (reader3.Read()))
                        {
                            colname = string.Empty;
                            colname = "intid";
                            var intidmotatruong = reader3.GetIntNullCheck(colname);
                            colname = "strten";
                            var strten = reader3.GetStringNullCheck_Unicode(colname);
                            colname = "IsRequire";
                            var isRequire = reader3.GetBitNullCheck(colname);
                            colname = "intorder";
                            var intorder = reader3.GetIntNullCheck(colname);

                            var tableTruong = "phanloaitruong";

                            // update id cua phan loai van banden
                            var sPhanloaitruong = "";// " Set IDENTITY_INSERT " + tableTruong + " ON;";

                            sPhanloaitruong += "insert into  " + tableTruong
                                    + "(intloai, "
                                    + "intidphanloaivanban, "
                                    + "intidmotatruong, "
                                    + "isDisplay, "
                                    + "intorder, "
                                    + "isRequire, "
                                    + "strmota, "
                                    + "intloaitruong "
                                    + " ) ";

                            sPhanloaitruong += "values("
                                    + "'" + intloai + "', "
                                    + "'" + idloaivb + "', "
                                    + "'" + intidmotatruong + "', "
                                    + "'true', "
                                    + "'" + intorder + "', "
                                    + "'" + isRequire + "', "
                                    + "N'" + strten + "', "
                                    + "'" + (int)enumPhanloaiTruong.intloaitruong.Default + "'"
                                    + " ) ;";

                            //sPhanloaitruong += " Set IDENTITY_INSERT " + tableTruong + " OFF;";

                            Utils.RunQuery(sPhanloaitruong, strconnectdich);
                        }
                        reader3.Close();
                    }
                    else
                    {
                        _logger.Error("Lỗi khi thêm loại văn bản đến:" + strtenhinhthuc);
                    }

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    return;
                }
            }

            cmd.CommandText = "select * from " + tableD
                    + " where intloai='" + (int)enumPhanloaiVanban.intloai.vanbanden + "'  order by intid";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionDich;

            var reader2 = cmd.ExecuteReader();
            while ((reader2 != null) && (reader2.Read()))
            {
                var colname = string.Empty;
                colname = "intid";
                var intid = reader2.GetIntNullCheck(colname);
                colname = "id1";
                var id1 = reader2.GetIntNullCheck(colname);
                // update id cua phan loai van banden
                var sUpdate = "update vanbanden set intidphanloaivanbanden='" + intid +
                        "' where intidphanloaivanbanden='" + id1 + "'";
                Utils.RunQuery(sUpdate, strconnectdich);
            }

            _logger.Info(tableS + " -- Done");


            reader2.Close();
            reader.Close();
            sqlConnectionDich.Close();
            sqlConnectionnguon.Close();
        }

        public void Phanloaivanbandi(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            var tableS = "tblplcongvanphathanh";
            var tableD = "phanloaivanban";

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
            var sqlTruncate = "delete from " + tableD + " where intloai=" + (int)enumPhanloaiVanban.intloai.vanbandi;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            // them cot id1 de luu intid cua qlvb 1 
            var sqlAddColumn = SQLQuery.AddColumn(tableD, "id1", "int");
            Utils.RunQuery(sqlAddColumn, strconnectdich);

            // mo ket noi toi qlvb2
            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while ((reader.Read()))
            {
                count++;
                ReportProgress?.Invoke(countrows, count);

                var colname = "intid";
                var intid = reader.GetIntNullCheck(colname);

                colname = "strmacongvan";
                var strmacongvan = reader.GetStringNullCheck(colname);

                colname = "strkyhieu";
                var strkyhieu = reader.GetStringNullCheck(colname);

                colname = "strtenhinhthuc";
                var strtenhinhthuc = reader.GetStringNullCheck(colname);

                colname = "strghichu";
                var strghichu = reader.GetStringNullCheck(colname);

                colname = "bitmacdinh";
                var intmacdinh = reader.GetIntNullCheck(colname);

                var intloai = (int)enumPhanloaiVanban.intloai.vanbandi;
                try
                {
                    var sInsert = "";// " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(id1, "
                            + "intloai, "
                            + "strmavanban, "
                            + "strkyhieu, "
                            + "strtenvanban, "
                            + "strghichu, "
                            + "isdefault, "
                            + "inttrangthai "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "', "
                            + "'" + intloai + "', "
                            + "N'" + strmacongvan + "', "
                            + "N'" + strkyhieu + "', "
                            + "N'" + strtenhinhthuc + "', "
                            + "N'" + strghichu + "', "
                            + "'" + intmacdinh + "', "
                            + "'1' "
                            + " ) ;";

                    //sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    Utils.RunQuery(sInsert, strconnectdich);
                    //==============================================
                    // lay idloaivanban vua moi them vao
                    cmd.CommandText = "select * from " + tableD
                    + " where intloai='" + (int)enumPhanloaiVanban.intloai.vanbandi + "' and id1=" + intid;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnectionDich;
                    var reader11 = cmd.ExecuteReader();
                    var idloaivb = 0;
                    while ((reader11 != null) && (reader11.Read()))
                    {
                        colname = string.Empty;
                        colname = "intid";
                        idloaivb = (int)reader11.GetIntNullCheck(colname);
                    }
                    reader11.Close();

                    if (idloaivb != 0)
                    {
                        //================================================
                        // them motatruong vao phanloaitruong
                        cmd.CommandText = "select * from motatruong "
                        + " where intloai='" + (int)enumPhanloaiVanban.intloai.vanbandi + "'  order by intid";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = sqlConnectionDich;

                        var reader3 = cmd.ExecuteReader();
                        while ((reader3 != null) && (reader3.Read()))
                        {
                            colname = string.Empty;
                            colname = "intid";
                            var intidmotatruong = reader3.GetIntNullCheck(colname);
                            colname = "strten";
                            var strten = reader3.GetStringNullCheck_Unicode(colname);
                            colname = "IsRequire";
                            var isRequire = reader3.GetBitNullCheck(colname);
                            colname = "intorder";
                            var intorder = reader3.GetIntNullCheck(colname);

                            var tableTruong = "phanloaitruong";

                            // update id cua phan loai van banden
                            var sPhanloaitruong = "";// " Set IDENTITY_INSERT " + tableTruong + " ON;";

                            sPhanloaitruong += "insert into  " + tableTruong
                                    + "(intloai, "
                                    + "intidphanloaivanban, "
                                    + "intidmotatruong, "
                                    + "isDisplay, "
                                    + "intorder, "
                                    + "isRequire, "
                                    + "strmota, "
                                    + "intloaitruong "
                                    + " ) ";

                            sPhanloaitruong += "values("
                                    + "'" + intloai + "', "
                                    + "'" + idloaivb + "', "
                                    + "'" + intidmotatruong + "', "
                                    + "'true', "
                                    + "'" + intorder + "', "
                                    + "'" + isRequire + "', "
                                    + "N'" + strten + "', "
                                    + "'" + (int)enumPhanloaiTruong.intloaitruong.Default + "'"
                                    + " ) ;";

                            //sPhanloaitruong += " Set IDENTITY_INSERT " + tableTruong + " OFF;";

                            Utils.RunQuery(sPhanloaitruong, strconnectdich);
                        }
                        reader3.Close();

                        // fix loi bắt buộc phải có loại văn bản khi thêm mới vb đến/đi
                        var sqlLoaivanban = " update phanloaitruong set isrequire=1 , isdisplay=1 where strmota=N'Loại văn bản'";
                        Utils.RunQuery(sqlLoaivanban, strconnectdich);
                    }
                    else
                    {
                        _logger.Error("Lỗi khi thêm loại văn bản đi:" + strtenhinhthuc);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    return;
                }
            }
            cmd.CommandText = "select * from " + tableD
                    + " where intloai='" + (int)enumPhanloaiVanban.intloai.vanbandi + "'  order by intid";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionDich;

            var reader2 = cmd.ExecuteReader();
            while ((reader2 != null) && (reader2.Read()))
            {
                var colname = "intid";
                var intid = reader2.GetIntNullCheck(colname);
                colname = "id1";
                var id1 = reader2.GetIntNullCheck(colname);
                // update id cua phan loai van banden
                var sUpdate = "update vanbandi set intidphanloaivanbandi='" + intid +
                        "' where intidphanloaivanbandi='" + id1 + "'";
                Utils.RunQuery(sUpdate, strconnectdich);
            }

            _logger.Info(tableS + " -- Done");

            reader2.Close();
            reader.Close();
            sqlConnectionDich.Close();
            sqlConnectionnguon.Close();
        }

        public void Phanloaivanbanduthao(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            var tableS = "tblplvanbanduthao";
            var tableD = "phanloaivanban";

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
            var sqlTruncate = "delete from " + tableD + " where intloai=" + (int)enumPhanloaiVanban.intloai.vanbanduthao;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            // them cot id1 de luu intid cua qlvb 1 
            var sqlAddColumn = SQLQuery.AddColumn(tableD, "id1", "int");
            Utils.RunQuery(sqlAddColumn, strconnectdich);

            // mo ket noi toi qlvb2
            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while ((reader.Read()))
            {
                count++;
                ReportProgress?.Invoke(countrows, count);

                var colname = "intid";
                var intid = reader.GetIntNullCheck(colname);

                colname = "strmacongvan";
                var strmacongvan = reader.GetStringNullCheck(colname);

                colname = "strtenhinhthuc";
                var strtenhinhthuc = reader.GetStringNullCheck(colname);

                colname = "strghichu";
                var strghichu = reader.GetStringNullCheck(colname);

                colname = "bitmacdinh";
                var intmacdinh = reader.GetIntNullCheck(colname);

                var intloai = (int)enumPhanloaiVanban.intloai.vanbanduthao;
                try
                {
                    var sInsert = "";// " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(id1, "
                            + "intloai, "
                            + "strmavanban, "
                            + "strtenvanban, "
                            + "strghichu, "
                            + "isdefault, "
                            + "inttrangthai "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "', "
                            + "'" + intloai + "', "
                            + "N'" + strmacongvan + "', "
                            + "N'" + strtenhinhthuc + "', "
                            + "N'" + strghichu + "', "
                            + "'" + intmacdinh + "', "
                            + "'1' "
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
            //cmd.CommandText = "select * from " + tableD
            //        + " where intloai='" + (int)enumPhanloaiVanban.intloai.vanbandi + "'  order by intid";
            //cmd.CommandType = CommandType.Text;
            //cmd.Connection = sqlConnectionDich;

            //SqlDataReader reader2 = cmd.ExecuteReader();
            //while ((reader2 != null) && (reader2.Read()))
            //{
            //    string colname = string.Empty;
            //    colname = "intid";
            //    int? intid = Utils.GetIntNullCheck(reader2, colname);
            //    colname = "id1";
            //    int? id1 = Utils.GetIntNullCheck(reader2, colname);
            //    // update id cua phan loai van banden
            //    string sUpdate = "update vanbandi set intidphanloaivanbandi='" + intid +
            //            "' where intidphanloaivanbandi='" + id1 + "'";
            //    Utils.RunQuery(sUpdate, strconnectdich);
            //}

            _logger.Info(tableS + " -- Done");

            //reader2.Close();
            reader.Close();
            sqlConnectionDich.Close();
            sqlConnectionnguon.Close();
        }

        public void Tinhchatvanban(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            var tableS = "tbltinhchatvanban";
            var tableD = "tinhchatvanban";

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
            var sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            // mo ket noi toi qlvb2
            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while ((reader.Read()))
            {
                count++;
                ReportProgress?.Invoke(countrows, count);

                var colname = "intid";
                var intid = reader.GetIntNullCheck(colname);

                colname = "strtentinhchatvb";
                var strtentinhchatvb = reader.GetStringNullCheck(colname);

                colname = "strkyhieu";
                var strkyhieu = reader.GetStringNullCheck(colname);

                colname = "strmota";
                var strmota = reader.GetStringNullCheck(colname);

                colname = "bitloai";
                var bitloai = reader.GetBitNullCheck(colname);
                int? intloai = (bitloai == true) ?
                    (int)enumTinhchatvanban.intloai.Mat : (int)enumTinhchatvanban.intloai.Khan;

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "strtentinhchatvb, "
                            + "strkyhieu,"
                            + "strmota,"
                            + "inttrangthai, "
                            + "intloai "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "', "
                            + "N'" + strtentinhchatvb + "', "
                            + "N'" + strkyhieu + "', "
                            + "N'" + strmota + "', "
                            + "'1', "
                            + "'" + intloai + "' "
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

        public void Tochucdoitac(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            var tableS = "tbltochucdoitac";
            var tableD = "tochucdoitac";

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
            var sqlTruncate = "delete from " + tableD;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            // mo ket noi toi qlvb2
            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Converting...");

            var count = 0;
            while ((reader.Read()))
            {
                count++;
                ReportProgress?.Invoke(countrows, count);

                var colname = "intid";
                var intid = reader.GetIntNullCheck(colname);

                colname = "intidkhoi";
                var intidkhoi = reader.GetIntNullCheck(colname);

                colname = "strmatochucdoitac";
                var strmatochucdoitac = reader.GetStringNullCheck(colname);

                colname = "strtentochucdoitac";
                var strtentochucdoitac = reader.GetStringNullCheck(colname);

                colname = "strdiachi";
                var strdiachi = reader.GetStringNullCheck(colname);

                colname = "strphone";
                var strphone = reader.GetStringNullCheck(colname);

                colname = "strfax";
                var strfax = reader.GetStringNullCheck(colname);

                colname = "stremail";
                var stremail = reader.GetStringNullCheck(colname);

                colname = "stremailvbdt";
                var stremailvbdt = reader.GetStringNullCheck(colname);

                colname = "bittrangthai";
                var bittrangthai = reader.GetBitNullCheck(colname);
                int? isvbdt = (bittrangthai == true) ?
                    (int)enumTochucdoitac.inttrangthai.IsActive : (int)enumTochucdoitac.inttrangthai.NotActive;

                colname = "bithoibao";
                var bithoibao = reader.GetBitNullCheck(colname);
                int? inthoibao = (bithoibao == true) ?
                    (int)enumTochucdoitac.inthoibao.IsActive : (int)enumTochucdoitac.inthoibao.NotActive;

                var inttrangthai = (int)enumTochucdoitac.inttrangthai.IsActive;

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "intidkhoi, "
                            + "strmatochucdoitac, "
                            + "strtentochucdoitac, "
                            + "strdiachi, "
                            + "strphone, "
                            + "strfax, "
                            + "stremail, "
                            + "stremailvbdt, "
                            + "inthoibao, "
                            + "isvbdt,"
                            + "inttrangthai "
                            + " ) ";

                    sInsert += "values("
                            + "'" + intid + "', "
                            + "'" + intidkhoi + "', "
                            + "N'" + strmatochucdoitac + "', "
                            + "N'" + strtentochucdoitac + "', "
                            + "N'" + strdiachi + "', "
                            + "N'" + strphone + "', "
                            + "N'" + strfax + "', "
                            + "N'" + stremail + "', "
                            + "N'" + stremailvbdt + "', "
                            + "'" + inthoibao + "', "
                            + "'" + isvbdt + "', "
                            + "'" + inttrangthai + "' "
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

    }
}
