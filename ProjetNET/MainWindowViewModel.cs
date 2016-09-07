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

        private DispatcherTimer dispatcherTimer;
        private bool tickerStarted;
        private WholeViewModel wholeView;

        public ObservableCollection<IGenerateHistoryViewModel> CounterList { get; private set; }

        #endregion Private Fields

        #region Public Constructors

        public MainWindowViewModel()
        {
            BackTestGenerateHistory backTest = new BackTestGenerateHistory();
            ForwardTestGenerateHistory forwardTest = new ForwardTestGenerateHistory();

            List<IGenerateHistoryViewModel> myList = new List<IGenerateHistoryViewModel>() { backTest, forwardTest };

        }

        #endregion Public Constructors

    }
}
