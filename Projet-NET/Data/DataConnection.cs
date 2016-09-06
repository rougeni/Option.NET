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
            set{}
 
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
        public string getID(String nom)
        {
            BaseDataContext baseData = new BaseDataContext();
            var id = from p in baseData.ShareNames
                     where p.name == nom
                     select p.id;
            return id.FirstOrDefault();
        }

        /**
         * méthode qui donne les cotations pour une action donnée
         * a partir de son ID
         * et pour une durée (nombre de jours ouvrés)
         * */
        public double[] getCotation(String ID, int duree)
        {
            BaseDataContext baseData = new BaseDataContext();
            var cotations = from p in baseData.HistoricalShareValues
                            where p.id == ID
                            orderby p.date
                            select p.value;
            double[] tableauCotation = new double[duree];
            int compteur = 0;
            foreach (var cote in cotations){
                tableauCotation[compteur] = (double)cote;
                compteur++;
                if (compteur >= duree)
                {
                    return tableauCotation;
                }
            }
                return tableauCotation;
        }

        public List<String> getNames()
        {
            BaseDataContext baseData = new BaseDataContext();
            var NomsActions = (from p in baseData.ShareNames
                               select p.id);
            List<String> appellations = new List<String>();
            foreach (var appellation in NomsActions)
            {
                appellations.Add(appellation);
                //string affiche = (appellation);
                System.Console.WriteLine(appellation);
            }
            return appellations;
        }

        #endregion Public Methods


    }
}

