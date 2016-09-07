using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetNET.Models;

namespace ProjetNET.ViewModels
{
    internal class IGenerateHistoryViewModel
    {
        private IGenerateHistory generateHistory;

        #region Public Properties

        IGenerateHistory GenerateHistory { get { return generateHistory; } }
        string Name { get; }

        #endregion Public Properties
    }
}
