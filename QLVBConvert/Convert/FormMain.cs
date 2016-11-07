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
        readonly Stringconnect _strNguon;
        readonly Stringconnect _strDich;

        readonly List<string> _allTable = new List<string>();

        public FormMain()
        {
            InitializeComponent();
            _strNguon = GetStrconnectConfig("qlvb1");
            _strDich = GetStrconnectConfig("QLVBDatabase");

            txtCSDLNguon.Text = _strNguon.Database;
            txtSQLServerNguon.Text = _strNguon.Server;
            txtLoginNguon.Text = _strNguon.Username;
            txtPassNguon.Text = _strNguon.Pass;

            txtCSDLDich.Text = _strDich.Database;
            txtSQLServerDich.Text = _strDich.Server;
            txtLoginDich.Text = _strDich.Username;
            txtPassDich.Text = _strDich.Pass;

            lbTable.Visible = false;
            // To report progress from the background worker we need to set this property
            //backgroundWorker1.WorkerReportsProgress = true;
            // This event will be raised on the worker thread when the worker starts
            //backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            // This event will be raised when we call ReportProgress
            //backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

        }

        private static Stringconnect GetStrconnectConfig(string database)
        {
            var strNguon = new Stringconnect();
            var strnguon = Utils.GetConnectionStringByName(database);
            var split = strnguon.Split(';');
            foreach (var s in split)
            {
                var strgiatri = s;
                // p="password=1"
                var key = "Data Source";
                if (strgiatri.Contains(key))
                {
                    var len = key.Length;
                    strNguon.Server = strgiatri.Substring(len + 1);
                }
                key = "User ID";
                if (strgiatri.Contains(key))
                {
                    var len = key.Length;
                    strNguon.Username = strgiatri.Substring(len + 1);
                }
                key = "Password";
                if (strgiatri.Contains(key))
                {
                    var len = key.Length;
                    strNguon.Pass = strgiatri.Substring(len + 1);
                }
                key = "Initial Catalog";
                if (strgiatri.Contains(key))
                {
                    var len = key.Length;
                    strNguon.Database = strgiatri.Substring(len + 1);
                }
            }
            return strNguon;
        }


        //public ProgressBar progress = new ProgressBar();

        private void Convertbtn_Click(object sender, EventArgs e)
        {
            // xoa log file truoc khi convert
            System.IO.File.Delete(@"LogError.log");
            System.IO.File.Delete(@"LogUser.log");

            this.Enabled = false;

            GetAllTable();

            var counttable = System.Convert.ToInt32(_CountTable());
            SetProgressbarTong(counttable);

            lbTable.Visible = true;
            foreach (var table in _allTable)
            {
                RunProgressbarTong();

                if (table == "tblattachcongvan")
                {
                    lbTable.Text = "Convert tblattachcongvan ...";
                    var convert = new AttachFile();
                    convert.ReportProgress += RunProgressbarChitiet;
                    convert.AttachCongvan(_strNguon, _strDich);
                }
                if (table == "tblattachflow")
                {
                    // khong co
                }
                if (table == "tblattachhoso")
                {
                    lbTable.Text = "Convert tblattachhoso ...";
                    var convert = new AttachFile();
                    convert.ReportProgress += RunProgressbarChitiet;
                    convert.AttachHoso(_strNguon, _strDich);
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
                    var convert = new UserProfile();
                    convert.ReportProgress += RunProgressbarChitiet;
                    convert.Canbo(_strNguon, _strDich);
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
                    var dm = new Danhmuc();
                    dm.ReportProgress += RunProgressbarChitiet;
                    dm.Cauhinh(_strNguon, _strDich);
                }
                if (table == "tblchucdanh")
                {
                    lbTable.Text = "Convert tblchucdanh ...";
                    var dm = new Danhmuc();
                    dm.ReportProgress += RunProgressbarChitiet;
                    dm.Chucdanh(_strNguon, _strDich);
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
                    var vb = new Vanban();
                    vb.ReportProgress += RunProgressbarChitiet;
                    vb.Vanbanden(_strNguon, _strDich);
                }
                if (table == "tblcongvandencanbo")
                {
                    lbTable.Text = "Convert tblcongvandencanbo ...";
                    var vb = new Vanban();
                    vb.ReportProgress += RunProgressbarChitiet;
                    vb.VanbandenCanbo(_strNguon, _strDich);
                }
                if (table == "tblcongvanphathanh")
                {
                    lbTable.Text = "Convert tblcongvanphathanh ...";
                    var vb = new Vanban();
                    vb.ReportProgress += RunProgressbarChitiet;
                    vb.Vanbandi(_strNguon, _strDich);
                }
                if (table == "tblcongvanphathanhcanbo")
                {
                    lbTable.Text = "Convert tblcongvanphathanhcanbo ...";
                    var vb = new Vanban();
                    vb.ReportProgress += RunProgressbarChitiet;
                    vb.VanbandiCanbo(_strNguon, _strDich);
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
                    var dm = new Danhmuc();
                    dm.ReportProgress += RunProgressbarChitiet;
                    dm.Diachiluutru(_strNguon, _strDich);
                }
                if (table == "tbldoituongxuly")
                {
                    lbTable.Text = "Convert tbldoituongxuly ...";
                    var hs = new Hoso();
                    hs.ReportProgress += RunProgressbarChitiet;
                    hs.Doituongxuly(_strNguon, _strDich);
                }
                if (table == "tbldonvitructhuoc")
                {
                    lbTable.Text = "Convert tbldonvitructhuoc ...";
                    var convert = new UserProfile();
                    convert.ReportProgress += RunProgressbarChitiet;
                    convert.Donvitructhuoc(_strNguon, _strDich);
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
                    var vb = new Vanban();
                    vb.ReportProgress += RunProgressbarChitiet;
                    vb.Guicongvan(_strNguon, _strDich);
                }
                if (table == "tblhoibaovanban")
                {
                    lbTable.Text = "Convert tblhoibaovanban ...";
                    var vb = new Vanban();
                    vb.ReportProgress += RunProgressbarChitiet;
                    vb.Hoibaovanban(_strNguon, _strDich);
                }
                if (table == "tblhosocongviec")
                {
                    lbTable.Text = "Convert tblhosocongviec ...";
                    var hs = new Hoso();
                    hs.ReportProgress += RunProgressbarChitiet;
                    hs.Hosocongviec(_strNguon, _strDich);
                }
                if (table == "tblhosohoibao")
                {
                    // qlvb2 khong co
                }
                if (table == "tblhosovanban")
                {
                    lbTable.Text = "Convert tblhosovanban ...";
                    var hs = new Hoso();
                    hs.ReportProgress += RunProgressbarChitiet;
                    hs.Hosovanban(_strNguon, _strDich);
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
                    var hs = new Hoso();
                    hs.ReportProgress += RunProgressbarChitiet;
                    hs.Hosoykienxuly(_strNguon, _strDich);
                }
                if (table == "tblketquagiaoban")
                {
                    // qlvb2 chua co
                }
                if (table == "tblkhoiphathanh")
                {
                    lbTable.Text = "Convert tblkhoiphathanh...";
                    var dm = new Danhmuc();
                    dm.ReportProgress += RunProgressbarChitiet;
                    dm.Khoiphathanh(_strNguon, _strDich);
                }
                if (table == "tbllichlamviec")
                {
                    // qlvb2 chua co
                }
                if (table == "tbllinhvuc")
                {
                    lbTable.Text = "Convert tbllinhvuc...";
                    var dm = new Danhmuc();
                    dm.ReportProgress += RunProgressbarChitiet;
                    dm.Linhvuc(_strNguon, _strDich);
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
                    var convert = new UserProfile();
                    convert.ReportProgress += RunProgressbarChitiet;
                    convert.Nhomquyen(_strNguon, _strDich);
                }
                if (table == "tblphieutrinh")
                {
                    lbTable.Text = "Convert tblphieutrinh ...";
                    var hs = new Hoso();
                    hs.ReportProgress += RunProgressbarChitiet;
                    hs.Phieutrinh(_strNguon, _strDich);
                }
                if (table == "tblplcongvanden")
                {
                    lbTable.Text = "Convert tblplcongvanden...";
                    var dm = new Danhmuc();
                    dm.ReportProgress += RunProgressbarChitiet;
                    dm.Phanloaivanbanden(_strNguon, _strDich);
                }
                if (table == "tblplcongvanphathanh")
                {
                    lbTable.Text = "Convert tblplcongvanphathanh...";
                    var dm = new Danhmuc();
                    dm.ReportProgress += RunProgressbarChitiet;
                    dm.Phanloaivanbandi(_strNguon, _strDich);
                }
                if (table == "tblplvanbanduthao")
                {
                    lbTable.Text = "Convert tblplvanbanduthao...";
                    var dm = new Danhmuc();
                    dm.ReportProgress += RunProgressbarChitiet;
                    dm.Phanloaivanbanduthao(_strNguon, _strDich);
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
                    var dm = new Danhmuc();
                    dm.ReportProgress += RunProgressbarChitiet;
                    dm.SoVanban(_strNguon, _strDich);
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
                    var dm = new Danhmuc();
                    dm.ReportProgress += RunProgressbarChitiet;
                    dm.Tinhchatvanban(_strNguon, _strDich);
                }
                if (table == "tbltochucdoitac")
                {
                    lbTable.Text = "Convert tbltochucdoitac...";
                    var dm = new Danhmuc();
                    dm.ReportProgress += RunProgressbarChitiet;
                    dm.Tochucdoitac(_strNguon, _strDich);
                }
                //.....tbltonghopgiaoban, tbltotrinh,tbltruynhaphoso....
                //........................
                if (table == "tbluquyen")
                {
                    lbTable.Text = "Convert tbluquyen ...";
                    var convert = new UserProfile();
                    convert.ReportProgress += RunProgressbarChitiet;
                    convert.Uyquyen(_strNguon, _strDich);
                }
                if (table == "tblvaitrovblq")
                {
                    // chua co ???
                }
                if (table == "tblvanbandenmail")
                {
                    lbTable.Text = "Convert tblvanbandenmail ...";
                    var convert = new Vanban();
                    convert.ReportProgress += RunProgressbarChitiet;
                    convert.Vanbandenmail(_strNguon, _strDich);
                }



            }

            lbTable.Text = "RebuildIndex...";
            var fix = new FixError();
            fix.BuildIndex(_strDich);

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
            var percent = (int)(((double)progressBarChitiet.Value / (double)progressBarChitiet.Maximum) * 100);
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
            var percent = (int)(((double)progressBarTong.Value / (double)progressBarTong.Maximum) * 100);
            progressBarTong.CreateGraphics().DrawString(percent.ToString() + "%", new Font("Arial", (float)12, FontStyle.Bold), Brushes.Red, new PointF(progressBarTong.Width / 2 - 10, progressBarTong.Height / 2 - 7));
            progressBarTong.Refresh();
            Application.DoEvents();
        }

        private void GetAllTable()
        {
            var sqlConnectionnguon = Utils.GetSqlConnection(_strNguon); //new SqlConnection(strcon);
            var cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = SQLQuery.GetAllTableInDatabase;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnectionnguon;

            sqlConnectionnguon.Open();

            reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                var colname = string.Empty;

                colname = "table_name";
                var tablename = Utils.GetStringNullCheck(reader, colname).ToLower();
                _allTable.Add(tablename);
            }

            reader.Close();
            sqlConnectionnguon.Close();
        }
        private string _CountTable()
        {
            var sqlConnection1 = Utils.GetSqlConnection(_strNguon);
            var cmd = new SqlCommand();
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
