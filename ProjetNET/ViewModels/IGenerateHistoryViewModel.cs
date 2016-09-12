using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetNET.Models;

namespace ProjetNET.ViewModels
{
    /**
   * 
    * Interface permettant de lie la vue et 
    * les differents model d'historique (backward et forward)
    * 
    * */
    public interface IGenerateHistoryViewModel
    {
        #region Public Properties

        IGenerateHistory GenerateHistory { get; }
        string Name { get; }

        #endregion Public Properties
    }
}
