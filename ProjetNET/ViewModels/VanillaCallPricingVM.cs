using ProjetNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.ViewModels
{
    public class VanillaCallPricingVM : IPricingViewModel
    {

        private IPricing vanillaCall;

        public VanillaCallPricingVM()
        {
            vanillaCall = new VanillaCallPricingModel();
        }

        public IPricing Pricing
        {
            get {return vanillaCall; }
        }

        public string Name
        {
            get { return "Vanilla Call"; }
        }
    }
}
