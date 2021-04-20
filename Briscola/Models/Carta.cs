using Briscola.Models.Enumeratori;
using System;

namespace Briscola.Models
{
    public class Carta
    {
        public Carta(int numero, Seme seme)
        {
            Numero = (NumeroCarta)numero;
            Seme = seme;
        }

        public NumeroCarta Numero { get; private set; }

        public Seme Seme { get; private set; }

        public string Img { get; protected set; }

        public int ValoreCarta
        {
            get
            {
                switch (Numero)
                {
                    case NumeroCarta.Asso:
                        return 11;
                    case NumeroCarta.Tre:
                        return 10;
                    case NumeroCarta.Fante:
                        return 2;
                    case NumeroCarta.Cavallo:
                        return 3;
                    case NumeroCarta.Re:
                        return 4;
                    default:
                        return 0;
                }
            }
        }
    }
    public class CartaTrevisana : Carta
    {
        public CartaTrevisana(int numero, Seme seme) : base(numero, seme)
        {
            Img = Environment.CurrentDirectory + "\\Resources\\CarteTrevisane\\" + Numero.ToString() + Seme.ToString() + ".png";
        }
    }
    public class CartaNapoletana : Carta
    {
        public CartaNapoletana(int numero, Seme seme) : base(numero, seme)
        {
            Img = Environment.CurrentDirectory + "\\Resources\\CarteNapoletane\\" + Numero.ToString() + Seme.ToString() + ".png";
        }
    }
}
