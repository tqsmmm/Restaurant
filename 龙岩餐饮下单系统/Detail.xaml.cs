using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Controls;
using System.Drawing.Printing;
using System.Drawing;

namespace 龙岩餐饮下单系统
{
    /// <summary>
    /// Detail.xaml 的交互逻辑
    /// </summary>
    public partial class Detail : Page
    {
        public Detail()
        {
            InitializeComponent();
        }

        public string strOrder_ID = string.Empty;
        public string strTable_ID = string.Empty;

        ObservableCollection<Item> Items = new ObservableCollection<Item>();

        ObservableCollection<Selected> Selecteds = new ObservableCollection<Selected>();

        public class Item
        {
            public int Item_ID { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Type { get; set; }
        }

        public class Selected
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public decimal Num { get; set; }
            public decimal Amount { get; set; }
            public string Background { get; set; }
        }

        private void btnExit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Tables p = new Tables();
            NavigationService.Navigate(p);
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Load_Detail();

            Load_Items();
        }

        private void Load_Detail()
        {
            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT Bills_Detail.id,Items.Name,Price,Num,Amount,[Save] FROM Bills_Detail LEFT JOIN Items ON Items.id=Bills_Detail.Item_ID WHERE Order_ID='{0}'", strOrder_ID));

            Selecteds.Clear();

            string strStatic = string.Empty;

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                if (Convert.ToBoolean(Ds.Tables[0].Rows[i][5]))
                    strStatic = "Green";
                else
                    strStatic = "";

                Selecteds.Add(new Selected
                {
                    ID = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                    Name = Ds.Tables[0].Rows[i][1].ToString(),
                    Price = Convert.ToDecimal(Ds.Tables[0].Rows[i][2]),
                    Num = Convert.ToDecimal(Ds.Tables[0].Rows[i][3]),
                    Amount = Convert.ToDecimal(Ds.Tables[0].Rows[i][4]),
                    Background = strStatic
                });
            }

            lvDetail.ItemsSource = Selecteds;

