using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows.Threading;
using ProjetNET.ViewModels;
using PricingLibrary.FinancialProducts;
using System.Windows;

namespace ProjetNET
{
    public class MainWindowViewModel : BindableBase
    {
        #region Private Fields

        private WholeViewModel wholeView;

        private IGenerateHistoryViewModel selectedTesting;
        private IPricingViewModel selectedPricing;
        private AbstractOptionCombobox selectedOption;

        private String rebalancement;
        private String estimation;
        private String optionInformation;
        private String dateDebut;

        private int rebalancementValue;
        private DateTime dateDebutTime;
        private int estimationValue;

        #endregion Private Fields

        #region Public Fields
        public DelegateCommand StartCommand { get; private set; }        

        public ObservableCollection<IGenerateHistoryViewModel> TestGenerateHistory { get; private set; }

        public ObservableCollection<AbstractOptionCombobox> AvailableOptions { get; private set; }

        #endregion Public Fields

        #region Public Getters and Setters


        public String DateDebut
        {
            get { return dateDebut; }
            set
            {
                SetProperty(ref dateDebut, value);
            }
        }

        public String Estimation
        {
            get { return estimation; }
            set
            {
                SetProperty(ref estimation, value);
            }
        }

        public String Rebalancement
        {
            get { return rebalancement; }
            set
            {
                SetProperty(ref rebalancement, value);
            }
        }

        public String OptionInformation
        {
            get { return optionInformation; }
            set
            {
                SetProperty(ref optionInformation, value);
            }
        }

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
            }
        }

        public AbstractOptionCombobox SelectedOption
        {
            get { return selectedOption; }
            set
            {
                SetProperty(ref selectedOption, value);
                selectedPricing = selectedOption.myPricer;
                OptionInformation = selectedOption.toTextBox();
                DateDebut = selectedOption.currentDate.ToShortDateString();

            }
        }
        #endregion Public Getters and Setters

        #region Public Constructors

        public MainWindowViewModel()
        {

            // creation du delegate responsable de la génération du graphique
            StartCommand = new DelegateCommand(StartAnalyse, CanLaunch);
            wholeView = new WholeViewModel();

            VanillaCallPricingVM vanille = new VanillaCallPricingVM();
            BasketPricingVM basket = new BasketPricingVM();

            Share accorSA = new Share("ACCOR SA", "AC FP     ");
            Share alstom = new Share("ALSTOM", "ALO FP    ");
            Share edf = new Share("EDF", "EDF FP    ");
            Share axaSA = new Share("AIR LIQUIDE SA", "AI FP     ");


            // Generation of options 
            
            // First Vanilla Call
            OptionVanilla optVanilla1 = new OptionVanilla(vanille, "First Vanilla Call", new DateTime(2014, 01, 10), new DateTime(2015, 08, 20), new Share[1] { edf }, 24);
            OptionVanilla optVanilla2 = new OptionVanilla(vanille, "Second Vanilla Call", new DateTime(2014, 01, 17), new DateTime(2014, 01, 24), new Share[1] { axaSA }, 7);
            OptionBasket optBasket1 = new OptionBasket(basket, "First Basket Option", new DateTime(2014, 01, 10), new DateTime(2015, 08, 20), new Share[4] { accorSA, alstom, edf, axaSA }, 30, new double[4] { 0.2, 0.2, 0.2, 0.4 });
            OptionBasket optBasket2 = new OptionBasket(basket, "Second Basket Option", new DateTime(2014, 01, 17), new DateTime(2015, 08, 13), new Share[2] { alstom, edf }, 14, new double[2] { 0.8, 0.2 });
            SelectedOption = optVanilla1;
            DateDebut = selectedOption.currentDate.ToShortDateString();


            DateDebut = selectedOption.currentDate.ToShortDateString();
            optionInformation = optVanilla1.toTextBox();

            List<AbstractOptionCombobox> myListOption = new List<AbstractOptionCombobox>() { optVanilla1, optVanilla2, optBasket1, optBasket2 };
            AvailableOptions = new ObservableCollection<AbstractOptionCombobox>(myListOption);
        
            BackTestGenerateHistoryVM backTest = new BackTestGenerateHistoryVM();
            ForwardTestGenerateHistoryVM forwardTest = new ForwardTestGenerateHistoryVM();
            SelectedTesting = backTest;
            Rebalancement = "1";
            Estimation = "30";
            List<IGenerateHistoryViewModel> myListGenerator = new List<IGenerateHistoryViewModel>() { backTest, forwardTest };
            TestGenerateHistory = new ObservableCollection<IGenerateHistoryViewModel>(myListGenerator);
        }

        #endregion Public Constructors

        private void StartAnalyse()
        {

            selectedOption.setPricer(selectedPricing, selectedTesting);

            selectedTesting.GenerateHistory.startDate = dateDebutTime.AddDays(-(int)(estimationValue * 8 / 5));

            selectedPricing.Pricing.oRebalancement = rebalancementValue;

            selectedPricing.Pricing.currentDate = dateDebutTime;

            selectedPricing.Pricing.oObservation = estimationValue;

            wholeView.PricingViewModel = selectedPricing;
            wholeView.GenrateHistory = selectedTesting;
            try
            {
                wholeView.ViewFacade.Launch();
            } 
            catch (NullReferenceException nullRef) {
                MessageBox.Show("La période considérée ne correspond à aucune donnée.");
            }
            catch (ApplicationException appli)
            {
                MessageBox.Show("Une erreur interne s'est produite.\nInformation supplémentaire : " + appli.Message);
            }
            catch (InvalidOperationException invalid)
            {
                MessageBox.Show(invalid.Message);
            }
        }

        private bool CanLaunch()
        {
            if (!int.TryParse(rebalancement, out rebalancementValue))
            {
                MessageBox.Show("Le période de rebalancement doit être un entier correspondant aux nombres de jours entre deux réévaluations du portefeuille.");
                return false;
            }
            if (!DateTime.TryParse(dateDebut, out dateDebutTime))
            {
                MessageBox.Show("Date de début doit être au format dd/MM/yyyy");
                return false;
            }
            if (!int.TryParse(estimation, out estimationValue))
            {
                MessageBox.Show("Le période d'estimation doit être un entier correspondant à un nombre de jour.");
                return false;
            }
            return true;
        }

    }
}
