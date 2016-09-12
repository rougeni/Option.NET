using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PricingLibrary;
using PricingLibrary.Utilities.MarketDataFeed;
using PricingLibrary.FinancialProducts;

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
        }
        #region Public Methods

        /**
        * méthode qui retourne une liste de DataFeed
        */
        public List<DataFeed> getListDataField(DateTime startDate, DateTime endTime, Share[] underlyingShares)
        {
            string[] ids = new string[underlyingShares.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = underlyingShares[i].Id;
            }
            //List<DataFeed> listDF = null;
            using (var dc = new BaseDataContext())
            {
                var tmp = dc.HistoricalShareValues.Where(el => ids.Contains(el.id.Trim()) && el.date >= startDate && el.date <= endTime ).Select(el => new ShareValue(el.id, el.date, el.value)).ToList();
                var listDF = tmp.GroupBy(d => d.DateOfPrice,
                                     t => new { Symb = t.Id, Val = t.Value },
                                     (key, g) => new DataFeed(key, g.ToDictionary(e => e.Symb, e => e.Val)));
                return listDF.ToList();
            }
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

        #endregion Public Methods


    }
}



