using System;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 餐台信息.xaml 的交互逻辑
    /// </summary>
    public partial class 账单信息 : Page
    {
        public 账单信息()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Load_Bills(tbOrder_ID.Text);
            Load_Bills_Detail(tbOrder_ID.Text);

            Load_Items();

            tbSearch.Focus();
            tbSearch.SelectAll();
        }

        private void Load_Items()
        {
            if (tbMember_ID.Text != string.Empty)
            {
                dgItems.ItemsSource = DB_Work.DataSetCmd("SELECT id,Name,Member_Price AS Price,Type FROM Items").Tables[0].DefaultView;
            }
            else
            {
                dgItems.ItemsSource = DB_Work.DataSetCmd("SELECT id,Name,Normal_Price AS Price,Type FROM Items").Tables[0].DefaultView;
            }
        }

        private void Load_Bills(string strOrder_ID)
        {
            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT Table_ID,Member_ID,Members.Name,Members.Balance,Start_Date,End_Date,Total,Amount,Pay_Cash,Pay_Bank,Pay_Coupon,Pay_Exempt,Pay_Member FROM Bills LEFT JOIN Members ON Members.Code=Bills.Member_ID WHERE Order_ID='{0}'", strOrder_ID));

            if (Ds.Tables[0].Rows.Count > 0)
            {
                tbTable.Text = Ds.Tables[0].Rows[0][0].ToString();
                tbMember_ID.Text = Ds.Tables[0].Rows[0][1].ToString();
                tbMember_Name.Text = Ds.Tables[0].Rows[0][2].ToString();
                tbMember_Balance.Text = Ds.Tables[0].Rows[0][3].ToString();
                tbStart.Text = Convert.ToDateTime(Ds.Tables[0].Rows[0][4]).ToString("yyyy-MM-dd HH:mm");

                if (Ds.Tables[0].Rows[0][5] != DBNull.Value)
                    tbEnd.Text = Convert.ToDateTime(Ds.Tables[0].Rows[0][5]).ToString("yyyy-MM-dd HH:mm");

                tbTotal.Text = Ds.Tables[0].Rows[0][6].ToString();
                tbAmount.Text = Ds.Tables[0].Rows[0][7].ToString();

                tbPay_Cash.Text = Ds.Tables[0].Rows[0][8].ToString();
                tbPay_Bank.Text = Ds.Tables[0].Rows[0][9].ToString();
                tbPay_Coupon.Text = Ds.Tables[0].Rows[0][10].ToString();
                tbPay_Exempt.Text = Ds.Tables[0].Rows[0][11].ToString();
                tbPay_Member.Text = Ds.Tables[0].Rows[0][12].ToString();
            }
        }

        private void Load_Bills_Detail(string strOrder_ID)
        {
            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT Bills_Detail.id,Bills_Detail.Item_ID,Items.Name,Num,Items.Unit,Bills_Detail.Price,Total,Discount,Amount,Gift,[Save] FROM Bills_Detail LEFT JOIN Items ON Bills_Detail.Item_ID=Items.id WHERE Order_ID='{0}' ORDER BY id", strOrder_ID));

            dgDetail.ItemsSource = Ds.Tables[0].DefaultView;

            if (dgDetail.Items.Count > 0)
            {
                Ds = DB_Work.DataSetCmd(string.Format("SELECT SUM(Total),SUM(Amount) FROM Bills_Detail WHERE Order_ID='{0}'", tbOrder_ID.Text));

                tbTotal.Text = Convert.ToDecimal(Ds.Tables[0].Rows[0][0]).ToString();
                tbAmount.Text = Convert.ToDecimal(Ds.Tables[0].Rows[0][1]).ToString();

                DB_Work.ExecuteCmd(string.Format("UPDATE Bills SET Total={0},Amount={1},Member_ID='{2}' WHERE Order_ID='{3}'", tbTotal.Text, tbAmount.Text, tbMember_ID.Text, tbOrder_ID.Text));
                DB_Work.ExecuteCmd(string.Format("UPDATE Tables SET Total={0},Amount={1} WHERE Order_ID='{2}'", tbTotal.Text, tbAmount.Text, tbOrder_ID.Text));
            }
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    btnCheck_Click(null, null);
                    break;
                case Key.F6:
                    btnCheckout_Click(null, null);
                    break;
            }
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            DB_Work.ExecuteCmd(string.Format("INSERT INTO Printing(Table_ID,Order_ID,DateTime,Item_Name,Num,Unit,Gift,Printing,[Print]) SELECT '{0}','{1}',GETDATE(),Items.Name,Bills_Detail.Num,Items.Unit,Bills_Detail.Gift,Items.Printing,0 FROM Bills_Detail LEFT JOIN Items ON Bills_Detail.Item_ID=Items.id WHERE Bills_Detail.[Save]=0 AND Order_ID='{1}'",
                        tbTable.Text, tbOrder_ID.Text));

            DB_Work.ExecuteCmd(string.Format("UPDATE Bills_Detail SET [Save]=1 WHERE Order_ID='{0}' AND [Save]=0", tbOrder_ID.Text));

            Load_Bills_Detail(tbOrder_ID.Text);

            PrintVegetable();
            PrintBBQ();
        }

        private void PrintVegetable()
        {
            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT Table_ID,Order_ID,DateTime,Item_Name,Num,Unit,Gift FROM Printing WHERE Order_ID='{0}' AND Printing='菜品打印机' AND [Print]=0", tbOrder_ID.Text));

            if (Ds.Tables[0].Rows.Count > 0)
            {
                PrintDocument PrintingVegetable = new PrintDocument();
                PrintingVegetable.PrinterSettings.PrinterName = AppSetter.Printing_Vegetable;

                PrintingVegetable.PrintPage += new PrintPageEventHandler(PrintingVegetable_PrintPage);

                PrintingVegetable.Print();

                DB_Work.ExecuteCmd(string.Format("UPDATE Printing SET [Print]=1 WHERE Order_ID='{0}' AND Printing='菜品打印机' AND [Print]=0", tbOrder_ID.Text));
            }
        }

        private void PrintBBQ()
        {
            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT Table_ID,Order_ID,DateTime,Item_Name,Num,Unit,Gift FROM Printing WHERE Order_ID='{0}' AND Printing='烤串打印机' AND [Print]=0", tbOrder_ID.Text));

            if (Ds.Tables[0].Rows.Count > 0)
            {
                PrintDocument PrintingBBQ = new PrintDocument();
                PrintingBBQ.PrinterSettings.PrinterName = AppSetter.Printing_BBQ;

                PrintingBBQ.PrintPage += new PrintPageEventHandler(PrintingBBQ_PrintPage);

                PrintingBBQ.Print();

                DB_Work.ExecuteCmd(string.Format("UPDATE Printing SET [Print]=1 WHERE Order_ID='{0}' AND Printing='烤串打印机' AND [Print]=0", tbOrder_ID.Text));
            }
        }

        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            结账 c = new 结账();
            c.tbAmount.Text = tbAmount.Text;
            c.tbMember_Balance.Text = tbMember_Balance.Text;
            c.strOrder_ID = tbOrder_ID.Text;
            c.ShowDialog();

            if (c.DialogResult.Value)
            {
                DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT End_Date,Pay_Cash,Pay_Bank,Pay_Coupon,Pay_Exempt,Pay_Member FROM Bills WHERE Order_ID='{0}'", tbOrder_ID.Text));

                if (Ds.Tables[0].Rows.Count > 0)
                {
                    tbEnd.Text = Convert.ToDateTime(Ds.Tables[0].Rows[0][0]).ToString("yyyy-MM-dd HH:mm");

                    tbPay_Cash.Text = Ds.Tables[0].Rows[0][1].ToString();
                    tbPay_Bank.Text = Ds.Tables[0].Rows[0][2].ToString();
                    tbPay_Coupon.Text = Ds.Tables[0].Rows[0][3].ToString();
                    tbPay_Exempt.Text = Ds.Tables[0].Rows[0][4].ToString();
                    tbPay_Member.Text = Ds.Tables[0].Rows[0][5].ToString();
                }

                if (tbMember_ID.Text != string.Empty)
                {
                    decimal dIntegral = Convert.ToDecimal(tbPay_Cash.Text) + Convert.ToDecimal(tbPay_Bank.Text) + Convert.ToDecimal(tbPay_Member.Text);

                    tbMember_Balance.Text = (Convert.ToDecimal(tbMember_Balance.Text) - Convert.ToDecimal(tbPay_Member.Text)).ToString();

                    DB_Work.ExecuteCmd(string.Format("UPDATE Members SET Balance={0},Integral={1} WHERE Code='{2}'", tbMember_Balance.Text, dIntegral, tbMember_ID.Text));
                }

                DB_Work.ExecuteCmd(string.Format("UPDATE Tables SET Static='空闲',Total=0,Amount=0,People=0,Order_ID='' WHERE Name='{0}'", tbTable.Text));

                if (c.rbPrint_Yes.IsChecked.Value)
                {
                    PrintDocument PrintCheckout = new PrintDocument();
                    PrintCheckout.PrinterSettings.PrinterName = AppSetter.Printing_Checkout;

                    PrintCheckout.PrintPage += new PrintPageEventHandler(PrintCheckout_PrintPage);

                    PrintCheckout.Print();
                }

                餐台列表 pinfo = new 餐台列表();
                NavigationService.Navigate(pinfo);
            }
        }

        private void PrintCheckout_PrintPage(object sender, PrintPageEventArgs e)
        {
            var pen = new Pen(Brushes.Black, 1);

            e.Graphics.DrawString("烤烤乐园结算单", new Font("微软雅黑", 12, System.Drawing.FontStyle.Bold), Brushes.Black, 30, 0);

            e.Graphics.DrawString("客单编码：" + tbOrder_ID.Text, new Font("微软雅黑", 10), Brushes.Black, 0, 20);
            e.Graphics.DrawString("餐台号：" + tbTable.Text, new Font("微软雅黑", 10), Brushes.Black, 0, 40);
            e.Graphics.DrawString("开台时间：" + tbStart.Text, new Font("微软雅黑", 10), Brushes.Black, 0, 60);
            e.Graphics.DrawString("结账时间：" + tbEnd.Text, new Font("微软雅黑", 10), Brushes.Black, 0, 80);

            e.Graphics.DrawString(string.Format("会员：{0},{1}", tbMember_ID.Text, tbMember_Name.Text), new Font("微软雅黑", 10), Brushes.Black, 0, 100);

            e.Graphics.DrawLine(pen, new System.Drawing.Point(0, 120), new System.Drawing.Point(200, 120));
            e.Graphics.DrawString("名称", new Font("微软雅黑", 10, System.Drawing.FontStyle.Underline), Brushes.Black, 0, 120);
            e.Graphics.DrawString("单价", new Font("微软雅黑", 10, System.Drawing.FontStyle.Underline), Brushes.Black, 0, 140);
            e.Graphics.DrawString("数量", new Font("微软雅黑", 10, System.Drawing.FontStyle.Underline), Brushes.Black, 80, 140);
            e.Graphics.DrawString("金额", new Font("微软雅黑", 10, System.Drawing.FontStyle.Underline), Brushes.Black, 140, 140);

            var intX = 140;

            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT Items.Name,Items.Type,Bills_Detail.Price,SUM(Bills_Detail.Num),SUM(Bills_Detail.Amount) FROM Bills_Detail LEFT JOIN Items ON Items.id=Bills_Detail.Item_ID WHERE Order_ID='{0}' GROUP BY Items.Name,Items.Type,Bills_Detail.Price", tbOrder_ID.Text));

            for (var i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                intX = intX + 20;

                e.Graphics.DrawString(Ds.Tables[0].Rows[i][0].ToString(), new Font("微软雅黑", 10), Brushes.Black, 0, intX);

                intX = intX + 20;

                e.Graphics.DrawString(Ds.Tables[0].Rows[i][2].ToString(), new Font("微软雅黑", 10), Brushes.Black, 0, intX);
                e.Graphics.DrawString(Ds.Tables[0].Rows[i][3].ToString(), new Font("微软雅黑", 10), Brushes.Black, 80, intX);
                e.Graphics.DrawString(Ds.Tables[0].Rows[i][4].ToString(), new Font("微软雅黑", 10), Brushes.Black, 140, intX);
            }

            e.Graphics.DrawLine(pen, new System.Drawing.Point(0, intX + 20), new System.Drawing.Point(200, intX + 20));

            e.Graphics.DrawString("应收：" + string.Format("{0:N}", tbTotal.Text), new Font("微软雅黑", 10, System.Drawing.FontStyle.Bold), Brushes.Black, 0, intX + 20);
            e.Graphics.DrawString("实收：" + string.Format("{0:N}", tbAmount.Text), new Font("微软雅黑", 10, System.Drawing.FontStyle.Bold), Brushes.Black, 100, intX + 20);

            e.Graphics.DrawString("现金：" + string.Format("{0:N}", tbPay_Cash.Text), new Font("微软雅黑", 10, System.Drawing.FontStyle.Bold), Brushes.Black, 0, intX + 40);
            e.Graphics.DrawString("卡付：" + string.Format("{0:N}", tbPay_Bank.Text), new Font("微软雅黑", 10, System.Drawing.FontStyle.Bold), Brushes.Black, 100, intX + 40);
            e.Graphics.DrawString("免单：" + string.Format("{0:N}", tbPay_Exempt.Text), new Font("微软雅黑", 10, System.Drawing.FontStyle.Bold), Brushes.Black, 0, intX + 60);
            e.Graphics.DrawString("活动：" + string.Format("{0:N}", tbPay_Coupon.Text), new Font("微软雅黑", 10, System.Drawing.FontStyle.Bold), Brushes.Black, 100, intX + 60);

            e.Graphics.DrawString("会员：" + string.Format("{0:N}", tbPay_Member.Text), new Font("微软雅黑", 10, System.Drawing.FontStyle.Bold), Brushes.Black, 0, intX + 80);
            e.Graphics.DrawString("余额：" + string.Format("{0:N}", tbMember_Balance.Text), new Font("微软雅黑", 10, System.Drawing.FontStyle.Bold), Brushes.Black, 100, intX + 80);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgDetail.SelectedItems.Count > 0)
            {
                if (Public.Sys_MsgYN("是否确定删除选定项？"))
                {
                    DataRowView d = (DataRowView)dgDetail.SelectedItem;

                    DB_Work.ExecuteCmd(string.Format("DELETE FROM Bills_Detail WHERE id={0}", d.Row[0]));

                    Load_Bills_Detail(tbOrder_ID.Text);
                }
            }
        }

        private void PrintingVegetable_PrintPage(object sender, PrintPageEventArgs e)
        {
            var pen = new Pen(Brushes.Black, 1);

            DataSet Ds = DB_Work.DataSetCmd(String.Format("SELECT DateTime,Item_Name,SUM(Num),Unit,Gift FROM Printing WHERE Order_ID='{0}' AND Printing='菜品打印机' AND [Print]=0 GROUP BY DateTime,Item_Name,Unit,Gift", tbOrder_ID.Text));

            e.Graphics.DrawString("菜品打印机", new Font("微软雅黑", 18, System.Drawing.FontStyle.Bold), Brushes.Black, 80, 0);

            e.Graphics.DrawString("客单：" + tbOrder_ID.Text, new Font("微软雅黑", 16), Brushes.Black, 0, 30);
            e.Graphics.DrawString("餐台号：" + tbTable.Text, new Font("微软雅黑", 16), Brushes.Black, 0, 60);
            e.Graphics.DrawString("时间：" + Convert.ToDateTime(Ds.Tables[0].Rows[0][0]).ToString("yyyy-MM-dd HH:mm"), new Font("微软雅黑", 16), Brushes.Black, 0, 90);

            e.Graphics.DrawLine(pen, new System.Drawing.Point(0, 120), new System.Drawing.Point(300, 120));

            e.Graphics.DrawString("名称", new Font("微软雅黑", 16, System.Drawing.FontStyle.Underline), Brushes.Black, 0, 120);
            e.Graphics.DrawString("数量", new Font("微软雅黑", 16, System.Drawing.FontStyle.Underline), Brushes.Black, 170, 120);
            e.Graphics.DrawString("金额", new Font("微软雅黑", 16, System.Drawing.FontStyle.Underline), Brushes.Black, 230, 120);

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

            e.Graphics.DrawLine(pen, new System.Drawing.Point(0, intX + 30), new System.Drawing.Point(300, intX + 30));
        }

        private void PrintingBBQ_PrintPage(object sender, PrintPageEventArgs e)
        {
            var pen = new Pen(Brushes.Black, 1);

            DataSet Ds = DB_Work.DataSetCmd(String.Format("SELECT DateTime,Item_Name,SUM(Num),Unit,Gift FROM Printing WHERE Order_ID='{0}' AND Printing='烤串打印机' AND [Print]=0 GROUP BY DateTime,Item_Name,Unit,Gift", tbOrder_ID.Text));

            e.Graphics.DrawString("烤串打印机", new Font("微软雅黑", 18, System.Drawing.FontStyle.Bold), Brushes.Black, 80, 0);

            e.Graphics.DrawString("客单：" + tbOrder_ID.Text, new Font("微软雅黑", 16), Brushes.Black, 0, 30);
            e.Graphics.DrawString("餐台号：" + tbTable.Text, new Font("微软雅黑", 16), Brushes.Black, 0, 60);
            e.Graphics.DrawString("时间：" + Convert.ToDateTime(Ds.Tables[0].Rows[0][0]).ToString("yyyy-MM-dd HH:mm"), new Font("微软雅黑", 16), Brushes.Black, 0, 90);

            e.Graphics.DrawLine(pen, new System.Drawing.Point(0, 120), new System.Drawing.Point(300, 120));

            e.Graphics.DrawString("名称", new Font("微软雅黑", 16, System.Drawing.FontStyle.Underline), Brushes.Black, 0, 120);
            e.Graphics.DrawString("数量", new Font("微软雅黑", 16, System.Drawing.FontStyle.Underline), Brushes.Black, 170, 120);
            e.Graphics.DrawString("金额", new Font("微软雅黑", 16, System.Drawing.FontStyle.Underline), Brushes.Black, 230, 120);

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

            e.Graphics.DrawLine(pen, new System.Drawing.Point(0, intX + 30), new System.Drawing.Point(300, intX + 30));
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbMember_ID.Text != string.Empty)
            {
                dgItems.ItemsSource = DB_Work.DataSetCmd(string.Format("SELECT id,Name,Member_Price,Type FROM Items WHERE dbo.fun_getPY(Name) LIKE N'%{0}%'", tbSearch.Text)).Tables[0].DefaultView;
            }
            else
            {
                dgItems.ItemsSource = DB_Work.DataSetCmd(string.Format("SELECT id,Name,Normal_Price,Type FROM Items WHERE dbo.fun_getPY(Name) LIKE N'%{0}%'", tbSearch.Text)).Tables[0].DefaultView;
            }
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (dgItems.Items.Count > 0)
                {
                    DataRowView d = (DataRowView)dgItems.Items[0];

                    点菜信息 i = new 点菜信息();
                    i.tbPrice.Text = d.Row[2].ToString();
                    i.tbNum.Text = "1.00";
                    i.ShowDialog();

                    if (i.DialogResult.Value)
                    {
                        DB_Work.ExecuteCmd(string.Format("INSERT INTO Bills_Detail(Order_ID,Item_ID,Num,Price,Total,Discount,Amount,Gift,[Save]) VALUES('{0}',{1},{2},{3},{4},{5},{6},'{7}',0)",
                                tbOrder_ID.Text, d.Row[0], Convert.ToDecimal(i.tbNum.Text), Convert.ToDecimal(i.tbPrice.Text), Convert.ToDecimal(i.tbPrice.Text) * Convert.ToDecimal(i.tbNum.Text), 1, Convert.ToDecimal(i.tbPrice.Text) * Convert.ToDecimal(i.tbNum.Text), false));

                        Load_Bills_Detail(tbOrder_ID.Text);
                    }
                }

                tbSearch.Focus();
                tbSearch.SelectAll();
            }
        }

        private void lvItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgItems.SelectedItems.Count > 0)
            {
                DataRowView d = (DataRowView)dgItems.SelectedItem;

                点菜信息 i = new 点菜信息();
                i.tbPrice.Text = d.Row[2].ToString();
                i.tbNum.Text = "1.00";
                i.ShowDialog();

                if (i.DialogResult.Value)
                {
                    DB_Work.ExecuteCmd(string.Format("INSERT INTO Bills_Detail(Order_ID,Item_ID,Num,Price,Total,Discount,Amount,Gift,[Save]) VALUES('{0}',{1},{2},{3},{4},{5},{6},'{7}',0)",
                            tbOrder_ID.Text, d.Row[0], Convert.ToDecimal(i.tbNum.Text), Convert.ToDecimal(i.tbPrice.Text), Convert.ToDecimal(i.tbPrice.Text) * Convert.ToDecimal(i.tbNum.Text), 1, Convert.ToDecimal(i.tbPrice.Text) * Convert.ToDecimal(i.tbNum.Text), false));

                    Load_Bills_Detail(tbOrder_ID.Text);
                }

                tbSearch.Focus();
                tbSearch.SelectAll();
            }
        }

        private void btnGift_Click(object sender, RoutedEventArgs e)
        {
            DataRowView d = (DataRowView)dgDetail.SelectedItem;

            if (d.Row[9].ToString() == "赠")
            {
                DB_Work.ExecuteCmd(string.Format("UPDATE Bills_Detail SET Gift=0,Amount=Price*Num*Discount WHERE id={0} AND Order_ID='{1}'", d.Row[0], tbOrder_ID.Text));
            }
            else
            {
                DB_Work.ExecuteCmd(string.Format("UPDATE Bills_Detail SET Gift=1,Amount=0 WHERE id={0} AND Order_ID='{1}'", d.Row[0], tbOrder_ID.Text));
            }

            Load_Bills_Detail(tbOrder_ID.Text);
        }

        private void btnMember_Click(object sender, RoutedEventArgs e)
        {
            会员查找 m = new 会员查找();
            m.ShowDialog();

            if (m.DialogResult.Value)
            {
                tbMember_ID.Text = m.strMember_ID;
                tbMember_Name.Text = m.strMember_Name;
                tbMember_Balance.Text = m.dMember_Balance.ToString();

                Load_Items();

                DB_Work.ExecuteCmd(string.Format("UPDATE Bills_Detail SET Price=Items.Member_Price,Total=Items.Member_Price*Bills_Detail.Num,Amount=(CASE WHEN Bills_Detail.Gift=1 THEN 0 ELSE Items.Member_Price*Bills_Detail.Num END) FROM Bills_Detail,Items WHERE Order_ID='{0}' AND Bills_Detail.Item_ID=Items.id", tbOrder_ID.Text));

                Load_Bills_Detail(tbOrder_ID.Text);
            }
        }

        private void btnPreCheckout_Click(object sender, RoutedEventArgs e)
        {
            PrintDocument PrintPreCheckout = new PrintDocument();
            PrintPreCheckout.PrinterSettings.PrinterName = AppSetter.Printing_Checkout;

            PrintPreCheckout.PrintPage += new PrintPageEventHandler(PrintPreCheckout_PrintPage);

            PrintPreCheckout.Print();
        }

        private void PrintPreCheckout_PrintPage(object sender, PrintPageEventArgs e)
        {
            var pen = new Pen(Brushes.Black, 1);

            e.Graphics.DrawString("烤烤乐园预结单", new Font("微软雅黑", 12, System.Drawing.FontStyle.Bold), Brushes.Black, 30, 0);

            e.Graphics.DrawString("客单编码：" + tbOrder_ID.Text, new Font("微软雅黑", 10), Brushes.Black, 0, 20);
            e.Graphics.DrawString("餐台号：" + tbTable.Text, new Font("微软雅黑", 10), Brushes.Black, 0, 40);
            e.Graphics.DrawString("开台时间：" + tbStart.Text, new Font("微软雅黑", 10), Brushes.Black, 0, 60);
            e.Graphics.DrawString("结账时间：" + tbEnd.Text, new Font("微软雅黑", 10), Brushes.Black, 0, 80);

            e.Graphics.DrawString(string.Format("会员：{0},{1}", tbMember_ID.Text, tbMember_Name.Text), new Font("微软雅黑", 10), Brushes.Black, 0, 100);

            e.Graphics.DrawLine(pen, new System.Drawing.Point(0, 120), new System.Drawing.Point(200, 120));
            e.Graphics.DrawString("名称", new Font("微软雅黑", 10, System.Drawing.FontStyle.Underline), Brushes.Black, 0, 120);
            e.Graphics.DrawString("单价", new Font("微软雅黑", 10, System.Drawing.FontStyle.Underline), Brushes.Black, 0, 140);
            e.Graphics.DrawString("数量", new Font("微软雅黑", 10, System.Drawing.FontStyle.Underline), Brushes.Black, 80, 140);
            e.Graphics.DrawString("金额", new Font("微软雅黑", 10, System.Drawing.FontStyle.Underline), Brushes.Black, 140, 140);

            var intX = 140;

            DataSet Ds = DB_Work.DataSetCmd(string.Format("SELECT Items.Name,Items.Type,Bills_Detail.Price,SUM(Bills_Detail.Num),SUM(Bills_Detail.Amount) FROM Bills_Detail LEFT JOIN Items ON Items.id=Bills_Detail.Item_ID WHERE Order_ID='{0}' GROUP BY Items.Name,Items.Type,Bills_Detail.Price", tbOrder_ID.Text));

            for (var i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                intX = intX + 20;

                e.Graphics.DrawString(Ds.Tables[0].Rows[i][0].ToString(), new Font("微软雅黑", 10), Brushes.Black, 0, intX);

                intX = intX + 20;

                e.Graphics.DrawString(Ds.Tables[0].Rows[i][2].ToString(), new Font("微软雅黑", 10), Brushes.Black, 0, intX);
                e.Graphics.DrawString(Ds.Tables[0].Rows[i][3].ToString(), new Font("微软雅黑", 10), Brushes.Black, 80, intX);
                e.Graphics.DrawString(Ds.Tables[0].Rows[i][4].ToString(), new Font("微软雅黑", 10), Brushes.Black, 140, intX);
            }

            e.Graphics.DrawLine(pen, new System.Drawing.Point(0, intX + 20), new System.Drawing.Point(200, intX + 20));

            e.Graphics.DrawString("应收：" + string.Format("{0:N}", tbTotal.Text), new Font("微软雅黑", 10, System.Drawing.FontStyle.Bold), Brushes.Black, 0, intX + 20);
            e.Graphics.DrawString("实收：" + string.Format("{0:N}", tbAmount.Text), new Font("微软雅黑", 10, System.Drawing.FontStyle.Bold), Brushes.Black, 100, intX + 20);
        }

        private void lvDetail_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgDetail.SelectedItems.Count > 0)
            {
                DataRowView d = (DataRowView)dgDetail.SelectedItem;

                if (d.Row[10].ToString() == string.Empty || !Convert.ToBoolean(d.Row[10]))
                {
                    点菜信息 i = new 点菜信息();
                    i.intID = Convert.ToInt32(d.Row[0]);
                    i.tbPrice.Text = d.Row[5].ToString();
                    i.tbNum.Text = d.Row[3].ToString();
                    i.ShowDialog();

                    if (i.DialogResult.Value)
                    {
                        DB_Work.ExecuteCmd(string.Format("UPDATE Bills_Detail SET Price={0},Num={1} WHERE id={2}", i.tbPrice.Text, i.tbNum.Text, i.intID));

                        Load_Bills_Detail(tbOrder_ID.Text);
                    }

                    tbSearch.Focus();
                    tbSearch.SelectAll();
                }
            }
        }
    }
}
