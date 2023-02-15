using System.Data;
using System.Windows;
using System.Windows.Input;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 换台.xaml 的交互逻辑
    /// </summary>
    public partial class 换台 : Window
    {
        public 换台()
        {
            InitializeComponent();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            DB_Work.ExecuteCmd(string.Format("Change_Tables '{0}','{1}'", tbFrom.Text, cbTo.Text));

            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataSet Ds = DB_Work.DataSetCmd("SELECT Name FROM Tables WHERE Static='空闲'");

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                cbTo.Items.Add(Ds.Tables[0].Rows[i][0].ToString());
            }
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
