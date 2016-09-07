using ProjetNET.Models;
using ProjetNET.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetNET
{
    class BackTestGenerateHistoryVM : IGenerateHistoryViewModel
    {

        private IGenerateHistory forwardTest;

        public BackTestGenerateHistoryVM()
        {
            forwardTest = new BackTestGenerate();
        }

        public Models.IGenerateHistory GenerateHistory
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { return "BackTesting"; }
        }
    }
}
