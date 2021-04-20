using Briscola.Models.Enumeratori;
using System;
using System.Collections.Generic;

namespace Briscola.Models
{
    public class TwoPlayers
    {
        public static Carta GetCartaCpu(List<Carta> mazzoCpu1, Carta briscola, out int index, Carta cartaG1 = null)
        {
            #region Inizia la CPU
            if (cartaG1 == null)
            {
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    int valoreCarta = mazzoCpu1[i].ValoreCarta;
                    if (valoreCarta == 0 && mazzoCpu1[i].Seme != briscola.Seme)//provo a buttare una scartella) //provo a buttare asso o tre di briscola
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }
                }
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].ValoreCarta < 10 && mazzoCpu1[i].Seme != briscola.Seme) //provo a buttare una figura
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }
                }
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].ValoreCarta == 0 && mazzoCpu1[i].Seme == briscola.Seme) //provo a buttare una briscoletta
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }
                }
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].ValoreCarta < 10 && mazzoCpu1[i].Seme == briscola.Seme) //provo a buttare una figura di briscola
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }
                }
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].ValoreCarta >= 10 && mazzoCpu1[i].Seme != briscola.Seme) //provo a buttare un carico
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }
                }
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].ValoreCarta >= 10 && mazzoCpu1[i].Seme == briscola.Seme) //provo a buttare asso o tre di briscola
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }
                }

            }
            #endregion

            #region Carta del giocatore é una briscola alta (figure, asso o tre)
            if (cartaG1.ValoreCarta != 0 && cartaG1.Seme == briscola.Seme)
            {
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme != briscola.Seme &&
                        mazzoCpu1[i].ValoreCarta == 0)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }                // Se arrivo a questo punto significa che in mano ho tutte briscole oppure tutti carichi, perció butto la piú bassa
                }

                if (mazzoCpu1[0].Numero < mazzoCpu1[1].Numero &&
                    mazzoCpu1[0].Numero < mazzoCpu1[2].Numero &&
                    mazzoCpu1[0].ValoreCarta < 10 &&
                    mazzoCpu1[0].Seme != briscola.Seme)
                {
                    index = 0;
                    return mazzoCpu1[0];
                }

                if (mazzoCpu1[1].Numero < mazzoCpu1[0].Numero &&
                    mazzoCpu1[1].Numero < mazzoCpu1[2].Numero &&
                    mazzoCpu1[1].ValoreCarta < 10 &&
                    mazzoCpu1[1].Seme != briscola.Seme)
                {
                    index = 1;
                    return mazzoCpu1[1];
                }

                if (mazzoCpu1[2].Numero < mazzoCpu1[0].Numero &&
                    mazzoCpu1[2].Numero < mazzoCpu1[1].Numero &&
                    mazzoCpu1[2].ValoreCarta < 10 &&
                    mazzoCpu1[2].Seme != briscola.Seme)
                {
                    index = 2;
                    return mazzoCpu1[2];
                } //Se arrivo a questo punto ho carichi (anche di briscola) o figure di briscola

                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Numero > (NumeroCarta)3)
                    {
                        index = 0;
                        return mazzoCpu1[0];
                    }                //Se arrivo a questo punto, in mano ho solo carichi (asso o tre) non di briscola
                }

                index = 0;
                return mazzoCpu1[0];
            }
            #endregion

            #region Carta del giocatore é un carico non briscola
            if (cartaG1.ValoreCarta >= 10 && cartaG1.Seme != briscola.Seme)
            {
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme == cartaG1.Seme &&
                        mazzoCpu1[i].ValoreCarta > cartaG1.ValoreCarta)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }
                } //Se arrivo a questo punto significa che non ho potuto superare il suo carico nel suo seme: cerco una briscola piccola

                if (mazzoCpu1[0].Seme == briscola.Seme &&
                    mazzoCpu1[0].ValoreCarta == 0)
                {
                    index = 0;
                    return mazzoCpu1[0];
                }

                if (mazzoCpu1[1].Seme == briscola.Seme &&
                    mazzoCpu1[1].ValoreCarta == 0)
                {
                    index = 1;
                    return mazzoCpu1[1];
                }

                if (mazzoCpu1[2].Seme == briscola.Seme &&
                    mazzoCpu1[2].ValoreCarta == 0)
                {
                    index = 2;
                    return mazzoCpu1[2];
                } //Se arrivo a questo punto significa che non ho briscole basse, perció cerco una briscola qualsiasi

                if (mazzoCpu1[0].ValoreCarta < mazzoCpu1[1].ValoreCarta &&
                    mazzoCpu1[0].ValoreCarta < mazzoCpu1[2].ValoreCarta &&
                    mazzoCpu1[0].Seme == briscola.Seme)
                {
                    index = 0;
                    return mazzoCpu1[0];
                }

                if (mazzoCpu1[1].ValoreCarta < mazzoCpu1[0].ValoreCarta &&
                    mazzoCpu1[1].ValoreCarta < mazzoCpu1[2].ValoreCarta &&
                    mazzoCpu1[1].Seme == briscola.Seme)
                {
                    index = 1;
                    return mazzoCpu1[1];
                }

                if (mazzoCpu1[2].ValoreCarta < mazzoCpu1[0].ValoreCarta &&
                    mazzoCpu1[2].ValoreCarta < mazzoCpu1[1].ValoreCarta &&
                    mazzoCpu1[2].Seme == briscola.Seme)
                {
                    index = 2;
                    return mazzoCpu1[2];
                } //Se arrivo a questo punto significa che non ho briscole per prendere perció butto una scartina

                if (mazzoCpu1[0].ValoreCarta < mazzoCpu1[1].ValoreCarta &&
                    mazzoCpu1[0].ValoreCarta < mazzoCpu1[2].ValoreCarta)
                {
                    index = 0;
                    return mazzoCpu1[0];
                }

                if (mazzoCpu1[1].ValoreCarta < mazzoCpu1[0].ValoreCarta &&
                    mazzoCpu1[1].ValoreCarta < mazzoCpu1[2].ValoreCarta)
                {
                    index = 1;
                    return mazzoCpu1[1];
                }

                if (mazzoCpu1[2].ValoreCarta < mazzoCpu1[0].ValoreCarta &&
                    mazzoCpu1[2].ValoreCarta < mazzoCpu1[1].ValoreCarta)
                {
                    index = 2;
                    return mazzoCpu1[2];
                }
            }
            #endregion

            #region Carta del giocatore é una briscola bassa (fino al 7)
            if (cartaG1.ValoreCarta < 10 && cartaG1.Seme == briscola.Seme)
            {
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme != briscola.Seme && mazzoCpu1[i].ValoreCarta == 0)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    } //Se arrivo a questo punto significa che in mano non avevo scartelle, perció posso avere briscole oppure figure e carichi
                }

                if (mazzoCpu1[0].Numero < mazzoCpu1[1].Numero &&
                    mazzoCpu1[0].Numero < mazzoCpu1[2].Numero &&
                    mazzoCpu1[0].Seme != briscola.Seme &&
                    mazzoCpu1[0].ValoreCarta < 10)
                {
                    index = 0;
                    return mazzoCpu1[0];
                }

                if (mazzoCpu1[1].Numero < mazzoCpu1[0].Numero &&
                    mazzoCpu1[1].Numero < mazzoCpu1[2].Numero &&
                    mazzoCpu1[1].Seme != briscola.Seme &&
                    mazzoCpu1[1].ValoreCarta < 10)
                {
                    index = 1;
                    return mazzoCpu1[1];
                }

                if (mazzoCpu1[2].Numero < mazzoCpu1[0].Numero &&
                    mazzoCpu1[2].Numero < mazzoCpu1[1].Numero &&
                    mazzoCpu1[2].Seme != briscola.Seme &&
                    mazzoCpu1[2].ValoreCarta < 10)
                {
                    index = 2;
                    return mazzoCpu1[2];
                } //Se arrivo a questo punto significa che in mano ho carichi oppure briscole

                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme == briscola.Seme &&
                        mazzoCpu1[i].Numero < cartaG1.Numero &&
                        mazzoCpu1[i].ValoreCarta <= cartaG1.ValoreCarta)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    } //Se arrivo a questo punto significa che in mano ho carichi normali o di briscola
                }

                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme != briscola.Seme)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    } //Se arrivo a questo punto ho solamente carichi di briscola
                }
                index = 0;
                return mazzoCpu1[0];
            }
            #endregion

            #region Carta del giocatore é una scartina non briscola
            if (cartaG1.ValoreCarta < 10 && cartaG1.Seme != briscola.Seme)
            {
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme == cartaG1.Seme &&
                        mazzoCpu1[i].ValoreCarta > cartaG1.ValoreCarta)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }                //Se arrivo a questo punto significa che non ho potuto superare: cerco una scartina di un altro seme
                }

                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme != briscola.Seme &&
                        mazzoCpu1[i].Seme != cartaG1.Seme &&
                        mazzoCpu1[i].ValoreCarta == 0)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }                 //Se arrivo a questo punto significa che non ho scartine
                }

                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme != briscola.Seme &&
                        mazzoCpu1[i].Seme != cartaG1.Seme &&
                        mazzoCpu1[i].ValoreCarta < 10)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }                 //Se arrivo a questo punto significa che non ho pochi punti da dargli, provo a prendere con una briscoletta
                }

                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme == briscola.Seme &&
                        mazzoCpu1[i].ValoreCarta == 0)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }                 //Se arrivo a questo punto devo prendere con una figura di briscola per non regalare carichi
                }

                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme == briscola.Seme &&
                        mazzoCpu1[i].ValoreCarta < 10)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }                 //Se arrivo a questo punto devo dargli un carico non briscola
                }

                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme != briscola.Seme)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }                 //Se arrivo qui sono costretto a prendere con un carico briscola
                }

                index = 0;
                return mazzoCpu1[0];
            }
            #endregion

            Random rnd = new Random();
            index = rnd.Next(0, 3);
            return mazzoCpu1[index];
        }
    }
    public class ThreePlayers
    {
        public static Carta GetCartaCpu(List<Giocatore> giocatori, List<Carta> mazzoCpu, Carta briscola, string primo, Carta cartaG1 = null, Carta cartaAltraCpu = null)
        {
            #region  Il primo a tirare é CPU

            Carta c = null;

            if (cartaG1 == null && cartaAltraCpu == null)
            {
                for (int i = 0; i < mazzoCpu.Count; i++)
                {
                    if (mazzoCpu[i].ValoreCarta == 0 && mazzoCpu[i].Seme != briscola.Seme) //provo a buttare una scartella
                    {
                        c = mazzoCpu[i];
                    }
                }
                for (int i = 0; i < mazzoCpu.Count; i++)
                {
                    if (mazzoCpu[i].ValoreCarta < 10 && mazzoCpu[i].Seme != briscola.Seme) //provo a buttare una figura
                    {
                        c = mazzoCpu[i];
                    }
                }
                for (int i = 0; i < mazzoCpu.Count; i++)
                {
                    if (mazzoCpu[i].ValoreCarta == 0 && mazzoCpu[i].Seme == briscola.Seme) //provo a buttare una briscoletta
                    {
                        c = mazzoCpu[i];
                    }
                }
                for (int i = 0; i < mazzoCpu.Count; i++)
                {
                    if (mazzoCpu[i].ValoreCarta < 10 && mazzoCpu[i].Seme == briscola.Seme) //provo a buttare una figura di briscola
                    {
                        c = mazzoCpu[i];
                    }
                }
                for (int i = 0; i < mazzoCpu.Count; i++)
                {
                    if (mazzoCpu[i].ValoreCarta >= 10 && mazzoCpu[i].Seme != briscola.Seme) //provo a buttare un carico
                    {
                        c = mazzoCpu[i];
                    }
                }
                for (int i = 0; i < mazzoCpu.Count; i++)
                {
                    if (mazzoCpu[i].ValoreCarta >= 10 && mazzoCpu[i].Seme == briscola.Seme) //provo a buttare asso o tre di briscola
                        c = mazzoCpu[i];
                }
                return c;
            }
            #endregion

            #region CPU gioca da secondo dopo il GIOCATORE 1
            if (cartaG1 != null && cartaAltraCpu == null)
            {
                #region Carta del giocatore é una briscola alta (figure, asso o tre)
                if (cartaG1.ValoreCarta != 0 && cartaG1.Seme == briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].ValoreCarta == 0)
                            return mazzoCpu[i];
                    // Se arrivo a questo punto significa che in mano ho tutte briscole oppure tutti carichi, perció butto la piú bassa
                    if (mazzoCpu[0].Numero < mazzoCpu[1].Numero && mazzoCpu[0].Numero < mazzoCpu[2].Numero && mazzoCpu[0].ValoreCarta < 10 && mazzoCpu[0].Seme != briscola.Seme)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].Numero < mazzoCpu[0].Numero && mazzoCpu[1].Numero < mazzoCpu[2].Numero && mazzoCpu[1].ValoreCarta < 10 && mazzoCpu[1].Seme != briscola.Seme)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].Numero < mazzoCpu[0].Numero && mazzoCpu[2].Numero < mazzoCpu[1].Numero && mazzoCpu[2].ValoreCarta < 10 && mazzoCpu[2].Seme != briscola.Seme)
                        return mazzoCpu[2];
                    //Se arrivo a questo punto ho carichi (anche di briscola) o figure di briscola
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Numero > (NumeroCarta)3)
                            return mazzoCpu[0];
                    //Se arrivo a questo punto, in mano ho solo carichi (asso o tre) non di briscola
                    return mazzoCpu[0];
                }
                #endregion

                #region Carta del giocatore é un carico non briscola
                if (cartaG1.ValoreCarta >= 10 && cartaG1.Seme != briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == cartaG1.Seme && mazzoCpu[i].ValoreCarta > cartaG1.ValoreCarta)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho potuto superare il suo carico nel suo seme: cerco una briscola piccola
                    if (mazzoCpu[0].Numero < mazzoCpu[1].Numero && mazzoCpu[0].Numero < mazzoCpu[2].Numero && mazzoCpu[0].Seme == briscola.Seme && mazzoCpu[0].ValoreCarta == 0)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].Numero < mazzoCpu[0].Numero && mazzoCpu[1].Numero < mazzoCpu[2].Numero && mazzoCpu[1].Seme == briscola.Seme && mazzoCpu[1].ValoreCarta == 0)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].Numero < mazzoCpu[0].Numero && mazzoCpu[2].Numero < mazzoCpu[1].Numero && mazzoCpu[2].Seme == briscola.Seme && mazzoCpu[2].ValoreCarta == 0)
                        return mazzoCpu[2];
                    //Se arrivo a questo punto significa che non ho briscole basse, perció cerco una briscola qualsiasi
                    if (mazzoCpu[0].ValoreCarta < mazzoCpu[1].ValoreCarta && mazzoCpu[0].ValoreCarta < mazzoCpu[2].ValoreCarta && mazzoCpu[0].Seme == briscola.Seme)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].ValoreCarta < mazzoCpu[0].ValoreCarta && mazzoCpu[1].ValoreCarta < mazzoCpu[2].ValoreCarta && mazzoCpu[1].Seme == briscola.Seme)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].ValoreCarta < mazzoCpu[0].ValoreCarta && mazzoCpu[2].ValoreCarta < mazzoCpu[1].ValoreCarta && mazzoCpu[2].Seme == briscola.Seme)
                        return mazzoCpu[2];
                    //Se arrivo a questo punto significa che non ho briscole per prendere perció butto una scartina
                    if (mazzoCpu[0].ValoreCarta < mazzoCpu[1].ValoreCarta && mazzoCpu[0].ValoreCarta < mazzoCpu[2].ValoreCarta)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].ValoreCarta < mazzoCpu[0].ValoreCarta && mazzoCpu[1].ValoreCarta < mazzoCpu[2].ValoreCarta)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].ValoreCarta < mazzoCpu[0].ValoreCarta && mazzoCpu[2].ValoreCarta < mazzoCpu[1].ValoreCarta)
                        return mazzoCpu[2];

                }
                #endregion

                #region Carta del giocatore é una briscola bassa (fino al 7)
                if (cartaG1.ValoreCarta < 10 && cartaG1.Seme == briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].ValoreCarta == 0)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che in mano non avevo scartelle, perció posso avere briscole oppure figure e carichi
                    if (mazzoCpu[0].Numero < mazzoCpu[1].Numero && mazzoCpu[0].Numero < mazzoCpu[2].Numero && mazzoCpu[0].Seme != briscola.Seme && mazzoCpu[0].ValoreCarta < 10)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].Numero < mazzoCpu[0].Numero && mazzoCpu[1].Numero < mazzoCpu[2].Numero && mazzoCpu[1].Seme != briscola.Seme && mazzoCpu[1].ValoreCarta < 10)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].Numero < mazzoCpu[0].Numero && mazzoCpu[2].Numero < mazzoCpu[1].Numero && mazzoCpu[2].Seme != briscola.Seme && mazzoCpu[2].ValoreCarta < 10)
                        return mazzoCpu[2];
                    //Se arrivo a questo punto significa che in mano ho carichi oppure briscole
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == briscola.Seme && mazzoCpu[i].Numero < cartaG1.Numero && mazzoCpu[i].ValoreCarta <= cartaG1.ValoreCarta)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che in mano ho carichi normali o di briscola
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto ho solamente carichi di briscola
                    return mazzoCpu[0];
                }
                #endregion

                #region Carta del giocatore é una scartina non briscola
                if (cartaG1.ValoreCarta < 10 && cartaG1.Seme != briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == cartaG1.Seme && mazzoCpu[i].ValoreCarta > cartaG1.ValoreCarta)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho potuto superare: cerco una scartina di un altro seme
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].Seme != cartaG1.Seme && mazzoCpu[i].ValoreCarta == 0)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho scartine
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].Seme != cartaG1.Seme && mazzoCpu[i].ValoreCarta < 10)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho pochi punti da dargli, provo a prendere con una briscoletta
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == briscola.Seme && mazzoCpu[i].ValoreCarta == 0)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto devo prendere con una figura di briscola per non regalare carichi
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == briscola.Seme && mazzoCpu[i].ValoreCarta < 10)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto devo dargli un carico non briscola
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme)
                            return mazzoCpu[i];
                    //Se arrivo qui sono costretto a prendere con un carico briscola
                    return mazzoCpu[0];
                }
                #endregion

                else
                    return null;

            }
            #endregion

            #region CPU gioca da secondo dopo la CPU
            if (cartaG1 == null && cartaAltraCpu != null)
            {
                #region Carta della CPU2 é una briscola alta (figure, asso o tre)
                if (cartaAltraCpu.ValoreCarta != 0 && cartaAltraCpu.Seme == briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].ValoreCarta == 0)
                            return mazzoCpu[i];
                    // Se arrivo a questo punto significa che in mano ho tutte briscole oppure tutti carichi, perció butto la piú bassa
                    if (mazzoCpu[0].Numero < mazzoCpu[1].Numero && mazzoCpu[0].Numero < mazzoCpu[2].Numero && mazzoCpu[0].ValoreCarta < 10 && mazzoCpu[0].Seme != briscola.Seme)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].Numero < mazzoCpu[0].Numero && mazzoCpu[1].Numero < mazzoCpu[2].Numero && mazzoCpu[1].ValoreCarta < 10 && mazzoCpu[1].Seme != briscola.Seme)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].Numero < mazzoCpu[0].Numero && mazzoCpu[2].Numero < mazzoCpu[1].Numero && mazzoCpu[2].ValoreCarta < 10 && mazzoCpu[2].Seme != briscola.Seme)
                        return mazzoCpu[2];
                    //Se arrivo a questo punto ho carichi (anche di briscola) o figure di briscola
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Numero > (NumeroCarta)3)
                            return mazzoCpu[0];
                    //Se arrivo a questo punto, in mano ho solo carichi (asso o tre) non di briscola
                    return mazzoCpu[0];
                }
                #endregion

                #region Carta della CPU2 é un carico non briscola
                if (cartaAltraCpu.ValoreCarta >= 10 && cartaAltraCpu.Seme != briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == cartaAltraCpu.Seme && mazzoCpu[i].ValoreCarta > cartaAltraCpu.ValoreCarta)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho potuto superare il suo carico nel suo seme: cerco una briscola piccola
                    if (mazzoCpu[0].Numero < mazzoCpu[1].Numero && mazzoCpu[0].Numero < mazzoCpu[2].Numero && mazzoCpu[0].Seme == briscola.Seme && mazzoCpu[0].ValoreCarta == 0)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].Numero < mazzoCpu[0].Numero && mazzoCpu[1].Numero < mazzoCpu[2].Numero && mazzoCpu[1].Seme == briscola.Seme && mazzoCpu[1].ValoreCarta == 0)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].Numero < mazzoCpu[0].Numero && mazzoCpu[2].Numero < mazzoCpu[1].Numero && mazzoCpu[2].Seme == briscola.Seme && mazzoCpu[2].ValoreCarta == 0)
                        return mazzoCpu[2];
                    //Se arrivo a questo punto significa che non ho briscole basse, perció cerco una briscola qualsiasi
                    if (mazzoCpu[0].ValoreCarta < mazzoCpu[1].ValoreCarta && mazzoCpu[0].ValoreCarta < mazzoCpu[2].ValoreCarta && mazzoCpu[0].Seme == briscola.Seme)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].ValoreCarta < mazzoCpu[0].ValoreCarta && mazzoCpu[1].ValoreCarta < mazzoCpu[2].ValoreCarta && mazzoCpu[1].Seme == briscola.Seme)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].ValoreCarta < mazzoCpu[0].ValoreCarta && mazzoCpu[2].ValoreCarta < mazzoCpu[1].ValoreCarta && mazzoCpu[2].Seme == briscola.Seme)
                        return mazzoCpu[2];
                    //Se arrivo a questo punto significa che non ho briscole per prendere perció butto una scartina
                    if (mazzoCpu[0].ValoreCarta < mazzoCpu[1].ValoreCarta && mazzoCpu[0].ValoreCarta < mazzoCpu[2].ValoreCarta)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].ValoreCarta < mazzoCpu[0].ValoreCarta && mazzoCpu[1].ValoreCarta < mazzoCpu[2].ValoreCarta)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].ValoreCarta < mazzoCpu[0].ValoreCarta && mazzoCpu[2].ValoreCarta < mazzoCpu[1].ValoreCarta)
                        return mazzoCpu[2];

                }
                #endregion

                #region Carta della CPU2 é una briscola bassa (fino al 7)
                if (cartaAltraCpu.ValoreCarta < 10 && cartaAltraCpu.Seme == briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].ValoreCarta == 0)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che in mano non avevo scartelle, perció posso avere briscole oppure figure e carichi
                    if (mazzoCpu[0].Numero < mazzoCpu[1].Numero && mazzoCpu[0].Numero < mazzoCpu[2].Numero && mazzoCpu[0].Seme != briscola.Seme && mazzoCpu[0].ValoreCarta < 10)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].Numero < mazzoCpu[0].Numero && mazzoCpu[1].Numero < mazzoCpu[2].Numero && mazzoCpu[1].Seme != briscola.Seme && mazzoCpu[1].ValoreCarta < 10)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].Numero < mazzoCpu[0].Numero && mazzoCpu[2].Numero < mazzoCpu[1].Numero && mazzoCpu[2].Seme != briscola.Seme && mazzoCpu[2].ValoreCarta < 10)
                        return mazzoCpu[2];
                    //Se arrivo a questo punto significa che in mano ho carichi oppure briscole
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == briscola.Seme && mazzoCpu[i].Numero < cartaG1.Numero && mazzoCpu[i].ValoreCarta <= cartaG1.ValoreCarta)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che in mano ho carichi normali o di briscola
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto ho solamente carichi di briscola
                    return mazzoCpu[0];
                }
                #endregion

                #region Carta della CPU2 é una scartina non briscola
                if (cartaAltraCpu.ValoreCarta < 10 && cartaAltraCpu.Seme != briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == cartaG1.Seme && mazzoCpu[i].ValoreCarta > cartaG1.ValoreCarta)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho potuto superare: cerco una scartina di un altro seme
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].Seme != cartaAltraCpu.Seme && mazzoCpu[i].ValoreCarta == 0)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho scartine
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].Seme != cartaAltraCpu.Seme && mazzoCpu[i].ValoreCarta < 10)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho pochi punti da dargli, provo a prendere con una briscoletta
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == briscola.Seme && mazzoCpu[i].ValoreCarta == 0)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto devo prendere con una figura di briscola per non regalare carichi
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == briscola.Seme && mazzoCpu[i].ValoreCarta < 10)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto devo dargli un carico non briscola
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme)
                            return mazzoCpu[i];
                    //Se arrivo qui sono costretto a prendere con un carico briscola
                    return mazzoCpu[0];
                }
                #endregion
            }
            #endregion

            #region CPU2 é l'ultimo a tirare
            if (cartaAltraCpu != null && cartaG1 != null)
            {
                #region PRIMO = SCARTINA
                if (giocatori[0].CartaGiocata.Seme != briscola.Seme && giocatori[0].CartaGiocata.ValoreCarta < 10)
                {
                    #region SECONDO = SUPERA
                    if (giocatori[1].CartaGiocata.Seme == giocatori[0].CartaGiocata.Seme && giocatori[1].CartaGiocata.Numero > giocatori[0].CartaGiocata.Numero && giocatori[1].CartaGiocata.ValoreCarta < 10)
                    {
                        c = SuperaNoBriscola(mazzoCpu, giocatori[0].CartaGiocata, giocatori[1].CartaGiocata);
                        if (c == null)
                        {
                            c = Scartina(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                            if (c == null)
                            {
                                c = BriscolaPiccola(mazzoCpu, briscola);

                                if (c == null)
                                {
                                    c = Carico(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                                    if (c == null)
                                    {
                                        c = BriscolaAlta(mazzoCpu, briscola);
                                        if (c == null)
                                            return null;
                                        else
                                            return c;
                                    }
                                    else
                                        return c;
                                }
                                else
                                    return c;

                            }
                            else
                                return c;
                        }
                        else
                            return c;
                    }
                    #endregion

                    #region SECONDO = BRISCOLA
                    if (giocatori[1].CartaGiocata.Seme == briscola.Seme)
                    {
                        c = Scartina(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                        if (c == null)
                        {
                            c = Figura(mazzoCpu, briscola);
                            if (c == null)
                            {
                                c = SuperaBriscola(mazzoCpu, giocatori[1].CartaGiocata, briscola);
                                if (c == null)
                                {
                                    c = Carico(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                                    if (c == null)
                                        return null;
                                    else
                                        return c;
                                }
                                else
                                    return c;

                            }
                            else
                                return c;

                        }
                        else
                            return c;
                    }
                    #endregion

                    #region SECONDO = CARICO
                    if (giocatori[1].CartaGiocata.ValoreCarta >= 10 && giocatori[1].CartaGiocata.Seme != briscola.Seme && giocatori[1].CartaGiocata.Seme != giocatori[0].CartaGiocata.Seme)
                    {
                        c = SuperaNoBriscola(mazzoCpu, giocatori[0].CartaGiocata, giocatori[1].CartaGiocata);
                        if (c == null)
                        {
                            c = BriscolaPiccola(mazzoCpu, briscola);
                            if (c == null)
                            {
                                c = BriscolaAlta(mazzoCpu, briscola);
                                if (c == null)
                                {
                                    c = Scartina(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                                    if (c == null)
                                    {
                                        c = Carico(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                                        if (c == null)
                                            return null;
                                        else
                                            return c;
                                    }
                                    else
                                        return c;
                                }
                                else
                                    return c;
                            }
                            else
                                return c;
                        }
                        else
                            return c;
                    }
                    #endregion

                    #region SECONDO = SCARTINA
                    if (giocatori[1].CartaGiocata.Seme != giocatori[0].CartaGiocata.Seme && giocatori[1].CartaGiocata.Seme != briscola.Seme && giocatori[1].CartaGiocata.ValoreCarta < 10)
                    {
                        c = SuperaNoBriscola(mazzoCpu, giocatori[0].CartaGiocata, giocatori[1].CartaGiocata);
                        if (c == null)
                        {
                            c = Scartina(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                            if (c == null)
                            {
                                c = BriscolaPiccola(mazzoCpu, briscola);
                                if (c == null)
                                {
                                    c = Carico(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                                    if (c == null)
                                    {
                                        c = BriscolaAlta(mazzoCpu, briscola);
                                        if (c == null)
                                            return null;
                                        else
                                            return c;
                                    }
                                    else
                                        return c;
                                }
                                else
                                    return c;
                            }
                            else
                                return c;
                        }
                        else
                            return c;
                    }
                    #endregion
                }
                #endregion

                #region PRIMO = BRISCOLA BASSA
                if (giocatori[0].CartaGiocata.Seme == briscola.Seme && giocatori[0].CartaGiocata.ValoreCarta < 10)
                {
                    #region SECONDO = SCARTINA
                    if (giocatori[1].CartaGiocata.Seme != briscola.Seme && giocatori[1].CartaGiocata.ValoreCarta < 10)
                    {
                        c = Scartina(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                        if (c == null)
                        {
                            c = Figura(mazzoCpu, briscola);
                            if (c == null)
                            {
                                c = BriscolaPiccola(mazzoCpu, briscola);
                                if (c == null)
                                {
                                    c = Carico(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                                    if (c == null)
                                    {
                                        c = BriscolaAlta(mazzoCpu, briscola);
                                        if (c == null)
                                            return null;
                                        else
                                            return c;
                                    }
                                    else
                                        return c;
                                }
                                else
                                    return c;
                            }
                            else
                                return c;
                        }
                        else
                            return c;
                    }
                    #endregion

                    #region SECONDO = CARICO
                    if (giocatori[1].CartaGiocata.Seme != briscola.Seme && giocatori[1].CartaGiocata.Seme != giocatori[0].CartaGiocata.Seme && giocatori[1].CartaGiocata.ValoreCarta >= 10)
                    {
                        c = SuperaBriscola(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                        if (c == null)
                        {
                            c = Scartina(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                            if (c == null)
                            {
                                c = Carico(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                                if (c == null)
                                    return null;
                                else
                                    return c;
                            }
                            else
                                return c;
                        }
                        else
                            return c;
                    }
                    #endregion

                    #region SECONDO = SUPERA BRISCOLA
                    if (giocatori[1].CartaGiocata.Seme == giocatori[0].CartaGiocata.Seme && (giocatori[1].CartaGiocata.Numero > giocatori[0].CartaGiocata.Numero || giocatori[1].CartaGiocata.ValoreCarta < 11))
                    {
                        c = Scartina(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                        if (c == null)
                        {
                            c = SuperaBriscola(mazzoCpu, giocatori[1].CartaGiocata, briscola);
                            if (c == null)
                            {
                                c = Carico(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                                if (c == null)
                                    return null;
                                else
                                    return c;
                            }
                            else
                                return c;
                        }
                        else
                            return c;
                    }
                    #endregion

                    #region SECONDO = BRISCOLA PIÚ BASSA
                    if (giocatori[1].CartaGiocata.Seme == giocatori[0].CartaGiocata.Seme && (giocatori[0].CartaGiocata.ValoreCarta <= giocatori[1].CartaGiocata.ValoreCarta || giocatori[0].CartaGiocata.Numero < giocatori[1].CartaGiocata.Numero))
                    {
                        c = Scartina(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                        if (c == null)
                        {
                            c = BriscolaPiccola(mazzoCpu, briscola);
                            if (c == null)
                            {
                                c = Carico(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                                if (c == null)
                                    return null;
                                else
                                    return c;
                            }
                            else
                                return c;
                        }
                        else
                            return c;
                    }
                    #endregion
                }
                #endregion

                #region PRIMO = ASSO DI BRISCOLA
                if (giocatori[0].CartaGiocata.Seme == briscola.Seme && giocatori[0].CartaGiocata.Numero == (NumeroCarta)1)
                {
                    //OGNI QUALSIASI CARTA VENGA GIOCATA DAL SECONDO GIOCATORE
                    c = Scartina(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                    if (c == null)
                    {
                        c = BriscolaPiccola(mazzoCpu, briscola);
                        if (c == null)
                        {
                            c = Carico(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                            if (c == null)
                                return null;
                            else
                                return c;
                        }
                        else
                            return c;
                    }
                    else
                        return c;
                }
                #endregion

                #region PRIMO = TRE DI BRISCOLA
                if (giocatori[0].CartaGiocata.Seme == briscola.Seme && giocatori[0].CartaGiocata.Numero == (NumeroCarta)3)
                {
                    //QUALSIASI CARTA GIOCHI GIOCATORE 2 SVOLGO I SEGUENTI CASI
                    c = Scartina(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                    if (c == null)
                    {
                        c = BriscolaPiccola(mazzoCpu, briscola);
                        if (c == null)
                        {
                            c = SuperaBriscola(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                            if (c == null)
                            {
                                c = Carico(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                                if (c == null)
                                    return null;
                                else
                                    return c;
                            }
                            else
                                return c;
                        }
                        else
                            return c;
                    }
                    else
                        return c;
                }
                #endregion

                #region PRIMO = CARICO
                if (giocatori[0].CartaGiocata.ValoreCarta >= 10 && giocatori[0].CartaGiocata.Seme != briscola.Seme)
                {
                    #region SECONDO = NO BRISCOLA
                    if (giocatori[1].CartaGiocata.Seme != briscola.Seme)
                    {
                        c = SuperaNoBriscola(mazzoCpu, giocatori[0].CartaGiocata, giocatori[1].CartaGiocata);
                        if (c == null)
                        {
                            c = BriscolaPiccola(mazzoCpu, briscola);
                            if (c == null)
                            {
                                c = Scartina(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                                if (c == null)
                                {
                                    c = BriscolaAlta(mazzoCpu, briscola);
                                    if (c == null)
                                    {
                                        c = Carico(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                                        if (c == null)
                                            return null;
                                        else
                                            return c;
                                    }
                                    else
                                        return c;
                                }
                                else
                                    return c;
                            }
                            else
                                return c;
                        }
                        else
                            return c;
                    }
                    #endregion

                    #region SECONDO = BRISCOLA
                    if (giocatori[1].CartaGiocata.Seme == briscola.Seme)
                    {
                        c = SuperaBriscola(mazzoCpu, giocatori[1].CartaGiocata, briscola);
                        if (SuperaBriscola(mazzoCpu, giocatori[1].CartaGiocata, briscola).Numero == (NumeroCarta)3 || SuperaBriscola(mazzoCpu, giocatori[1].CartaGiocata, briscola).Numero == (NumeroCarta)1 || c == null)
                        {
                            c = Scartina(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                            if (c == null)
                            {
                                c = BriscolaPiccola(mazzoCpu, briscola);
                                if (c == null)
                                {
                                    c = BriscolaAlta(mazzoCpu, briscola);
                                    if (c == null)
                                    {
                                        c = Carico(mazzoCpu, giocatori[0].CartaGiocata, briscola);
                                        if (c == null)
                                            return null;
                                        else
                                            return c;
                                    }
                                    else
                                        return c;
                                }
                                else
                                    return c;
                            }
                            else
                                return c;
                        }
                        else
                            return c;
                    }
                    #endregion
                }
                #endregion

            }
            #endregion

            return null;

        }

        #region METODI PER LA SCELTA DELLA CARTA CPU
        protected static Carta SuperaNoBriscola(List<Carta> mazzoUtente, Carta carta1, Carta carta2)
        {
            for (int i = 0; i < mazzoUtente.Count; i++)
            {
                if (mazzoUtente[i].Seme == carta1.Seme &&
                    (mazzoUtente[i].Numero > carta1.Numero ||
                     mazzoUtente[i].Numero > carta2.Numero ||
                     mazzoUtente[i].ValoreCarta > carta1.ValoreCarta ||
                     mazzoUtente[i].ValoreCarta > carta2.ValoreCarta))
                {
                    return mazzoUtente[i];
                }
            }
            return null;
        }

        protected static Carta Scartina(List<Carta> mazzoUtente, Carta carta1, Carta briscola)
        {
            for (int i = 0; i < mazzoUtente.Count; i++)
            {
                if (mazzoUtente[i].Seme != carta1.Seme &&
                    mazzoUtente[i].ValoreCarta == 0 &&
                    mazzoUtente[i].Seme != briscola.Seme)
                {
                    return mazzoUtente[i];
                }
            }
            return null;
        }

        protected static Carta BriscolaPiccola(List<Carta> mazzoUtente, Carta briscola)
        {
            for (int i = 0; i < mazzoUtente.Count; i++)
            {
                if (mazzoUtente[i].Seme == briscola.Seme && mazzoUtente[i].ValoreCarta < 10)
                {
                    return mazzoUtente[i];
                }
            }
            return null;
        }

        protected static Carta Carico(List<Carta> mazzoUtente, Carta carta1, Carta briscola)
        {
            for (int i = 0; i < mazzoUtente.Count; i++)
            {
                if (mazzoUtente[i].ValoreCarta >= 10 &&
                    mazzoUtente[i].Seme != carta1.Seme &&
                    mazzoUtente[i].Seme != briscola.Seme)
                {
                    return mazzoUtente[i];
                }
            }
            return null;
        }

        protected static Carta BriscolaAlta(List<Carta> mazzoUtente, Carta briscola)
        {
            for (int i = 0; i < mazzoUtente.Count; i++)
            {
                if (mazzoUtente[i].Seme == briscola.Seme && mazzoUtente[i].ValoreCarta >= 10)
                {
                    return mazzoUtente[i];
                }
            }
            return null;
        }

        protected static Carta Figura(List<Carta> mazzoUtente, Carta briscola)
        {
            for (int i = 0; i < mazzoUtente.Count; i++)
            {
                if (mazzoUtente[i].Seme != briscola.Seme &&
                    mazzoUtente[i].ValoreCarta != 0 &&
                    mazzoUtente[i].ValoreCarta < 10)
                {
                    return mazzoUtente[i];
                }
            }
            return null;
        }

        protected static Carta SuperaBriscola(List<Carta> mazzoUtente, Carta carta, Carta briscola)
        {
            for (int i = 0; i < mazzoUtente.Count; i++)
            {
                if (mazzoUtente[i].Seme == briscola.Seme &&
                    (mazzoUtente[i].Numero > carta.Numero ||
                    mazzoUtente[i].ValoreCarta >= carta.ValoreCarta))
                {
                    return mazzoUtente[i];
                }
            }
            return null;
        }
        #endregion
    }

    #region QUATTRO GIOCATORI (CHE MAI FAREMO)
    /*class FourPlayers : ThreePlayers
    {
        public static Carta GetCartaCpu(List<Giocatore> giocatori, List<Carta> mazzoCpu1, List<Carta> mazzoCpu2, List<Carta> mazzoCpu3, Carta briscola, string NomeGiocatore, string primo, Carta cartaG1 = null, Carta cartaCPUb = null, Carta cartaCPUc = null)
        {
            Carta cartaCPUa = null;

            #region IL PRIMO A GIOCARE É GIOCATORE 1, SEGUONO LE CPU
            if (giocatori[0].Username == NomeGiocatore)
            {
                #region TURNO DI CPU1
                if (giocatori[1].CartaGiocata == null)
                {
                    #region Carta del giocatore é una briscola alta (figure, asso o tre)
                    if (cartaG1.ValoreCarta != 0 && cartaG1.Seme == briscola.Seme)
                    {
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme != briscola.Seme && mazzoCpu1[i].ValoreCarta == 0)
                                return cartaCPUa = mazzoCpu1[i];
                        // Se arrivo a questo punto significa che in mano ho tutte briscole oppure tutti carichi, perció butto la piú bassa
                        if (mazzoCpu1[0].Numero < mazzoCpu1[1].Numero && mazzoCpu1[0].Numero < mazzoCpu1[2].Numero && mazzoCpu1[0].ValoreCarta < 10 && mazzoCpu1[0].Seme != briscola.Seme)
                            return cartaCPUa = mazzoCpu1[0];
                        if (mazzoCpu1[1].Numero < mazzoCpu1[0].Numero && mazzoCpu1[1].Numero < mazzoCpu1[2].Numero && mazzoCpu1[1].ValoreCarta < 10 && mazzoCpu1[1].Seme != briscola.Seme)
                            return cartaCPUa = mazzoCpu1[1];
                        if (mazzoCpu1[2].Numero < mazzoCpu1[0].Numero && mazzoCpu1[2].Numero < mazzoCpu1[1].Numero && mazzoCpu1[2].ValoreCarta < 10 && mazzoCpu1[2].Seme != briscola.Seme)
                            return cartaCPUa = mazzoCpu1[2];
                        //Se arrivo a questo punto ho carichi (anche di briscola) o figure di briscola
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Numero > (NumeroCarta)3)
                                return cartaCPUa = mazzoCpu1[0];
                        //Se arrivo a questo punto, in mano ho solo carichi (asso o tre) non di briscola
                        return cartaCPUa = mazzoCpu1[0];
                    }
                    #endregion

                    #region Carta del giocatore é un carico non briscola
                    if (cartaG1.ValoreCarta >= 10 && cartaG1.Seme != briscola.Seme)
                    {
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme == cartaG1.Seme && mazzoCpu1[i].ValoreCarta > cartaG1.ValoreCarta)
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto significa che non ho potuto superare il suo carico nel suo seme: cerco una briscola piccola
                        if (mazzoCpu1[0].Numero < mazzoCpu1[1].Numero && mazzoCpu1[0].Numero < mazzoCpu1[2].Numero && mazzoCpu1[0].Seme == briscola.Seme && mazzoCpu1[0].ValoreCarta == 0)
                            return cartaCPUa = mazzoCpu1[0];
                        if (mazzoCpu1[1].Numero < mazzoCpu1[0].Numero && mazzoCpu1[1].Numero < mazzoCpu1[2].Numero && mazzoCpu1[1].Seme == briscola.Seme && mazzoCpu1[1].ValoreCarta == 0)
                            return cartaCPUa = mazzoCpu1[1];
                        if (mazzoCpu1[2].Numero < mazzoCpu1[0].Numero && mazzoCpu1[2].Numero < mazzoCpu1[1].Numero && mazzoCpu1[2].Seme == briscola.Seme && mazzoCpu1[2].ValoreCarta == 0)
                            return cartaCPUa = mazzoCpu1[2];
                        //Se arrivo a questo punto significa che non ho briscole basse, perció cerco una briscola qualsiasi
                        if (mazzoCpu1[0].ValoreCarta < mazzoCpu1[1].ValoreCarta && mazzoCpu1[0].ValoreCarta < mazzoCpu1[2].ValoreCarta && mazzoCpu1[0].Seme == briscola.Seme)
                            return cartaCPUa = mazzoCpu1[0];
                        if (mazzoCpu1[1].ValoreCarta < mazzoCpu1[0].ValoreCarta && mazzoCpu1[1].ValoreCarta < mazzoCpu1[2].ValoreCarta && mazzoCpu1[1].Seme == briscola.Seme)
                            return cartaCPUa = mazzoCpu1[1];
                        if (mazzoCpu1[2].ValoreCarta < mazzoCpu1[0].ValoreCarta && mazzoCpu1[2].ValoreCarta < mazzoCpu1[1].ValoreCarta && mazzoCpu1[2].Seme == briscola.Seme)
                            return cartaCPUa = mazzoCpu1[2];
                        //Se arrivo a questo punto significa che non ho briscole per prendere perció butto una scartina
                        if (mazzoCpu1[0].ValoreCarta < mazzoCpu1[1].ValoreCarta && mazzoCpu1[0].ValoreCarta < mazzoCpu1[2].ValoreCarta)
                            return cartaCPUa = mazzoCpu1[0];
                        if (mazzoCpu1[1].ValoreCarta < mazzoCpu1[0].ValoreCarta && mazzoCpu1[1].ValoreCarta < mazzoCpu1[2].ValoreCarta)
                            return cartaCPUa = mazzoCpu1[1];
                        if (mazzoCpu1[2].ValoreCarta < mazzoCpu1[0].ValoreCarta && mazzoCpu1[2].ValoreCarta < mazzoCpu1[1].ValoreCarta)
                            return cartaCPUa = mazzoCpu1[2];

                    }
                    #endregion

                    #region Carta del giocatore é una briscola bassa (fino al 7)
                    if (cartaG1.ValoreCarta < 10 && cartaG1.Seme == briscola.Seme)
                    {
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme != briscola.Seme && mazzoCpu1[i].ValoreCarta == 0)
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto significa che in mano non avevo scartelle, perció posso avere briscole oppure figure e carichi
                        if (mazzoCpu1[0].Numero < mazzoCpu1[1].Numero && mazzoCpu1[0].Numero < mazzoCpu1[2].Numero && mazzoCpu1[0].Seme != briscola.Seme && mazzoCpu1[0].ValoreCarta < 10)
                            return cartaCPUa = mazzoCpu1[0];
                        if (mazzoCpu1[1].Numero < mazzoCpu1[0].Numero && mazzoCpu1[1].Numero < mazzoCpu1[2].Numero && mazzoCpu1[1].Seme != briscola.Seme && mazzoCpu1[1].ValoreCarta < 10)
                            return cartaCPUa = mazzoCpu1[1];
                        if (mazzoCpu1[2].Numero < mazzoCpu1[0].Numero && mazzoCpu1[2].Numero < mazzoCpu1[1].Numero && mazzoCpu1[2].Seme != briscola.Seme && mazzoCpu1[2].ValoreCarta < 10)
                            return cartaCPUa = mazzoCpu1[2];
                        //Se arrivo a questo punto significa che in mano ho carichi oppure briscole
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme == briscola.Seme && mazzoCpu1[i].Numero < cartaG1.Numero && mazzoCpu1[i].ValoreCarta <= cartaG1.ValoreCarta)
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto significa che in mano ho carichi normali o di briscola
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme != briscola.Seme)
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto ho solamente carichi di briscola
                        return cartaCPUa = mazzoCpu1[0];
                    }
                    #endregion

                    #region Carta del giocatore é una scartina non briscola
                    if (cartaG1.ValoreCarta < 10 && cartaG1.Seme != briscola.Seme)
                    {
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme == cartaG1.Seme && mazzoCpu1[i].ValoreCarta > cartaG1.ValoreCarta)
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto significa che non ho potuto superare: cerco una scartina di un altro seme
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme != briscola.Seme && mazzoCpu1[i].Seme != cartaG1.Seme && mazzoCpu1[i].ValoreCarta == 0)
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto significa che non ho scartine
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme != briscola.Seme && mazzoCpu1[i].Seme != cartaG1.Seme && mazzoCpu1[i].ValoreCarta < 10)
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto significa che non ho pochi punti da dargli, provo a prendere con una briscoletta
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme == briscola.Seme && mazzoCpu1[i].ValoreCarta == 0)
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto devo prendere con una figura di briscola per non regalare carichi
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme == briscola.Seme && mazzoCpu1[i].ValoreCarta < 10)
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto devo dargli un carico non briscola
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme != briscola.Seme)
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo qui sono costretto a prendere con un carico briscola
                        return cartaCPUa = mazzoCpu1[0];
                    }
                    #endregion
  
                }

                #endregion

                #region TURNO DI CPU2
                if (cartaCPUa != null && cartaG1 != null)
                {

                }
                #endregion

                #region TURNO DI CPU3
                if (cartaCPUb != null && cartaCPUa != null && cartaG1 != null)
                {

                }
                #endregion
            }
            #endregion

            #region IL PRIMO A GIOCARE É UNA CPU, SEGUONO DUE CPU E GIOCATORE 1
            if (giocatori[0].Username == "Giocatore 2")
            {

            }
            #endregion

            #region IL PRIMO A GIOCARE É UNA CPU, SEGUONO UNA CPU, GIOCATORE 1 E L'ALTRA CPU
            if(giocatori[0].Username == "Giocatore 3")
            {

            }
            #endregion

            #region IL PRIMO A GIOCARE É UNA CPU, SEGUE GIOCATORE 1 E ALTRE DUE CPU
            if(giocatori[0].Username == "Giocatore 4")
            {

            }
            #endregion

            return null;
        }
    }*/
    #endregion

}
