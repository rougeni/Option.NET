using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetNET.Models;

namespace ProjetNET.ViewModels
{
    internal interface IGenerateHistoryViewModel
    {
        #region Public Properties

        IGenerateHistory GenerateHistory { get; }
        string Name { get; }

        #endregion Public Properties
    }
}
