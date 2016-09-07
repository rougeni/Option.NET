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
            BaseDataContext baseData = new BaseDataContext();
            List<String> identifiants = getListofID();
            int nbr = identifiants.Count;
            String[,] table = new String[nbr,2];
            int a =0;
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
        #endregion Public Methods


    }
}

