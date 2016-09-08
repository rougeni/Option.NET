using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetNET.Models;
using Prism.Mvvm;
using OxyPlot;
using OxyPlot.Series;
using PricingLibrary.Computations;

namespace ProjetNET.ViewModels
{
    internal class ViewFacade : BindableBase
    {
        #region Private Fields

        private Facade underlyingFacade;

        private  PlotModel myModel;
        public IList<DataPoint> Points { get; private set; }

        public ViewFacade(Facade facade)
        {
            underlyingFacade = facade;
            MyModel = new PlotModel { Title = "Example 1" };
            
        }

        #endregion Private Fields

        public void Launch() {        
            underlyingFacade.update();
            MyModel = ToObservableView(underlyingFacade.ListePricingResult, underlyingFacade.ListePortefeuille);

        }

        private PlotModel ToObservableView(List<PricingResults> pricingResults, List<Portefeuille> portefeuilles)
        {

            Console.WriteLine("Size " + pricingResults.Count + " " + portefeuilles.Count);

            PlotModel model = new PlotModel();

            LineSeries plotOption = new OxyPlot.Series.LineSeries();
            LineSeries plotPortefeuille = new OxyPlot.Series.LineSeries();

            drawOption(pricingResults, plotOption);
            drawPortefeuille(portefeuilles, plotPortefeuille);
            model.Series.Add(plotOption);
            model.Series.Add(plotPortefeuille);

            return model;
        }

        private void drawPortefeuille(List<Portefeuille> portefeuilles, LineSeries plotPortefeuille)
        {
            int i = 0;
            Console.WriteLine("View Facade " + portefeuilles.Count);
            foreach (Portefeuille port in portefeuilles)
            {

                plotPortefeuille.Points.Add(new DataPoint(i++, port.Valeur));
            }
        }

        private void drawOption(List<PricingResults> pricingResults, LineSeries plotOption)
        {
            int i = 0;
            foreach (PricingResults option in pricingResults)
            {
                plotOption.Points.Add(new DataPoint(i++, option.Price));
            }
        }

        public PlotModel MyModel
        {
            get {return myModel;}  
            private set 
            {
                SetProperty(ref myModel, value);
            }
        }

        public Facade UnderlyingFacade { get { return underlyingFacade; } }


    }
}
