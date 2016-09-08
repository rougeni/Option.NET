using ProjetNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.ViewModels
{
    internal class BasketPricingVM : IPricingViewModel
    {
        private IPricing basket;

        public IPricing Pricing
        {
            get { return basket; }
        }

        public string Name
        {
            get { return "Moyenne d'un panier"; }
        }
    }
}
