using Briscola.Models.Enumeratori;
using MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Briscola.Models
{
    public class Partita
    {
        private readonly string _nomeGiocatore;
        public const string CPU1 = "Giocatore 2";
        public const string CPU2 = "Giocatore 3";
        public const string CPU3 = "Giocatore 4";
        public const int NUM_CARTE_MAZZO = 40;
        private readonly List<Carta> _mazzoOrdinato;

        public int NumeroGiocatori { get; private set; }

        public List<Carta> Mazzo { get; set; }

        public List<Giocatore> Giocatori { get; private set; }

        public Carta Briscola { get; private set; }

        public bool IsUltimoTurno
        {
            get
            {
                switch (Giocatori.Count)
                {
                    default:
                        return Giocatori[0]?.MazzoGiocatore?.Count == 0
                            && Giocatori[1]?.MazzoGiocatore?.Count == 0;
                    case (3):
                        return Giocatori[0]?.MazzoGiocatore?.Count == 0
                            && Giocatori[1]?.MazzoGiocatore?.Count == 0
                            && Giocatori[2]?.MazzoGiocatore?.Count == 0;
                    case (4):
                        return Giocatori[0]?.MazzoGiocatore?.Count == 0
                            && Giocatori[1]?.MazzoGiocatore?.Count == 0
                            && Giocatori[2]?.MazzoGiocatore?.Count == 0
                            && Giocatori[3]?.MazzoGiocatore?.Count == 0;
                }
            }
        }

        public Partita(int numeroGiocatori, Giocatore giocatore, TipoCarta tipoCarte)
        {
            Giocatori = new List<Giocatore>();

            _mazzoOrdinato = CreaMazzo(tipoCarte);
            Mazzo = new List<Carta>();

            MescolaMazzo();
            Briscola = Mazzo[Mazzo.Count - 1];

            if (numeroGiocatori == 3) //SE GIOCO A TRE GIOCATORI TOLGO LA CARTA IN PIÚ DAL MAZZO
            {
                TogliCartaExtra();
            }

            Giocatori.Add(giocatore);

            #region CREO LE CPU
            switch (numeroGiocatori)
            {
                case 2:
                    Giocatori.Add(new Giocatore(CPU1, "000"));
                    break;
                case 3:
                    Giocatori.Add(new Giocatore(CPU1, "000"));
                    Giocatori.Add(new Giocatore(CPU2, "000"));
                    break;
                case 4:
                    Giocatori.Add(new Giocatore(CPU1, "000"));
                    Giocatori.Add(new Giocatore(CPU2, "000"));
                    Giocatori.Add(new Giocatore(CPU3, "000"));
                    break;
            }
            #endregion

            _nomeGiocatore = giocatore.Username;
            NumeroGiocatori = numeroGiocatori;

        }

        /// <summary>
        /// Genera il mazzo di carte
        /// </summary>
        private List<Carta> CreaMazzo(TipoCarta tipo)
        {
            List<Carta> mazzo = new List<Carta>();

            for (int i = 1; i <= 4; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    if (tipo == TipoCarta.Trevisana)
                    {
                        mazzo.Add(new CartaTrevisana(j, (Seme)i));
                    }
                    else
                    {
                        mazzo.Add(new CartaNapoletana(j, (Seme)i));
                    }
                }
            }

            return mazzo;
        }

        /// <summary>
        /// Mescola il mazzo di carte da giocare
        /// </summary>
        private void MescolaMazzo()
        {
            for (int i = 0; i < NUM_CARTE_MAZZO; i++)
            {
                Thread.Sleep(10);
                Random rnd = new Random();
                int num = rnd.Next(0, _mazzoOrdinato.Count);
                Mazzo.Add(_mazzoOrdinato[num]);
                _mazzoOrdinato.RemoveAt(num);
            }
        }

        /// <summary>
        /// Pesca una carta casuale dal mazzo in tavola
        /// </summary>
        /// <returns>ritorna la carta pescata</returns>
        public Carta Pesca()
        {
            Carta carta = Mazzo[0];
            Mazzo.RemoveFirst();
            return carta;
        }

        /// <summary>
        /// Confronta le carte giocate dai giocatori e verifica chi ha vinto
        /// </summary>
        /// <param name="carta1">Carta giocatore 1</param>
        /// <param name="carta2">Carta giocatore 2</param>
        public Giocatore Confronto()
        {
            try
            {
                int puntiInTavola = 0;

                foreach (var item in Giocatori) //AGGIORNO IL NUMERO DI PUNTI IN TAVOLA
                {
                    puntiInTavola += item.CartaGiocata.ValoreCarta;
                }

                Giocatore winner = null;

                #region CONFRONTO

                #region DUE GIOCATORI
                if (NumeroGiocatori == 2)
                {
                    #region PRIMO GIOCATORE TIRA BRISCOLA
                    if (Giocatori[0].CartaGiocata.Seme == Briscola.Seme)
                    {
                        #region SECONDO TIRA BRISCOLA
                        if (Giocatori[1].CartaGiocata.Seme == Briscola.Seme)
                        {
                            #region SE BRISCOLA2 SUPERA BRISCOLA1
                            if (Giocatori[1].CartaGiocata.Numero > Giocatori[0].CartaGiocata.Numero)
                            {
                                if (Giocatori[1].CartaGiocata.ValoreCarta >= Giocatori[0].CartaGiocata.ValoreCarta)
                                {
                                    winner = Giocatori[1];
                                }
                            }

                            Carta asso = new Carta(1, Giocatori[0].CartaGiocata.Seme);
                            Carta tre = new Carta(3, Giocatori[0].CartaGiocata.Seme);

                            if (Giocatori[1].CartaGiocata.Numero == asso.Numero)
                            {
                                winner = Giocatori[1];
                            }

                            if (Giocatori[1].CartaGiocata.Numero == tre.Numero && Giocatori[0].CartaGiocata.Numero != asso.Numero)
                            {
                                winner = Giocatori[1];
                            }
                            #endregion

                        }
                        #endregion
                        if (winner == null)
                        {
                            winner = Giocatori[0];
                        }
                    }
                    #endregion

                    #region PRIMO GIOCATORE TIRA NON BRISCOLA
                    else
                    {
                        #region SECONDO GIOCATORE TIRA BRISCOLA
                        if (Giocatori[1].CartaGiocata.Seme == Briscola.Seme)
                        {
                            winner = Giocatori[1];
                        }
                        #endregion

                        #region SECONDO SEME = PRIMO SEME
                        else if (Giocatori[1].CartaGiocata.Seme == Giocatori[0].CartaGiocata.Seme)
                        {
                            #region SECONDO SUPERA PRIMO
                            if (Giocatori[1].CartaGiocata.Numero > Giocatori[0].CartaGiocata.Numero)
                            {
                                if (Giocatori[1].CartaGiocata.ValoreCarta >= Giocatori[0].CartaGiocata.ValoreCarta)
                                {
                                    winner = Giocatori[1];
                                }
                            }

                            Carta asso = new Carta(1, Giocatori[0].CartaGiocata.Seme);
                            Carta tre = new Carta(3, Giocatori[0].CartaGiocata.Seme);

                            if (Giocatori[1].CartaGiocata.Numero == asso.Numero)
                            {
                                winner = Giocatori[1];
                            }

                            if (Giocatori[1].CartaGiocata.Numero == tre.Numero && Giocatori[0].CartaGiocata.Numero != asso.Numero)
                            {
                                winner = Giocatori[1];
                            }
                            #endregion
                        }
                        #endregion

                        if (winner == null)
                        {
                            winner = Giocatori[0];
                        }
                    }
                    #endregion
                }
                #endregion

                #region TRE GIOCATORI
                else if (NumeroGiocatori == 3)
                {
                    #region PRIMO TIRA BRISCOLA
                    if (Giocatori[0].CartaGiocata.Seme == Briscola.Seme)
                    {
                        #region SECONDO TIRA BRISCOLA
                        if (Giocatori[1].CartaGiocata.Seme == Briscola.Seme)
                        {
                            #region BRISCOLA2 SUPERA BRISCOLA1
                            if (Giocatori[1].CartaGiocata.Numero > Giocatori[0].CartaGiocata.Numero)
                            {
                                if (Giocatori[1].CartaGiocata.ValoreCarta >= Giocatori[0].CartaGiocata.ValoreCarta)
                                {
                                    winner = Giocatori[1];
                                    #region TERZO TIRA BRISCOLA
                                    if (Giocatori[2].CartaGiocata.Seme == Briscola.Seme)
                                    {
                                        #region BRISCOLA3 SUPERA BRISCOLA2
                                        if (Giocatori[2].CartaGiocata.Numero > Giocatori[1].CartaGiocata.Numero || Giocatori[2].CartaGiocata.ValoreCarta >= Giocatori[2].CartaGiocata.ValoreCarta)
                                        {
                                            winner = Giocatori[2];
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                            }

                            Carta asso = new Carta(1, Giocatori[0].CartaGiocata.Seme);
                            Carta tre = new Carta(3, Giocatori[0].CartaGiocata.Seme);
                            if (Giocatori[1].CartaGiocata.Numero == asso.Numero)
                            {
                                winner = Giocatori[1];
                                #region TERZO TIRA BRISCOLA
                                if (Giocatori[2].CartaGiocata.Seme == Briscola.Seme)
                                {
                                    #region BRISCOLA3 SUPERA BRISCOLA2
                                    if (Giocatori[2].CartaGiocata.Numero > Giocatori[1].CartaGiocata.Numero || Giocatori[2].CartaGiocata.ValoreCarta >= Giocatori[2].CartaGiocata.ValoreCarta)
                                    {
                                        winner = Giocatori[2];
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            if (Giocatori[1].CartaGiocata.Numero == tre.Numero && Giocatori[0].CartaGiocata.Numero != asso.Numero)
                            {
                                winner = Giocatori[1];
                                #region TERZO TIRA BRISCOLA
                                if (Giocatori[2].CartaGiocata.Seme == Briscola.Seme)
                                {
                                    #region BRISCOLA3 SUPERA BRISCOLA2
                                    if (Giocatori[2].CartaGiocata.Numero > Giocatori[1].CartaGiocata.Numero || Giocatori[2].CartaGiocata.ValoreCarta >= Giocatori[2].CartaGiocata.ValoreCarta)
                                    {
                                        winner = Giocatori[2];
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion
                        }
                        #endregion

                        #region TERZO TIRA BRISCOLA
                        if (Giocatori[2].CartaGiocata.Seme == Briscola.Seme)
                        {
                            #region BRISCOLA3 SUPERA BRISCOLA1
                            if (Giocatori[2].CartaGiocata.Numero > Giocatori[0].CartaGiocata.Numero)
                            {
                                if (Giocatori[2].CartaGiocata.ValoreCarta >= Giocatori[0].CartaGiocata.ValoreCarta)
                                {
                                    winner = Giocatori[2];
                                }
                            }

                            Carta asso = new Carta(1, Giocatori[0].CartaGiocata.Seme);
                            Carta tre = new Carta(3, Giocatori[0].CartaGiocata.Seme);
                            if (Giocatori[2].CartaGiocata.Numero == asso.Numero)
                            {
                                winner = Giocatori[2];
                            }

                            if (Giocatori[2].CartaGiocata.Numero == tre.Numero && Giocatori[0].CartaGiocata.Numero != asso.Numero)
                            {
                                winner = Giocatori[2];
                            }
                            #endregion
                        }
                        #endregion

                    }
                    #endregion

                    #region PRIMO TIRA NON BRISCOLA
                    else if (Giocatori[0].CartaGiocata.Seme != Briscola.Seme)
                    {
                        #region SECONDO TIRA BRISCOLA
                        if (Giocatori[1].CartaGiocata.Seme == Briscola.Seme)
                        {
                            winner = Giocatori[1];
                            #region TERZO TIRA BRISCOLA E SUPERA BRISCOLA 2
                            if (Giocatori[2].CartaGiocata.Seme == Briscola.Seme)
                            {
                                if (Giocatori[2].CartaGiocata.Numero > Giocatori[1].CartaGiocata.Numero)
                                {
                                    if (Giocatori[2].CartaGiocata.ValoreCarta >= Giocatori[1].CartaGiocata.ValoreCarta)
                                    {
                                        winner = Giocatori[2];
                                    }
                                }

                                Carta asso = new Carta(1, Giocatori[1].CartaGiocata.Seme);
                                Carta tre = new Carta(3, Giocatori[1].CartaGiocata.Seme);

                                if (Giocatori[2].CartaGiocata.Numero == asso.Numero)
                                {
                                    winner = Giocatori[2];
                                }

                                if (Giocatori[2].CartaGiocata.Numero == tre.Numero && Giocatori[1].CartaGiocata.Numero != asso.Numero)
                                {
                                    winner = Giocatori[2];
                                }
                            }
                            #endregion
                        }
                        #endregion

                        #region SECONDO SUPERA
                        else if (Giocatori[1].CartaGiocata.Seme == Giocatori[0].CartaGiocata.Seme)
                        {
                            if (Giocatori[1].CartaGiocata.Numero > Giocatori[0].CartaGiocata.Numero)
                            {
                                if (Giocatori[1].CartaGiocata.ValoreCarta >= Giocatori[0].CartaGiocata.ValoreCarta)
                                {
                                    winner = Giocatori[1];
                                    #region TERZO TIRA SEME UGUALE
                                    if (Giocatori[2].CartaGiocata.Seme == Giocatori[0].CartaGiocata.Seme)
                                    {
                                        if (Giocatori[2].CartaGiocata.Numero > Giocatori[1].CartaGiocata.Numero &&
                                            Giocatori[2].CartaGiocata.ValoreCarta >= Giocatori[1].CartaGiocata.ValoreCarta)
                                        {
                                            winner = Giocatori[2];
                                        }
                                    }
                                    #endregion
                                }
                            }

                            Carta asso = new Carta(1, Giocatori[0].CartaGiocata.Seme);
                            Carta tre = new Carta(3, Giocatori[0].CartaGiocata.Seme);

                            if (Giocatori[1].CartaGiocata.Numero == asso.Numero)
                            {
                                winner = Giocatori[1];

                                #region TERZO TIRA SEME UGUALE
                                if (Giocatori[2].CartaGiocata.Seme == Giocatori[0].CartaGiocata.Seme)
                                {
                                    if (Giocatori[2].CartaGiocata.Numero > Giocatori[1].CartaGiocata.Numero &&
                                        Giocatori[2].CartaGiocata.ValoreCarta >= Giocatori[1].CartaGiocata.ValoreCarta)
                                    {
                                        winner = Giocatori[2];
                                    }
                                }
                                #endregion
                            }
                            if (Giocatori[1].CartaGiocata.Numero == tre.Numero &&
                                Giocatori[0].CartaGiocata.Numero != asso.Numero)
                            {
                                winner = Giocatori[1];
                                #region TERZO TIRA SEME UGUALE
                                if (Giocatori[2].CartaGiocata.Seme == Giocatori[0].CartaGiocata.Seme)
                                {
                                    if (Giocatori[2].CartaGiocata.Numero > Giocatori[1].CartaGiocata.Numero &&
                                        Giocatori[2].CartaGiocata.ValoreCarta >= Giocatori[1].CartaGiocata.ValoreCarta)
                                    {
                                        winner = Giocatori[2];
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion

                        #region TERZO TIRA BRISCOLA
                        else if (Giocatori[2].CartaGiocata.Seme == Briscola.Seme)
                        {
                            winner = Giocatori[2];
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion

                if (winner == null)
                {
                    winner = Giocatori[0];
                }
                #endregion

                //Giocatore g = Giocatori.Find(x => x.Username == winner.Username);
                //g.Punti += puntiInTavola;
                //g.MazzoPunti.AddRange(Giocatori.Select(item => item.CartaGiocata));//ASSEGNO I PUNTI AL VINCITORE

                winner.Punti += puntiInTavola;
                winner.MazzoPunti.AddRange(Giocatori.Select(item => item.CartaGiocata));//ASSEGNO I PUNTI AL VINCITORE

                return winner;
            }
            catch (Exception ex)
            {
                MsgBox.Show("Attenzione", ex.Message, MessageBoxType.Error);
                return null;
            }
        }

        /// <summary>
        /// Togliere una carta (un 2 non briscola) dal mazzo se si gioca in 3
        /// </summary>
        private void TogliCartaExtra()
        {
            Carta c = Briscola.Seme == Seme.Bastoni ? new Carta(2, Seme.Coppe) : new Carta(2, Seme.Bastoni);

            Mazzo.Remove(c);
        }

        /// <summary>
        /// Ordina i giocatori dopo ogni turno
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool OrdineTurno(int index)
        {
            try
            {
                List<Giocatore> ordineGiocatori = new List<Giocatore>();

                for (int i = index; i <= Giocatori.Count && ordineGiocatori.Count < Giocatori.Count;)
                {
                    if (i != Giocatori.Count)
                    {
                        ordineGiocatori.Add(Giocatori[i]);
                        i++;
                    }
                    else
                    {
                        i = 0;
                    }
                }
                Giocatori = ordineGiocatori;

                return Giocatori[0]?.Username == _nomeGiocatore;
            }
            catch (Exception ex)
            {
                MsgBox.Show("Attenzione", ex.Message, MessageBoxType.Error);
                return false;
            }
        }

        /// <summary>
        /// A fine partita si verifica il vincitore
        /// </summary>
        public Giocatore ConfrontaVincitore() =>
            Giocatori?.OrderByDescending(g => g.Punti)?.First();
    }
}
