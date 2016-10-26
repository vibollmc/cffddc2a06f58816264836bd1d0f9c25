using System.Web;
using System.Web.Optimization;

namespace DLTD.Web.Main
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js", 
                      "~/Scripts/toastr.min.js",
                      "~/Scripts/bootbox.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/toastr.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/moment").Include("~/Scripts/moment-with-locales.min.js"));

            //  using kendo
            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
            "~/Scripts/kendo/kendo.all.min.js",
            "~/Scripts/kendo/kendo.aspnetmvc.min.js",
            "~/Scripts/kendo/cultures/kendo.culture.vi-VN.min.js")
            .Include("~/Scripts/kendo/jszip.min.js"));

            //string kendoTheme = "~/Content/kendo/kendo." + AppSettings.DefaultKendoTheme + ".min.css";

            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
            "~/Content/kendo/kendo.common-bootstrap.min.css",
            "~/Content/kendo/kendo.bootstrap.min.css"));


            // using themeTemplate
            bundles.Add(new StyleBundle("~/Themes/css").Include(
                "~/Themes/css/bootstrap.min.css",
                "~/Themes/css/bootstrap-theme.css",
                "~/Themes/css/elegant-icons-style.css",
                "~/Themes/css/font-awesome.min.css",
                "~/Themes/css/style.css",
                "~/Themes/css/style-responsive.css"
                ));

            bundles.Add(new ScriptBundle("~/Themes/JsIEOld").Include(
                "~/Themes/js/html5shiv.js",
                "~/Themes/js/respond.min.js",
                "~/Themes/js/lte-ie7.js"
                ));

            bundles.Add(new ScriptBundle("~/Themes/js").Include(
                "~/Themes/js/jquery.js",
                "~/Themes/js/bootstrap.min.js",
                "~/Themes/js/jquery.scrollTo.min.js",
                "~/Themes/js/jquery.nicescroll.js",
                "~/Themes/js/scripts.js"
                ));
        }
    }
}
