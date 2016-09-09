using PricingLibrary.FinancialProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.ViewModels
{
    class ActionCheckBox
    {
        private Share action;

        private Boolean isSelected;

        public ActionCheckBox(Share action)
        {
            this.action = action;
            isSelected = false;
        }

        public ActionCheckBox(Share accorSA, bool p)
        {
            // TODO: Complete member initialization
            this.action = accorSA;
            this.isSelected = p;
        }
        public string Name { get { return action.Name; } }
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
            }
        }

        public Share Share { get { return action; } }
    }
}
