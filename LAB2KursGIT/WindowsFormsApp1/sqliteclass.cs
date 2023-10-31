using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
//using Finisar.SQLite;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    class sqliteclass
    {
        //Конструктор
        public sqliteclass()
        {
        }
        #region iExecuteNonQuery
        public int iExecuteNonQuery(string FileData, string sSql, int where)
        {
            int n = 0;
            try
            {
                using (SQLiteConnection con = new SQLiteConnection())
                {
                    if (where == 0)
                    {
                        con.ConnectionString = @"Data Source=" + FileData + ";New=True;Version=3";
                    }
                    else
                    {
                        con.ConnectionString = @"Data Source=" + FileData + ";New=False;Version=3";
                    }
                    con.Open();
                    using (SQLiteCommand sqlCommand = con.CreateCommand())
                    {
                        sqlCommand.CommandText = sSql;
                        n = sqlCommand.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                n = 0;
            }
            return n;
        }
        #endregion
        #region drExecute
        public DataRow[] drExecute(string FileData, string sSql)
        {
            DataRow[] datarows = null;
            SQLiteDataAdapter dataadapter = null;
            DataSet dataset = new DataSet();
            DataTable datatable = new DataTable();
            try
            {
                using (SQLiteConnection con = new SQLiteConnection())
                {
                    con.ConnectionString = @"Data Source=" + FileData + ";New=False;Version=3";
                    con.Open();
                    using (SQLiteCommand sqlCommand = con.CreateCommand())
                    {
                        dataadapter = new SQLiteDataAdapter(sSql, con);
                        dataset.Reset();
                        dataadapter.Fill(dataset);
                        datatable = dataset.Tables[0];
                        datarows = datatable.Select();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                datarows = null;
            }
            return datarows;
        }
        #endregion
    }
}
