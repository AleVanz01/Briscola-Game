using Briscola.Models;
using Briscola.Models.Enumeratori;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Button = System.Windows.Controls.Button;

namespace Briscola
{
    /// <summary>
    /// Logica di interazione per Gioco.xaml
    /// </summary>
    public partial class Gioco : Window
    {
        //ANIMARE LA PESCA
        //SISTEMARE DAL SECONDO TURNO IN POI
        //PULIRE LA TAVOLA BENE

        //PULIZIA DEL CODICE (COMMENTI E REGIONI OVE NECESSARIO)
        //ESISTE UN CASO IN CUI LA CPU RIMANE CON CARTA NULLA (GUARDARE IA)

        public Gioco(Giocatore giocatore, string tipoCarte, BitmapImage sfondo)
        {
            InitializeComponent();
            this.giocatore = giocatore;

        }
        public Gioco()
        {
            InitializeComponent();
            mazzoTavola = new List<Carta>();
            gbCpu.IsEnabled = false;
            gbGiocatore1.IsEnabled = false;

            //CONTATORE DI CARTE PER LA VISUALIZZAZIONE DELLE CARTE RIMASTE
            cartePescate = 0;

            //GIOCATORE 1 TEMPORANEO
            giocatore = new Giocatore("a", "a", "a", "a", "1");
            partita = new Partita(2, giocatore, TipoCarta.Trevisana);
            txtG1.Text = giocatore.Username;

            //PREPARAZIONE DEL PRIMO TURNO
            giocatore.MazzoGiocatore.Add(partita.Pesca());
            giocatore.MazzoGiocatore.Add(partita.Pesca());
            giocatore.MazzoGiocatore.Add(partita.Pesca());
            partita.Giocatori[1].MazzoGiocatore.Add(partita.Pesca());
            partita.Giocatori[1].MazzoGiocatore.Add(partita.Pesca());
            partita.Giocatori[1].MazzoGiocatore.Add(partita.Pesca());

            cartePescate += 6;
            btnCarteRimaste.Text = (40 - cartePescate).ToString();

            CreaCarte();
            LoadResources(TipoCarta.Trevisana);

            //VARIABILE PER DIVERSIFICARE I CASI NEI VARI TURNI
            numeroTurno = 0;

            //GIOCATORE 1 SARÁ SEMPRE PRIMO
            Random r = new Random();
            turnoPrimoG1 = partita.OrdineTurno(r.Next(0, 2));

            //DECISIONE SU CHI TIRA
            if (!turnoPrimoG1)
            {
                TurnoCpu();
            }
            else
            {
                turnoG1 = true;
                gbGiocatore1.IsEnabled = true;
            }
        }

        private bool IsC1Selected = false;
        private bool IsC2Selected = false;
        private bool IsC3Selected = false;
        private bool lanciata = false;
        private bool turnoG1 = false;
        private bool turnoPrimoG1 = false;
        private int tempoCpu;
        private int cartePescate;
        private int numeroTurno;
        private int posizionePrecedenteCarta; //Salva la posizione/angolo con cui è stata buttata l'altra carta in tavola
        private int cartaLanciataG1;
        private int cartaLanciataCpu;
        private readonly Giocatore giocatore;
        private readonly Partita partita;
        private readonly List<Carta> mazzoTavola;
        private DispatcherTimer timer;
        private Point posCartaGiocataG1;
        private Point posCartaGiocataCpu;
        private List<Button> carte;
        private Button cartaGiocataG1;
        private Button cartaGiocataCpu;

