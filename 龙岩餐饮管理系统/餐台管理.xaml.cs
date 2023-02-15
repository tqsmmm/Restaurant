using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 餐台管理.xaml 的交互逻辑
    /// </summary>
    public partial class 餐台管理 : Page
    {
        public 餐台管理()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            餐台信息 m = new 餐台信息();
            m.Title = "新建餐台信息";
            m.ShowDialog();

            if (m.DialogResult.Value)
            {
                btnRefresh_Click(null, null);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgTables.SelectedItems.Count > 0)
            {
                DataRowView i = (DataRowView)dgTables.SelectedItem;

                餐台信息 m = new 餐台信息();
                m.Title = "修改餐台信息";
                m.intID = Convert.ToInt32(i.Row[0]);
                m.tbName.Text = i.Row[1].ToString();
                m.tbType.Text = i.Row[2].ToString();
                m.tbPeople.Text = i.Row[3].ToString();
                m.cbStatic.Text = i.Row[4].ToString();
                m.ShowDialog();

                if (m.DialogResult.Value)
                {
                    btnRefresh_Click(null, null);
                }
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dgTables.SelectedItems.Count > 0)
            {
                if (Public.Sys_MsgYN("是否确定删除选定项？"))
                {
                    DataRowView i = (DataRowView)dgTables.SelectedItem;

                    DB_Work.ExecuteCmd(string.Format("DELETE FROM Items WHERE id='{0}'", i.Row[0]));

                    btnRefresh_Click(null, null);
                }
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            dgTables.ItemsSource = DB_Work.DataSetCmd("SELECT id,Name,Type,People,Static FROM Tables").Tables[0].DefaultView;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            btnRefresh_Click(null, null);
        }
    }
}
