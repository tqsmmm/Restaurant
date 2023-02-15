using System.Windows;
using System.Windows.Input;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 餐台信息.xaml 的交互逻辑
    /// </summary>
    public partial class 餐台信息 : Window
    {
        public 餐台信息()
        {
            InitializeComponent();
        }

        public int intID = 0;

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (Title == "新建餐台信息")
            {
                DB_Work.ExecuteCmd(string.Format("INSERT INTO Tables(Name,Type,People,Static,Total,Amount,Order_ID) VALUES('{0}','{1}',{2},'{3}',0,0,'')", tbName.Text, tbType.Text, tbPeople.Text, cbStatic.Text));

                DialogResult = true;
            }
            else if (Title == "修改餐台信息")
            {
                DB_Work.ExecuteCmd(string.Format("UPDATE Tables SET Name='{0}',Type='{1}',People={2},Static='{3}' WHERE id={4}", tbName.Text, tbType.Text, tbPeople.Text, cbStatic.Text, intID));

                DialogResult = true;
            }
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
    }
}