        #region CREAZIONE CARTE
        /// <summary>
        /// Crea dinamicamente le carte dei 2 giocatori
        /// </summary>
        public void CreaCarte()
        {
            carte = new List<Button>();

            for (int i = 0; i < 6; i++)
            {
                if (i < 3)
                {
                    switch (i)
                    {
                        case 0:
                            carte.Add(new Button() { Width = 170, Height = 360, BorderThickness = new Thickness(0), Name = "Carta1" });
                            Canvas.SetLeft(carte[i], 828);
                            break;
                        case 1:
                            carte.Add(new Button() { Width = 170, Height = 360, BorderThickness = new Thickness(0), Name = "Carta2" });
                            Canvas.SetLeft(carte[i], 1086);
                            break;
                        case 2:
                            carte.Add(new Button() { Width = 170, Height = 360, BorderThickness = new Thickness(0), Name = "Carta3" });
                            Canvas.SetLeft(carte[i], 1353);
                            break;
                    }
                    Canvas.SetTop(carte[i], 666);
                    carte[i].Click += btnCarta1_Click;
                }
                else
                {
                    switch (i)
                    {
                        case 3:
                            carte.Add(new Button() { Width = 170, Height = 180, BorderThickness = new Thickness(0), Name = "CartaCpu1" });
                            Canvas.SetLeft(carte[i], 828);
                            break;
                        case 4:
                            carte.Add(new Button() { Width = 170, Height = 180, BorderThickness = new Thickness(0), Name = "CartaCpu2" });
                            Canvas.SetLeft(carte[i], 1086);
                            break;
                        case 5:
                            carte.Add(new Button() { Width = 170, Height = 180, BorderThickness = new Thickness(0), Name = "CartaCpu3" });
                            Canvas.SetLeft(carte[i], 1353);
                            break;
                    }
                    Canvas.SetTop(carte[i], 0);
                }
                gridMain.Children.Add(carte[i]);
            }
        }
        #endregion
        /// <summary>
        /// Carica le risorse per la partita
        /// </summary>
        /// <param name="tipoCarte"></param>
        private void LoadResources(TipoCarta tipoCarte)
        {
            string path;
            gridMain.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Immagini\\Sfondi\\Legno.png")));
            if (tipoCarte == TipoCarta.Trevisana)
            {
                path = Environment.CurrentDirectory + "\\Immagini\\CarteTrevisane";
            }
            else
            {
                path = Environment.CurrentDirectory + "\\Immagini\\CarteNapoletane";
            }

            cartaMazzo.Background = new ImageBrush(new BitmapImage(new Uri(path + "\\Retro.png")));
            carte[3].Background = new ImageBrush(new BitmapImage(new Uri(path + "\\RetroMeta.png")));
            carte[4].Background = new ImageBrush(new BitmapImage(new Uri(path + "\\RetroMeta.png")));
            carte[5].Background = new ImageBrush(new BitmapImage(new Uri(path + "\\RetroMeta.png")));
            CartePunti.Background = new ImageBrush(new BitmapImage(new Uri(path + "\\RetroPiccolo.png")));
            CartePuntiEnemy.Background = new ImageBrush(new BitmapImage(new Uri(path + "\\RetroPiccolo.png")));
            CartaBriscola.Background = new ImageBrush(new BitmapImage(new Uri(partita.Briscola.Img)));
            carte[0].Background = new ImageBrush(new BitmapImage(new Uri(giocatore.MazzoGiocatore[0].Img)));
            carte[1].Background = new ImageBrush(new BitmapImage(new Uri(giocatore.MazzoGiocatore[1].Img)));
            carte[2].Background = new ImageBrush(new BitmapImage(new Uri(giocatore.MazzoGiocatore[2].Img)));
            /*carte[3].Background = new ImageBrush(new BitmapImage(new Uri(partita.Giocatori[1].MazzoGiocatore[0].Img)));
            carte[4].Background = new ImageBrush(new BitmapImage(new Uri(partita.Giocatori[1].MazzoGiocatore[1].Img)));
            carte[5].Background = new ImageBrush(new BitmapImage(new Uri(partita.Giocatori[1].MazzoGiocatore[2].Img)));*/

            btnGioca.Visibility = Visibility.Collapsed;
            carte[3].IsHitTestVisible = false;
            carte[4].IsHitTestVisible = false;
            carte[5].IsHitTestVisible = false;
            cnvTavola.Opacity = 0;
            CartePuntiEnemy.IsHitTestVisible = false;
            CartaBriscola.IsHitTestVisible = false;
        }

