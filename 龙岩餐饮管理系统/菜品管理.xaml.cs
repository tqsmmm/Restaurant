using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 菜品管理.xaml 的交互逻辑
    /// </summary>
    public partial class 菜品管理 : Page
    {
        public 菜品管理()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            菜品信息 m = new 菜品信息();
            m.Title = "新建菜品信息";
            m.ShowDialog();

            if (m.DialogResult.Value)
            {
                btnRefresh_Click(null, null);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgItems.SelectedItems.Count > 0)
            {
                DataRowView i = (DataRowView)dgItems.SelectedItem;

                菜品信息 m = new 菜品信息();
                m.Title = "修改菜品信息";
                m.intID = Convert.ToInt32(i.Row[0]);
                m.ShowDialog();

                if (m.DialogResult.Value)
                {
                    btnRefresh_Click(null, null);
                }
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dgItems.SelectedItems.Count > 0)
            {
                if (Public.Sys_MsgYN("是否确定删除选定项？"))
                {
                    DataRowView i = (DataRowView)dgItems.SelectedItem;

                    DB_Work.ExecuteCmd(string.Format("DELETE FROM Items WHERE id='{0}'", i.Row[0]));

                    btnRefresh_Click(null, null);
                }
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            dgItems.ItemsSource = DB_Work.DataSetCmd("SELECT id,Code,Name,Normal_Price,Member_Price,Unit,Type,Printing FROM Items ORDER BY Type,Normal_Price,Member_Price").Tables[0].DefaultView;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            btnRefresh_Click(null, null);
        }
    }
}
