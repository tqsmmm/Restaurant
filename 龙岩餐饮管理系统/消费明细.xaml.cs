using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 消费明细.xaml 的交互逻辑
    /// </summary>
    public partial class 消费明细 : Page
    {
        public 消费明细()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgBills.ItemsSource = DB_Work.DataSetCmd(string.Format("Select_Bills '{0}','{1}'", Convert.ToDateTime(dpFrom.Text).Date, Convert.ToDateTime(dpTo.Text).Date)).Tables[0].DefaultView;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dpFrom.Text = DateTime.Now.ToString();
            dpTo.Text = DateTime.Now.ToString();

            btnSearch_Click(null, null);
        }

        private void dgBills_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            btnView_Click(null, null);
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            if (dgBills.SelectedItems.Count > 0)
            {
                DataRowView i = (DataRowView)dgBills.SelectedItem;

                查看账单 frm = new 查看账单();
                frm.tbOrder_ID.Text = i.Row[0].ToString();
                frm.ShowDialog();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgBills.SelectedItems.Count > 0)
            {
                DataRowView i = (DataRowView)dgBills.SelectedItem;

                账单信息 pinfo1 = new 账单信息();
                pinfo1.tbOrder_ID.Text = i.Row[0].ToString();
                NavigationService.Navigate(pinfo1);
            }
        }
    }
}
