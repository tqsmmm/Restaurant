using System;
using System.Windows;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// frmMain.xaml 的交互逻辑
    /// </summary>
    public partial class frmMain : Window
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppSetter.Printing_Checkout = DB_Work.DataSetCmd("SELECT Printing FROM Prints WHERE Name='前台打印机'").Tables[0].Rows[0][0].ToString();
            AppSetter.Printing_Vegetable = DB_Work.DataSetCmd("SELECT Printing FROM Prints WHERE Name='菜品打印机'").Tables[0].Rows[0][0].ToString();
            AppSetter.Printing_BBQ = DB_Work.DataSetCmd("SELECT Printing FROM Prints WHERE Name='烤串打印机'").Tables[0].Rows[0][0].ToString();

            btnStatic_Click(null, null);

            sbiAppName.Text = AppSetter.strApplicationName;
            sbiDateTime.Text = "营业时间:"+DateTime.Now.ToString("yyyy-MM-dd");
            sbiPrint.Text = "打印机正常";
        }

        private void btnStatic_Click(object sender, RoutedEventArgs e)
        {
            fMain.Source = new Uri("餐台列表.xaml", UriKind.Relative);
        }

        private void btnBills_Click(object sender, RoutedEventArgs e)
        {
            fMain.Source = new Uri("消费明细.xaml",UriKind.Relative);
        }

        private void btnStatistic_Click(object sender, RoutedEventArgs e)
        {
            fMain.Source = new Uri("菜品统计.xaml", UriKind.Relative);
        }

        private void btnMembers_Click(object sender, RoutedEventArgs e)
        {
            fMain.Source = new Uri("会员管理.xaml", UriKind.Relative);
        }

        private void btnHandle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnItems_Click(object sender, RoutedEventArgs e)
        {
            fMain.Source = new Uri("菜品管理.xaml", UriKind.Relative);
        }

        private void btnSettlement_Click(object sender, RoutedEventArgs e)
        {
            if (Public.Sys_MsgYN("是否确定日结所有账单？"))
            {
                DB_Work.ExecuteCmd("UPDATE Bills SET Checked=1 WHERE Checked<>1 OR Checked IS NULL");
            }
        }

        private void btnTables_Click(object sender, RoutedEventArgs e)
        {
            fMain.Source = new Uri("餐台管理.xaml", UriKind.Relative);
        }
    }
}
