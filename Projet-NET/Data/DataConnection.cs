using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.Data
{
    public class DataConnection
    {
        #region Public Methods

        public void ConnectToSql()
        {
            System.Data.SqlClient.SqlConnection conn =
                new System.Data.SqlClient.SqlConnection();
            // TODO: Modify the connection string and include any
            // additional required properties for your database.
            conn.ConnectionString =
             "integrated security=SSPI;data source=SQL Server Name;" +
             "persist security info=False;initial catalog=northwind" +
             "Server=ingefin.ensimag.fr;Database=DotNetDB;User Id=etudiant; Password=edn!2015;";
            try
            {
                conn.Open();
                // Insert code to process data.
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Failed to connect to data source" + ex);
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion Public Methods
    }
}

