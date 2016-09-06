using Projet_NET.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.Data
{
    public class DataConnection
    {
        public BaseDataContext bd
        {
            public get;
            private set;
        }
        #region Public Methods

        #endregion Public Methods
        public List<String> ListofNames;
        public  List<String> getNames(){
            ListofNames = new List<String>();
            un_nom = from Sharename in 
        }

    }
}

