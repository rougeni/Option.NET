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
        private IControleViewModel selectedControl;
        private WholeViewModel wholeView;
        #endregion Private Fields

        public IControleViewModel SelectedControl
        {
            get { return selectedControl; }
            set 
            {
                selectedControl.Leave(wholeView);
                SetProperty(ref selectedControl, value);
                selectedControl.Arrive(wholeView);
            }
        }

        public ObservableCollection<IControleViewModel> AvailableControl { get; private set; }

        #region Public Constructors

        public MainWindowViewModel()
        {
            IControleViewModel init = new InitControleViewModel();
            System.Console.WriteLine("init " + init.Name);
            List<IControleViewModel> myList = new List<IControleViewModel>() {init};
            AvailableControl = new ObservableCollection<IControleViewModel>(myList);
            wholeView = new WholeViewModel();
        }

        #endregion Public Constructors

    }
}
