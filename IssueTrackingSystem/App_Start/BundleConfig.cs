using System.Web;
using System.Web.Optimization;

namespace IssueTrackingSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            bundles.Add(new ScriptBundle("~/bundles/dragula", "//cdnjs.cloudflare.com/ajax/libs/dragula/3.7.2/dragula.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/its").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-{version},js",
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*",
                "~/Scripts/bootstrap*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap*",
                      "~/Content/site.css",
                      "~/Content/jquery-ui*"));

            bundles.Add(new StyleBundle("~/Content/dragula").Include(
                "~/Content/dragula.min.css"));

            bundles.Add(new StyleBundle("~/Content/cardwall").Include(
                "~/Content/Cardwall.css",
                "~/Content/dragula.min.css"));

            bundles.Add(new StyleBundle("~/Content/login").Include(
                "~/Content/Login.css"));

            bundles.Add(new StyleBundle("~/Content/index").Include(
                "~/Content/Index.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
