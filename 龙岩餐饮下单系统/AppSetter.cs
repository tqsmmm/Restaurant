using System.Data.SqlClient;

namespace 龙岩餐饮下单系统
{
    class AppSetter
    {
        public static string strApplicationName = "龙岩餐饮管理系统";

        public static SqlConnection SqlConn = new SqlConnection(string.Format("Server={0},{1};Database={2};Uid={3};Pwd={4};Persist Security Info=True", "lysoftware.cn", "1433", "LY_Restaurant", "sa", "23long"));

        public static string Printing_Checkout = "XP-80";

        public static string Printing_Vegetable = "XP-80";

        public static string Printing_BBQ = "XP-80";
    }
}
