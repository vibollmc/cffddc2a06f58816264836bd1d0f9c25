
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using RSAEncryptionLib;
using CertificateDll;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;
using QLVB.Common.Sessions;
using QLVB.Common.Certificate;
using QLVB.Common.Utilities;
namespace QLVB.WebUI.Controllers
{
    public class CertificateController : Controller
    {
        CertificateDll.CertificateDll obj = new CertificateDll.CertificateDll();
        //CheckCA objca = new CheckCA();
        
        string strpathsave;
        bool flag = false;
        static string strcon = WebConfigurationManager.ConnectionStrings["CheckCAConnectionString"].ConnectionString;
        SqlConnection con;
        DataSet ds;
        SqlDataAdapter da;
        SqlCommand cmd;
        DataTable dt = new DataTable();
        string strchungthuc;
        string tenfileupload;
        string path;
        FileInfo filename;
        string flag_result = "";
        // CheckCA obj_check = new CheckCA();

        private ICheckCA _CheckCA;

        public CertificateController(ICheckCA CheckCA)
        {
            _CheckCA = CheckCA;
        }
        public ActionResult CheckCA()
        {
            //CheckCA obj = new CheckCA();
            //bool ca = AppSettings.IsCA;
            //if (ca == true)
            //{
            //    string checkca = _CheckCA.get_strCheckCA();
            //    if ((checkca == ""))
            //    {
            //        return View();
            //    }
            //    else
            //    {
            //        return RedirectToAction("Login", "Account");
            //    }
            //}
            //else
            //{
            //    _CheckCA.set_strCheckCA("OK");
            //    return RedirectToAction("Login", "Account");
            //}
            string checkca = _CheckCA.get_strCheckCA();
            ////Session["checkca"] = "";

            if ((checkca == ""))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        [HttpPost]
        public ActionResult UploadCA(HttpPostedFileBase file)
        {
            if (Request.Files.Count > 0)
            {
                //objca = null;
                //CheckCA obj2 = new CheckCA();
                //obj2.set_strCheckCA("OK");
                var file2 = Request.Files[0];
                try
                {
                    if (file2 != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Certificate/ClientCertificate"), fileName);
                        file.SaveAs(path);
                        tenfileupload = fileName.ToString();
                        try
                        {
                            strchungthuc = obj.doc_CA_UploadFromClient(Server.MapPath("~/Certificate/ClientCertificate/"), tenfileupload, Server.MapPath("~/Certificate/ServerCertificate/PrivateKey.xml"));
                            if (strchungthuc == "0")
                            {
                                // Response.Write("<script language=JavaScript>alert('chứng thực ko thành công');</script>");
                                GC.Collect(1);

                                path = Server.MapPath("~/Certificate/ClientCertificate/" + tenfileupload);
                                filename = new FileInfo(path);
                                if (filename.Exists)//check file exsit or not
                                {
                                    filename.Delete();

                                }
                                _CheckCA.set_strCheckCA("");
                                return Json("0");
                            }
                            else
                            {
                                string sql = "select * from tbl_TestDLL where [Status]=1";
                                con = new SqlConnection(strcon);
                                da = new SqlDataAdapter(sql, con);
                                ds = new DataSet();
                                da.Fill(ds, "tbl");
                                dt = ds.Tables["tbl"];
                                if (con.State != ConnectionState.Closed)
                                {
                                    con.Close();
                                    con.Dispose();
                                    con = null;
                                }
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    string strsosanh = dt.Rows[i][0].ToString() + dt.Rows[i][1].ToString();
                                    if (strsosanh == strchungthuc)
                                    {
                                        GC.Collect(1);

                                        path = Server.MapPath("~/Certificate/ClientCertificate/" + tenfileupload);
                                        filename = new FileInfo(path);
                                        if (filename.Exists)//check file exsit or not
                                        {
                                            filename.Delete();

                                        }
                                        flag_result = "1";

                                        // obj_check.set_strCheckCA("OK");
                                        // Response.Redirect("/Account/Login");
                                        //_CheckCA.set_strCheckCA("OK");
                                        //return Json("1");

                                    }


                                    //else
                                    //{
                                    //    // Response.Write("<script language=javascript>alert('chứng thực ko thành công');</script>");
                                    //    GC.Collect(1);

                                    //    path = Server.MapPath("~/Certificate/ClientCertificate/" + tenfileupload);
                                    //    filename = new FileInfo(path);
                                    //    if (filename.Exists)//check file exsit or not
                                    //    {
                                    //        filename.Delete();

                                    //    }
                                    //    _CheckCA.set_strCheckCA("");
                                    //    return Json("0");

                                    //}

                                }
                                if (flag_result == "1")
                                {

                                    _CheckCA.set_strCheckCA("OK");
                                    return Json("1");
                                }
                                else
                                {
                                    _CheckCA.set_strCheckCA("");
                                    return Json("");

                                }
                                //_CheckCA.set_strCheckCA("");
                                //return Json("0");

                            }
                            //_CheckCA.set_strCheckCA("");
                            //return Json("0");
                        }
                        catch (Exception ex)
                        {

                            // Response.Write("<script language=javascript>alert('chứng thực ko thành công');</script>");
                            GC.Collect(1);

                            path = Server.MapPath("~/Certificate/ClientCertificate/" + tenfileupload);
                            filename = new FileInfo(path);
                            if (filename.Exists)//check file exsit or not
                            {
                                filename.Delete();

                            }
                            _CheckCA.set_strCheckCA("");
                            return Json("0");
                        }



                        _CheckCA.set_strCheckCA("OK");
                        Response.Redirect("/Account/Login");
                        return RedirectToAction("Login", "Account");
                        return Json("1");

                        //------test service---------

                        //upload(fileName.ToString());

                        //return Json("1");
                    }
                    else
                    {
                        //return View();
                        _CheckCA.set_strCheckCA("");
                        return Json("0");
                    }
                }
                catch (Exception ex)
                {
                    //return View();
                    _CheckCA.set_strCheckCA("");
                    return Json("0");
                }

            }
            else
            {
                //return View();
                _CheckCA.set_strCheckCA("");
                return Json("0");
            }


        }
       
	}
}