using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetNET.Models;
using Prism.Mvvm;

namespace ProjetNET.ViewModels
{
    internal class WholeViewModel : BindableBase
    {
        #region Private Fields

        private IGenerateHistoryViewModel generateHistory;

        private IPricingViewModel pricingViewModel;

        private ViewFacade facadeView;

        #endregion Private Fields

        public WholeViewModel(int lineNb, int columnNb)
        {
            // generateHistory = new 
            Facade facade = new Facade(generateHistory.GenerateHistory, pricingViewModel.Pricing);
            facadeView = new ViewFacade(facade);
        }

        #region Public Properties

        public ViewFacade Facade
        {
            get { return facadeView; }
        }

        public IGenerateHistoryViewModel GenrateHistory
        {
            get { return generateHistory; }
            set
            {
                Facade.GenrateHistory = generateHistory.GenerateHistory;
            }
        }

        #endregion Public Properties

    }
}
