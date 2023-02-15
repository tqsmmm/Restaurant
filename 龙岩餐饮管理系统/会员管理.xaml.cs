using System.Data;
using System.Windows.Controls;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 会员查询.xaml 的交互逻辑
    /// </summary>
    public partial class 会员管理 : Page
    {
        public 会员管理()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            btnRefresh_Click(null, null);
        }

        private void btnRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            dgMembers.ItemsSource = DB_Work.DataSetCmd("SELECT Code,Name,Mobile,Balance,Integral,Create_Date FROM Members ORDER BY Code").Tables[0].DefaultView;
        }

        private void btnNew_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            会员信息 m = new 会员信息();
            m.Title = "新建会员信息";
            m.ShowDialog();

            if (m.DialogResult.Value)
            {
                btnRefresh_Click(null, null);
            }
        }

        private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (dgMembers.SelectedItems.Count > 0)
            {
                DataRowView i = (DataRowView)dgMembers.SelectedItem;

                会员信息 m = new 会员信息();
                m.Title = "修改会员信息";
                m.tbCode.Text = i.Row[0].ToString();
                m.tbName.Text = i.Row[1].ToString();
                m.tbMobile.Text = i.Row[2].ToString();
                m.tbBalance.Text = i.Row[3].ToString();
                m.tbIntegral.Text = i.Row[4].ToString();
                m.ShowDialog();

                if (m.DialogResult.Value)
                {
                    btnRefresh_Click(null, null);
                }
            }
        }

        private void btnDel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (dgMembers.SelectedItems.Count > 0)
            {
                if (Public.Sys_MsgYN("是否确定删除选定项？"))
                {
                    DataRowView i = (DataRowView)dgMembers.SelectedItem;

                    DB_Work.ExecuteCmd(string.Format("DELETE FROM Members WHERE Code='{0}'", i.Row[0]));

                    btnRefresh_Click(null, null);
                }
            }
        }
    }
}
