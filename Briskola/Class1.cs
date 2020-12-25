using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Threading;

namespace Briskola
{
    #region Statistiche
    public class Statistiche
    {
        public int PartiteVinte { get; private set; }
        public int PartitePerse { get; private set; }
        public int PunteggioTotale { get; private set; }
        public int PunteggioMaxPartita { get; private set; }

        public Statistiche(OleDbConnection connection, Giocatore giocatore)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter($"SELECT * FROM STATISTICHE WHERE USERNAME = '{giocatore.Username}'", connection);
            DataTable table = new DataTable("Statistiche");
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                PartiteVinte = (int)table.Rows[0][1];
                PartitePerse = (int)table.Rows[0][2];
                PunteggioTotale = (int)table.Rows[0][3];
                PunteggioMaxPartita = (int)table.Rows[0][4];
            }
        }
    }
    #endregion
    #region Giocatore
    public class Giocatore
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Nome { get; private set; }
        public string Cognome { get; private set; }
        public string Eta { get; private set; }
        public int Punti { get; set; }
        public List<Carta> MazzoPunti { get; set; }
        public List<Carta> MazzoGiocatore { get; set; }
        public Carta CartaGiocata { get; set; }

        public Giocatore(string username, string psw, string nome = "", string cognome = "", string eta = "")
        {
            Username = username;
            Password = psw;
            Nome = nome;
            Cognome = cognome;
            Eta = eta;
            MazzoGiocatore = new List<Carta>();
            MazzoPunti = new List<Carta>();
        }
        public Statistiche GetPlayerStats(OleDbConnection connection)
        {
            Statistiche stats = new Statistiche(connection, this);
            return stats;
        }
    }
    #endregion
    #region Enumeratori Carte
    public enum NumeroCarta
    {
        Asso = 1,
        Due = 2,
        Tre = 3,
        Quattro = 4,
        Cinque = 5,
        Sei = 6,
        Sette = 7,
        Fante = 8,
        Cavallo = 9,
        Re = 10
    }
    public enum Seme
    {
        Spade = 1,
        Bastoni = 2,
        Denari = 3,
        Coppe = 4
    }
    #endregion
    #region Carta
    public class Carta
    {
        public NumeroCarta Numero { get; private set; }
        public Seme Seme { get; private set; }
        public string Img { get; protected set; }

        public Carta(int numero, Seme seme)
        {
            Numero = (NumeroCarta)numero;
            Seme = seme;
        }

        public int OttieniValoreCarta()
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
    #endregion

    public class Partita
    {
        private string _nomeGiocatore;
        public const string CPU1 = "Giocatore 2";
        public const string CPU2 = "Giocatore 3";
        public const string CPU3 = "Giocatore 4";

        public int NumeroGiocatori { get; private set; }

        private List<Carta> mazzoOrdinato;

        public readonly List<Carta> Mazzo;

        public List<Giocatore> Giocatori { get; private set; }

        public Carta Briscola { get; private set; }

        public Partita(int numeroGiocatori, Giocatore giocatore, string tipoCarte)
        {
            Giocatori = new List<Giocatore>();

            mazzoOrdinato = CreaMazzo(tipoCarte);
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
        List<Carta> CreaMazzo(string tipo)
        {
            List<Carta> mazzo = new List<Carta>();

            for (int i = 1; i <= 4; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    if (tipo == "trevisane")
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
            Random rnd;
            for (int i = 0; i < 40; i++)
            {
                Thread.Sleep(10);
                rnd = new Random();
                int num = rnd.Next(0, mazzoOrdinato.Count);
                Mazzo.Add(mazzoOrdinato[num]);
                mazzoOrdinato.RemoveAt(num);
            }
        }

        /// <summary>
        /// Pesca una carta casuale dal mazzo in tavola
        /// </summary>
        /// <returns>ritorna la carta pescata</returns>
        public Carta Pesca(int index)
        {
            Carta carta = Mazzo[index];
            Mazzo.RemoveAt(index);
            return carta;
        }

        /// <summary>
        /// Confronta le carte giocate dai giocatori e verifica chi ha vinto
        /// </summary>
        /// <param name="carta1">Carta giocatore 1</param>
        /// <param name="carta2">Carta giocatore 2</param>
        public Giocatore Confronto()
        {
            int puntiInTavola = 0;
            foreach (var item in Giocatori) //AGGIORNO IL NUMERO DI PUNTI IN TAVOLA
            {
                puntiInTavola += item.CartaGiocata.OttieniValoreCarta();
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
                            if (Giocatori[1].CartaGiocata.OttieniValoreCarta() >= Giocatori[0].CartaGiocata.OttieniValoreCarta())
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
                        winner = Giocatori[0];
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
                            if (Giocatori[1].CartaGiocata.OttieniValoreCarta() >= Giocatori[0].CartaGiocata.OttieniValoreCarta())
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
                            if (Giocatori[1].CartaGiocata.OttieniValoreCarta() >= Giocatori[0].CartaGiocata.OttieniValoreCarta())
                            {
                                winner = Giocatori[1];
                                #region TERZO TIRA BRISCOLA
                                if (Giocatori[2].CartaGiocata.Seme == Briscola.Seme)
                                {
                                    #region BRISCOLA3 SUPERA BRISCOLA2
                                    if (Giocatori[2].CartaGiocata.Numero > Giocatori[1].CartaGiocata.Numero || Giocatori[2].CartaGiocata.OttieniValoreCarta() >= Giocatori[2].CartaGiocata.OttieniValoreCarta())
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
                                if (Giocatori[2].CartaGiocata.Numero > Giocatori[1].CartaGiocata.Numero || Giocatori[2].CartaGiocata.OttieniValoreCarta() >= Giocatori[2].CartaGiocata.OttieniValoreCarta())
                                    winner = Giocatori[2];
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
                                if (Giocatori[2].CartaGiocata.Numero > Giocatori[1].CartaGiocata.Numero || Giocatori[2].CartaGiocata.OttieniValoreCarta() >= Giocatori[2].CartaGiocata.OttieniValoreCarta())
                                    winner = Giocatori[2];
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
                            if (Giocatori[2].CartaGiocata.OttieniValoreCarta() >= Giocatori[0].CartaGiocata.OttieniValoreCarta())
                                winner = Giocatori[2];
                        Carta asso = new Carta(1, Giocatori[0].CartaGiocata.Seme);
                        Carta tre = new Carta(3, Giocatori[0].CartaGiocata.Seme);
                        if (Giocatori[2].CartaGiocata.Numero == asso.Numero)
                            winner = Giocatori[2];
                        if (Giocatori[2].CartaGiocata.Numero == tre.Numero && Giocatori[0].CartaGiocata.Numero != asso.Numero)
                            winner = Giocatori[2];
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
                                if (Giocatori[2].CartaGiocata.OttieniValoreCarta() >= Giocatori[1].CartaGiocata.OttieniValoreCarta())
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
                            if (Giocatori[1].CartaGiocata.OttieniValoreCarta() >= Giocatori[0].CartaGiocata.OttieniValoreCarta())
                            {
                                winner = Giocatori[1];
                                #region TERZO TIRA SEME UGUALE
                                if (Giocatori[2].CartaGiocata.Seme == Giocatori[0].CartaGiocata.Seme)
                                {
                                    if (Giocatori[2].CartaGiocata.Numero > Giocatori[1].CartaGiocata.Numero &&
                                        Giocatori[2].CartaGiocata.OttieniValoreCarta() >= Giocatori[1].CartaGiocata.OttieniValoreCarta())
                                    {
                                        winner = Giocatori[2];
                                    }
                                }
                                #endregion
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
                                    Giocatori[2].CartaGiocata.OttieniValoreCarta() >= Giocatori[1].CartaGiocata.OttieniValoreCarta())
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
                                    Giocatori[2].CartaGiocata.OttieniValoreCarta() >= Giocatori[1].CartaGiocata.OttieniValoreCarta())
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

            for (int i = 0; i < Giocatori.Count; i++) //TROVO IL VINCITORE E GLI AGGIUNGO PUNTI E CARTE VINTE
            {
                if (Giocatori[i].Username == winner.Username)
                {
                    Giocatori[i].Punti += puntiInTavola;
                    Giocatori[i].MazzoPunti.AddRange(Giocatori.Select(item => item.CartaGiocata));//ASSEGNO I PUNTI AL VINCITORE
                }
            }
            return winner;
        }

        /// <summary>
        /// Togliere una carta (un 2 non briscola) dal mazzo se si gioca in 3
        /// </summary>
        void TogliCartaExtra()
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
            List<Giocatore> ordineGiocatori = new List<Giocatore>();
            for (int i = index; i <= Giocatori.Count && ordineGiocatori.Count < Giocatori.Count;)
            {
                if (i != Giocatori.Count)
                {
                    ordineGiocatori.Add(Giocatori[i]);
                    i++;
                }
                else
                    i = 0;
            }
            Giocatori = ordineGiocatori;

            return Giocatori[0].Username == _nomeGiocatore;
        }

        /// <summary>
        /// A fine partita si verifica il vincitore
        /// </summary>
        public Giocatore ConfrontaVincitore()
        {
            for (int i = 1; i < Giocatori.Count; i++)
            {
                for (int j = 0; j < Giocatori.Count - i; j++)
                {
                    if (Giocatori[j].Punti < Giocatori[j + 1].Punti)
                    {
                        Giocatore temp = Giocatori[j];
                        Giocatori[j] = Giocatori[j + 1];
                        Giocatori[j + 1] = temp;
                    }
                }
            }
            return Giocatori[0];
        }
    }

    #region IA CPU
    public class TwoPlayers
    {
        public static Carta GetCartaCpu(List<Carta> mazzoCpu1, Carta briscola, out int index, Carta cartaG1 = null)
        {
            #region Inizia la CPU
            if (cartaG1 == null)
            {
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    int valoreCarta = mazzoCpu1[i].OttieniValoreCarta();
                    if (valoreCarta == 0 && mazzoCpu1[i].Seme != briscola.Seme)//provo a buttare una scartella) //provo a buttare asso o tre di briscola
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }
                }
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].OttieniValoreCarta() < 10 && mazzoCpu1[i].Seme != briscola.Seme) //provo a buttare una figura
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }
                }
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].OttieniValoreCarta() == 0 && mazzoCpu1[i].Seme == briscola.Seme) //provo a buttare una briscoletta
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }
                }
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].OttieniValoreCarta() < 10 && mazzoCpu1[i].Seme == briscola.Seme) //provo a buttare una figura di briscola
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }
                }
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].OttieniValoreCarta() >= 10 && mazzoCpu1[i].Seme != briscola.Seme) //provo a buttare un carico
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }
                }
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].OttieniValoreCarta() >= 10 && mazzoCpu1[i].Seme == briscola.Seme) //provo a buttare asso o tre di briscola
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }
                }

            }
            #endregion

            #region Carta del giocatore é una briscola alta (figure, asso o tre)
            if (cartaG1.OttieniValoreCarta() != 0 && cartaG1.Seme == briscola.Seme)
            {
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme != briscola.Seme &&
                        mazzoCpu1[i].OttieniValoreCarta() == 0)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }                // Se arrivo a questo punto significa che in mano ho tutte briscole oppure tutti carichi, perció butto la piú bassa
                }

                if (mazzoCpu1[0].Numero < mazzoCpu1[1].Numero &&
                    mazzoCpu1[0].Numero < mazzoCpu1[2].Numero &&
                    mazzoCpu1[0].OttieniValoreCarta() < 10 &&
                    mazzoCpu1[0].Seme != briscola.Seme)
                {
                    index = 0;
                    return mazzoCpu1[0];
                }

                if (mazzoCpu1[1].Numero < mazzoCpu1[0].Numero &&
                    mazzoCpu1[1].Numero < mazzoCpu1[2].Numero &&
                    mazzoCpu1[1].OttieniValoreCarta() < 10 &&
                    mazzoCpu1[1].Seme != briscola.Seme)
                {
                    index = 1;
                    return mazzoCpu1[1];
                }

                if (mazzoCpu1[2].Numero < mazzoCpu1[0].Numero &&
                    mazzoCpu1[2].Numero < mazzoCpu1[1].Numero &&
                    mazzoCpu1[2].OttieniValoreCarta() < 10 &&
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
            if (cartaG1.OttieniValoreCarta() >= 10 && cartaG1.Seme != briscola.Seme)
            {
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme == cartaG1.Seme &&
                        mazzoCpu1[i].OttieniValoreCarta() > cartaG1.OttieniValoreCarta())
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }
                } //Se arrivo a questo punto significa che non ho potuto superare il suo carico nel suo seme: cerco una briscola piccola

                if (mazzoCpu1[0].Seme == briscola.Seme &&
                    mazzoCpu1[0].OttieniValoreCarta() == 0)
                {
                    index = 0;
                    return mazzoCpu1[0];
                }

                if (mazzoCpu1[1].Seme == briscola.Seme &&
                    mazzoCpu1[1].OttieniValoreCarta() == 0)
                {
                    index = 1;
                    return mazzoCpu1[1];
                }

                if (mazzoCpu1[2].Seme == briscola.Seme &&
                    mazzoCpu1[2].OttieniValoreCarta() == 0)
                {
                    index = 2;
                    return mazzoCpu1[2];
                } //Se arrivo a questo punto significa che non ho briscole basse, perció cerco una briscola qualsiasi

                if (mazzoCpu1[0].OttieniValoreCarta() < mazzoCpu1[1].OttieniValoreCarta() &&
                    mazzoCpu1[0].OttieniValoreCarta() < mazzoCpu1[2].OttieniValoreCarta() &&
                    mazzoCpu1[0].Seme == briscola.Seme)
                {
                    index = 0;
                    return mazzoCpu1[0];
                }

                if (mazzoCpu1[1].OttieniValoreCarta() < mazzoCpu1[0].OttieniValoreCarta() &&
                    mazzoCpu1[1].OttieniValoreCarta() < mazzoCpu1[2].OttieniValoreCarta() &&
                    mazzoCpu1[1].Seme == briscola.Seme)
                {
                    index = 1;
                    return mazzoCpu1[1];
                }

                if (mazzoCpu1[2].OttieniValoreCarta() < mazzoCpu1[0].OttieniValoreCarta() &&
                    mazzoCpu1[2].OttieniValoreCarta() < mazzoCpu1[1].OttieniValoreCarta() &&
                    mazzoCpu1[2].Seme == briscola.Seme)
                {
                    index = 2;
                    return mazzoCpu1[2];
                } //Se arrivo a questo punto significa che non ho briscole per prendere perció butto una scartina

                if (mazzoCpu1[0].OttieniValoreCarta() < mazzoCpu1[1].OttieniValoreCarta() &&
                    mazzoCpu1[0].OttieniValoreCarta() < mazzoCpu1[2].OttieniValoreCarta())
                {
                    index = 0;
                    return mazzoCpu1[0];
                }

                if (mazzoCpu1[1].OttieniValoreCarta() < mazzoCpu1[0].OttieniValoreCarta() &&
                    mazzoCpu1[1].OttieniValoreCarta() < mazzoCpu1[2].OttieniValoreCarta())
                {
                    index = 1;
                    return mazzoCpu1[1];
                }

                if (mazzoCpu1[2].OttieniValoreCarta() < mazzoCpu1[0].OttieniValoreCarta() &&
                    mazzoCpu1[2].OttieniValoreCarta() < mazzoCpu1[1].OttieniValoreCarta())
                {
                    index = 2;
                    return mazzoCpu1[2];
                }
            }
            #endregion

            #region Carta del giocatore é una briscola bassa (fino al 7)
            if (cartaG1.OttieniValoreCarta() < 10 && cartaG1.Seme == briscola.Seme)
            {
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme != briscola.Seme && mazzoCpu1[i].OttieniValoreCarta() == 0)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    } //Se arrivo a questo punto significa che in mano non avevo scartelle, perció posso avere briscole oppure figure e carichi
                }

                if (mazzoCpu1[0].Numero < mazzoCpu1[1].Numero &&
                    mazzoCpu1[0].Numero < mazzoCpu1[2].Numero &&
                    mazzoCpu1[0].Seme != briscola.Seme &&
                    mazzoCpu1[0].OttieniValoreCarta() < 10)
                {
                    index = 0;
                    return mazzoCpu1[0];
                }

                if (mazzoCpu1[1].Numero < mazzoCpu1[0].Numero &&
                    mazzoCpu1[1].Numero < mazzoCpu1[2].Numero &&
                    mazzoCpu1[1].Seme != briscola.Seme &&
                    mazzoCpu1[1].OttieniValoreCarta() < 10)
                {
                    index = 1;
                    return mazzoCpu1[1];
                }

                if (mazzoCpu1[2].Numero < mazzoCpu1[0].Numero &&
                    mazzoCpu1[2].Numero < mazzoCpu1[1].Numero &&
                    mazzoCpu1[2].Seme != briscola.Seme &&
                    mazzoCpu1[2].OttieniValoreCarta() < 10)
                {
                    index = 2;
                    return mazzoCpu1[2];
                } //Se arrivo a questo punto significa che in mano ho carichi oppure briscole

                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme == briscola.Seme &&
                        mazzoCpu1[i].Numero < cartaG1.Numero &&
                        mazzoCpu1[i].OttieniValoreCarta() <= cartaG1.OttieniValoreCarta())
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
            if (cartaG1.OttieniValoreCarta() < 10 && cartaG1.Seme != briscola.Seme)
            {
                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme == cartaG1.Seme &&
                        mazzoCpu1[i].OttieniValoreCarta() > cartaG1.OttieniValoreCarta())
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }                //Se arrivo a questo punto significa che non ho potuto superare: cerco una scartina di un altro seme
                }

                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme != briscola.Seme &&
                        mazzoCpu1[i].Seme != cartaG1.Seme &&
                        mazzoCpu1[i].OttieniValoreCarta() == 0)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }                 //Se arrivo a questo punto significa che non ho scartine
                }

                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme != briscola.Seme &&
                        mazzoCpu1[i].Seme != cartaG1.Seme &&
                        mazzoCpu1[i].OttieniValoreCarta() < 10)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }                 //Se arrivo a questo punto significa che non ho pochi punti da dargli, provo a prendere con una briscoletta
                }

                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme == briscola.Seme &&
                        mazzoCpu1[i].OttieniValoreCarta() == 0)
                    {
                        index = i;
                        return mazzoCpu1[i];
                    }                 //Se arrivo a questo punto devo prendere con una figura di briscola per non regalare carichi
                }

                for (int i = 0; i < mazzoCpu1.Count; i++)
                {
                    if (mazzoCpu1[i].Seme == briscola.Seme &&
                        mazzoCpu1[i].OttieniValoreCarta() < 10)
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
                    if (mazzoCpu[i].OttieniValoreCarta() == 0 && mazzoCpu[i].Seme != briscola.Seme) //provo a buttare una scartella
                    {
                        c = mazzoCpu[i];
                    }
                }
                for (int i = 0; i < mazzoCpu.Count; i++)
                {
                    if (mazzoCpu[i].OttieniValoreCarta() < 10 && mazzoCpu[i].Seme != briscola.Seme) //provo a buttare una figura
                    {
                        c = mazzoCpu[i];
                    }
                }
                for (int i = 0; i < mazzoCpu.Count; i++)
                {
                    if (mazzoCpu[i].OttieniValoreCarta() == 0 && mazzoCpu[i].Seme == briscola.Seme) //provo a buttare una briscoletta
                    {
                        c = mazzoCpu[i];
                    }
                }
                for (int i = 0; i < mazzoCpu.Count; i++)
                {
                    if (mazzoCpu[i].OttieniValoreCarta() < 10 && mazzoCpu[i].Seme == briscola.Seme) //provo a buttare una figura di briscola
                    {
                        c = mazzoCpu[i];
                    }
                }
                for (int i = 0; i < mazzoCpu.Count; i++)
                {
                    if (mazzoCpu[i].OttieniValoreCarta() >= 10 && mazzoCpu[i].Seme != briscola.Seme) //provo a buttare un carico
                    {
                        c = mazzoCpu[i];
                    }
                }
                for (int i = 0; i < mazzoCpu.Count; i++)
                {
                    if (mazzoCpu[i].OttieniValoreCarta() >= 10 && mazzoCpu[i].Seme == briscola.Seme) //provo a buttare asso o tre di briscola
                        c = mazzoCpu[i];
                }
                return c;
            }
            #endregion

            #region CPU gioca da secondo dopo il GIOCATORE 1
            if (cartaG1 != null && cartaAltraCpu == null)
            {
                #region Carta del giocatore é una briscola alta (figure, asso o tre)
                if (cartaG1.OttieniValoreCarta() != 0 && cartaG1.Seme == briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].OttieniValoreCarta() == 0)
                            return mazzoCpu[i];
                    // Se arrivo a questo punto significa che in mano ho tutte briscole oppure tutti carichi, perció butto la piú bassa
                    if (mazzoCpu[0].Numero < mazzoCpu[1].Numero && mazzoCpu[0].Numero < mazzoCpu[2].Numero && mazzoCpu[0].OttieniValoreCarta() < 10 && mazzoCpu[0].Seme != briscola.Seme)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].Numero < mazzoCpu[0].Numero && mazzoCpu[1].Numero < mazzoCpu[2].Numero && mazzoCpu[1].OttieniValoreCarta() < 10 && mazzoCpu[1].Seme != briscola.Seme)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].Numero < mazzoCpu[0].Numero && mazzoCpu[2].Numero < mazzoCpu[1].Numero && mazzoCpu[2].OttieniValoreCarta() < 10 && mazzoCpu[2].Seme != briscola.Seme)
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
                if (cartaG1.OttieniValoreCarta() >= 10 && cartaG1.Seme != briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == cartaG1.Seme && mazzoCpu[i].OttieniValoreCarta() > cartaG1.OttieniValoreCarta())
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho potuto superare il suo carico nel suo seme: cerco una briscola piccola
                    if (mazzoCpu[0].Numero < mazzoCpu[1].Numero && mazzoCpu[0].Numero < mazzoCpu[2].Numero && mazzoCpu[0].Seme == briscola.Seme && mazzoCpu[0].OttieniValoreCarta() == 0)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].Numero < mazzoCpu[0].Numero && mazzoCpu[1].Numero < mazzoCpu[2].Numero && mazzoCpu[1].Seme == briscola.Seme && mazzoCpu[1].OttieniValoreCarta() == 0)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].Numero < mazzoCpu[0].Numero && mazzoCpu[2].Numero < mazzoCpu[1].Numero && mazzoCpu[2].Seme == briscola.Seme && mazzoCpu[2].OttieniValoreCarta() == 0)
                        return mazzoCpu[2];
                    //Se arrivo a questo punto significa che non ho briscole basse, perció cerco una briscola qualsiasi
                    if (mazzoCpu[0].OttieniValoreCarta() < mazzoCpu[1].OttieniValoreCarta() && mazzoCpu[0].OttieniValoreCarta() < mazzoCpu[2].OttieniValoreCarta() && mazzoCpu[0].Seme == briscola.Seme)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].OttieniValoreCarta() < mazzoCpu[0].OttieniValoreCarta() && mazzoCpu[1].OttieniValoreCarta() < mazzoCpu[2].OttieniValoreCarta() && mazzoCpu[1].Seme == briscola.Seme)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].OttieniValoreCarta() < mazzoCpu[0].OttieniValoreCarta() && mazzoCpu[2].OttieniValoreCarta() < mazzoCpu[1].OttieniValoreCarta() && mazzoCpu[2].Seme == briscola.Seme)
                        return mazzoCpu[2];
                    //Se arrivo a questo punto significa che non ho briscole per prendere perció butto una scartina
                    if (mazzoCpu[0].OttieniValoreCarta() < mazzoCpu[1].OttieniValoreCarta() && mazzoCpu[0].OttieniValoreCarta() < mazzoCpu[2].OttieniValoreCarta())
                        return mazzoCpu[0];
                    if (mazzoCpu[1].OttieniValoreCarta() < mazzoCpu[0].OttieniValoreCarta() && mazzoCpu[1].OttieniValoreCarta() < mazzoCpu[2].OttieniValoreCarta())
                        return mazzoCpu[1];
                    if (mazzoCpu[2].OttieniValoreCarta() < mazzoCpu[0].OttieniValoreCarta() && mazzoCpu[2].OttieniValoreCarta() < mazzoCpu[1].OttieniValoreCarta())
                        return mazzoCpu[2];

                }
                #endregion

                #region Carta del giocatore é una briscola bassa (fino al 7)
                if (cartaG1.OttieniValoreCarta() < 10 && cartaG1.Seme == briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].OttieniValoreCarta() == 0)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che in mano non avevo scartelle, perció posso avere briscole oppure figure e carichi
                    if (mazzoCpu[0].Numero < mazzoCpu[1].Numero && mazzoCpu[0].Numero < mazzoCpu[2].Numero && mazzoCpu[0].Seme != briscola.Seme && mazzoCpu[0].OttieniValoreCarta() < 10)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].Numero < mazzoCpu[0].Numero && mazzoCpu[1].Numero < mazzoCpu[2].Numero && mazzoCpu[1].Seme != briscola.Seme && mazzoCpu[1].OttieniValoreCarta() < 10)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].Numero < mazzoCpu[0].Numero && mazzoCpu[2].Numero < mazzoCpu[1].Numero && mazzoCpu[2].Seme != briscola.Seme && mazzoCpu[2].OttieniValoreCarta() < 10)
                        return mazzoCpu[2];
                    //Se arrivo a questo punto significa che in mano ho carichi oppure briscole
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == briscola.Seme && mazzoCpu[i].Numero < cartaG1.Numero && mazzoCpu[i].OttieniValoreCarta() <= cartaG1.OttieniValoreCarta())
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
                if (cartaG1.OttieniValoreCarta() < 10 && cartaG1.Seme != briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == cartaG1.Seme && mazzoCpu[i].OttieniValoreCarta() > cartaG1.OttieniValoreCarta())
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho potuto superare: cerco una scartina di un altro seme
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].Seme != cartaG1.Seme && mazzoCpu[i].OttieniValoreCarta() == 0)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho scartine
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].Seme != cartaG1.Seme && mazzoCpu[i].OttieniValoreCarta() < 10)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho pochi punti da dargli, provo a prendere con una briscoletta
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == briscola.Seme && mazzoCpu[i].OttieniValoreCarta() == 0)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto devo prendere con una figura di briscola per non regalare carichi
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == briscola.Seme && mazzoCpu[i].OttieniValoreCarta() < 10)
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
                if (cartaAltraCpu.OttieniValoreCarta() != 0 && cartaAltraCpu.Seme == briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].OttieniValoreCarta() == 0)
                            return mazzoCpu[i];
                    // Se arrivo a questo punto significa che in mano ho tutte briscole oppure tutti carichi, perció butto la piú bassa
                    if (mazzoCpu[0].Numero < mazzoCpu[1].Numero && mazzoCpu[0].Numero < mazzoCpu[2].Numero && mazzoCpu[0].OttieniValoreCarta() < 10 && mazzoCpu[0].Seme != briscola.Seme)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].Numero < mazzoCpu[0].Numero && mazzoCpu[1].Numero < mazzoCpu[2].Numero && mazzoCpu[1].OttieniValoreCarta() < 10 && mazzoCpu[1].Seme != briscola.Seme)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].Numero < mazzoCpu[0].Numero && mazzoCpu[2].Numero < mazzoCpu[1].Numero && mazzoCpu[2].OttieniValoreCarta() < 10 && mazzoCpu[2].Seme != briscola.Seme)
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
                if (cartaAltraCpu.OttieniValoreCarta() >= 10 && cartaAltraCpu.Seme != briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == cartaAltraCpu.Seme && mazzoCpu[i].OttieniValoreCarta() > cartaAltraCpu.OttieniValoreCarta())
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho potuto superare il suo carico nel suo seme: cerco una briscola piccola
                    if (mazzoCpu[0].Numero < mazzoCpu[1].Numero && mazzoCpu[0].Numero < mazzoCpu[2].Numero && mazzoCpu[0].Seme == briscola.Seme && mazzoCpu[0].OttieniValoreCarta() == 0)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].Numero < mazzoCpu[0].Numero && mazzoCpu[1].Numero < mazzoCpu[2].Numero && mazzoCpu[1].Seme == briscola.Seme && mazzoCpu[1].OttieniValoreCarta() == 0)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].Numero < mazzoCpu[0].Numero && mazzoCpu[2].Numero < mazzoCpu[1].Numero && mazzoCpu[2].Seme == briscola.Seme && mazzoCpu[2].OttieniValoreCarta() == 0)
                        return mazzoCpu[2];
                    //Se arrivo a questo punto significa che non ho briscole basse, perció cerco una briscola qualsiasi
                    if (mazzoCpu[0].OttieniValoreCarta() < mazzoCpu[1].OttieniValoreCarta() && mazzoCpu[0].OttieniValoreCarta() < mazzoCpu[2].OttieniValoreCarta() && mazzoCpu[0].Seme == briscola.Seme)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].OttieniValoreCarta() < mazzoCpu[0].OttieniValoreCarta() && mazzoCpu[1].OttieniValoreCarta() < mazzoCpu[2].OttieniValoreCarta() && mazzoCpu[1].Seme == briscola.Seme)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].OttieniValoreCarta() < mazzoCpu[0].OttieniValoreCarta() && mazzoCpu[2].OttieniValoreCarta() < mazzoCpu[1].OttieniValoreCarta() && mazzoCpu[2].Seme == briscola.Seme)
                        return mazzoCpu[2];
                    //Se arrivo a questo punto significa che non ho briscole per prendere perció butto una scartina
                    if (mazzoCpu[0].OttieniValoreCarta() < mazzoCpu[1].OttieniValoreCarta() && mazzoCpu[0].OttieniValoreCarta() < mazzoCpu[2].OttieniValoreCarta())
                        return mazzoCpu[0];
                    if (mazzoCpu[1].OttieniValoreCarta() < mazzoCpu[0].OttieniValoreCarta() && mazzoCpu[1].OttieniValoreCarta() < mazzoCpu[2].OttieniValoreCarta())
                        return mazzoCpu[1];
                    if (mazzoCpu[2].OttieniValoreCarta() < mazzoCpu[0].OttieniValoreCarta() && mazzoCpu[2].OttieniValoreCarta() < mazzoCpu[1].OttieniValoreCarta())
                        return mazzoCpu[2];

                }
                #endregion

                #region Carta della CPU2 é una briscola bassa (fino al 7)
                if (cartaAltraCpu.OttieniValoreCarta() < 10 && cartaAltraCpu.Seme == briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].OttieniValoreCarta() == 0)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che in mano non avevo scartelle, perció posso avere briscole oppure figure e carichi
                    if (mazzoCpu[0].Numero < mazzoCpu[1].Numero && mazzoCpu[0].Numero < mazzoCpu[2].Numero && mazzoCpu[0].Seme != briscola.Seme && mazzoCpu[0].OttieniValoreCarta() < 10)
                        return mazzoCpu[0];
                    if (mazzoCpu[1].Numero < mazzoCpu[0].Numero && mazzoCpu[1].Numero < mazzoCpu[2].Numero && mazzoCpu[1].Seme != briscola.Seme && mazzoCpu[1].OttieniValoreCarta() < 10)
                        return mazzoCpu[1];
                    if (mazzoCpu[2].Numero < mazzoCpu[0].Numero && mazzoCpu[2].Numero < mazzoCpu[1].Numero && mazzoCpu[2].Seme != briscola.Seme && mazzoCpu[2].OttieniValoreCarta() < 10)
                        return mazzoCpu[2];
                    //Se arrivo a questo punto significa che in mano ho carichi oppure briscole
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == briscola.Seme && mazzoCpu[i].Numero < cartaG1.Numero && mazzoCpu[i].OttieniValoreCarta() <= cartaG1.OttieniValoreCarta())
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
                if (cartaAltraCpu.OttieniValoreCarta() < 10 && cartaAltraCpu.Seme != briscola.Seme)
                {
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == cartaG1.Seme && mazzoCpu[i].OttieniValoreCarta() > cartaG1.OttieniValoreCarta())
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho potuto superare: cerco una scartina di un altro seme
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].Seme != cartaAltraCpu.Seme && mazzoCpu[i].OttieniValoreCarta() == 0)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho scartine
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme != briscola.Seme && mazzoCpu[i].Seme != cartaAltraCpu.Seme && mazzoCpu[i].OttieniValoreCarta() < 10)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto significa che non ho pochi punti da dargli, provo a prendere con una briscoletta
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == briscola.Seme && mazzoCpu[i].OttieniValoreCarta() == 0)
                            return mazzoCpu[i];
                    //Se arrivo a questo punto devo prendere con una figura di briscola per non regalare carichi
                    for (int i = 0; i < mazzoCpu.Count; i++)
                        if (mazzoCpu[i].Seme == briscola.Seme && mazzoCpu[i].OttieniValoreCarta() < 10)
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
                if (giocatori[0].CartaGiocata.Seme != briscola.Seme && giocatori[0].CartaGiocata.OttieniValoreCarta() < 10)
                {
                    #region SECONDO = SUPERA
                    if (giocatori[1].CartaGiocata.Seme == giocatori[0].CartaGiocata.Seme && giocatori[1].CartaGiocata.Numero > giocatori[0].CartaGiocata.Numero && giocatori[1].CartaGiocata.OttieniValoreCarta() < 10)
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
                    if (giocatori[1].CartaGiocata.OttieniValoreCarta() >= 10 && giocatori[1].CartaGiocata.Seme != briscola.Seme && giocatori[1].CartaGiocata.Seme != giocatori[0].CartaGiocata.Seme)
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
                    if (giocatori[1].CartaGiocata.Seme != giocatori[0].CartaGiocata.Seme && giocatori[1].CartaGiocata.Seme != briscola.Seme && giocatori[1].CartaGiocata.OttieniValoreCarta() < 10)
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
                if (giocatori[0].CartaGiocata.Seme == briscola.Seme && giocatori[0].CartaGiocata.OttieniValoreCarta() < 10)
                {
                    #region SECONDO = SCARTINA
                    if (giocatori[1].CartaGiocata.Seme != briscola.Seme && giocatori[1].CartaGiocata.OttieniValoreCarta() < 10)
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
                    if (giocatori[1].CartaGiocata.Seme != briscola.Seme && giocatori[1].CartaGiocata.Seme != giocatori[0].CartaGiocata.Seme && giocatori[1].CartaGiocata.OttieniValoreCarta() >= 10)
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
                    if (giocatori[1].CartaGiocata.Seme == giocatori[0].CartaGiocata.Seme && (giocatori[1].CartaGiocata.Numero > giocatori[0].CartaGiocata.Numero || giocatori[1].CartaGiocata.OttieniValoreCarta() < 11))
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
                    if (giocatori[1].CartaGiocata.Seme == giocatori[0].CartaGiocata.Seme && (giocatori[0].CartaGiocata.OttieniValoreCarta() <= giocatori[1].CartaGiocata.OttieniValoreCarta() || giocatori[0].CartaGiocata.Numero < giocatori[1].CartaGiocata.Numero))
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
                if (giocatori[0].CartaGiocata.OttieniValoreCarta() >= 10 && giocatori[0].CartaGiocata.Seme != briscola.Seme)
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
                     mazzoUtente[i].OttieniValoreCarta() > carta1.OttieniValoreCarta() ||
                     mazzoUtente[i].OttieniValoreCarta() > carta2.OttieniValoreCarta()))
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
                    mazzoUtente[i].OttieniValoreCarta() == 0 &&
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
                if (mazzoUtente[i].Seme == briscola.Seme && mazzoUtente[i].OttieniValoreCarta() < 10)
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
                if (mazzoUtente[i].OttieniValoreCarta() >= 10 &&
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
                if (mazzoUtente[i].Seme == briscola.Seme && mazzoUtente[i].OttieniValoreCarta() >= 10)
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
                    mazzoUtente[i].OttieniValoreCarta() != 0 &&
                    mazzoUtente[i].OttieniValoreCarta() < 10)
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
                    mazzoUtente[i].OttieniValoreCarta() >= carta.OttieniValoreCarta()))
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
                    if (cartaG1.OttieniValoreCarta() != 0 && cartaG1.Seme == briscola.Seme)
                    {
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme != briscola.Seme && mazzoCpu1[i].OttieniValoreCarta() == 0)
                                return cartaCPUa = mazzoCpu1[i];
                        // Se arrivo a questo punto significa che in mano ho tutte briscole oppure tutti carichi, perció butto la piú bassa
                        if (mazzoCpu1[0].Numero < mazzoCpu1[1].Numero && mazzoCpu1[0].Numero < mazzoCpu1[2].Numero && mazzoCpu1[0].OttieniValoreCarta() < 10 && mazzoCpu1[0].Seme != briscola.Seme)
                            return cartaCPUa = mazzoCpu1[0];
                        if (mazzoCpu1[1].Numero < mazzoCpu1[0].Numero && mazzoCpu1[1].Numero < mazzoCpu1[2].Numero && mazzoCpu1[1].OttieniValoreCarta() < 10 && mazzoCpu1[1].Seme != briscola.Seme)
                            return cartaCPUa = mazzoCpu1[1];
                        if (mazzoCpu1[2].Numero < mazzoCpu1[0].Numero && mazzoCpu1[2].Numero < mazzoCpu1[1].Numero && mazzoCpu1[2].OttieniValoreCarta() < 10 && mazzoCpu1[2].Seme != briscola.Seme)
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
                    if (cartaG1.OttieniValoreCarta() >= 10 && cartaG1.Seme != briscola.Seme)
                    {
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme == cartaG1.Seme && mazzoCpu1[i].OttieniValoreCarta() > cartaG1.OttieniValoreCarta())
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto significa che non ho potuto superare il suo carico nel suo seme: cerco una briscola piccola
                        if (mazzoCpu1[0].Numero < mazzoCpu1[1].Numero && mazzoCpu1[0].Numero < mazzoCpu1[2].Numero && mazzoCpu1[0].Seme == briscola.Seme && mazzoCpu1[0].OttieniValoreCarta() == 0)
                            return cartaCPUa = mazzoCpu1[0];
                        if (mazzoCpu1[1].Numero < mazzoCpu1[0].Numero && mazzoCpu1[1].Numero < mazzoCpu1[2].Numero && mazzoCpu1[1].Seme == briscola.Seme && mazzoCpu1[1].OttieniValoreCarta() == 0)
                            return cartaCPUa = mazzoCpu1[1];
                        if (mazzoCpu1[2].Numero < mazzoCpu1[0].Numero && mazzoCpu1[2].Numero < mazzoCpu1[1].Numero && mazzoCpu1[2].Seme == briscola.Seme && mazzoCpu1[2].OttieniValoreCarta() == 0)
                            return cartaCPUa = mazzoCpu1[2];
                        //Se arrivo a questo punto significa che non ho briscole basse, perció cerco una briscola qualsiasi
                        if (mazzoCpu1[0].OttieniValoreCarta() < mazzoCpu1[1].OttieniValoreCarta() && mazzoCpu1[0].OttieniValoreCarta() < mazzoCpu1[2].OttieniValoreCarta() && mazzoCpu1[0].Seme == briscola.Seme)
                            return cartaCPUa = mazzoCpu1[0];
                        if (mazzoCpu1[1].OttieniValoreCarta() < mazzoCpu1[0].OttieniValoreCarta() && mazzoCpu1[1].OttieniValoreCarta() < mazzoCpu1[2].OttieniValoreCarta() && mazzoCpu1[1].Seme == briscola.Seme)
                            return cartaCPUa = mazzoCpu1[1];
                        if (mazzoCpu1[2].OttieniValoreCarta() < mazzoCpu1[0].OttieniValoreCarta() && mazzoCpu1[2].OttieniValoreCarta() < mazzoCpu1[1].OttieniValoreCarta() && mazzoCpu1[2].Seme == briscola.Seme)
                            return cartaCPUa = mazzoCpu1[2];
                        //Se arrivo a questo punto significa che non ho briscole per prendere perció butto una scartina
                        if (mazzoCpu1[0].OttieniValoreCarta() < mazzoCpu1[1].OttieniValoreCarta() && mazzoCpu1[0].OttieniValoreCarta() < mazzoCpu1[2].OttieniValoreCarta())
                            return cartaCPUa = mazzoCpu1[0];
                        if (mazzoCpu1[1].OttieniValoreCarta() < mazzoCpu1[0].OttieniValoreCarta() && mazzoCpu1[1].OttieniValoreCarta() < mazzoCpu1[2].OttieniValoreCarta())
                            return cartaCPUa = mazzoCpu1[1];
                        if (mazzoCpu1[2].OttieniValoreCarta() < mazzoCpu1[0].OttieniValoreCarta() && mazzoCpu1[2].OttieniValoreCarta() < mazzoCpu1[1].OttieniValoreCarta())
                            return cartaCPUa = mazzoCpu1[2];

                    }
                    #endregion

                    #region Carta del giocatore é una briscola bassa (fino al 7)
                    if (cartaG1.OttieniValoreCarta() < 10 && cartaG1.Seme == briscola.Seme)
                    {
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme != briscola.Seme && mazzoCpu1[i].OttieniValoreCarta() == 0)
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto significa che in mano non avevo scartelle, perció posso avere briscole oppure figure e carichi
                        if (mazzoCpu1[0].Numero < mazzoCpu1[1].Numero && mazzoCpu1[0].Numero < mazzoCpu1[2].Numero && mazzoCpu1[0].Seme != briscola.Seme && mazzoCpu1[0].OttieniValoreCarta() < 10)
                            return cartaCPUa = mazzoCpu1[0];
                        if (mazzoCpu1[1].Numero < mazzoCpu1[0].Numero && mazzoCpu1[1].Numero < mazzoCpu1[2].Numero && mazzoCpu1[1].Seme != briscola.Seme && mazzoCpu1[1].OttieniValoreCarta() < 10)
                            return cartaCPUa = mazzoCpu1[1];
                        if (mazzoCpu1[2].Numero < mazzoCpu1[0].Numero && mazzoCpu1[2].Numero < mazzoCpu1[1].Numero && mazzoCpu1[2].Seme != briscola.Seme && mazzoCpu1[2].OttieniValoreCarta() < 10)
                            return cartaCPUa = mazzoCpu1[2];
                        //Se arrivo a questo punto significa che in mano ho carichi oppure briscole
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme == briscola.Seme && mazzoCpu1[i].Numero < cartaG1.Numero && mazzoCpu1[i].OttieniValoreCarta() <= cartaG1.OttieniValoreCarta())
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
                    if (cartaG1.OttieniValoreCarta() < 10 && cartaG1.Seme != briscola.Seme)
                    {
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme == cartaG1.Seme && mazzoCpu1[i].OttieniValoreCarta() > cartaG1.OttieniValoreCarta())
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto significa che non ho potuto superare: cerco una scartina di un altro seme
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme != briscola.Seme && mazzoCpu1[i].Seme != cartaG1.Seme && mazzoCpu1[i].OttieniValoreCarta() == 0)
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto significa che non ho scartine
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme != briscola.Seme && mazzoCpu1[i].Seme != cartaG1.Seme && mazzoCpu1[i].OttieniValoreCarta() < 10)
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto significa che non ho pochi punti da dargli, provo a prendere con una briscoletta
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme == briscola.Seme && mazzoCpu1[i].OttieniValoreCarta() == 0)
                                return cartaCPUa = mazzoCpu1[i];
                        //Se arrivo a questo punto devo prendere con una figura di briscola per non regalare carichi
                        for (int i = 0; i < mazzoCpu1.Count; i++)
                            if (mazzoCpu1[i].Seme == briscola.Seme && mazzoCpu1[i].OttieniValoreCarta() < 10)
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

    #endregion
}