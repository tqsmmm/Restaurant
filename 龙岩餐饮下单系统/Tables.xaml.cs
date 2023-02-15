using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Controls;

namespace 龙岩餐饮下单系统
{
    /// <summary>
    /// Tables.xaml 的交互逻辑
    /// </summary>
    public partial class Tables : Page
    {
        public Tables()
        {
            InitializeComponent();
        }

        ObservableCollection<Table> tables = new ObservableCollection<Table>();

        public class Table
        {
            public string Name { get; set; }
            public decimal Total { get; set; }
            public decimal Amount { get; set; }
            public int People { get; set; }
            public string Background { get; set; }
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            btnRefresh_Click(null, null);
        }

        private void btnChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DataSet Ds = DB_Work.DataSetCmd("SELECT Name,Total,Amount,People,Static FROM Tables");

            tables.Clear();

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

                tables.Add(new Table
                {
                    Name = Ds.Tables[0].Rows[i][0].ToString(),
                    Total = Convert.ToDecimal(Ds.Tables[0].Rows[i][1]),
                    Amount = Convert.ToDecimal(Ds.Tables[0].Rows[i][2]),
                    People = Convert.ToInt16(Ds.Tables[0].Rows[i][3]),
                    Background = strStatic
                });
            }

            lvTables.ItemsSource = tables;
        }

        private void btnDetail_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Table t = (Table)lvTables.SelectedItem;

            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT Order_ID,Static FROM Tables WHERE Name='{0}'", t.Name));

            Detail pinfo = new Detail();

            switch (Ds.Tables[0].Rows[0][1].ToString())
            {
                case "空闲":
                    pinfo.strOrder_ID = DB_Work.DataSetCmd(string.Format("Insert_Bills '{0}'", t.Name)).Tables[0].Rows[0][0].ToString();
                    pinfo.strTable_ID = t.Name;
                    NavigationService.Navigate(pinfo);
                    break;
                case "占用":
                    pinfo.strOrder_ID = Ds.Tables[0].Rows[0][0].ToString();
                    pinfo.strTable_ID = t.Name;
                    NavigationService.Navigate(pinfo);
                    break;
                case "脏台":
                    break;
            }
        }
    }
}
