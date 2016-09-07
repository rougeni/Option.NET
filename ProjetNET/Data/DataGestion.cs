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
                System.Console.WriteLine(appellation);
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
                System.Console.WriteLine(appellation);
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
        
        public List<DataFeed> getDataFeedClass(DateTime date_debut)
        {
            BaseDataContext baseData = new BaseDataContext();
            //récuperer la liste des id d'options
            List<String> list_id = getListofID();
            //récuperer le nombre de jours entre date_debut et date_courante
            DateTime thisDay = DateTime.Today;
             TimeSpan diff_time = date_debut.Subtract(thisDay);
             int days_to_print = diff_time.Days;
            //Pour chaque action réccupérer la liste des actions
            double[,] pre_list = new double[list_id.Count(),days_to_print];
            int a = 0;
             List<DataFeed> obj_ret = new List<DataFeed>();
            for(int i =0; i<days_to_print; i++){

             foreach (var iden in list_id)
             {
                 pre_list[a,] = getCotation(iden, days_to_print);
                 a=a+1;
             }
            
                Dictionary<string, decimal> one_action = new Dictionary<string,decimal>();
                int b = 0;
                foreach (var iden in list_id)
             {
                 getCotation(iden, days_to_print);
                 b=b+1;
             }
                one_action.Add()
                DataFeed one_day = new DataFeed(date_debut,one_action);
                obj_ret.Add(one_day);
            }
            return(obj_ret);
        
        * * */
        }
         
        #endregion Public Methods


    }
}

