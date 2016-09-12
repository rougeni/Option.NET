using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.ViewModels
{

    /**
     * 
     *  All  the option of type OptionBasket should instantiate
     *  this class with their specific data.
     * 
     * */

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

        /**
         * see AbstractOptionCombobox.toTestBox
         * */
        public override string toTextBox()
        {
            String infoText = "Basket : " + oName + "\n";
            infoText += "Date de maturité : " + oMaturity + "\n";
            infoText += "Strike : " + oStrike  + "\n";
            for (int i = 0; i < oShares.Length; i++)
            {
                infoText += "Underlying Share : " + oShares[i].Name + " - ponderation : " + oWeights[i] + "\n";
            }

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
            myPricingVM.Pricing.oWeights = oWeights;

            myGenHistoryVM.GenerateHistory.endTime = oMaturity;
            myGenHistoryVM.GenerateHistory.strike = oStrike;
            myGenHistoryVM.GenerateHistory.underlyingShares = oShares;
            myGenHistoryVM.GenerateHistory.vanillaCallName = oName;
            myGenHistoryVM.GenerateHistory.weight = oWeights;
        }
    }
}
