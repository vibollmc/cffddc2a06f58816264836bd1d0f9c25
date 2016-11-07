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
        readonly Logging _logger = new Logging();

        public event ProgressBarHandler ReportProgress;

        public void AttachCongvan(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            //QLVBDatabase context = new QLVBDatabase();

            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblattachcongvan";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

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

            var reader = cmd.ExecuteReader();

            // them cot id1 de luu intid cua qlvb 1 
            //string sqlAddColumn = SQLQuery.AddColumn(tableD, "id1", "int");
            //Utils.RunQuery(sqlAddColumn, strconnectdich);

            // truncate table truoc khi convert
            //string sqlTruncate = "truncate table " + tableD;
            //Utils.RunQuery(sqlTruncate, strconnectdich);

            // mo ket noi toi qlvb2
            var sqlConnectionDich = Utils.GetSqlConnection(strconnectdich);
            sqlConnectionDich.Open();

            _logger.Info(tableS + " -- Moving ...");

            var count = 0;
            while (reader.Read())
            {
                count++;
                ReportProgress?.Invoke(countrows, count);

                var colname = "intid";
                var intid = reader.GetIntNullCheck(colname);

                colname = "intloai";
                var intloai = reader.GetIntNullCheck(colname);

                colname = "intidcongvan";
                var intidcongvan = reader.GetIntNullCheck(colname);

                colname = "strtenfile";
                var strtenfile = reader.GetStringNullCheck(colname);

                colname = "strmota";
                var strmota = reader.GetStringNullCheck(colname);

                colname = "strngaycapnhat";
                var strngaycapnhat = reader.GetDateTimeNullCheck(colname);

                //int intidmodel = 0;
                try
                {
                    var sInsert = "";// " Set IDENTITY_INSERT " + tableD + " ON;";

                    string tableD;
                    var arrayParam = new List<SqlParameter>();
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
                                + "@id, "
                                + "@loai, "
                                + "@idcongvan, "
                                + "@tenfile, "
                                + "@mota, "
                                + "@ngaycapnhat, "
                                + "1);";

                        arrayParam.Add(new SqlParameter("@id", intid));
                        arrayParam.Add(new SqlParameter("@loai", intloai));
                        arrayParam.Add(new SqlParameter("@idcongvan", intidcongvan));
                        arrayParam.Add(new SqlParameter("@tenfile", strtenfile));
                        arrayParam.Add(new SqlParameter("@mota", strmota));
                        arrayParam.Add(new SqlParameter("@ngaycapnhat", strngaycapnhat));

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
                                + "@id, "
                                + "@loai, "
                                + "@idcongvan, "
                                + "@tenfile, "
                                + "@mota, "
                                + "@ngaycapnhat, "
                                + "1);";

                        arrayParam.Add(new SqlParameter("@id", intid));
                        arrayParam.Add(new SqlParameter("@loai", intloai));
                        arrayParam.Add(new SqlParameter("@idcongvan", intidcongvan));
                        arrayParam.Add(new SqlParameter("@tenfile", strtenfile));
                        arrayParam.Add(new SqlParameter("@mota", strmota));
                        arrayParam.Add(new SqlParameter("@ngaycapnhat", strngaycapnhat));
                    }

                    sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    Utils.RunQuery(sInsert, strconnectdich, arrayParam.ToArray());

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


        public void AttachHoso(Stringconnect strconnectnguon, Stringconnect strconnectdich)
        {
            //QLVBDatabase context = new QLVBDatabase();

            var sqlConnectionnguon = Utils.GetSqlConnection(strconnectnguon);
            var cmd = new SqlCommand();

            const string tableS = "tblattachhoso";
            const string tableD = "AttachHoso";

            var soField = Utils.CountFields(tableS, strconnectnguon);
            var countrows = Utils.CountRows(tableS, strconnectnguon);

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

            var reader = cmd.ExecuteReader();

            // them cot id1 de luu intid cua qlvb 1 
            //string sqlAddColumn = SQLQuery.AddColumn(tableD, "id1", "int");
            //Utils.RunQuery(sqlAddColumn, strconnectdich);

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

                colname = "intloai";
                var intloai = reader.GetIntNullCheck(colname);

                colname = "intidtailieu";
                var intidtailieu = reader.GetIntNullCheck(colname);

                colname = "intidhoso";
                var intidhoso = reader.GetIntNullCheck(colname);

                colname = "strtenfile";
                var strtenfile = reader.GetStringNullCheck(colname);

                colname = "strmota";
                var strmota = reader.GetStringNullCheck(colname);

                colname = "strngaycapnhat";
                var strngaycapnhat = reader.GetDateTimeNullCheck(colname);

                colname = "bittrangthai"; // = 0 la file van ban den
                var bittrangthai = reader.GetBitNullCheck(colname);

                try
                {
                    var sInsert = " Set IDENTITY_INSERT " + tableD + " ON;";

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
                            + "@id, "
                            + "@loai, "
                            + "@idhoso, "
                            + "@idtailieu, "
                            + "@tenfile, "
                            + "@mota, "
                            + "@ngaycapnhat, "
                            + " 1);";

                    var lstParams = new List<SqlParameter>
                    {
                        new SqlParameter("@id", intid),
                        new SqlParameter("@loai", intloai),
                        new SqlParameter("@idhoso", intidhoso),
                        new SqlParameter("@idtailieu", intidtailieu),
                        new SqlParameter("@tenfile", strtenfile),
                        new SqlParameter("@mota", strmota),
                        new SqlParameter("@ngaycapnhat", strngaycapnhat)
                    };

                    sInsert += " Set IDENTITY_INSERT " + tableD + " OFF;";

                    Utils.RunQuery(sInsert, strconnectdich, lstParams.ToArray());

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
