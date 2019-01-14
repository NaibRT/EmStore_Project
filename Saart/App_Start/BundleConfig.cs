using System.Web;
using System.Web.Optimization;

namespace Saart
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap/css/bootstrap.min.css",
                      "~/Content/font-awesome/css/font-awesome.min.css",
                      "~/Content/css/stylesheet.css",
                      "~/Content/css/responsive.css",
                      "~/Content/owl-carousel/owl.carousel.css",
                      "~/Content/owl-carousel/owl.transitions.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Content/javascript/jquery-2.1.1.min.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Content/bootstrap/js/bootstrap.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                       "~/Content/javascript/common.js",
                      "~/Content/javascript/global.js",
                       "~/Content/javascript/jstree.min.js",
                          "~/AdminContent/assets/js/CustomJS_By_Me.js"
                         
                       ));
            bundles.Add(new ScriptBundle("~/bundles/owlJs").Include(
                       "~/Content/owl-carousel/owl.carousel.min.js"
                       ));
           
        }
    }
}
