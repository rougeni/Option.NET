using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows.Threading;
using ProjetNET.ViewModels;

namespace ProjetNET
{
    internal class MainWindowViewModel : BindableBase
    {
        #region Private Fields

        private WholeViewModel wholeView;

        private IGenerateHistoryViewModel selectedTesting;

        private bool fieldCompleted;

        private IPricingViewModel selectedPricing;

        #endregion Private Fields

        public ObservableCollection<IGenerateHistoryViewModel> TestGenerateHistory { get; private set; }

        public DelegateCommand StartCommand { get; private set; }

        public ObservableCollection<IPricingViewModel> PricingMethods { get; private set; }

        #region Public Constructors

        public MainWindowViewModel()
        {
            StartCommand = new DelegateCommand(StartAnalyse, CanLaunch);

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

        }

        private void StartAnalyse()
        {
            wholeView.Facade.Test();
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
