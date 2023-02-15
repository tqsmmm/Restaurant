using System.Windows;
using System.Windows.Controls;

namespace 龙岩餐饮下单系统
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Tables pTables = new Tables();
            NavigationService.Navigate(pTables);
        }
    }
}
