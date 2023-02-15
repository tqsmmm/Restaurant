using System;
using System.Windows;

namespace 龙岩餐饮下单系统
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Main.Source = new Uri("Login.xaml", UriKind.Relative);
        }
    }
}
