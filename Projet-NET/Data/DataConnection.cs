using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.Data
{
    public class DataConnection
    {

        public DataConnection()
        {
            
        }
 
        public List<String> ListofNames;


        #region Public Methods

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
         * a partir de son ID
         * et pour une durée (nombre de jours ouvrés)
         * */
        public int[] getCotation(String ID, int duree)
        {
            var cotations = from p in this.bd.HistoricalShareValues
                            where p.id == ID
                            orderby p.date
                            select p.value;
            int[] tableauCotation = new int[duree];
            int compteur = 0;
            foreach (var cote in cotations){
                compteur++;
                tableauCotation[compteur] = (int)cote;
            }
                return tableauCotation;
        }
        public void getNames()
        {
            //ListofNames = new List<String>();
            BaseDataContext connection = new BaseDataContext();
            var un_nom = from p in this.bd.ShareNames
                         select p;

            System.Console.WriteLine(un_nom);
            return;
        }
        #endregion Public Methods


    }
}

