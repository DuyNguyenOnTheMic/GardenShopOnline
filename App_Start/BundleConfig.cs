using System.Web.Optimization;

namespace GardenShopOnline
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
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