        /// <summary>
        /// Anima lo spostamento delle carte di entrambi i giocatori
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            tempoCpu++;
            if (tempoCpu == 2)
            {
                turnoG1 = true;
                timer.Stop();
                timer.IsEnabled = false;
                tempoCpu = 0;
                Carta cartaCpu;
                int index;
                if (turnoPrimoG1)
                {
                    cartaCpu = GiocaCPU(1, out index);
                    mazzoTavola.Add(cartaCpu);//creare metodo animazione carte cpu
                }
                else
                {
                    cartaCpu = GiocaCPU(0, out index);
                    mazzoTavola.Add(cartaCpu);//creare metodo animazione carte cpu
                }


                //PRELEVO LE INFORMAZIONI DELLA CARTA GIOCATA PER LA RELATIVA ANIMAZIONE

                switch (index)
                {
                    case 0:
                        carte[3].Background = new ImageBrush(new BitmapImage(new Uri(cartaCpu.Img)));
                        carte[3].Height = 360;
                        posCartaGiocataCpu.X = Canvas.GetLeft(carte[3]);
                        posCartaGiocataCpu.Y = Canvas.GetTop(carte[3]);
                        cartaGiocataCpu = carte[3];
                        cartaLanciataCpu = 1;
                        LanciaCarta(carte[3]);
                        break;
                    case 1:
                        carte[4].Background = new ImageBrush(new BitmapImage(new Uri(cartaCpu.Img)));
                        carte[4].Height = 360;
                        posCartaGiocataCpu.X = Canvas.GetLeft(carte[4]);
                        posCartaGiocataCpu.Y = Canvas.GetTop(carte[4]);
                        cartaGiocataCpu = carte[4];
                        cartaLanciataCpu = 2;
                        LanciaCarta(carte[4]);
                        break;
                    case 2:
                        carte[5].Background = new ImageBrush(new BitmapImage(new Uri(cartaCpu.Img)));
                        carte[5].Height = 360;
                        posCartaGiocataCpu.X = Canvas.GetLeft(carte[5]);
                        posCartaGiocataCpu.Y = Canvas.GetTop(carte[5]);
                        cartaGiocataCpu = carte[5];
                        cartaLanciataCpu = 3;
                        LanciaCarta(carte[5]);
                        break;
                }
                gbCpu.IsEnabled = false;
                gbGiocatore1.IsEnabled = true;
                if (turnoPrimoG1)
                {
                    txtGioca.Text = "PESCA";
                }

                btnGioca.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Metodo per il turno di CPU
        /// </summary>
        private void TurnoCpu()
        {
            gbCpu.IsEnabled = true;
            timer = new DispatcherTimer
            {
                IsEnabled = true,
                Interval = TimeSpan.FromSeconds(2)
            };
            timer.Tick += Timer_Tick;
            tempoCpu = 0;
            timer.Start();
        }

        /// <summary>
        /// Lancia una carta tramite un'animazione
        /// </summary>
        /// <param name="bottone"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="mezza"></param>
        private void LanciaCarta(Button bottone)
        {
            int num;
            do
            {
                Thread.Sleep(100);
                Random rnd = new Random();
                num = rnd.Next(0, 4);

            } while (num == posizionePrecedenteCarta);
            posizionePrecedenteCarta = num;
            int cnvLeft = 0;
            int cnvTop = 0;
            double angolo = 0;
            switch (num)
            {
                case 0:
                    angolo = -60;
                    cnvLeft = 950;
                    cnvTop = 460;
                    break;
                case 1:
                    angolo = -30;
                    cnvLeft = 950;
                    cnvTop = 300;
                    break;
                case 2:
                    angolo = 90;
                    cnvLeft = 1300;
                    cnvTop = 370;
                    break;
                case 3:
                    angolo = 45;
                    cnvLeft = 1250;
                    cnvTop = 250;
                    break;
                default:
                    break;
            }

            bottone.RenderTransform = new RotateTransform(angolo);
            bottone.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation { To = cnvLeft, Duration = TimeSpan.FromMilliseconds(600) });
            bottone.BeginAnimation(Canvas.TopProperty, new DoubleAnimation { To = cnvTop, Duration = TimeSpan.FromMilliseconds(600) });

            if (cartaGiocataCpu != null)
            {
                Canvas.SetZIndex(cartaGiocataCpu, 1);
                Canvas.SetZIndex(bottone, 5);
            }
        }

