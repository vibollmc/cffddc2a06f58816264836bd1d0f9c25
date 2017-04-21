using System.Web;
using System.Web.Optimization;
using QLVB.Common.Utilities;

namespace QLVB.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            // Metis
            bundles.Add(new StyleBundle("~/Content/metis/css").Include(
                // "~/Content/assets/lib/bootstrap/css/bootstrap.min.css",  // loi icon 
                    "~/Content/assets/css/main.css",
                    "~/Content/assets/lib/metismenu/metisMenu.min.css",
                    "~/Content/assets/css/theme.css"));

            // fontello
            bundles.Add(new StyleBundle("~/fonts/fontello/css").Include(
                   "~/fonts/fontello/css/animation.css",
                   "~/fonts/fontello/css/fontello-codes.css",
                   "~/fonts/fontello/css/fontello-embedded.css",
                   "~/fonts/fontello/css/fontello-ie7-codes.css",
                   "~/fonts/fontello/css/fontello-ie7.css",
                   "~/fonts/fontello/css/fontello.css"));


            bundles.Add(new ScriptBundle("~/bundles/metis/js").Include(
           "~/Content/assets/lib/bootstrap/js/bootstrap.min.js",
           "~/Content/assets/lib/screenfull/screenfull.js",
            "~/Content/assets/lib/metismenu/metisMenu.js",
           "~/Content/assets/js/core.js"));

            // my lib
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
            "~/Scripts/app.js",
            "~/Scripts/jquery.PrintArea.js",
            "~/Scripts/jquery.redirect.min.js"));

            //  using kendo
            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
            "~/Scripts/kendo/kendo.all.min.js",
            "~/Scripts/kendo/kendo.aspnetmvc.min.js",
            "~/Scripts/kendo/cultures/kendo.culture.vi-VN.min.js")
            .Include("~/Scripts/kendo/jszip.min.js"));

            //string kendoTheme = "~/Content/kendo/kendo." + AppSettings.DefaultKendoTheme + ".min.css";

            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
            "~/Content/kendo/kendo.common-bootstrap.min.css"));
            //"~/Content/kendo/kendo.bootstrap.min.css"));
            //kendoTheme));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                       "~/Scripts/jquery-ui-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                       "~/Scripts/jquery.signalR-{version}.js"));


            bundles.Add(new ScriptBundle("~/bundles/flowchart").Include(
                        "~/Scripts/jquery.contextMenu.js",
                        "~/Scripts/jquery.jsPlumb-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/flowchart").Include(
                      "~/Content/jquery.contextMenu.css",
                      "~/Content/flowchart.css"));


            bundles.Add(new ScriptBundle("~/bundles/d3").Include(
                        "~/Scripts/D3Dashboard.js",
                        "~/Scripts/d3/d3.min.js"));

        }
    }
}
