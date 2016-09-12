using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNET.Models
{
    /*
     * Objet qui modélise la valeur d'un portefeuille de couverture
     * à une date donnée
     * */
    public class Portefeuille
    {
        private DateTime date;
        private double valeur;

        public Portefeuille(DateTime d, double val)
        {
            date = d;
            valeur = val;
        }

        public double Valeur
        {
            get { return valeur; }
        }

        public DateTime Date
        {
            get { return date; }
        }
    }
}
