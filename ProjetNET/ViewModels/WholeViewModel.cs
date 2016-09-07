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

        private IGenerateHistory generateHistory;

        private IPricingViewModel pricingViewModel;

        private ViewFacade facade;

        #endregion Private Fields

        public WholeViewModel(int lineNb, int columnNb)
        {
            // generateHistory = new 
            var facade = new Facade(generateHistory, pricing);
            facade = new ViewFacade(facade);
        }

        #region Public Properties

        public ViewFacade Facade
        {
            get { return facade; }
        }

        public IGenerateHistory GenrateHistory
        {
            get { return generateHistory; }
            set
            {
                Facade.GenrateHistory = generateHistory;
            }
        }

        #endregion Public Properties

    }
}
