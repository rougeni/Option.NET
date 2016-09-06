using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.ViewModels
{
    interface IControleViewModel
    {
        #region Public Properties

        string Name { get; }

        void Leave(WholeViewModel wholeView);

        void Arrive(WholeViewModel wholeView);

        #endregion Public Properties
    }
}
