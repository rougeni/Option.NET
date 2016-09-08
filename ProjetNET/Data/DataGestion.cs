using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary;
using PricingLibrary.Utilities.MarketDataFeed;

namespace ProjetNET.Data
{
    public class DataGestion
    {
        public DataGestion()
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
            /*
             * lol
             * */
        }
        #region Public Methods

        /**
         * méthode qui récupère les noms et id des différentes fonctions
         * */
        public String[,] getIDNames()
        {
            BaseDataContext baseData = new BaseDataContext();
            List<String> identifiants = getListofID();
            int nbr = identifiants.Count;
            String[,] table = new String[nbr, 2];
            int a = 0;
            foreach (var iden in identifiants)
            {
                table[a, 0] = iden;
                table[a, 1] = getName(iden);
                a = a + 1;
            }
            return table;

        }

        /**
         * méthode qui récupère le nom a partir de l'ID
         * */
        public String getName(String ID)
        {
            BaseDataContext baseData = new BaseDataContext();
            var action_name = from p in baseData.ShareNames
                              where p.id == ID
                              select p.name;
            return action_name.FirstOrDefault();
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
         * les valeurs les plus récentes sont en premier dans le tableau
         * */
        public double[] getCotation(String ID, int duree)
        {
            BaseDataContext baseData = new BaseDataContext();
            var cotations = from p in baseData.HistoricalShareValues
                            where p.id == ID
                            orderby p.date descending
                            select p.value;
            double[] tableauCotation = new double[duree];
            int compteur = 0;
            foreach (var cote in cotations)
            {
                tableauCotation[compteur] = (double)cote;
                compteur++;
                if (compteur >= duree)
                {
                    return tableauCotation;
                }
            }
            return tableauCotation;
        }

        /**
        * méthode qui retourne la liste des actions
        * */
        public List<String> getListofID()
        {
            BaseDataContext baseData = new BaseDataContext();
            var NomsActions = (from p in baseData.ShareNames
                               select p.id);
            List<String> appellations = new List<String>();
            foreach (var appellation in NomsActions)
            {
                appellations.Add(appellation);
                //string affiche = (appellation);
                //System.Console.WriteLine(appellation);
            }
            return appellations;
        }

        /**
        * méthode qui retourne le nom des actions
        * */
        public List<String> getListofNames()
        {
            BaseDataContext baseData = new BaseDataContext();
            var NomsActions = (from p in baseData.ShareNames
                               select p.name);
            List<String> appellations = new List<String>();
            
            foreach (var appellation in NomsActions)
            {
                appellations.Add(appellation);
                //System.Console.WriteLine(appellation);
            }
            return appellations;
        }
        
        /**
        * méthode qui retourne une liste de DataFeedClass
        * */
        public double getDayCotation(DateTime date_debut, String iden)
        {
            BaseDataContext baseData = new BaseDataContext();
            var cotation = from p in baseData.HistoricalShareValues
                            where p.id == iden && p.date == date_debut
                            select p.value;
            return (double) cotation.FirstOrDefault();
        }

        /**
        * méthode qui retourne une liste de DataFeedClass
        */
        public DataFeed getDataFeedClass(DateTime date)
        {
            BaseDataContext baseData = new BaseDataContext();
            var liste = from p in baseData.HistoricalShareValues
                        where p.date == date
                        orderby p.id
                        select p;

            Dictionary<string,decimal> dico = new Dictionary<string,decimal>();
            foreach (var elt in liste)
            {
                dico.Add(elt.id,elt.value);   
            }
            DataFeed dataF = new DataFeed(date, dico);
            return dataF;
        }

        public List<DataFeed> getListDataField(DateTime dateStart)
        {
            List<DataFeed> listeData = new List<DataFeed>();
            BaseDataContext baseData = new BaseDataContext();
            var listeDate = from p in baseData.HistoricalShareValues
                            where p.date >= dateStart
                            orderby p.date
                            group p by p.date into q
                            select q.Key;
            foreach (var date in listeDate)
            {
                listeData.Add(getDataFeedClass(date));
            }
            return listeData;
        }


        public int numberOfAssets()
        {
            BaseDataContext baseData = new BaseDataContext();
            var listeAssets = from p in baseData.ShareNames
                              orderby p.id
                              group p by p.id into q
                              select q;
            int a = 0;
            foreach (var iden in listeAssets)
            {
                a++;
            }
            return a;
        }

        public DateTime lastDay()
        {
            BaseDataContext baseData = new BaseDataContext();
            var lday = from p in baseData.HistoricalShareValues
                       orderby p.date
                       select p.date;
            return lday.ToArray().Last();
        }

        public double[] lastValues()
        {
            BaseDataContext baseData = new BaseDataContext();
            var lvalues = from p in baseData.HistoricalShareValues
                          where p.date == this.lastDay()
                          select p.value;
            double[] rv = new double[this.numberOfAssets()];
            int a = 0;
            foreach (var v in lvalues)
            {
                rv[a] = (double)v;
                a++;
            }
            return rv;
        }

        public double getCotation(string id, DateTime date)
        {
            BaseDataContext baseData = new BaseDataContext();
            var cote = (from p in baseData.HistoricalShareValues
                        where p.id == id && p.date == date
                        select p.value).FirstOrDefault();
            return (double)cote;
        }

        #endregion Public Methods


    }
}

