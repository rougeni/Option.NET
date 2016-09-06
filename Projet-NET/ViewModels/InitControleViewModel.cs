using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.ViewModels
{
    internal class InitControleViewModel: IControleViewModel
    {

        public string Name
        {
            get { return "Initialitation"; }
        }

        public void Leave(WholeViewModel wholeView)
        {
            throw new NotImplementedException();
        }

        public void Arrive(WholeViewModel wholeView)
        {
            throw new NotImplementedException();
        }
    }
}