        /// <summary>
        /// Pesca le ultime 2 carte dalla tavola, compresa la briscola
        /// </summary>
        private void PescaUltimeCarte(Button b, Carta cartaPescata, double ToX, double ToY, bool cpu = false)
        {

        }
        /// <summary>
        /// Pesca una carta dal mazzo tramite un'animazione e la posiziona al posto di quella giocata nel turno precedente
        /// </summary>
        /// <param name="b">Carta da pescare</param>
        /// <param name="cartaPescata">Carta pescata dal giocatore (sfondo del bottone)</param>
        /// <param name="ToX">Posizione orizzontale in cui posizionare la carta</param>
        /// <param name="ToY">Posizione verticale in cui posizionare la carta</param>
        /// <param name="cpu">Specifica se la carta viene pescata dalla cpu</param>
        private void PescaCarta(Button b, Carta cartaPescata, double ToX, double ToY, bool cpu = false)
        {
            Canvas.SetLeft(b, Canvas.GetLeft(cartaMazzo));
            Canvas.SetTop(b, Canvas.GetTop(cartaMazzo));
            b.Width = cartaMazzo.Width;
            b.Height = cartaMazzo.Height;
            b.Background = cartaMazzo.Background;
            b.BorderThickness = cartaMazzo.BorderThickness;
            DoubleAnimation animation = new DoubleAnimation() { To = ToX, Duration = TimeSpan.FromMilliseconds(1) };
            DoubleAnimation animation2 = new DoubleAnimation() { To = ToY, Duration = TimeSpan.FromMilliseconds(1) };

            b.RenderTransform = new RotateTransform(0);
            b.BeginAnimation(Canvas.LeftProperty, animation);
            b.BeginAnimation(Canvas.TopProperty, animation2);
            b.BeginAnimation(OpacityProperty, new DoubleAnimation() { To = 100, Duration = TimeSpan.FromMilliseconds(600) });
            b.IsHitTestVisible = true;

            if (cpu)
            {
                if (partita.Mazzo.Count == 2)
                {
                    btnCartaMazzo.Visibility = Visibility.Collapsed;
                    cartaMazzo.Visibility = Visibility.Collapsed;
                }
                else if (partita.Mazzo.Count == 1)
                {
                    CartaBriscola.Visibility = Visibility.Collapsed;
                }

                b.Height = 180;
                Canvas.SetTop(b, 0);
                b.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\CarteTrevisane\\RetroMeta.png")));
            }
            else
            {
                if (partita.Mazzo.Count == 2)
                {
                    btnCartaMazzo.Visibility = Visibility.Collapsed;
                    b.Background = new ImageBrush(new BitmapImage(new Uri(cartaPescata.Img)));
                    cartaMazzo.Visibility = Visibility.Collapsed;
                }
                else if (partita.Mazzo.Count == 1)
                {
                    b.Background = CartaBriscola.Background;
                    CartaBriscola.Visibility = Visibility.Collapsed;
                }
                else
                {
                    b.Background = new ImageBrush(new BitmapImage(new Uri(cartaPescata.Img)));
                }
            }
        }

