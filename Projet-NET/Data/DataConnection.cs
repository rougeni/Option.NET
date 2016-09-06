using Projet_NET.Data;
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

        public bool ConnectToSql()
        {
            bool connectionReussi = false;

            try
            {
                BaseDataContext bd = new BaseDataContext();
                connectionReussi = true;
                // Insert code to process data.
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Failed to connect to data source" + ex);
            }
            return connectionReussi;
        }

        #endregion Public Methods
    }
}

