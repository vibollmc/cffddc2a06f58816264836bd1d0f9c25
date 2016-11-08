using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convert
{
    public class AttachFile
    {
        Logging _logger = new Logging();

        public event ProgressBarHandler ReportProgress;

        public void AttachCongvan(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            //QLVBDatabase context = new QLVBDatabase();

            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblattachcongvan";
            string tableD = "AttachVanban";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from " + tableS + " order by intid";
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

            // them cot id1 de luu intid cua qlvb 1 
            //string sqlAddColumn = SQLQuery.AddColumn(tableD, "id1", "int");
            //Utils.RunQuery(sqlAddColumn, strconnectdich);

            // truncate table truoc khi convert
            //string sqlTruncate = "truncate table " + tableD;
            //Utils.RunQuery(sqlTruncate, strconnectdich);

            // mo ket noi toi qlvb2
            SqlConnection sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Moving ...");

            int count = 0;
            while ((reader != null) && (reader.Read()))
            {
                count++;
                ReportProgress(countrows, count);

                string colname = string.Empty;

                colname = "intid";
                int? intid = Utils.GetIntNullCheck(reader, colname);

                colname = "intloai";
                int? intloai = Utils.GetIntNullCheck(reader, colname);

                colname = "intidcongvan";
                int? intidcongvan = Utils.GetIntNullCheck(reader, colname);

                colname = "strtenfile";
                string strtenfile = Utils.GetStringNullCheck(reader, colname);

                colname = "strmota";
                string strmota = Utils.GetStringNullCheck(reader, colname);

                colname = "strngaycapnhat";
                DateTime? strngaycapnhat = Utils.GetDateTimeNullCheck(reader, colname);

                //int intidmodel = 0;
                try
                {
                    string sInsert = "";// " Set IDENTITY_INSERT " + tableD + " ON;";

                    if (intloai == 4)
                    {
                        tableD = "AttachMail";
                        intloai = (int)enumAttachMail.intloai.Vanbandendientu;
                        sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";
                        sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "intloai, "
                            + "intidmail,"
                            + "strtenfile, "
                            + "strmota, "
                            + "strngaycapnhat,"
                            + "inttrangthai ) ";

                        sInsert += "values("
                                + "'" + intid + "', "
                                + "'" + intloai + "', "
                                + "'" + intidcongvan + "', "
                                + "'" + strtenfile + "', "
                                + "'" + strmota + "', "
                                + "'" + strngaycapnhat + "', "
                                + " '1') ;";
                    }
                    else
                    {
                        tableD = "AttachVanban";
                        sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";
                        sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "intloai, "
                            + "intidvanban,"
                            + "strtenfile, "
                            + "strmota, "
                            + "strngaycapnhat,"
                            + "inttrangthai ) ";

                        sInsert += "values("
                                + "'" + intid + "', "
                                + "'" + intloai + "', "
                                + "'" + intidcongvan + "', "
                                + "'" + strtenfile + "', "
                                + "'" + strmota + "', "
                                + "'" + strngaycapnhat + "', "
                                + " '1') ;";
                    }

                    sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    Utils.RunQuery(sInsert, strconnectdich);

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    return;
                }
                //string sqlUpdate = SQLQuery.UpdateId1(tableD, intid.ToString(), intidmodel.ToString());
                //Utils.UpdateQuery(sqlUpdate, sqlConnectionDich);
            }
            _logger.Info(tableS + " -- Done");

            reader.Close();
            sqlConnectionDich.Close();
            sqlConnectionnguon.Close();

        }


        public void AttachHoso(stringconnect strconnectnguon, stringconnect strconnectdich)
        {
            //QLVBDatabase context = new QLVBDatabase();

            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string tableS = "tblattachhoso";
            string tableD = "AttachHoso";

            int soField = Utils.CountFields(tableS, strconnectnguon);
            int countrows = Utils.CountRows(tableS, strconnectnguon);

            cmd.CommandText = "select * from tblattachhoso order by intid";// where bittrangthai
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

            // them cot id1 de luu intid cua qlvb 1 
            //string sqlAddColumn = SQLQuery.AddColumn(tableD, "id1", "int");
            //Utils.RunQuery(sqlAddColumn, strconnectdich);

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

                colname = "intloai";
                int? intloai = Utils.GetIntNullCheck(reader, colname);

                colname = "intidtailieu";
                int? intidtailieu = Utils.GetIntNullCheck(reader, colname);

                colname = "intidhoso";
                int? intidhoso = Utils.GetIntNullCheck(reader, colname);

                colname = "strtenfile";
                string strtenfile = Utils.GetStringNullCheck(reader, colname);

                colname = "strmota";
                string strmota = Utils.GetStringNullCheck(reader, colname);

                colname = "strngaycapnhat";
                DateTime? strngaycapnhat = Utils.GetDateTimeNullCheck(reader, colname);

                colname = "bittrangthai"; // = 0 la file van ban den
                bool? bittrangthai = Utils.GetBitNullCheck(reader, colname);

                try
                {
                    string sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

                    sInsert += "insert into  " + tableD
                            + "(intid, "
                            + "intloai, "
                            + "intidhoso, "
                            + "intidtailieu, "
                            + "strtenfile, "
                            + "strmota, "
                            + "strngaycapnhat,"
                            + "inttrangthai ) ";

                    sInsert += "values("
                            + "'" + intid + "', "
                            + "'" + intloai + "', "
                            + "'" + intidhoso + "', "
                            + "'" + intidtailieu + "', "
                            + "'" + strtenfile + "', "
                            + "'" + strmota + "', "
                            + "'" + strngaycapnhat + "', "
                            + " '1') ;";

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
