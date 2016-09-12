using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.ViewModels
{
    public class OptionBasket : AbstractOptionCombobox
    {
        private double[] oWeights;
        /**
         * Constructor for a hardcoded Basket Option.
         * */
        public OptionBasket(IPricingViewModel ipricing, String name, DateTime startDate, DateTime maturity, Share[] shares, double strike, double[] weights)
        {
            myPricer = ipricing;
            oName = name;
            currentDate = startDate;
            oMaturity = maturity;
            oShares = shares;
            oStrike = strike;
            oWeights = weights;
        }

        /*
         * Method to print all the information of this option.
         * */
        public override string toTextBox()
        {
            String infoText = "Vanila Call : " + oName + "\n";
            infoText += "Date de maturité : " + oMaturity + "\n";
            infoText += "Strike : " + oStrike  + "\n";
            for (int i = 0; i < oShares.Length; i++)
            {
                infoText += "Underlying Share : " + oShares[i].Name + " - ponderation : " + oWeights[i] + "\n";
            }

            return infoText;
        }

        public override void setPricer(IPricingViewModel myPricingVM, IGenerateHistoryViewModel myGenHistoryVM)
        {
            myPricingVM.Pricing.oMaturity = oMaturity;
            myPricingVM.Pricing.oName = oName;
            myPricingVM.Pricing.oShares = oShares;
            myPricingVM.Pricing.oStrike = oStrike;
            myPricingVM.Pricing.oWeights = oWeights;

            myGenHistoryVM.GenerateHistory.endTime = oMaturity;
            myGenHistoryVM.GenerateHistory.strike = oStrike;
            myGenHistoryVM.GenerateHistory.underlyingShares = oShares;
            myGenHistoryVM.GenerateHistory.vanillaCallName = oName;
            myGenHistoryVM.GenerateHistory.weight = oWeights;
        }
    }
}