        /// <summary>
        /// Assegna le carte in tavola al mazzo di punti del giocatore che ha vinto la mano
        /// </summary>
        /// <param name="winner">Giocatore che ha vinto la mano</param>
        private void AssegnaCartePunti(Giocatore winner)
        {
            DoubleAnimation animation = new DoubleAnimation(0, TimeSpan.FromMilliseconds(600));
            DoubleAnimation animation2 = new DoubleAnimation(0, TimeSpan.FromMilliseconds(600));

            if (winner.Username == giocatore.Username)
            {
                animation.To = 1710;
                animation2.To = 830;
            }
            else
            {
                animation.To = 1710;
                animation2.To = 10;
            }
            cartaGiocataG1.IsHitTestVisible = false;
            cartaGiocataCpu.IsHitTestVisible = false;
            cartaGiocataG1.BeginAnimation(Canvas.LeftProperty, animation);
            cartaGiocataG1.BeginAnimation(Canvas.TopProperty, animation2);
            cartaGiocataCpu.BeginAnimation(Canvas.LeftProperty, animation);
            cartaGiocataCpu.BeginAnimation(Canvas.TopProperty, animation2);
            cartaGiocataG1.BeginAnimation(OpacityProperty, new DoubleAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(600) });
            cartaGiocataCpu.BeginAnimation(OpacityProperty, new DoubleAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(600) });
            txtGioca.Text = "PESCA";
            btnGioca.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Pulisce il tavolo, togliendo le carte dal mazzo e pescando le carte ai giocatori
        /// </summary>
        private void PulisciTavola()
        {
            //se ultimo turno devo pescare le 2 carte in tavola
            if (partita.Mazzo.Count > 0)
            {
                foreach (Giocatore g in partita.Giocatori)
                {
                    Carta c = partita.Pesca();

                    if (g.Username == giocatore.Username)
                    {
                        g.MazzoGiocatore.Insert(cartaLanciataG1 - 1, c);
                        if (IsC1Selected)
                        {
                            PescaCarta(carte[0], c, posCartaGiocataG1.X, posCartaGiocataG1.Y);
                        }

                        else if (IsC2Selected)
                        {
                            PescaCarta(carte[1], c, posCartaGiocataG1.X, posCartaGiocataG1.Y);
                        }

                        else
                        {
                            PescaCarta(carte[2], c, posCartaGiocataG1.X, posCartaGiocataG1.Y);
                        }
                    }
                    else
                    {
                        g.MazzoGiocatore.Insert(cartaLanciataCpu - 1, c);

                        switch (cartaLanciataCpu)
                        {
                            case 1:
                                PescaCarta(carte[3], c, posCartaGiocataCpu.X, posCartaGiocataCpu.Y, true);
                                break;
                            case 2:
                                PescaCarta(carte[4], c, posCartaGiocataCpu.X, posCartaGiocataCpu.Y, true);
                                break;
                            case 3:
                                PescaCarta(carte[5], c, posCartaGiocataCpu.X, posCartaGiocataCpu.Y, true);
                                break;
                        }
                    }
                    cartePescate++;
                }
            }
        }

