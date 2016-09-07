using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows.Threading;
using ProjetNET.ViewModels;

namespace ProjetNET
{
    internal class MainWindowViewModel : BindableBase
    {
        #region Private Fields

        private WholeViewModel wholeView;

        #endregion Private Fields

        public ObservableCollection<IGenerateHistoryViewModel> CounterList { get; private set; }


        #region Public Constructors

        public MainWindowViewModel()
        {
            BackTestGenerateHistoryVM backTest = new BackTestGenerateHistoryVM();
            ForwardTestGenerateHistoryVM forwardTest = new ForwardTestGenerateHistoryVM();

            List<IGenerateHistoryViewModel> myList = new List<IGenerateHistoryViewModel>() { backTest, forwardTest };

        }

        #endregion Public Constructors

    }
}