            if (Selecteds.Count > 0)
            {
                Ds = DB_Work.DataSetCmd(string.Format("SELECT SUM(Num),SUM(Total),SUM(Amount) FROM Bills_Detail WHERE Order_ID='{0}'", strOrder_ID));

                tbNum.Text = Ds.Tables[0].Rows[0][0].ToString();

                decimal dTotal = Convert.ToDecimal(Ds.Tables[0].Rows[0][1]);
                tbAmount.Text = Ds.Tables[0].Rows[0][1].ToString();

                DB_Work.ExecuteCmd(string.Format("UPDATE Bills SET Total={0},Amount={1} WHERE Order_ID='{2}'", dTotal, tbAmount.Text, strOrder_ID));
                DB_Work.ExecuteCmd(string.Format("UPDATE Tables SET Total={0},Amount={1} WHERE Order_ID='{2}'", dTotal, tbAmount.Text, strOrder_ID));
            }
        }

        private void Load_Items()
        {
            DataSet Ds = DB_Work.DataSetCmd("SELECT id,Name,Normal_Price,Type FROM Items");

            Items.Clear();

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                Items.Add(new Item
                {
                    Item_ID = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                    Name = Ds.Tables[0].Rows[i][1].ToString(),
                    Price = Convert.ToDecimal(Ds.Tables[0].Rows[i][2]),
                    Type = Ds.Tables[0].Rows[i][3].ToString()
                });
            }

            lvItems.ItemsSource = Items;
        }

        private void PrintVegetable()
        {
            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT Table_ID,Order_ID,DateTime,Item_Name,Num,Unit,Gift FROM Printing WHERE Order_ID='{0}' AND Printing='菜品打印机' AND [Print]=0", strOrder_ID));

            if (Ds.Tables[0].Rows.Count > 0)
            {
                PrintDocument PrintingVegetable = new PrintDocument();
                PrintingVegetable.PrinterSettings.PrinterName = AppSetter.Printing_Vegetable;

                PrintingVegetable.PrintPage += new PrintPageEventHandler(PrintingVegetable_PrintPage);

                PrintingVegetable.Print();

                DB_Work.ExecuteCmd(string.Format("UPDATE Printing SET [Print]=1 WHERE Order_ID='{0}' AND Printing='菜品打印机' AND [Print]=0", strOrder_ID));
            }
        }

        private void PrintBBQ()
        {
            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT Table_ID,Order_ID,DateTime,Item_Name,Num,Unit,Gift FROM Printing WHERE Order_ID='{0}' AND Printing='烤串打印机' AND [Print]=0", strOrder_ID));

            if (Ds.Tables[0].Rows.Count > 0)
            {
                PrintDocument PrintingBBQ = new PrintDocument();
                PrintingBBQ.PrinterSettings.PrinterName = AppSetter.Printing_BBQ;

                PrintingBBQ.PrintPage += new PrintPageEventHandler(PrintingBBQ_PrintPage);

                PrintingBBQ.Print();

                DB_Work.ExecuteCmd(string.Format("UPDATE Printing SET [Print]=1 WHERE Order_ID='{0}' AND Printing='烤串打印机' AND [Print]=0", strOrder_ID));
            }
        }

        private void PrintingVegetable_PrintPage(object sender, PrintPageEventArgs e)
        {
            var pen = new Pen(Brushes.Black, 1);

            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT DateTime,Item_Name,SUM(Num),Unit,Gift FROM Printing WHERE Order_ID='{0}' AND Printing='菜品打印机' AND [Print]=0 GROUP BY DateTime,Item_Name,Unit,Gift", strOrder_ID));

            e.Graphics.DrawString("菜品打印机", new Font("微软雅黑", 18, FontStyle.Bold), Brushes.Black, 80, 0);

            e.Graphics.DrawString("客单：" + strOrder_ID, new Font("微软雅黑", 16), Brushes.Black, 0, 30);
            e.Graphics.DrawString("餐台号：" + strTable_ID, new Font("微软雅黑", 16), Brushes.Black, 0, 60);
            e.Graphics.DrawString("时间：" + Convert.ToDateTime(Ds.Tables[0].Rows[0][0]).ToString("yyyy-MM-dd HH:mm"), new Font("微软雅黑", 16), Brushes.Black, 0, 90);

            e.Graphics.DrawLine(pen, new Point(0, 120), new Point(300, 120));

            e.Graphics.DrawString("名称", new Font("微软雅黑", 16, FontStyle.Underline), Brushes.Black, 0, 120);
            e.Graphics.DrawString("数量", new Font("微软雅黑", 16, FontStyle.Underline), Brushes.Black, 170, 120);
            e.Graphics.DrawString("金额", new Font("微软雅黑", 16, FontStyle.Underline), Brushes.Black, 230, 120);

            var intX = 120;

            for (var i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                intX = intX + 30;

                e.Graphics.DrawString(Ds.Tables[0].Rows[i][1].ToString(), new Font("微软雅黑", 16), Brushes.Black, 0, intX);
                e.Graphics.DrawString(Ds.Tables[0].Rows[i][2].ToString(), new Font("微软雅黑", 16), Brushes.Black, 170, intX);
                e.Graphics.DrawString(Ds.Tables[0].Rows[i][3].ToString(), new Font("微软雅黑", 16), Brushes.Black, 230, intX);

                if (Convert.ToBoolean(Ds.Tables[0].Rows[i][4]))
                    e.Graphics.DrawString("赠", new Font("微软雅黑", 16), Brushes.Black, 260, intX);
            }

            e.Graphics.DrawLine(pen, new Point(0, intX + 30), new Point(300, intX + 30));
        }

        private void PrintingBBQ_PrintPage(object sender, PrintPageEventArgs e)
        {
            var pen = new Pen(Brushes.Black, 1);

            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT DateTime,Item_Name,SUM(Num),Unit,Gift FROM Printing WHERE Order_ID='{0}' AND Printing='烤串打印机' AND [Print]=0 GROUP BY DateTime,Item_Name,Unit,Gift", strOrder_ID));

            e.Graphics.DrawString("烤串打印机", new Font("微软雅黑", 18, FontStyle.Bold), Brushes.Black, 80, 0);

            e.Graphics.DrawString("客单：" + strOrder_ID, new Font("微软雅黑", 16), Brushes.Black, 0, 30);
            e.Graphics.DrawString("餐台号：" + strTable_ID, new Font("微软雅黑", 16), Brushes.Black, 0, 60);
            e.Graphics.DrawString("时间：" + Convert.ToDateTime(Ds.Tables[0].Rows[0][0]).ToString("yyyy-MM-dd HH:mm"), new Font("微软雅黑", 16), Brushes.Black, 0, 90);

            e.Graphics.DrawLine(pen, new Point(0, 120), new Point(300, 120));

            e.Graphics.DrawString("名称", new Font("微软雅黑", 16, FontStyle.Underline), Brushes.Black, 0, 120);
            e.Graphics.DrawString("数量", new Font("微软雅黑", 16, FontStyle.Underline), Brushes.Black, 170, 120);
            e.Graphics.DrawString("金额", new Font("微软雅黑", 16, FontStyle.Underline), Brushes.Black, 230, 120);

            var intX = 120;

            for (var i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                intX = intX + 30;

                e.Graphics.DrawString(Ds.Tables[0].Rows[i][1].ToString(), new Font("微软雅黑", 16), Brushes.Black, 0, intX);
                e.Graphics.DrawString(Ds.Tables[0].Rows[i][2].ToString(), new Font("微软雅黑", 16), Brushes.Black, 170, intX);
                e.Graphics.DrawString(Ds.Tables[0].Rows[i][3].ToString(), new Font("微软雅黑", 16), Brushes.Black, 230, intX);

                if (Convert.ToBoolean(Ds.Tables[0].Rows[i][4]))
                    e.Graphics.DrawString("赠", new Font("微软雅黑", 16), Brushes.Black, 260, intX);
            }

            e.Graphics.DrawLine(pen, new Point(0, intX + 30), new Point(300, intX + 30));
        }

        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DB_Work.ExecuteCmd(string.Format("INSERT INTO Printing(Table_ID,Order_ID,DateTime,Item_Name,Num,Unit,Gift,Printing,[Print]) SELECT '{0}','{1}',GETDATE(),Items.Name,Bills_Detail.Num,Items.Unit,Bills_Detail.Gift,Items.Printing,0 FROM Bills_Detail LEFT JOIN Items ON Bills_Detail.Item_ID=Items.id WHERE Bills_Detail.[Save]=0 AND Order_ID='{1}'",
                        strTable_ID, strOrder_ID));

            DB_Work.ExecuteCmd(string.Format("UPDATE Bills_Detail SET [Save]=1 WHERE Order_ID='{0}' AND [Save]=0", strOrder_ID));

            Load_Detail();

            PrintVegetable();
            PrintBBQ();
        }

        private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (lvDetail.SelectedItems.Count > 0)
            {
                Selected t = (Selected)lvDetail.SelectedItem;

                if (t.Background == string.Empty)
                {
                    DB_Work.ExecuteCmd(string.Format("UPDATE Bills_Detail SET Num=Num+1,Amount=Price*(Num+1) WHERE id={0}", t.ID));

                    Load_Detail();
                }
            }
        }

        private void btnSubtract_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (lvDetail.SelectedItems.Count > 0)
            {
                Selected t = (Selected)lvDetail.SelectedItem;

                if (t.Background == string.Empty)
                {
                    DB_Work.ExecuteCmd(string.Format("UPDATE Bills_Detail SET Num=Num-1,Amount=Price*(Num-1) WHERE id={0}", t.ID));

                    Load_Detail();
                }
            }
        }

        private void btnDel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (lvDetail.SelectedItems.Count > 0)
            {
                Selected t = (Selected)lvDetail.SelectedItem;

                if (t.Background == string.Empty)
                {
                    DB_Work.ExecuteCmd(string.Format("Delete Bills_Detail WHERE id={0}", t.ID));

                    Load_Detail();
                }
            }
        }

        private void btnGift_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (lvDetail.SelectedItems.Count > 0)
            {
                Selected t = (Selected)lvDetail.SelectedItem;

                if (t.Background == string.Empty)
                {
                    DB_Work.ExecuteCmd(string.Format("UPDATE Bills_Detail SET Gift=1,Amount=0 WHERE id={0}", t.ID));

                    Load_Detail();
                }
            }
        }

        private void btnReturn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (lvDetail.SelectedItems.Count > 0)
            {
                Selected t = (Selected)lvDetail.SelectedItem;

                if (t.Background != string.Empty)
                {
                    DB_Work.ExecuteCmd(string.Format("INSERT INTO Bills_Detail(Order_ID,Item_ID,Num,Price,Total,Discount,Amount,Gift,[Save]) SELECT Order_ID,Item_ID,0-Num,Price,0-Total,Discount,0-Amount,Gift,0 FROM Bills_Detail WHERE id={0}", t.ID));

                    Load_Detail();
                }
            }
        }

        private void btnType1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DataSet Ds = DB_Work.DataSetCmd("SELECT id,Name,Normal_Price,Type FROM Items WHERE Type IN ('羊肉类','牛肉类','猪肉类','海鲜类','蔬菜类')");

            Items.Clear();

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                Items.Add(new Item
                {
                    Item_ID = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                    Name = Ds.Tables[0].Rows[i][1].ToString(),
                    Price = Convert.ToDecimal(Ds.Tables[0].Rows[i][2]),
                    Type = Ds.Tables[0].Rows[i][3].ToString()
                });
            }

            lvItems.ItemsSource = Items;
        }

        private void btnType2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DataSet Ds = DB_Work.DataSetCmd("SELECT id,Name,Normal_Price,Type FROM Items WHERE Type IN ('凉菜类')");

            Items.Clear();

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                Items.Add(new Item
                {
                    Item_ID = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                    Name = Ds.Tables[0].Rows[i][1].ToString(),
                    Price = Convert.ToDecimal(Ds.Tables[0].Rows[i][2]),
                    Type = Ds.Tables[0].Rows[i][3].ToString()
                });
            }

            lvItems.ItemsSource = Items;
        }

        private void btnType3_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DataSet Ds = DB_Work.DataSetCmd("SELECT id,Name,Normal_Price,Type FROM Items WHERE Type IN ('烤串类')");

            Items.Clear();

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                Items.Add(new Item
                {
                    Item_ID = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                    Name = Ds.Tables[0].Rows[i][1].ToString(),
                    Price = Convert.ToDecimal(Ds.Tables[0].Rows[i][2]),
                    Type = Ds.Tables[0].Rows[i][3].ToString()
                });
            }

            lvItems.ItemsSource = Items;
        }

        private void btnType4_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DataSet Ds = DB_Work.DataSetCmd("SELECT id,Name,Normal_Price,Type FROM Items WHERE Type IN ('川菜类')");

            Items.Clear();

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                Items.Add(new Item
                {
                    Item_ID = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                    Name = Ds.Tables[0].Rows[i][1].ToString(),
                    Price = Convert.ToDecimal(Ds.Tables[0].Rows[i][2]),
                    Type = Ds.Tables[0].Rows[i][3].ToString()
                });
            }

            lvItems.ItemsSource = Items;
        }

        private void btnType5_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DataSet Ds = DB_Work.DataSetCmd("SELECT id,Name,Normal_Price,Type FROM Items WHERE Type IN ('酒水类')");

            Items.Clear();

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                Items.Add(new Item
                {
                    Item_ID = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                    Name = Ds.Tables[0].Rows[i][1].ToString(),
                    Price = Convert.ToDecimal(Ds.Tables[0].Rows[i][2]),
                    Type = Ds.Tables[0].Rows[i][3].ToString()
                });
            }

            lvItems.ItemsSource = Items;
        }

        private void btnType6_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DataSet Ds = DB_Work.DataSetCmd("SELECT id,Name,Normal_Price,Type FROM Items WHERE Type IN ('主食类')");

            Items.Clear();

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                Items.Add(new Item
                {
                    Item_ID = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                    Name = Ds.Tables[0].Rows[i][1].ToString(),
                    Price = Convert.ToDecimal(Ds.Tables[0].Rows[i][2]),
                    Type = Ds.Tables[0].Rows[i][3].ToString()
                });
            }

            lvItems.ItemsSource = Items;
        }

        private void lvItems_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvItems.SelectedItems.Count > 0)
            {
                Item d = (Item)lvItems.SelectedItem;

                DB_Work.ExecuteCmd(string.Format("INSERT INTO Bills_Detail(Order_ID,Item_ID,Num,Price,Total,Discount,Amount,Gift,[Save]) VALUES('{0}',{1},{2},{3},{4},{5},{6},'{7}',0)",
                            strOrder_ID, d.Item_ID, 1, d.Price, d.Price, 1, d.Price, false));

                Load_Detail();
            }
        }
    }
}
