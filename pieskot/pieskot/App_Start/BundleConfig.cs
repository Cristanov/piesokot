using System.Web;
using System.Web.Optimization;

namespace NaSpacerDo
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
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/editCompany").Include(
                      "~/Scripts/tinymce/tinymce.min.js",
                      "~/Scripts/views/editCompany.js",
                      "~/Scripts/views/imageInput.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/addCompany").Include(
                     "~/Scripts/views/imageInput.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
                     "~/Scripts/dropzone/dropzone.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/flexslider").Include(
                     "~/Scripts/flexslider/jquery.flexslider-min.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/rating").Include(
                     "~/Scripts/jRate/jRate.min.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/jsCookie").Include(
                   "~/Scripts/js-cookie.js"
                   ));

            bundles.Add(new ScriptBundle("~/bundles/showCompany").Include(
                   "~/Scripts/views/showCompany.js"
                   ));
        
            bundles.Add(new ScriptBundle("~/bundles/fileExtensionsValidator").Include(
                  "~/Scripts/validators/fileExtensionsValidator.js"
                  ));

            bundles.Add(new StyleBundle("~/Content/dropzone").Include(
                      "~/Scripts/dropzone/dropzone.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/flexslider").Include(
                      "~/Scripts/flexslider/flexslider.css"));
        }
    }
}
