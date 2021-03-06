﻿using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * 
 *  All  the option of type VanillaCall should instantiate
 *  this class with their specific data.
 * 
 * */

namespace ProjetNET.ViewModels
{
    public class OptionVanilla : AbstractOptionCombobox
    {
        /**
         * Constructor for a hardcoded Vanilla Call Option.
         * */
        public OptionVanilla(IPricingViewModel ipricing, String name, DateTime startDate, DateTime maturity, Share[] shares, double strike)
        {
            currentDate = startDate;
            myPricer = ipricing;
            oName = name;
            oMaturity = maturity;
            oShares = shares;
            oStrike = strike;
            oWeight = new double[1];
            oWeight[0] = 1;
        }

        /**
          * see AbstractOptionCombobox.toTestBox
          * */
        public override string toTextBox()
        {
            String infoText = "Vanilla Call : " + oName + "\n";
            infoText += "Date de maturité : " + oMaturity + "\n";
            infoText += "Strike : " + oStrike + ", Underlying Share : " + oShares[0].Name + "\n";

            return infoText;
        }

        /**
        * see AbstractOptionCombobox.setPricer
        **/
        public override void setPricer(IPricingViewModel myPricingVM, IGenerateHistoryViewModel myGenHistoryVM)
        {
            myPricingVM.Pricing.oMaturity = oMaturity;
            myPricingVM.Pricing.oName = oName;
            myPricingVM.Pricing.oShares = oShares;
            myPricingVM.Pricing.oStrike = oStrike;

            myGenHistoryVM.GenerateHistory.endTime = oMaturity;
            myGenHistoryVM.GenerateHistory.strike = oStrike;
            myGenHistoryVM.GenerateHistory.underlyingShares = oShares;
            myGenHistoryVM.GenerateHistory.vanillaCallName = oName;
            myGenHistoryVM.GenerateHistory.weight = oWeight;
        }
    }
}
