using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetNET.Models;
using Prism.Mvvm;

namespace ProjetNET.ViewModels
{
    public class WholeViewModel : BindableBase
    {
        #region Private Fields

        private IGenerateHistoryViewModel generateHistory;

        private IPricingViewModel pricingViewModel;

        private ViewFacade viewFacade;

        #endregion Private Fields

        public WholeViewModel()
        {
            generateHistory = new BackTestGenerateHistoryVM();
            pricingViewModel = new VanillaCallPricingVM();
            Facade facade = new Facade(generateHistory.GenerateHistory, pricingViewModel.Pricing);
            viewFacade = new ViewFacade(facade);
        }

        #region Public Properties

        public ViewFacade ViewFacade
        {
            get { return viewFacade; }
        }

        public IGenerateHistoryViewModel GenrateHistory
        {
            get { return generateHistory; }
            set
            {
                SetProperty(ref generateHistory, value);
                ViewFacade.UnderlyingFacade.GenrateHistory = generateHistory.GenerateHistory;
            }
        }

        public IPricingViewModel PricingViewModel
        {
            get { return pricingViewModel; }
            set
            {
                SetProperty(ref pricingViewModel, value);
                ViewFacade.UnderlyingFacade.Pricing = pricingViewModel.Pricing;
            }
        }

        #endregion Public Properties

    }
}
