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
        Logging _logger = new Logging();

        public event ProgressBarHandler ReportProgress;

        public void SoVanban(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            //EntityConnection encon = Utils.GetEntityConnection(strconnectdich);
            //string strencon = Utils.GetStrConnectCSDL(strconnectdich);

            //QLVBDatabase context = new QLVBDatabase(); //new QLVBDatabase(strencon);

            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon); //new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblsovanban";
            string tableD = "sovanban";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

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

            reader = cmd.ExecuteReader();

            // truncate table truoc khi convert
            string sqlTruncate = "truncate table " + tableD;
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
                int intid = (int)Utils.GetIntNullCheck(reader, colname);

                colname = "strten";
                string strten = Utils.GetStringNullCheck(reader, colname);

                colname = "strkyhieu";
                string strkyhieu = Utils.GetStringNullCheck(reader, colname);

                colname = "strghichu";
                string strghichu = Utils.GetStringNullCheck(reader, colname);

                colname = "bitloai";
                int intloai = (int)Utils.GetSmallIntNullCheck(reader, colname);

                colname = "intidkhoiph";
                int? intidkhoiph = Utils.GetIntNullCheck(reader, colname);

                colname = "intidloaivb";
                int? intidloaivb = Utils.GetIntNullCheck(reader, colname);

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
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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

        public void Cauhinh(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblcauhinh";
            //string tableD = "Config";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

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

            reader = cmd.ExecuteReader();

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

                colname = "domainname";
                string domainname = Utils.GetStringNullCheck(reader, colname);

                colname = "smtpport";
                string smtpport = Utils.GetStringNullCheck(reader, colname);

                colname = "domainip";
                string domainip = Utils.GetStringNullCheck(reader, colname);

                colname = "pop3port";
                string pop3port = Utils.GetStringNullCheck(reader, colname);

                colname = "usernamemail";
                string usernamemail = Utils.GetStringNullCheck(reader, colname);

                colname = "passwordmail";
                string passwordmail = Utils.GetStringNullCheck(reader, colname);

                colname = "thoigian";
                int? thoigian = Utils.GetIntNullCheck(reader, colname);

                try
                {
                    string sqlUpdate = "update config set strgiatri='" + thoigian + "' where strthamso='SoNgayHienThi' ;";

                    sqlUpdate += "update config set strgiatri='" + domainname + "' where strthamso='SMTPServer' ;";
                    sqlUpdate += "update config set strgiatri='" + smtpport + "' where strthamso='SMTPPort' ;";

                    sqlUpdate += "update config set strgiatri='" + domainip + "' where strthamso='POP3Server' ;";
                    sqlUpdate += "update config set strgiatri='" + pop3port + "' where strthamso='POP3Port' ;";

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

        public void Chucdanh(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblchucdanh";
            string tableD = "chucdanh";

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
            string sqlTruncate = "truncate table " + tableD;
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

                colname = "strmachucdanh";
                string strmachucdanh = Utils.GetStringNullCheck(reader, colname);

                colname = "strtenchucdanh";
                string strtenchucdanh = Utils.GetStringNullCheck(reader, colname);

                colname = "strghichu";
                string strghichu = Utils.GetStringNullCheck(reader, colname);

                colname = "intloai";
                int? intloai = Utils.GetIntNullCheck(reader, colname);

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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

        public void Diachiluutru(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tbldiachiluutrutree";
            string tableD = "diachiluutru";

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
            string sqlTruncate = "truncate table " + tableD;
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

                colname = "strparent";
                string strparent = Utils.GetStringNullCheck(reader, colname);

                colname = "strname";
                string strname = Utils.GetStringNullCheck(reader, colname);

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
                        string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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

        public void Khoiphathanh(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblkhoiphathanh";
            string tableD = "khoiphathanh";

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

                colname = "strtenkhoi";
                string strtenkhoi = Utils.GetStringNullCheck(reader, colname);

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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

        public void Linhvuc(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tbllinhvuc";
            string tableD = "linhvuc";

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

                colname = "strtenlinhvuc";
                string strtenlinhvuc = Utils.GetStringNullCheck(reader, colname);

                colname = "strkyhieu";
                string strkyhieu = Utils.GetStringNullCheck(reader, colname);

                colname = "strghichu";
                string strghichu = Utils.GetStringNullCheck(reader, colname);

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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
        public void Mucquantrong(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblmucquantrong";
            string tableD = "linhvuc";

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

                colname = "strtenlinhvuc";
                string strtenlinhvuc = Utils.GetStringNullCheck(reader, colname);

                colname = "strkyhieu";
                string strkyhieu = Utils.GetStringNullCheck(reader, colname);

                colname = "strghichu";
                string strghichu = Utils.GetStringNullCheck(reader, colname);

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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

        public void Phanloaivanbanden(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblplcongvanden";
            string tableD = "phanloaivanban";

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
            string sqlTruncate = "delete from " + tableD + " where intloai=" + (int)enumPhanloaiVanban.intloai.vanbanden;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            // them cot id1 de luu intid cua qlvb 1 
            string sqlAddColumn = SQLQuery.AddColumn(tableD, "id1", "int");
            Utils.RunQuery(sqlAddColumn, strconnectdich);

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
                int? id1 = Utils.GetIntNullCheck(reader, colname);

                colname = "strmacongvan";
                string strmacongvan = Utils.GetStringNullCheck(reader, colname);

                colname = "strtenhinhthuc";
                string strtenhinhthuc = Utils.GetStringNullCheck(reader, colname);

                colname = "strghichu";
                string strghichu = Utils.GetStringNullCheck(reader, colname);

                colname = "bitmacdinh";
                int? intmacdinh = Utils.GetIntNullCheck(reader, colname);

                int intloai = (int)enumPhanloaiVanban.intloai.vanbanden;
                try
                {
                    string sInsert = "";// " Set IDENTITY_INSERT " + tableD + " ON;";

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
                    SqlDataReader reader11 = cmd.ExecuteReader();
                    int idloaivb = 0;
                    while ((reader11 != null) && (reader11.Read()))
                    {
                        colname = string.Empty;
                        colname = "intid";
                        idloaivb = (int)Utils.GetIntNullCheck(reader11, colname);
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

                        SqlDataReader reader3 = cmd.ExecuteReader();
                        while ((reader3 != null) && (reader3.Read()))
                        {
                            colname = string.Empty;
                            colname = "intid";
                            int? intidmotatruong = Utils.GetIntNullCheck(reader3, colname);
                            colname = "strten";
                            string strten = Utils.GetStringNullCheck_Unicode(reader3, colname);
                            colname = "IsRequire";
                            bool? IsRequire = Utils.GetBitNullCheck(reader3, colname);
                            colname = "intorder";
                            int? intorder = Utils.GetIntNullCheck(reader3, colname);

                            string tableTruong = "phanloaitruong";

                            // update id cua phan loai van banden
                            string sPhanloaitruong = "";// " Set IDENTITY_INSERT " + tableTruong + " ON;";

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
                                    + "'" + IsRequire + "', "
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

            SqlDataReader reader2 = cmd.ExecuteReader();
            while ((reader2 != null) && (reader2.Read()))
            {
                string colname = string.Empty;
                colname = "intid";
                int? intid = Utils.GetIntNullCheck(reader2, colname);
                colname = "id1";
                int? id1 = Utils.GetIntNullCheck(reader2, colname);
                // update id cua phan loai van banden
                string sUpdate = "update vanbanden set intidphanloaivanbanden='" + intid +
                        "' where intidphanloaivanbanden='" + id1 + "'";
                Utils.RunQuery(sUpdate, strconnectdich);
            }

            _logger.Info(tableS + " -- Done");


            reader2.Close();
            reader.Close();
            sqlConnectionDich.Close();
            sqlConnectionnguon.Close();
        }

        public void Phanloaivanbandi(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblplcongvanphathanh";
            string tableD = "phanloaivanban";

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
            string sqlTruncate = "delete from " + tableD + " where intloai=" + (int)enumPhanloaiVanban.intloai.vanbandi;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            // them cot id1 de luu intid cua qlvb 1 
            string sqlAddColumn = SQLQuery.AddColumn(tableD, "id1", "int");
            Utils.RunQuery(sqlAddColumn, strconnectdich);

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

                colname = "strmacongvan";
                string strmacongvan = Utils.GetStringNullCheck(reader, colname);

                colname = "strkyhieu";
                string strkyhieu = Utils.GetStringNullCheck(reader, colname);

                colname = "strtenhinhthuc";
                string strtenhinhthuc = Utils.GetStringNullCheck(reader, colname);

                colname = "strghichu";
                string strghichu = Utils.GetStringNullCheck(reader, colname);

                colname = "bitmacdinh";
                int? intmacdinh = Utils.GetIntNullCheck(reader, colname);

                int intloai = (int)enumPhanloaiVanban.intloai.vanbandi;
                try
                {
                    string sInsert = "";// " Set IDENTITY_INSERT " + tableD + " ON;";

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
                    SqlDataReader reader11 = cmd.ExecuteReader();
                    int idloaivb = 0;
                    while ((reader11 != null) && (reader11.Read()))
                    {
                        colname = string.Empty;
                        colname = "intid";
                        idloaivb = (int)Utils.GetIntNullCheck(reader11, colname);
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

                        SqlDataReader reader3 = cmd.ExecuteReader();
                        while ((reader3 != null) && (reader3.Read()))
                        {
                            colname = string.Empty;
                            colname = "intid";
                            int? intidmotatruong = Utils.GetIntNullCheck(reader3, colname);
                            colname = "strten";
                            string strten = Utils.GetStringNullCheck_Unicode(reader3, colname);
                            colname = "IsRequire";
                            bool? IsRequire = Utils.GetBitNullCheck(reader3, colname);
                            colname = "intorder";
                            int? intorder = Utils.GetIntNullCheck(reader3, colname);

                            string tableTruong = "phanloaitruong";

                            // update id cua phan loai van banden
                            string sPhanloaitruong = "";// " Set IDENTITY_INSERT " + tableTruong + " ON;";

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
                                    + "'" + IsRequire + "', "
                                    + "N'" + strten + "', "
                                    + "'" + (int)enumPhanloaiTruong.intloaitruong.Default + "'"
                                    + " ) ;";

                            //sPhanloaitruong += " Set IDENTITY_INSERT " + tableTruong + " OFF;";

                            Utils.RunQuery(sPhanloaitruong, strconnectdich);
                        }
                        reader3.Close();

                        // fix loi bắt buộc phải có loại văn bản khi thêm mới vb đến/đi
                        string sqlLoaivanban = " update phanloaitruong set isrequire=1 , isdisplay=1 where strmota=N'Loại văn bản'";
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

            SqlDataReader reader2 = cmd.ExecuteReader();
            while ((reader2 != null) && (reader2.Read()))
            {
                string colname = string.Empty;
                colname = "intid";
                int? intid = Utils.GetIntNullCheck(reader2, colname);
                colname = "id1";
                int? id1 = Utils.GetIntNullCheck(reader2, colname);
                // update id cua phan loai van banden
                string sUpdate = "update vanbandi set intidphanloaivanbandi='" + intid +
                        "' where intidphanloaivanbandi='" + id1 + "'";
                Utils.RunQuery(sUpdate, strconnectdich);
            }

            _logger.Info(tableS + " -- Done");

            reader2.Close();
            reader.Close();
            sqlConnectionDich.Close();
            sqlConnectionnguon.Close();
        }

        public void Phanloaivanbanduthao(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblplvanbanduthao";
            string tableD = "phanloaivanban";

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
            string sqlTruncate = "delete from " + tableD + " where intloai=" + (int)enumPhanloaiVanban.intloai.vanbanduthao;
            Utils.RunQuery(sqlTruncate, strconnectdich);

            // them cot id1 de luu intid cua qlvb 1 
            string sqlAddColumn = SQLQuery.AddColumn(tableD, "id1", "int");
            Utils.RunQuery(sqlAddColumn, strconnectdich);

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

                colname = "strmacongvan";
                string strmacongvan = Utils.GetStringNullCheck(reader, colname);

                colname = "strtenhinhthuc";
                string strtenhinhthuc = Utils.GetStringNullCheck(reader, colname);

                colname = "strghichu";
                string strghichu = Utils.GetStringNullCheck(reader, colname);

                colname = "bitmacdinh";
                int? intmacdinh = Utils.GetIntNullCheck(reader, colname);

                int intloai = (int)enumPhanloaiVanban.intloai.vanbanduthao;
                try
                {
                    string sInsert = "";// " Set IDENTITY_INSERT " + tableD + " ON;";

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

        public void Tinhchatvanban(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tbltinhchatvanban";
            string tableD = "tinhchatvanban";

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

                colname = "strtentinhchatvb";
                string strtentinhchatvb = Utils.GetStringNullCheck(reader, colname);

                colname = "strkyhieu";
                string strkyhieu = Utils.GetStringNullCheck(reader, colname);

                colname = "strmota";
                string strmota = Utils.GetStringNullCheck(reader, colname);

                colname = "bitloai";
                bool? bitloai = Utils.GetBitNullCheck(reader, colname);
                int? intloai = (bitloai == true) ?
                    (int)enumTinhchatvanban.intloai.Mat : (int)enumTinhchatvanban.intloai.Khan;

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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

        public void Tochucdoitac(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tbltochucdoitac";
            string tableD = "tochucdoitac";

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

                colname = "intidkhoi";
                int? intidkhoi = Utils.GetIntNullCheck(reader, colname);

                colname = "strmatochucdoitac";
                string strmatochucdoitac = Utils.GetStringNullCheck(reader, colname);

                colname = "strtentochucdoitac";
                string strtentochucdoitac = Utils.GetStringNullCheck(reader, colname);

                colname = "strdiachi";
                string strdiachi = Utils.GetStringNullCheck(reader, colname);

                colname = "strphone";
                string strphone = Utils.GetStringNullCheck(reader, colname);

                colname = "strfax";
                string strfax = Utils.GetStringNullCheck(reader, colname);

                colname = "stremail";
                string stremail = Utils.GetStringNullCheck(reader, colname);

                colname = "stremailvbdt";
                string stremailvbdt = Utils.GetStringNullCheck(reader, colname);

                colname = "bittrangthai";
                bool? bittrangthai = Utils.GetBitNullCheck(reader, colname);
                int? isvbdt = (bittrangthai == true) ?
                    (int)enumTochucdoitac.inttrangthai.IsActive : (int)enumTochucdoitac.inttrangthai.NotActive;

                colname = "bithoibao";
                bool? bithoibao = Utils.GetBitNullCheck(reader, colname);
                int? inthoibao = (bithoibao == true) ?
                    (int)enumTochucdoitac.inthoibao.IsActive : (int)enumTochucdoitac.inthoibao.NotActive;

                int inttrangthai = (int)enumTochucdoitac.inttrangthai.IsActive;

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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
