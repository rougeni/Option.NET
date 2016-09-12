using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.ViewModels
{
    public class OptionVanilla : AbstractOptionCombobox
    {
        /**
         * Constructor for a hardcoded Vanilla Call Option.
         * */
        public OptionVanilla(IPricingViewModel ipricing, String name, DateTime startDate, DateTime maturity, Share[] shares, double strike)
        {
            myPricer = ipricing;
            oName = name;
            currentDate = startDate;
            oMaturity = maturity;
            oShares = shares;
            oStrike = strike;
        }

        /*
         * Method to print all the information of this option.
         * */
        public override string toTextBox()
        {
            String infoText = "Vanila Call : " + oName + "\n";
            infoText += "Date de début : " + currentDate + ", Date de maturité : " + oMaturity + "\n";
            infoText += "Strike : " + oStrike + ", Underlying Share : " + oShares[0].Name + "\n";

            return infoText;
        }

        public override void setPricer(IPricingViewModel myPricingVM, IGenerateHistoryViewModel myGenHistoryVM)
        {
            myPricingVM.Pricing.currentDate = currentDate;
            myPricingVM.Pricing.oMaturity = oMaturity;
            myPricingVM.Pricing.oName = oName;
            myPricingVM.Pricing.oShares = oShares;
            myPricingVM.Pricing.oStrike = oStrike;

            myGenHistoryVM.GenerateHistory.startDate = currentDate;
            myGenHistoryVM.GenerateHistory.endTime = oMaturity;
            myGenHistoryVM.GenerateHistory.strike = oStrike;
            myGenHistoryVM.GenerateHistory.underlyingShares = oShares;
            myGenHistoryVM.GenerateHistory.vanillaCallName = oName;
        }
    }
}
