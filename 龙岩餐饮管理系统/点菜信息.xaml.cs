using System.Windows;
using System.Windows.Input;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 点菜信息.xaml 的交互逻辑
    /// </summary>
    public partial class 点菜信息 : Window
    {
        public 点菜信息()
        {
            InitializeComponent();
        }

        public int intID = 0;

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
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
            else if (e.Key == Key.Enter)
            {
                btnAccept_Click(null, null);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbNum.Focus();
            tbNum.SelectAll();
        }
    }
}
