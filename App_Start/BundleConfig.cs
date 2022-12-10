using System.Web.Optimization;

namespace GardenShopOnline
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new MyScriptBundle("~/bundles/js").Include(
                "~/assets/js/vendor/bootstrap.bundle.min.js",
                "~/assets/js/vendor/jquery-3.6.0.min.js",
                "~/assets/js/vendor/jquery-migrate-3.3.2.min.js",
                "~/assets/js/vendor/jquery.waypoints.js",
                "~/assets/js/vendor/modernizr-3.11.2.min.js",
                "~/assets/js/plugins/wow.min.js",
                "~/assets/js/plugins/swiper-bundle.min.js",
                "~/assets/js/plugins/jquery.nice-select.js",
                "~/assets/js/plugins/parallax.min.js",
                "~/assets/js/plugins/jquery.magnific-popup.min.js",
                "~/assets/js/plugins/tippy.min.js",
                "~/assets/js/plugins/ion.rangeSlider.min.js",
                "~/assets/js/plugins/mailchimp-ajax.js",
                "~/assets/js/plugins/jquery.counterup.js"));

            bundles.Add(new MyStyleBundle("~/Content/css").Include(
                "~/assets/css/bootstrap.min.css",
                "~/assets/css/animate.min.css",
                "~/assets/css/swiper-bundle.min.css",
                "~/assets/css/nice-select.css",
                "~/assets/css/magnific-popup.min.css",
                "~/assets/css/ion.rangeSlider.min.css"));

            BundleTable.EnableOptimizations= true;
        }
    }
}
