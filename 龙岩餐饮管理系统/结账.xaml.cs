using System;
using System.Windows;
using System.Windows.Input;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 结账.xaml 的交互逻辑
    /// </summary>
    public partial class 结账 : Window
    {
        public 结账()
        {
            InitializeComponent();
        }

        public string strOrder_ID = string.Empty;

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDecimal(tbRemain.Text) >= 0)
            {
                DB_Work.ExecuteCmd(string.Format("UPDATE Bills SET End_Date=GETDATE(),Pay_Cash={0},Pay_Bank={1},Pay_Coupon={2},Pay_Exempt={3},Pay_Member={4} WHERE Order_ID='{5}'", tbPay_Cash.Text, tbPay_Bank.Text, tbPay_Coupon.Text, tbPay_Exempt.Text, tbMember_Pay.Text, strOrder_ID));

                DialogResult = true;
            }
            else
            {
                Public.Sys_MsgBox("找零金额不能小于零！不能结帐！");
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                btnCancel_Click(null, null);
            }
        }

        private void Commit_Total()
        {
            if (tbMember_Balance.Text == string.Empty)
                tbMember_Balance.Text = "0.00";

            tbMember_Remain.Text = (Convert.ToDecimal(tbMember_Balance.Text) - Convert.ToDecimal(tbMember_Pay.Text)).ToString();

            tbPay.Text = (Convert.ToDecimal(tbPay_Cash.Text) +
                Convert.ToDecimal(tbPay_Bank.Text) +
                Convert.ToDecimal(tbMember_Pay.Text) +
                Convert.ToDecimal(tbPay_Exempt.Text) +
                Convert.ToDecimal(tbPay_Coupon.Text)).ToString();

            tbRemain.Text = (Convert.ToDecimal(tbPay.Text) - Convert.ToDecimal(tbAmount.Text)).ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Commit_Total();
        }

        private void tbMember_Pay_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Commit_Total();
        }

        private void tbPay_Cash_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Commit_Total();
        }

        private void tbPay_Bank_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Commit_Total();
        }

        private void tbPay_Exempt_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Commit_Total();
        }

        private void tbPay_Coupon_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Commit_Total();
        }
    }
}
