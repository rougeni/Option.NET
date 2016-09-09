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
        private String strike;
        private String dateDebut;
        private String dateFin;

        #endregion Private Fields

        public ObservableCollection<IGenerateHistoryViewModel> TestGenerateHistory { get; private set; }

        public ObservableCollection<IPricingViewModel> PricingMethods { get; private set; }

        public ObservableCollection<ActionCheckBox> AvailableAction { get; private set; }

        public String Strike
        {
            get { return strike; }
            set
            {
                SetProperty(ref strike, value);
            }
        }

        public String DateDebut
        {
            get { return dateDebut; }
            set
            {
                SetProperty(ref dateDebut, value);
            }
        }

        public String DateFin
        {
            get { return dateFin; }
            set
            {
                SetProperty(ref dateFin, value);
            }
        }

        public DelegateCommand StartCommand { get; private set; }        

        #region Public Constructors

        public MainWindowViewModel()
        {
            StartCommand = new DelegateCommand(StartAnalyse, CanLaunch);
            dateFin = "20/08/2015";
            dateDebut = "10/01/2014";
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

            List<ActionCheckBox> myListAction = new List<ActionCheckBox>() { new ActionCheckBox(accorSA, true), new ActionCheckBox(alstom),
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

            DateTime startDateTime = DateTime.ParseExact(dateDebut, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            DateTime maturityDate = DateTime.ParseExact(DateFin, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            selectedPricing.Pricing.oShares = actions.ToArray();
            selectedPricing.Pricing.oWeights = new double[actions.Count];
            for (int i = 0; i < actions.Count; i++)
            {
                selectedPricing.Pricing.oWeights[i] = 1 / actions.Count;
            }

            selectedPricing.Pricing.oMaturity = maturityDate;
            double[] oSpot = new double[1];
            selectedPricing.Pricing.oStrike = Convert.ToDouble(strike);
            wholeView.PricingViewModel = selectedPricing;

            selectedTesting.GenerateHistory.underlyingShares = actions.ToArray();
            selectedTesting.GenerateHistory.weight = selectedPricing.Pricing.oWeights;
            selectedTesting.GenerateHistory.vanillaCallName = "Vanilla";
            selectedTesting.GenerateHistory.startDate = startDateTime.AddDays(-30);
            selectedTesting.GenerateHistory.endTime = maturityDate;

            double[] weight = new double[4];
            weight[0] = 0.25; weight[1] = 0.25; weight[2] = 0.25; weight[3] = 0.25;
            selectedTesting.GenerateHistory.weight = weight;
            selectedTesting.GenerateHistory.underlyingShares = actions.ToArray();
            selectedTesting.GenerateHistory.vanillaCallName = "Vanilla";
            selectedTesting.GenerateHistory.startDate = startDateTime;
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
