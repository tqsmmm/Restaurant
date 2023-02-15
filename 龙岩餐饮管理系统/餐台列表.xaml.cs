using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Controls;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 餐台状态.xaml 的交互逻辑
    /// </summary>
    public partial class 餐台列表 : Page
    {
        public 餐台列表()
        {
            InitializeComponent();
        }

        ObservableCollection<Table> Tables = new ObservableCollection<Table>();

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            btnRefresh_Click(null, null);
        }

        public class Table
        {
            public string Name { get; set; }
            public decimal Total { get; set; }
            public decimal Amount { get; set; }
            public int People { get; set; }
            public string Background { get; set; }
        }

        private void btnRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DataSet Ds = DB_Work.DataSetCmd("SELECT Name,Total,Amount,People,Static FROM Tables");

            Tables.Clear();

            string strStatic = string.Empty;

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                switch (Ds.Tables[0].Rows[i][4].ToString())
                {
                    case "空闲":
                        strStatic = "Green";
                        break;
                    case "占用":
                        strStatic = "Blue";
                        break;
                    case "脏台":
                        strStatic = "Orange";
                        break;
                }

                Tables.Add(new Table
                {
                    Name = Ds.Tables[0].Rows[i][0].ToString(),
                    Total = Convert.ToDecimal(Ds.Tables[0].Rows[i][1]),
                    Amount=Convert.ToDecimal(Ds.Tables[0].Rows[i][2]),
                    People = Convert.ToInt16(Ds.Tables[0].Rows[i][3]),
                    Background = strStatic
                });
            }

            lvTables.ItemsSource = Tables;

            Load_Total();
        }

        private void Load_Total()
        {
            DataSet Ds = DB_Work.DataSetCmd("SELECT COUNT(id),SUM(Amount) FROM Bills WHERE End_Date is NOT NULL AND Checked IS NULL");

            tbCheckouted_Num.Text = Ds.Tables[0].Rows[0][0].ToString();
            tbCheckouted_Amount.Text = Ds.Tables[0].Rows[0][1].ToString();

            Ds = DB_Work.DataSetCmd("SELECT COUNT(id),SUM(Amount) FROM Bills WHERE End_Date IS NULL AND Checked IS NULL");

            tbCheckout_Num.Text = Ds.Tables[0].Rows[0][0].ToString();
            tbCheckout_Amount.Text = Ds.Tables[0].Rows[0][1].ToString();

            Ds = DB_Work.DataSetCmd("SELECT COUNT(id),SUM(Amount) FROM Bills WHERE Checked IS NULL");

            tbTotal_Num.Text = Ds.Tables[0].Rows[0][0].ToString();
            tbTotal_Amount.Text = Ds.Tables[0].Rows[0][1].ToString();
        }

        private void lvTables_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Table t = (Table)lvTables.SelectedItem;

            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT Order_ID,Static FROM Tables WHERE Name='{0}'", t.Name));

            switch (Ds.Tables[0].Rows[0][1].ToString())
            {
                case "空闲":
                    账单信息 pinfo1 = new 账单信息();
                    pinfo1.tbOrder_ID.Text = DB_Work.DataSetCmd(string.Format("Insert_Bills '{0}'", t.Name)).Tables[0].Rows[0][0].ToString();
                    NavigationService.Navigate(pinfo1);
                    break;
                case "占用":
                    账单信息 pinfo2 = new 账单信息();
                    pinfo2.tbOrder_ID.Text = Ds.Tables[0].Rows[0][0].ToString();
                    NavigationService.Navigate(pinfo2);
                    break;
                case "脏台":
                    break;
            }
        }

        private void btnChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (lvTables.SelectedItems.Count > 0)
            {
                Table t = (Table)lvTables.SelectedItem;

                换台 i = new 换台();
                i.tbFrom.Text = t.Name;
                i.ShowDialog();

                if (i.DialogResult.Value)
                {
                    btnRefresh_Click(null, null);
                }
            }
        }
    }
}