        /// <summary>
        /// Clic della carta del GIOCATORE 1 => LAVORO VISUALE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCarta1_Click(object sender, RoutedEventArgs e)
        {
            if (cartaGiocataG1 != null)
            {
                cartaGiocataG1 = null;
            }

            if (turnoG1 && !lanciata)
            {
                Button bottone = sender as Button;

                switch (bottone.Name)
                {
                    #region CARTA 1
                    case "Carta1":
                        if (!IsC1Selected)
                        {
                            carte[1].Visibility = Visibility.Hidden;
                            carte[2].Visibility = Visibility.Hidden;
                            IsC1Selected = true;
                            Canvas.SetTop((UIElement)sender, Canvas.GetTop((UIElement)sender));
                            btnGioca.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            Canvas.SetTop((UIElement)sender, Canvas.GetTop((UIElement)sender));
                            IsC1Selected = false;

                            carte[1].Visibility = Visibility.Visible;
                            carte[2].Visibility = Visibility.Visible;
                            btnGioca.Visibility = Visibility.Collapsed;
                        }
                        break;
                    #endregion
                    #region CARTA 2
                    case "Carta2":
                        if (!IsC2Selected)
                        {
                            carte[0].Visibility = Visibility.Hidden;
                            carte[2].Visibility = Visibility.Hidden;
                            IsC2Selected = true;
                            Canvas.SetTop(carte[1], Canvas.GetTop(carte[1]));
                            btnGioca.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            Canvas.SetTop(carte[1], Canvas.GetTop(carte[1]));
                            IsC2Selected = false;

                            carte[0].Visibility = Visibility.Visible;
                            carte[2].Visibility = Visibility.Visible;
                            btnGioca.Visibility = Visibility.Collapsed;
                        }
                        break;
                    #endregion
                    #region CARTA 3
                    case "Carta3":
                        if (!IsC3Selected)
                        {
                            carte[0].Visibility = Visibility.Hidden;
                            carte[1].Visibility = Visibility.Hidden;
                            IsC3Selected = true;
                            Canvas.SetTop(carte[2], Canvas.GetTop(carte[2]));
                            btnGioca.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            Canvas.SetTop(carte[2], Canvas.GetTop(carte[2]));
                            IsC3Selected = false;
                            carte[0].Visibility = Visibility.Visible;
                            carte[1].Visibility = Visibility.Visible;
                            btnGioca.Visibility = Visibility.Collapsed;
                        }
                        break;
                        #endregion
                }
            }
        }

        /// <summary>
        /// Deseleziona tutto dopo ogni turno
        /// </summary>
        private void AzzeraTurno()
        {
            IsC1Selected = false;
            IsC2Selected = false;
            IsC3Selected = false;
            carte[0].IsEnabled = true;
            carte[1].IsEnabled = true;
            carte[2].IsEnabled = true;
            cartaGiocataG1 = null;
            cartaGiocataCpu = null;
            lanciata = false;
        }

        /// <summary>
        /// Azioni che succedono il clic del pulsante GIOCA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGioca_Click(object sender, RoutedEventArgs e)
        {
            if (txtGioca.Text == "GIOCA")
            {
                numeroTurno++;
                if (IsC1Selected)
                {
                    cartaGiocataG1 = carte[0];
                    cartaLanciataG1 = 1;
                }
                else if (IsC2Selected)
                {
                    cartaGiocataG1 = carte[1];
                    cartaLanciataG1 = 2;
                }
                else
                {
                    cartaGiocataG1 = carte[2];
                    cartaLanciataG1 = 3;
                }
                posCartaGiocataG1.X = Canvas.GetLeft(cartaGiocataG1);
                posCartaGiocataG1.Y = Canvas.GetTop(cartaGiocataG1);

                LanciaCarta(cartaGiocataG1);

                carte[0].Visibility = Visibility.Visible;
                carte[1].Visibility = Visibility.Visible;
                carte[2].Visibility = Visibility.Visible;

                lanciata = true;
                giocatore.CartaGiocata = giocatore.MazzoGiocatore[cartaLanciataG1 - 1];
                GiocaCarta(giocatore, giocatore.CartaGiocata);

                if (turnoPrimoG1)
                {
                    gbGiocatore1.IsEnabled = false;
                    TurnoCpu();
                }

                btnGioca.Visibility = Visibility.Hidden;
                gbGiocatore1.IsEnabled = false;
                gbCpu.IsEnabled = false;
            }
            else if (txtGioca.Text == "PESCA")
            {
                GiocaTurno();
                txtGioca.Text = "GIOCA";
                btnGioca.Visibility = Visibility.Hidden;
                btnCarteRimaste.Text = (40 - cartePescate).ToString();
            }
        }

