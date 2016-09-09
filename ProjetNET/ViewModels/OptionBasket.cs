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

        public override string toTextBox()
        {
            String infoText = "Vanila Call : " + oName + "\n";
            infoText += "Date de début : " + currentDate + ", Date de maturité : " + oMaturity + "\n";
            infoText += "Strike : " + oStrike  + "\n";
            for (int i = 0; i < oShares.Length; i++)
            {
                infoText += "Underlying Share : " + oShares[i].Name + " - ponderation : " + oWeights[i] + "\n";
            }

            return infoText;
        }
    }
}
