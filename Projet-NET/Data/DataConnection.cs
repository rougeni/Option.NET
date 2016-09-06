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

        /**
         * méthode qui récupère les noms et id des différentes fonctions
         * */
        public String[,] getIDNames()
        {
            return null;
        }

        /**
         * méthode qui récupère le nom a partir de l'ID
         * */
        public String getName(String ID)
        {
            return null;
        }

        /** 
         * Méthode qui renvoie l'ID a partir du nom
         * */
        public String getID(String nom)
        {
            return null;
        }

        /**
         * méthode qui donne les cotations pour une action donnée
         * et pour une durée (nombre de jours ouvrés)
         * */
        public int[] getCotation(String ID, int duree)
        {
            return null;
        }

        #endregion Public Methods
    }
}

