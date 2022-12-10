using System.Web.Optimization;

namespace GardenShopOnline
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new MyScriptBundle("~/bundles/customerJs").Include(
                "~/assets/customer/js/vendor/bootstrap.bundle.min.js",
                "~/assets/customer/js/vendor/jquery-3.6.0.min.js",
                "~/assets/customer/js/vendor/jquery-migrate-3.3.2.min.js",
                "~/assets/customer/js/vendor/jquery.waypoints.js",
                "~/assets/customer/js/vendor/modernizr-3.11.2.min.js",
                "~/assets/customer/js/plugins/wow.min.js",
                "~/assets/customer/js/plugins/swiper-bundle.min.js",
                "~/assets/customer/js/plugins/jquery.nice-select.js",
                "~/assets/customer/js/plugins/parallax.min.js",
                "~/assets/customer/js/plugins/jquery.magnific-popup.min.js",
                "~/assets/customer/js/plugins/tippy.min.js",
                "~/assets/customer/js/plugins/ion.rangeSlider.min.js",
                "~/assets/customer/js/plugins/mailchimp-ajax.js",
                "~/assets/customer/js/plugins/jquery.counterup.js"));

            bundles.Add(new MyScriptBundle("~/bundles/vendorJs").Include(
                "~/assets/admin-staff/vendors/js/core/popper.min.js",
                "~/assets/admin-staff/vendors/js/core/bootstrap.min.js",
                "~/assets/admin-staff/vendors/js/perfect-scrollbar.jquery.min.js",
                "~/assets/admin-staff/vendors/js/prism.min.js",
                "~/assets/admin-staff/vendors/js/jquery.matchHeight-min.js",
                "~/assets/admin-staff/vendors/js/jquery.validate.min.js",
                "~/assets/admin-staff/vendors/js/jquery.validate.unobtrusive.min.js",
                "~/assets/admin-staff/vendors/js/screenfull.min.js",
                "~/assets/admin-staff/vendors/js/pace/pace.min.js",
                "~/assets/admin-staff/vendors/js/toastr.min.js",
                "~/assets/admin-staff/vendors/js/sweetalert2.min.js"));

            bundles.Add(new MyScriptBundle("~/bundles/jquery").Include(
                 "~/assets/admin-staff/vendors/js/core/jquery-{version}.min.js"));

            bundles.Add(new MyScriptBundle("~/bundles/sidebarJs").Include(
               "~/assets/admin-staff/js/app-sidebar.min.js",
               "~/assets/admin-staff/js/notification-sidebar.min.js"));

            bundles.Add(new MyStyleBundle("~/Content/customerCss").Include(
                "~/assets/customer/css/bootstrap.min.css",
                "~/assets/customer/css/animate.min.css",
                "~/assets/customer/css/swiper-bundle.min.css",
                "~/assets/customer/css/nice-select.css",
                "~/assets/customer/css/magnific-popup.min.css",
                "~/assets/customer/css/ion.rangeSlider.min.css"));

            bundles.Add(new MyStyleBundle("~/Content/css").Include(
                "~/assets/admin-staff/vendors/css/perfect-scrollbar.min.css",
                "~/assets/admin-staff/vendors/css/prism.min.css",
                "~/assets/admin-staff/vendors/css/toastr.min.css",
                "~/assets/admin-staff/vendors/css/sweetalert2.min.css",
                "~/assets/admin-staff/css/app.min.css"));

            BundleTable.EnableOptimizations= true;
        }
    }
}
