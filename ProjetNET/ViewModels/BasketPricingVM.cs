using ProjetNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.ViewModels
{
    public class BasketPricingVM : IPricingViewModel
    {
        private IPricing basket;

        public BasketPricingVM()
        {
            basket = new BasketPricingModel();
        }

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
