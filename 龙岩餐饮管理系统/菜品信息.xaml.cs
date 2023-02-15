using System.Data;
using System.Windows;
using System.Windows.Input;

namespace 龙岩餐饮管理系统
{
    /// <summary>
    /// 菜品信息.xaml 的交互逻辑
    /// </summary>
    public partial class 菜品信息 : Window
    {
        public 菜品信息()
        {
            InitializeComponent();
        }

        public int intID = 0;

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (Title == "新建菜品信息")
            {
                DB_Work.ExecuteCmd(string.Format("INSERT INTO Items(Code,Name,Normal_Price,Member_Price,Unit,Type,Printing) VALUES('{0}','{1}',{2},{3},'{4}','{5}','{6}')", tbCode.Text, tbName.Text, tbNormal_Price.Text, tbMember_Price.Text, tbUnit.Text, tbType.Text, tbPrinting.Text));

                DialogResult = true;
            }
            else if (Title == "修改菜品信息")
            {
                DB_Work.ExecuteCmd(string.Format("UPDATE Items SET Code='{0}',Name='{1}',Normal_Price={2},Member_Price={3},Unit='{4}',Type='{5}',Printing='{6}' WHERE id={7}", tbCode.Text, tbName.Text, tbNormal_Price.Text, tbMember_Price.Text, tbUnit.Text, tbType.Text, tbPrinting.Text, intID));

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataSet Ds = DB_Work.DataSetCmd("SELECT Name FROM Items_Types");

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                tbType.Items.Add(Ds.Tables[0].Rows[i][0].ToString());
            }

            if (Title == "修改菜品信息")
            {
                Ds = DB_Work.DataSetCmd(string.Format("SELECT Code,Name,Normal_Price,Member_Price,Unit,Type,Printing FROM Items wHERE id={0}", intID));

                if (Ds.Tables[0].Rows.Count > 0)
                {
                    tbCode.Text = Ds.Tables[0].Rows[0][0].ToString();
                    tbName.Text = Ds.Tables[0].Rows[0][1].ToString();
                    tbNormal_Price.Text = Ds.Tables[0].Rows[0][2].ToString();
                    tbMember_Price.Text = Ds.Tables[0].Rows[0][3].ToString();
                    tbUnit.Text = Ds.Tables[0].Rows[0][4].ToString();
                    tbType.Text = Ds.Tables[0].Rows[0][5].ToString();
                    tbPrinting.Text = Ds.Tables[0].Rows[0][6].ToString();
                }
            }
        }
    }
}
