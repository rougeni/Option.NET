using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.ViewModels
{
    internal class WholeViewModel
    {
        #region Private Fields

        private IControleViewModel controlViewModel;

        private ViewFacade facade;

        #endregion Private Fields

        public WholeViewModel(int lineNb, int columnNb)
        {
            controlViewModel = new InitControleViewModel();
            facade = new ViewFacade();
        }

    }
}
