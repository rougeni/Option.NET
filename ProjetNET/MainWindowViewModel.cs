using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows.Threading;
using ProjetNET.ViewModels;
using PricingLibrary.FinancialProducts;

namespace ProjetNET
{
    internal class MainWindowViewModel : BindableBase
    {
        #region Private Fields

        private WholeViewModel wholeView;

        private IGenerateHistoryViewModel selectedTesting;

        private bool fieldCompleted;

        private IPricingViewModel selectedPricing;
        private String maturity;
        private String strike;
        private String spot;
        private String startDate;

        #endregion Private Fields

        public ObservableCollection<IGenerateHistoryViewModel> TestGenerateHistory { get; private set; }

        public ObservableCollection<IPricingViewModel> PricingMethods { get; private set; }

        public ObservableCollection<ActionCheckBox> AvailableAction { get; private set; }

        public String Maturity
        {
            get { return maturity; }
            set
            {
                SetProperty(ref maturity, value);
            }
        }

        public String Strike
        {
            get { return strike; }
            set
            {
                SetProperty(ref strike, value);
            }
        }

        public String Spot
        {
            get { return spot; }
            set
            {
                SetProperty(ref spot, value);
            }
        }

        public DelegateCommand StartCommand { get; private set; }        

        #region Public Constructors

        public MainWindowViewModel()
        {
            StartCommand = new DelegateCommand(StartAnalyse, CanLaunch);
            maturity = "01/01/2016";
            startDate = "14/08/2014";
            spot = "20";
            strike = "10";

            wholeView = new WholeViewModel();
            BackTestGenerateHistoryVM backTest = new BackTestGenerateHistoryVM();
            ForwardTestGenerateHistoryVM forwardTest = new ForwardTestGenerateHistoryVM();
            selectedTesting = backTest;

            List<IGenerateHistoryViewModel> myListGenerator = new List<IGenerateHistoryViewModel>() { backTest, forwardTest };
            TestGenerateHistory = new ObservableCollection<IGenerateHistoryViewModel>(myListGenerator);

            VanillaCallPricingVM vanille = new VanillaCallPricingVM();
            BasketPricingVM basket = new BasketPricingVM();
            selectedPricing = vanille;

            List<IPricingViewModel> myListPricing = new List<IPricingViewModel>() { vanille, basket };
            PricingMethods = new ObservableCollection<IPricingViewModel>(myListPricing);

            Share accorSA = new Share("ACCOR SA", "AC FP     ");
            Share alstom = new Share("ALSTOM", "ALO FP    ");
            Share edf = new Share("EDF", "EDF FP    ");
            Share axaSA = new Share("AIR LIQUIDE SA", "AI FP     ");

            List<ActionCheckBox> myListAction = new List<ActionCheckBox>() { new ActionCheckBox(accorSA), new ActionCheckBox(alstom),
            new ActionCheckBox(edf),new ActionCheckBox(axaSA)};
            AvailableAction = new ObservableCollection<ActionCheckBox>(myListAction);

        }

        private void StartAnalyse()
        {
            List<Share> actions = new List<Share>();
            foreach(ActionCheckBox action in AvailableAction) 
            {
                if (action.IsSelected)
                    actions.Add(action.Share);
            }

            DateTime startDateTime = DateTime.ParseExact(startDate, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            DateTime maturityDate = DateTime.ParseExact(maturity, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            selectedPricing.Pricing.oShares = actions.ToArray();
            selectedPricing.Pricing.oWeights = new double[actions.Count];
            for (int i = 0; i < actions.Count; i++)
            {
                selectedPricing.Pricing.oWeights[i] = 1 / actions.Count;
            }

                selectedPricing.Pricing.oMaturity = maturityDate;
            double[] oSpot = new double[1];
            oSpot[0] = Convert.ToDouble(spot);
            selectedPricing.Pricing.oSpot = oSpot;
            selectedPricing.Pricing.oStrike = Convert.ToDouble(strike);
            wholeView.PricingViewModel = selectedPricing;

            selectedTesting.GenerateHistory.strike = Convert.ToDouble(strike);
            selectedTesting.GenerateHistory.underlyingShares = actions.ToArray();
            selectedTesting.GenerateHistory.weight = selectedPricing.Pricing.oWeights;
            Console.WriteLine("Shares " + actions.ToArray()[0].Id + actions.ToArray()[0].Name);
            selectedTesting.GenerateHistory.vanillaCallName = "Vanilla";
            selectedTesting.GenerateHistory.endTime = maturityDate;

            wholeView.GenrateHistory = selectedTesting;
            wholeView.PricingViewModel = selectedPricing;

            wholeView.ViewFacade.Launch();
        }

        #endregion Public Constructors

        public WholeViewModel WholeView
        {
            get { return wholeView; }
        }

        public IGenerateHistoryViewModel SelectedTesting
        {
            get { return selectedTesting; }
            set
            {
                SetProperty(ref selectedTesting, value);
                wholeView.GenrateHistory = selectedTesting;
            }
        }
        
        public IPricingViewModel SelectedPricing
        {
            get { return selectedPricing; }
            set
            {
                SetProperty(ref selectedPricing, value);
                wholeView.PricingViewModel = selectedPricing;
            }
        }

        private bool CanLaunch()
        {
            return true;
        }

    }
}
