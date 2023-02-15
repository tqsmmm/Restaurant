using System;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 会员查找.xaml 的交互逻辑
    /// </summary>
    public partial class 会员查找 : Window
    {
        public 会员查找()
        {
            InitializeComponent();
        }

        public string strMember_ID = string.Empty;
        public string strMember_Name = string.Empty;
        public decimal dMember_Balance = 0;

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                btnCancel_Click(null, null);
            }
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            DataSet Ds;

            if (!string.IsNullOrEmpty(tbMember_ID.Text))
            {
                Ds = DB_Work.DataSetCmd(string.Format("SELECT Code,Name,Balance FROM Members WHERE Code='{0}'", tbMember_ID.Text));

                if (Ds.Tables[0].Rows.Count > 0)
                {
                    strMember_ID = Ds.Tables[0].Rows[0][0].ToString();
                    strMember_Name = Ds.Tables[0].Rows[0][1].ToString();
                    dMember_Balance = Convert.ToDecimal(Ds.Tables[0].Rows[0][2]);

                    DialogResult = true;
                }
            }

            if (!string.IsNullOrEmpty(tbMember_Name.Text))
            {

            }

            if (string.IsNullOrEmpty(tbMember_Mobile.Text))
            {
                Ds = DB_Work.DataSetCmd(string.Format("SELECT Code,Name,Balance FROM Members WHERE Mobile='{0}'", tbMember_Mobile.Text));

                if (Ds.Tables[0].Rows.Count > 0)
                {
                    strMember_ID = Ds.Tables[0].Rows[0][0].ToString();
                    strMember_Name = Ds.Tables[0].Rows[0][1].ToString();
                    dMember_Balance = Convert.ToDecimal(Ds.Tables[0].Rows[0][2]);

                    DialogResult = true;
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
