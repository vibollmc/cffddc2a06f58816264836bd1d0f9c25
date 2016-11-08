using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace Convert
{
    public delegate void ProgressBarHandler(int max, int count);
    public partial class FormMain : Form
    {
        stringconnect strNguon = new stringconnect();
        stringconnect strDich = new stringconnect();

        List<string> allTable = new List<string>();

        public FormMain()
        {
            InitializeComponent();
            stringconnect _strNguon = GetStrconnectConfig("qlvb1");
            stringconnect _strDich = GetStrconnectConfig("QLVBDatabase");

            txtCSDLNguon.Text = _strNguon.database;
            txtSQLServerNguon.Text = _strNguon.server;
            txtLoginNguon.Text = _strNguon.username;
            txtPassNguon.Text = _strNguon.pass;

            txtCSDLDich.Text = _strDich.database;
            txtSQLServerDich.Text = _strDich.server;
            txtLoginDich.Text = _strDich.username;
            txtPassDich.Text = _strDich.pass;

            lbTable.Visible = false;

            strNguon = _strNguon;
            strDich = _strDich;

            // To report progress from the background worker we need to set this property
            //backgroundWorker1.WorkerReportsProgress = true;
            // This event will be raised on the worker thread when the worker starts
            //backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            // This event will be raised when we call ReportProgress
            //backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

        }

        private stringconnect GetStrconnectConfig(string database)
        {
            stringconnect _strNguon = new stringconnect();
            var strnguon = Utils.GetConnectionStringByName(database);
            string[] split = strnguon.Split(';');
            string strgiatri = string.Empty;
            string key = string.Empty;
            foreach (string s in split)
            {
                strgiatri = s;
                // p="password=1"
                key = "Data Source";
                if (strgiatri.Contains(key))
                {
                    int len = key.Length;
                    _strNguon.server = strgiatri.Substring(len + 1);
                }
                key = "User ID";
                if (strgiatri.Contains(key))
                {
                    int len = key.Length;
                    _strNguon.username = strgiatri.Substring(len + 1);
                }
                key = "Password";
                if (strgiatri.Contains(key))
                {
                    int len = key.Length;
                    _strNguon.pass = strgiatri.Substring(len + 1);
                }
                key = "Initial Catalog";
                if (strgiatri.Contains(key))
                {
                    int len = key.Length;
                    _strNguon.database = strgiatri.Substring(len + 1);
                }
            }
            return _strNguon;
        }


        //public ProgressBar progress = new ProgressBar();

        private void Convertbtn_Click(object sender, EventArgs e)
        {
            // xoa log file truoc khi convert
            System.IO.File.Delete(@"LogError.log");
            System.IO.File.Delete(@"LogUser.log");

            this.Enabled = false;

            GetAllTable();

            int counttable = System.Convert.ToInt32(_CountTable());
            SetProgressbarTong(counttable);

            lbTable.Visible = true;
            foreach (var table in allTable)
            {
                RunProgressbarTong();

                if (table == "tblattachcongvan")
                {
                    lbTable.Text = "Convert tblattachcongvan ...";
                    AttachFile convert = new AttachFile();
                    convert.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    convert.AttachCongvan(strNguon, strDich);
                }
                if (table == "tblattachflow")
                {
                    // khong co
                }
                if (table == "tblattachhoso")
                {
                    lbTable.Text = "Convert tblattachhoso ...";
                    AttachFile convert = new AttachFile();
                    convert.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    convert.AttachHoso(strNguon, strDich);
                }
                if (table == "tblbaocao")
                {
                    // giu nguyen theo qlvb 2
                }
                if (table == "tblbaocaohoatdongcuachuyenvien")
                {
                    // khong co
                }
                if (table == "tblcanbo")
                {
                    lbTable.Text = "Convert tblcanbo ...";
                    UserProfile convert = new UserProfile();
                    convert.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    convert.Canbo(strNguon, strDich);
                }
                if (table == "tblcanbodonvi")
                {
                    // nhap chung voi table canbo
                }
                if (table == "tblcanbonhom")
                {
                    // khong co
                }
                if (table == "tblcanbonhomquyen")
                {
                    // nhap chung voi table canbo
                }
                if (table == "tblcanboquyen")
                {
                    // bo 
                }
                if (table == "tblcapnhatgiaoban")
                {
                    // khong co
                }
                if (table == "tblcategorycongvan")
                {
                    // giu nguyen theo qlvb2
                }
                if (table == "tblcauhinh")
                {
                    lbTable.Text = "Convert tblcauhinh...";
                    Danhmuc dm = new Danhmuc();
                    dm.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    dm.Cauhinh(strNguon, strDich);
                }
                if (table == "tblchucdanh")
                {
                    lbTable.Text = "Convert tblchucdanh ...";
                    Danhmuc dm = new Danhmuc();
                    dm.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    dm.Chucdanh(strNguon, strDich);
                }
                if (table == "tblchuthichlich")
                {
                    // khong co
                }
                if (table == "tblchuyenphathanh")
                {
                    // khong co
                }
                if (table == "tblcongvanden")
                {
                    lbTable.Text = "Convert tblcongvanden ...";
                    Vanban vb = new Vanban();
                    vb.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    vb.Vanbanden(strNguon, strDich);
                }
                if (table == "tblcongvandencanbo")
                {
                    lbTable.Text = "Convert tblcongvandencanbo ...";
                    Vanban vb = new Vanban();
                    vb.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    vb.VanbandenCanbo(strNguon, strDich);
                }
                if (table == "tblcongvanphathanh")
                {
                    lbTable.Text = "Convert tblcongvanphathanh ...";
                    Vanban vb = new Vanban();
                    vb.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    vb.Vanbandi(strNguon, strDich);
                }
                if (table == "tblcongvanphathanhcanbo")
                {
                    lbTable.Text = "Convert tblcongvanphathanhcanbo ...";
                    Vanban vb = new Vanban();
                    vb.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    vb.VanbandiCanbo(strNguon, strDich);
                }
                if (table == "tbldangkylich")
                {
                    // khong co
                }
                if (table == "tbldangvblq")
                {
                    // giu nguyen theo qlvb2
                }
                if (table == "tbldiachiluutru")
                {
                    // khong co
                }
                if (table == "tbldiachiluutrutree")
                {
                    lbTable.Text = "Convert tbldiachiluutrutree ...";
                    Danhmuc dm = new Danhmuc();
                    dm.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    dm.Diachiluutru(strNguon, strDich);
                }
                if (table == "tbldoituongxuly")
                {
                    lbTable.Text = "Convert tbldoituongxuly ...";
                    Hoso hs = new Hoso();
                    hs.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    hs.Doituongxuly(strNguon, strDich);
                }
                if (table == "tbldonvitructhuoc")
                {
                    lbTable.Text = "Convert tbldonvitructhuoc ...";
                    UserProfile convert = new UserProfile();
                    convert.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    convert.Donvitructhuoc(strNguon, strDich);
                }
                if (table == "tblfiledname")
                {
                    // cac column add them 
                }
                //........................
                //........................
                if (table == "tblguicongvan")
                {
                    lbTable.Text = "Convert tblguicongvan ...";
                    Vanban vb = new Vanban();
                    vb.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    vb.Guicongvan(strNguon, strDich);
                }
                if (table == "tblhoibaovanban")
                {
                    lbTable.Text = "Convert tblhoibaovanban ...";
                    Vanban vb = new Vanban();
                    vb.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    vb.Hoibaovanban(strNguon, strDich);
                }
                if (table == "tblhosocongviec")
                {
                    lbTable.Text = "Convert tblhosocongviec ...";
                    Hoso hs = new Hoso();
                    hs.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    hs.Hosocongviec(strNguon, strDich);
                }
                if (table == "tblhosohoibao")
                {
                    // qlvb2 khong co
                }
                if (table == "tblhosovanban")
                {
                    lbTable.Text = "Convert tblhosovanban ...";
                    Hoso hs = new Hoso();
                    hs.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    hs.Hosovanban(strNguon, strDich);
                }
                if (table == "tblhosovanbanlq")
                {
                    lbTable.Text = "Convert tblhosovanbanlq ...";

                    // table hosovanbanlienquan con khac voi tblhosovanbanlq

                    //Hoso hs = new Hoso();
                    //hs.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    //hs.Hosovanban(strNguon, strDich);
                }
                if (table == "tblhosovbduthao")
                {
                    // qlvb2 chua co
                }
                if (table == "tblhosoykienxuly")
                {
                    lbTable.Text = "Convert tblhosoykienxuly ...";
                    Hoso hs = new Hoso();
                    hs.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    hs.Hosoykienxuly(strNguon, strDich);
                }
                if (table == "tblketquagiaoban")
                {
                    // qlvb2 chua co
                }
                if (table == "tblkhoiphathanh")
                {
                    lbTable.Text = "Convert tblkhoiphathanh...";
                    Danhmuc dm = new Danhmuc();
                    dm.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    dm.Khoiphathanh(strNguon, strDich);
                }
                if (table == "tbllichlamviec")
                {
                    // qlvb2 chua co
                }
                if (table == "tbllinhvuc")
                {
                    lbTable.Text = "Convert tbllinhvuc...";
                    Danhmuc dm = new Danhmuc();
                    dm.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    dm.Linhvuc(strNguon, strDich);
                }
                if (table == "tbllinkperss")
                {
                    // qlvb2 chua co
                    // dung de lam gi?
                }
                //.......tbl modulegroups, module, motatruong 
                // giu nguyen theo qlvb2
                if (table == "tblmucquantrong")
                {
                    lbTable.Text = "Convert tblmucquantrong ...";
                    // chua co
                    //Danhmuc dm = new Danhmuc();
                    //dm.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    //dm.Linhvuc(strNguon, strDich);
                }
                //........................
                // giu nguyen theo qlvb2
                if (table == "tblnhomquyen")
                {
                    lbTable.Text = "Convert tblnhomquyen ...";
                    UserProfile convert = new UserProfile();
                    convert.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    convert.Nhomquyen(strNguon, strDich);
                }
                if (table == "tblphieutrinh")
                {
                    lbTable.Text = "Convert tblphieutrinh ...";
                    Hoso hs = new Hoso();
                    hs.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    hs.Phieutrinh(strNguon, strDich);
                }
                if (table == "tblplcongvanden")
                {
                    lbTable.Text = "Convert tblplcongvanden...";
                    Danhmuc dm = new Danhmuc();
                    dm.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    dm.Phanloaivanbanden(strNguon, strDich);
                }
                if (table == "tblplcongvanphathanh")
                {
                    lbTable.Text = "Convert tblplcongvanphathanh...";
                    Danhmuc dm = new Danhmuc();
                    dm.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    dm.Phanloaivanbandi(strNguon, strDich);
                }
                if (table == "tblplvanbanduthao")
                {
                    lbTable.Text = "Convert tblplvanbanduthao...";
                    Danhmuc dm = new Danhmuc();
                    dm.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    dm.Phanloaivanbanduthao(strNguon, strDich);
                }
                //.....tblprintnoidunghoso, tblquyen, tblquyennhomquyen,
                //........................
                if (table == "tblsodoxulyvanban")
                {
                    // chua co ???
                }
                if (table == "tblsovanban")
                {
                    lbTable.Text = "Convert tblsovanban...";
                    Danhmuc dm = new Danhmuc();
                    dm.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    dm.SoVanban(strNguon, strDich);
                }
                if (table == "tblthuocloai")
                {
                    // chua co ???
                }
                //.....tblthvb, tblthvb1,....
                //........................
                if (table == "tbltinhchatvanban")
                {
                    lbTable.Text = "Convert tbltinhchatvanban...";
                    Danhmuc dm = new Danhmuc();
                    dm.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    dm.Tinhchatvanban(strNguon, strDich);
                }
                if (table == "tbltochucdoitac")
                {
                    lbTable.Text = "Convert tbltochucdoitac...";
                    Danhmuc dm = new Danhmuc();
                    dm.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    dm.Tochucdoitac(strNguon, strDich);
                }
                //.....tbltonghopgiaoban, tbltotrinh,tbltruynhaphoso....
                //........................
                if (table == "tbluquyen")
                {
                    lbTable.Text = "Convert tbluquyen ...";
                    UserProfile convert = new UserProfile();
                    convert.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    convert.Uyquyen(strNguon, strDich);
                }
                if (table == "tblvaitrovblq")
                {
                    // chua co ???
                }
                if (table == "tblvanbandenmail")
                {
                    lbTable.Text = "Convert tblvanbandenmail ...";
                    Vanban convert = new Vanban();
                    convert.ReportProgress += new ProgressBarHandler(RunProgressbarChitiet);
                    convert.Vanbandenmail(strNguon, strDich);
                }



            }

            lbTable.Text = "RebuildIndex...";
            FixError fix = new FixError();
            fix.BuildIndex(strDich);

            this.Enabled = true;
            lbTable.Text = "Hoàn thành";
        }



        private void SetProgressbarChitiet(int countrows)
        {
            //bắt buộc nấc trong progress bar xuất phát từ số 0 sử dụng thuộc tính Minimum
            progressBarChitiet.Minimum = 0;
            //và chỉ cho phép nấc này chạy đến giá trị tối đa là 2000 sử dụng thuộc tính Maximum
            progressBarChitiet.Maximum = countrows; //; //reader.FieldCount;
            //khởi tạo giá trị ban đầu cho progress bar sử thuộc tính Value
            progressBarChitiet.Value = 1;
            //khoảng tăng giữa các nấc trong ProgressBar
            progressBarChitiet.Step = 1;
            //bắt đầu chạy
            //progressBarChitiet.PerformStep();
            //int percent = (progressBarChitiet.Value / progressBarChitiet.Maximum) * 100;

            //progressBarChitiet.Refresh();
            //int percent = (int)(((double)progressBarChitiet.Value / (double)progressBarChitiet.Maximum) * 100);
            //progressBarChitiet.CreateGraphics().DrawString(percent.ToString() + "%", new Font("Arial", (float)8.25, FontStyle.Regular), Brushes.Black, new PointF(progressBarChitiet.Width / 2 - 10, progressBarChitiet.Height / 2 - 7));

        }

        public void RunProgressbarChitiet(int max, int count)
        {
            progressBarChitiet.Minimum = 0;
            progressBarChitiet.Maximum = max;
            progressBarChitiet.Value = count;

            progressBarChitiet.PerformStep();
            int percent = (int)(((double)progressBarChitiet.Value / (double)progressBarChitiet.Maximum) * 100);
            progressBarChitiet.CreateGraphics().DrawString(percent.ToString() + "%", new Font("Arial", (float)12, FontStyle.Bold), Brushes.Red, new PointF(progressBarChitiet.Width / 2 - 10, progressBarChitiet.Height / 2 - 7));
            progressBarChitiet.Refresh();
            Application.DoEvents();
        }

        private void SetProgressbarTong(int counttable)
        {
            progressBarTong.Minimum = 0;
            progressBarTong.Maximum = counttable;
            progressBarTong.Value = 1;
            progressBarTong.Step = 1;
        }

        private void RunProgressbarTong()
        {
            progressBarTong.PerformStep();
            int percent = (int)(((double)progressBarTong.Value / (double)progressBarTong.Maximum) * 100);
            progressBarTong.CreateGraphics().DrawString(percent.ToString() + "%", new Font("Arial", (float)12, FontStyle.Bold), Brushes.Red, new PointF(progressBarTong.Width / 2 - 10, progressBarTong.Height / 2 - 7));
            progressBarTong.Refresh();
            Application.DoEvents();
        }

        private void GetAllTable()
        {
            SqlConnection sqlConnectionnguon = Utils.GetSqlConnection(strNguon); //new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = SQLQuery.GetAllTableInDatabase;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                string colname = string.Empty;

                colname = "table_name";
                string tablename = Utils.GetStringNullCheck(reader, colname).ToLower();
                allTable.Add(tablename);
            }

            reader.Close();
            sqlConnectionnguon.Close();
        }
        private string _CountTable()
        {
            SqlConnection sqlConnection1 = Utils.GetSqlConnection(strNguon);
            SqlCommand cmd = new SqlCommand();
            Object returnValue;

            //cmd.CommandText = "SELECT COUNT(*) FROM Customers";
            cmd.CommandText = SQLQuery.countTableInDatabase;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            returnValue = cmd.ExecuteScalar();

            sqlConnection1.Close();
            return returnValue.ToString();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    FixError fix = new FixError();
        //    fix.BuildIndex(strDich);
        //}



    }
}
