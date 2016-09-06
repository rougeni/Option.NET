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
            this.bd = new BaseDataContext();
        }
        public BaseDataContext bd
        {
            get
            {
                return this.bd;
            }
            set { }

        }
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
            BaseDataContext baseData = new BaseDataContext();
            string id = (from p in baseData.ShareNames
                         where p.name == nom
                         select p.id).FirstOrDefault();
            return id;
        }

        /**
         * méthode qui donne les cotations pour une action donnée
         * a partir de son ID
         * et pour une durée (nombre de jours ouvrés)
         * */
        public int[] getCotation(String ID, int duree)
        {
            BaseDataContext baseData = new BaseDataContext();
            var cotations = from p in baseData.HistoricalShareValues
                            where p.id == ID
                            orderby p.date
                            select p.value;
            int[] tableauCotation = new int[duree];
            int compteur = 0;
            foreach (var cote in cotations)
            {
                tableauCotation[compteur] = (int)cote;
                compteur++;
                if (compteur >= duree)
                {
                    return tableauCotation;
                }
            }
            return tableauCotation;
        }

        public void getNames()
        {
            BaseDataContext baseData = new BaseDataContext();
            var NomsActions = (from p in baseData.ShareNames
                               select p.name);
            foreach (var appellation in NomsActions)
            {
                string affiche = (appellation);
                System.Console.WriteLine(affiche);
            }
        }
        #endregion Public Methods
        /*public List<String> ListofNames;
        public  List<String> getNames(){
            ListofNames = new List<String>();
            un_nom = from Sharename in 
        }*/

    }
}

