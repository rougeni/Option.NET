using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetNET.Models;
using Prism.Mvvm;

namespace ProjetNET.ViewModels
{
    internal class ViewFacade : BindableBase
    {
        #region Private Fields

        private  IGenerateHistory myInterface;

        private Facade underlyingFacade;

        public ViewFacade(Facade facade)
        {
            underlyingFacade = facade;
        }

        #endregion Private Fields

        public IGenerateHistory GenrateHistory { get; set; }
    }
}
