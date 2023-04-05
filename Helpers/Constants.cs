using System.Collections.Generic;

namespace GardenShopOnline.Helpers
{
    public class Constants
    {
        /// <summary>
        /// Account Web
        /// </summary>
        public const string ACCOUNT_BONSAIGARDEN = "bonsaigarden6@gmail.com";

        /// <summary>
        /// User roles
        /// </summary>
        public static readonly int ADMIN = 1;
        public static readonly int STAFF = 2;


        /// To use switch case User roles
        /// </summary>
        public const string ACCOUNT_ADMIN = "1";
        public const string ACCOUNT_STAFF = "2";


        /// <summary>
        /// Alert Type For Message
        /// </summary>
        public const string ALERT_TYPE_SUCCESS = "success"; // THÀNH CÔNG

        public const string ALERT_TYPE_ERROR = "error"; // THẤT BẠI
        public const string ALERT_TYPE_WARNING = "warning"; // CẢNH BÁO


        ///
        public static readonly Dictionary<int, string> RolePer = new Dictionary<int, string> {
            {ADMIN, "Admin"},
            {STAFF, "Staff" },
        };


        ///Trạng thái đơn hàng
        ///
        public static int WAIT_FOR_CONFIRMATION = 1;
        public static int APPROVED = 2;
        public static int DELIVERING = 3;
        public static int COMPLETED = 4;
        public static int PAY_IN_ADVANCE = 5;
        public static int CANCELED = 6;

        public static readonly Dictionary<int, string> StateOder = new Dictionary<int, string> {
            {WAIT_FOR_CONFIRMATION, "Wait for confirmation"},
            {APPROVED, "Approved" },
            {DELIVERING, "Delivering" },
            {COMPLETED, "Completed" },
            {PAY_IN_ADVANCE, "Pay in advance" },
            {CANCELED, "Canceled" }
        };

        ///Trạng thái ẩn/hiện
        ///
        public static int SHOW_STATUS = 1;
        public static int HIDDEN_STATUS = 2;
        public static readonly Dictionary<int, string> Status = new Dictionary<int, string> {
            {SHOW_STATUS, "Show"},
            {HIDDEN_STATUS, "Hidden" }
        };

        ///Phương thức thanh toán
        ///
        public static int BANK_METHOD = 2;
        public static int CASH_METHOD = 1;

        ///Phương thức thanh toán
        ///
        public static int NEW_COMMENT = 1;
        public static int APPROVED_COMMENT = 2;
        public static int REFUSED_COMMENT = 3;

        ///Loại tin nhắn
        ///
        public static int TYPE_TEXT = 1;
        public static int TYPR_IMAGE = 2;
    }
}