        /// <summary>
        /// Gioco del turno completo
        /// </summary>
        public void GiocaTurno()
        {
            if (partita.Mazzo.Count >= 0 && !partita.IsUltimoTurno)
            {
                mazzoTavola.Clear();

                #region ORDINE PRIMO TURNO
                if (numeroTurno == 0)
                {
                    Thread.Sleep(100);
                    Random rnd = new Random();
                    partita.OrdineTurno(rnd.Next(0, 2));
                }
                #endregion

                PulisciTavola();
                AzzeraTurno();

                if (turnoPrimoG1)
                {
                    turnoG1 = true;
                }

                else
                {
                    turnoG1 = false;
                    TurnoCpu();
                }
                numeroTurno++;/*
                btnCarteRimaste.Text = (40 - cartePescate).ToString();
                btnPuntiG1.Text = giocatore.Punti.ToString();
                btnPuntiCpu.Text = partita.Giocatori.Find(x => x.Username == Partita.CPU1).Punti.ToString();*/
            }
            else //SE ULTIMO TURNO (mazzi carte vuoto e mazzi giocatori vuoti)
            {
                partita.ConfrontaVincitore();
            }
        }

        /// <summary>
        /// In base al turno, definisce quale CPU debba giocare (piú giocatori)
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Carta GiocaCPU(int i, out int index)
        {
            Carta cartaG1 = null;

            if (i != 0)
            {
                cartaG1 = partita.Giocatori[partita.Giocatori.FindIndex(x => x.Username == giocatore.Username)].CartaGiocata;
            }

            if (i == 0)
            {
                partita.Giocatori[i].CartaGiocata = TwoPlayers.GetCartaCpu(partita.Giocatori[i].MazzoGiocatore,
                                                                           partita.Briscola,
                                                                           out index);
            }
            else
            {
                partita.Giocatori[i].CartaGiocata = TwoPlayers.GetCartaCpu(partita.Giocatori[i].MazzoGiocatore,
                                                                           partita.Briscola, out index,
                                                                           partita.Giocatori[0].CartaGiocata);
            }

            GiocaCarta(partita.Giocatori[i], partita.Giocatori[i].CartaGiocata);
            return partita.Giocatori[i].CartaGiocata;
        }

        /// <summary>
        /// Successivamente al clic del tavolo si procede ad un nuovo turno
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cnvTavola_Click(object sender, RoutedEventArgs e)
        {
            //if (cartaGiocataG1 != null && cartaGiocataCpu != null)
            //{
            turnoPrimoG1 = partita.OrdineTurno(partita.Giocatori.IndexOf(partita.Confronto()));
            AssegnaCartePunti(partita.Giocatori[0]);
            btnPuntiG1.Text = giocatore.Punti.ToString();
            btnPuntiCpu.Text = partita.Giocatori.Find(x => x.Username == Partita.CPU1).Punti.ToString();
            //}
        }

        /// <summary>
        /// Rimozione della carta giocata dal mazzo giocatore
        /// </summary>
        /// <param name="g"></param>
        /// <param name="c"></param>
        public void GiocaCarta(Giocatore g, Carta c = null)
        {
            g.MazzoGiocatore.Remove(c);
            mazzoTavola.Add(c);
        }

        /// <summary>
        /// Username > 10 caratteri => Diminuzione del font (per text block giocatore)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if (txtG1.Text.Length > 10)
            {
                txtG1.FontSize = 33;
            }
        }

        /// <summary>
        /// Visualizzazione delle carte vinte dal giocatore
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CartePunti_Click(object sender, RoutedEventArgs e)
        {
            MazzoPunti form = new MazzoPunti(giocatore);
            form.ShowDialog();
        }
    }
}