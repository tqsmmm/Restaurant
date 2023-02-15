using System.Windows;
using System.Windows.Input;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 会员信息.xaml 的交互逻辑
    /// </summary>
    public partial class 会员信息 : Window
    {
        public 会员信息()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                btnCancel_Click(null, null);
            }
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (Title == "新建会员信息")
            {
                DB_Work.ExecuteCmd(string.Format("INSERT INTO Members(Code,Name,Mobile,Balance,Integral,Create_Date) VALUES('{0}','{1}','{2}',{3},{4},GETDATE())", tbCode.Text, tbName.Text, tbMobile.Text, tbBalance.Text, tbIntegral.Text));

                DialogResult = true;
            }
            else if (Title == "修改会员信息")
            {
                DB_Work.ExecuteCmd(string.Format("UPDATE Members SET Name='{0}',Mobile='{1}',Balance={2},Integral={3} WHERE Code='{4}'", tbName.Text, tbMobile.Text, tbBalance.Text, tbIntegral.Text, tbCode.Text));

                DialogResult = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Title == "修改会员信息")
            {
                tbCode.IsEnabled = false;
            }
        }
    }
}
