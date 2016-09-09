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

        public OptionVanilla(IPricingViewModel ipricing, String name, DateTime startDate, DateTime maturity, Share[] shares, double strike)
        {
            myPricer = ipricing;
            oName = name;
            currentDate = startDate;
            oMaturity = maturity;
            oShares = shares;
            oStrike = strike;
        }

        public override string toTextBox()
        {
            String infoText = "Vanila Call : " + oName + "\n";
            infoText += "Date de début : " + currentDate + ", Date de maturité : " + oMaturity + "\n";
            infoText += "Strike : " + oStrike + ", Underlying Share : " + oShares[0].Name + "\n";

            return infoText;
        }
    }
}
