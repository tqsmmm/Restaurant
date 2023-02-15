using System;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 查看账单.xaml 的交互逻辑
    /// </summary>
    public partial class 查看账单 : Window
    {
        public 查看账单()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Load_Bills(tbOrder_ID.Text);
            Load_Bills_Detail(tbOrder_ID.Text);
        }

        private void Load_Bills(string strOrder_ID)
        {
            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT Table_ID,Member_ID,Members.Name,Members.Balance,Start_Date,End_Date,Total,Amount,Pay_Cash,Pay_Bank,Pay_Coupon,Pay_Exempt,Pay_Member FROM Bills LEFT JOIN Members ON Members.Code=Bills.Member_ID WHERE Order_ID='{0}'", strOrder_ID));

            if (Ds.Tables[0].Rows.Count > 0)
            {
                tbTable.Text = Ds.Tables[0].Rows[0][0].ToString();
                tbMember_ID.Text = Ds.Tables[0].Rows[0][1].ToString();
                tbMember_Name.Text = Ds.Tables[0].Rows[0][2].ToString();
                tbMember_Balance.Text = Ds.Tables[0].Rows[0][3].ToString();
                tbStart.Text = Convert.ToDateTime(Ds.Tables[0].Rows[0][4]).ToString("yyyy-MM-dd HH:mm");

                if (Ds.Tables[0].Rows[0][5] != DBNull.Value)
                    tbEnd.Text = Convert.ToDateTime(Ds.Tables[0].Rows[0][5]).ToString("yyyy-MM-dd HH:mm");

                tbTotal.Text = Ds.Tables[0].Rows[0][6].ToString();
                tbAmount.Text = Ds.Tables[0].Rows[0][7].ToString();

                tbPay_Cash.Text = Ds.Tables[0].Rows[0][8].ToString();
                tbPay_Bank.Text = Ds.Tables[0].Rows[0][9].ToString();
                tbPay_Coupon.Text = Ds.Tables[0].Rows[0][10].ToString();
                tbPay_Exempt.Text = Ds.Tables[0].Rows[0][11].ToString();
                tbPay_Member.Text = Ds.Tables[0].Rows[0][12].ToString();
            }
        }

        private void Load_Bills_Detail(string strOrder_ID)
        {
            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT Bills_Detail.id,Bills_Detail.Item_ID,Items.Name,Num,Items.Unit,Bills_Detail.Price,Total,Discount,Amount,Gift,[Save] FROM Bills_Detail LEFT JOIN Items ON Bills_Detail.Item_ID=Items.id WHERE Order_ID='{0}' ORDER BY id", strOrder_ID));

            dgDetail.ItemsSource = Ds.Tables[0].DefaultView;

            if (dgDetail.Items.Count > 0)
            {
                Ds = DB_Work.DataSetCmd(string.Format("SELECT SUM(Total),SUM(Amount) FROM Bills_Detail WHERE Order_ID='{0}'", tbOrder_ID.Text));

                tbTotal.Text = Convert.ToDecimal(Ds.Tables[0].Rows[0][0]).ToString();
                tbAmount.Text = Convert.ToDecimal(Ds.Tables[0].Rows[0][1]).ToString();

                DB_Work.ExecuteCmd(string.Format("UPDATE Bills SET Total={0},Amount={1},Member_ID='{2}' WHERE Order_ID='{3}'", tbTotal.Text, tbAmount.Text, tbMember_ID.Text, tbOrder_ID.Text));
                DB_Work.ExecuteCmd(string.Format("UPDATE Tables SET Total={0},Amount={1} WHERE Order_ID='{2}'", tbTotal.Text, tbAmount.Text, tbOrder_ID.Text));
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Escape)
            {
                Close();
            }
        }
    }
}
