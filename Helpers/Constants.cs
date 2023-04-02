using System;
using System.Collections.Generic;

namespace GardenShopOnline.Helpers
{
    public class Constants
    {
        /// <summary>
        /// subdomain
        /// </summary>
        public static readonly string subdomain = "";

        public static readonly string webUrl = "";

        /// <summary>
        /// User roles
        /// </summary>
        public static readonly int ADMIN = 1;
        public static readonly int STUDENT = 2;
        public static readonly int STAFF = 3;
        public static readonly int TEACHER = 4;
        public static readonly int LEADER = 5;
        public static readonly int ACCOUNTANT = 6;

        /// To use switch case User roles
        /// </summary>
        public const string ACCOUNT_ADMIN = "1";

        public const string ACCOUNT_STUDENT = "2";
        public const string ACCOUNT_STAFF = "3";
        public const string ACCOUNT_TEACHER = "4";
        public const string ACCOUNT_LEADER = "5";
        public const string ACCOUNT_ACCOUNTANT = "6";
        public const string TEMP_ACCOUNT = "temp";

        /// <summary>
        /// Alert Type For Message
        /// </summary>
        public const string ALERT_TYPE_SUCCESS = "success"; // THÀNH CÔNG

        public const string ALERT_TYPE_ERROR = "error"; // THẤT BẠI
        public const string ALERT_TYPE_WARNING = "warning"; // CẢNH BÁO

      
        ///
        public static readonly Dictionary<int, string> RolePer = new Dictionary<int, string> {
            {ADMIN, "Quản trị hệ thống"},
            {STUDENT, "Sinh viên" },
            {STAFF, "Thủ kho" },
            {TEACHER, "Giảng viên" },
            {LEADER, "Lãnh đạo" },
            {ACCOUNTANT, "Kế toán" },
        };


        ///Trạng thái đơn hàng
        ///
        public static int WAIT_FOR_CONFIRMATION = 1;
        public static int APPROVED = 2;
        public static int DELIVERING = 3;
        public static int COMPLETED = 4;
        public static int PAY_IN_ADVANCE = 5;
        public static int CANCELED = 6;

        public static readonly Dictionary<int, string> StateReceipt = new Dictionary<int, string> {
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
            {SHOW_STATUS, "Trạng thái hiện"},
            {HIDDEN_STATUS, "Trạng thái ẩn" }
        };

        ///Phương thức thanh toán
        ///
        public static int BANK_METHOD = 1;

        ///Loan Form state
        ///
        public static int SIGN_UP_LOAN = 1;
        public static int BORROW_MORE = 2;
        public static int LOANING = 3;
        public static int RETURNED_LOAN = 4;
        public static int INFULL_REFUND_LOAN = 5;
        public static int CANCEL_LOAN = 6;

        public static readonly Dictionary<int, Tuple<string, string>> StateLoanForm = new Dictionary<int, Tuple<string, string>> {
            {SIGN_UP_LOAN, new Tuple<string, string>("Đăng ký", "secondary")},
            {BORROW_MORE, new Tuple<string, string>("Yêu cầu mượn thêm", "info") },
            {LOANING, new Tuple<string, string>("Đang mượn", "primary") },
            {RETURNED_LOAN, new Tuple < string, string >("Đã trả", "success") },
            {INFULL_REFUND_LOAN, new Tuple < string, string >("Chưa trả hết", "warning") },
            {CANCEL_LOAN, new Tuple < string, string >("Huỷ bỏ", "danger") }
        };

       

        /// <summary>
        /// connectionString for Import Excel Data
        /// </summary>
        public const string MS_EXCEL = "application/vnd.ms-excel";

        public const string OPENXMLFORMATS = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string Microsoft_Jet_OLEDB_4 = "Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;";
        public const string Microsoft_ACE_OLEDB_12 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";";


    }
}