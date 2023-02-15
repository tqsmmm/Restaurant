using System;
using System.Data;
using System.Data.SqlClient;

namespace 龙岩餐饮管理系统
{
    class DB_Work
    {
        public static bool ExecuteCmd(string strSQL)
        {
            try
            {
                var CmdObj = new SqlCommand(strSQL, AppSetter.SqlConn);
                AppSetter.SqlConn.Open();
                CmdObj.ExecuteNonQuery();
                AppSetter.SqlConn.Close();

                return true;
            }
            catch (Exception Ex)
            {
                Public.Sys_MsgBox(Ex.Message);
                return false;
            }
            finally
            {
                if (AppSetter.SqlConn.State != ConnectionState.Closed)
                {
                    AppSetter.SqlConn.Close();
                }
            }
        }
        public static DataSet DataSetCmd(string strSQL)
        {
            try
            {
                var Ds = new DataSet();
                var DaPayType = new SqlDataAdapter(strSQL, AppSetter.SqlConn);
                DaPayType.Fill(Ds, "Table");
                AppSetter.SqlConn.Close();

                return Ds;
            }
            catch (Exception e)
            {
                Public.Sys_MsgBox(e.Message);
                return null;
            }
            finally
            {
                if (AppSetter.SqlConn.State != ConnectionState.Closed)
                {
                    AppSetter.SqlConn.Close();
                }
            }
        }

        public static bool ConnectionTest(SqlConnection strConn)
        {
            try
            {
                if (strConn.State != ConnectionState.Open)
                {
                    strConn.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (strConn.State != ConnectionState.Closed)
                {
                    strConn.Close();
                }
            }
        }
    }
}