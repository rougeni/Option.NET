﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.Data
{
    public class Action
    {
        #region Private Fields

        string name;
        int date;
        int price;

        #endregion Private Fields

        #region Public Constructor

        public Action(String nom, int date, int prix)
        {
            this.name = nom;
            this.date = date;
            this.price = prix;
            /*
 * lgnes de commmentaires
 * pour tester git
 * ça commence à me 
 * souler */
        }

        #endregion Public Constructor

        public string Name { get { return name; } }

        public int Date { get { return date; } }

        public int Price { get { return price; } }

        /* fonction pour tester le  projet de tests */
        public String afficher(){
            return this.name;
        }
    }
}