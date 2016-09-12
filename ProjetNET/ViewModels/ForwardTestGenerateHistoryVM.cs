using ProjetNET.Models;
using ProjetNET.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetNET
{

    public class ForwardTestGenerateHistoryVM : IGenerateHistoryViewModel
    {

        private IGenerateHistory forwardTest;

        public ForwardTestGenerateHistoryVM()
        {
            forwardTest = new ForwardTestGenerate();
        }

        public IGenerateHistory GenerateHistory
        {
            get { return forwardTest; }
        }

        public string Name
        {
            get { return "Forward Testing"; }
        }
    }
}
