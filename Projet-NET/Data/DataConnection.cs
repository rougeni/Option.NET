﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.Data
{
    public class DataConnection
    {
        #region Public Methods

        public bool ConnectToSql()
        {
            bool connectionReussi = false;
            System.Data.SqlClient.SqlConnection conn =
                new System.Data.SqlClient.SqlConnection();
            // TODO: Modify the connection string and include any
            // additional required properties for your database.
            conn.ConnectionString = "Data Source=ingefin.ensimag.fr;Initial Gestion=DotNetDB;User ID=etudiant;Password=edn!2015";
            try
            {
                conn.Open();
                connectionReussi = true;
                // Insert code to process data.
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Failed to connect to data source" + ex);
            }
            finally
            {
                conn.Close();
                /*
                 * lgnes de commmentaires
                 * pour tester git
                 * ça commence à me 
                 * souler */
            }
            return connectionReussi;
        }

        #endregion Public Methods
    }
}

