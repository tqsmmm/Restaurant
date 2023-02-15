using System;
using System.Windows;
using System.Windows.Controls;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 菜品统计.xaml 的交互逻辑
    /// </summary>
    public partial class 菜品统计 : Page
    {
        public 菜品统计()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgItems.ItemsSource = DB_Work.DataSetCmd(string.Format("SELECT Code,Items.Name,Price,SUM(Num) AS [Num],Unit,SUM(Bills_Detail.Total) AS [Total],SUM(Bills_Detail.Amount) AS [Amount],Type,Gift FROM Bills_Detail LEFT JOIN Items ON Items.id=Bills_Detail.Item_ID LEFT JOIN Bills ON Bills.Order_ID=Bills_Detail.Order_ID WHERE CONVERT(VARCHAR(10),End_Date,120)>='{0:yyyy-MM-dd}' AND CONVERT(VARCHAR(10),End_Date,120)<='{1:yyyy-MM-dd}' GROUP BY Code,Items.Name,Price,Unit,Type,Gift ORDER BY Type,Price,Num", Convert.ToDateTime(dpFrom.Text).Date, Convert.ToDateTime(dpTo.Text).Date)).Tables[0].DefaultView;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dpFrom.Text = DateTime.Now.ToString();
            dpTo.Text = DateTime.Now.ToString();

            btnSearch_Click(null, null);
        }
    }
}
