using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetNET.Models;
using Prism.Mvvm;
using OxyPlot;
using OxyPlot.Series;

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
            var model = new PlotModel { Title = "Example 1"  };
            model.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
            MyModel = model;
            
        }

        #endregion Private Fields

        public void Test()
        {
            MyModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
            MyModel.InvalidatePlot(true);
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
