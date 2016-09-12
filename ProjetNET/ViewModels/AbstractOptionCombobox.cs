﻿using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.ViewModels
{
    /**
     * Abstract class to provide a structure for hard-coded options.
     * */
    public abstract class AbstractOptionCombobox
    {

        // commun

        public IPricingViewModel myPricer { get; set; }

        public string oName { get; set; }

        public Share[] oShares { get; set; }

        public DateTime oMaturity { get; set; }

        public double oStrike { get; set; }

        public DateTime currentDate { get; set; }

        public double[] oWeight { get; set; }

        /*
         * Method to connect dynamicly the interface and the data in an option.
         * */
        abstract public void setPricer(IPricingViewModel myPricingVM, IGenerateHistoryViewModel myGenHistoryVM);

        abstract public String toTextBox();
    }
}
