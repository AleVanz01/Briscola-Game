using Briscola.Models;
using Briscola.Models.Enumeratori;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Briscola.ViewModels
{
    public class GiocoDueGiocatoriViewModel : ViewModelBase
    {
        private bool _lanciata = false;
        private bool _isG1PrimoTurno = false;
        private int _tempoCpu;
        private int _cartePescate;
        private int _numeroTurno;
        private int _posizionePrecedenteCarta; //Salva la posizione/angolo con cui è stata buttata l'altra carta in tavola
        private int _cartaLanciataG1;
        private int _cartaLanciataCpu;
        private readonly Partita _partita;
        private readonly List<Carta> _mazzoTavola;
        private Point _posCartaGiocataG1;
        private Point _posCartaGiocataCpu;
        private List<Button> _carte;
        private Button _cartaGiocataG1;
        private Button _cartaGiocataCpu;
        private ImageBrush _sfondo;
        private ImageBrush _retroCartaMazzo;
        private ImageBrush _cartaBriscola;
        private ImageBrush _retroCartePiccolo;
        private Giocatore _giocatore;
        private int _carteRimaste;
        private bool _isTurnoG1;
        private bool _isTurnoCpu;
        private bool _isUltimoTurno;
        private bool _isCarta1Selezionata;
        private bool _isCarta2Selezionata;
        private bool _isCarta3Selezionata;
        private bool _canPlayerPlay;
        private double _opacitaTavola;
        private string _pescaGioca;
        private bool _isCartaMazzoVisibile;
        private int _puntiCpu;
        private Button _cartaMazzo;
        private Grid _stkPanel;

        public GiocoDueGiocatoriViewModel(Giocatore giocatore, TipoCarta tipoCarta, BitmapImage sfondo)
        {
            Sfondo = new ImageBrush(sfondo);
            _mazzoTavola = new List<Carta>();

            //CONTATORE DI CARTE PER LA VISUALIZZAZIONE DELLE CARTE RIMASTE
            _cartePescate = 0;

            //giocatore = new Giocatore("a", "a", "a", "a", "1");
            Giocatore = new Giocatore
            {
                Username = giocatore?.Username
            };

            _partita = new Partita(2, Giocatore, TipoCarta.Trevisana);

            //PREPARAZIONE DEL PRIMO TURNO
            Giocatore.MazzoGiocatore.Add(_partita.Pesca());
            Giocatore.MazzoGiocatore.Add(_partita.Pesca());
            Giocatore.MazzoGiocatore.Add(_partita.Pesca());
            _partita.Giocatori[1].MazzoGiocatore.Add(_partita.Pesca());
            _partita.Giocatori[1].MazzoGiocatore.Add(_partita.Pesca());
            _partita.Giocatori[1].MazzoGiocatore.Add(_partita.Pesca());

            _cartePescate += 6;
            CarteRimaste = 40 - _cartePescate;

            //VARIABILE PER DIVERSIFICARE I CASI NEI VARI TURNI
            _numeroTurno = 0;

            Random r = new Random();
            _isG1PrimoTurno = _partita.OrdineTurno(r.Next(0, 2));

            //DECISIONE SU CHI TIRA
            if (!_isG1PrimoTurno)
            {
                TurnoCpu();
            }
            else
            {
                IsTurnoG1 = true;
            }
        }

        #region Proprietà Binding

        public ImageBrush Sfondo
        {
            get => _sfondo;
            set => SetProperty(ref _sfondo, value);
        }

        public ImageBrush RetroCartaMazzo
        {
            get => _retroCartaMazzo;
            set => SetProperty(ref _retroCartaMazzo, value);
        }

        public ImageBrush CartaBriscola
        {
            get => _cartaBriscola;
            set => SetProperty(ref _cartaBriscola, value);
        }

        public ImageBrush RetroCartePiccolo
        {
            get => _retroCartePiccolo;
            set => SetProperty(ref _retroCartePiccolo, value);
        }

        public Giocatore Giocatore
        {
            get => _giocatore;
            set => SetProperty(ref _giocatore, value);
        }

        public int CarteRimaste
        {
            get => _carteRimaste;
            set => SetProperty(ref _carteRimaste, value);
        }

        public bool IsTurnoG1
        {
            get => _isTurnoG1;
            set => SetProperty(ref _isTurnoG1, value);
        }

        public bool IsTurnoCpu
        {
            get => _isTurnoCpu;
            set => SetProperty(ref _isTurnoCpu, value);
        }

        public bool IsUltimoTurno
        {
            get => _isUltimoTurno;
            set => SetProperty(ref _isUltimoTurno, value);
        }

        public bool IsCartaMazzoVisibile
        {
            get => _isCartaMazzoVisibile;
            set => SetProperty(ref _isCartaMazzoVisibile, value);
        }

        public bool IsCarta1Selezionata
        {
            get => _isCarta1Selezionata;
            set => SetProperty(ref _isCarta1Selezionata, value);
        }

        public bool IsCarta2Selezionata
        {
            get => _isCarta2Selezionata;
            set => SetProperty(ref _isCarta2Selezionata, value);
        }

        public bool IsCarta3Selezionata
        {
            get => _isCarta3Selezionata;
            set => SetProperty(ref _isCarta3Selezionata, value);
        }

        public List<Button> Carte
        {
            get => _carte;
            set => SetProperty(ref _carte, value);
        }

        public Button CartaMazzo
        {
            get => _cartaMazzo;
            set => SetProperty(ref _cartaMazzo, value);
        }

        public bool CanPlayerPlay
        {
            get => _canPlayerPlay;
            set => SetProperty(ref _canPlayerPlay, value);
        }

        public double OpacitaTavola
        {
            get => _opacitaTavola;
            set => SetProperty(ref _opacitaTavola, value);
        }

        public string PescaGioca
        {
            get => _pescaGioca;
            set => SetProperty(ref _pescaGioca, value);
        }

        public int PuntiCpu
        {
            get => _puntiCpu;
            set => SetProperty(ref _puntiCpu, value);
        }

        public Grid GridMain
        {
            get => _stkPanel;
            set => SetProperty(ref _stkPanel, value);
        }
        #endregion

        public ICommand PescaGiocaCommand => new RelayCommand(EseguiPescaGioca);

        public new ICommand WindowLoadedCommand => new RelayCommand(Carica);

        public ICommand SelezionaCartaCommand => new RelayCommand(SelezionaCarta);

        public ICommand VisCartePuntiCommand => new RelayCommand(VisCartePunti);

        public ICommand PulisciTavolaCommand => new RelayCommand(PulisciTavola);

        public event EventHandler OnCarteCreate;
        public event EventHandler OnCartaPescata;

        #region CODICE ONTO

        public void Carica(object p)
        {
            CreaCarte();
            LoadResources(TipoCarta.Trevisana);
        }

        /// <summary>
        /// Deseleziona tutto dopo ogni turno
        /// </summary>
        private void AzzeraTurno()
        {
            IsCarta1Selezionata = false;
            IsCarta2Selezionata = false;
            IsCarta3Selezionata = false;
            Carte[0].IsEnabled = true;
            Carte[1].IsEnabled = true;
            Carte[2].IsEnabled = true;
            _cartaGiocataG1 = null;
            _cartaGiocataCpu = null;
            _lanciata = false;
        }

        /// <summary>
        /// Crea dinamicamente le carte dei 2 giocatori
        /// </summary>
        public void CreaCarte()
        {
            Carte = new List<Button>();

            for (int i = 0; i < 6; i++)
            {
                if (i < 3)
                {
                    switch (i)
                    {
                        case 0:
                            Carte.Add(new Button() { Width = 170, Height = 360, BorderThickness = new Thickness(0), Name = "Carta1" });
                            Canvas.SetLeft(_carte[i], 828);
                            break;
                        case 1:
                            Carte.Add(new Button() { Width = 170, Height = 360, BorderThickness = new Thickness(0), Name = "Carta2" });
                            Canvas.SetLeft(_carte[i], 1086);
                            break;
                        case 2:
                            Carte.Add(new Button() { Width = 170, Height = 360, BorderThickness = new Thickness(0), Name = "Carta3" });
                            Canvas.SetLeft(_carte[i], 1353);
                            break;
                    }
                    Canvas.SetTop(_carte[i], 666);

                    Binding commandBinding = new Binding
                    {
                        Path = new PropertyPath("SelezionaCartaCommand"),
                        RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(GiocoDueGiocatoriViewModel), 1)
                    };
                    Carte[i].SetBinding(Button.CommandProperty, commandBinding);
                    Carte[i].CommandParameter = Carte[i];
                }
                else
                {
                    switch (i)
                    {
                        case 3:
                            _carte.Add(new Button() { Width = 170, Height = 180, BorderThickness = new Thickness(0), Name = "CartaCpu1" });
                            Grid.SetColumn(_carte[i], 828);
                            break;
                        case 4:
                            _carte.Add(new Button() { Width = 170, Height = 180, BorderThickness = new Thickness(0), Name = "CartaCpu2" });
                            Canvas.SetLeft(_carte[i], 1086);
                            break;
                        case 5:
                            _carte.Add(new Button() { Width = 170, Height = 180, BorderThickness = new Thickness(0), Name = "CartaCpu3" });
                            Canvas.SetLeft(_carte[i], 1353);
                            break;
                    }
                    Canvas.SetTop(_carte[i], 0);
                }

                OnCarteCreate(null, null);
            }
        }

        /// <summary>
        /// Carica le risorse per la partita
        /// </summary>
        private void LoadResources(TipoCarta tipoCarte)
        {
            //Sfondo = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Immagini\\Sfondi\\Legno.png")));
            string path;

            if (tipoCarte == TipoCarta.Trevisana)
            {
                path = Environment.CurrentDirectory + "\\Immagini\\CarteTrevisane";
            }
            else
            {
                path = Environment.CurrentDirectory + "\\Immagini\\CarteNapoletane";
            }

            RetroCartaMazzo = new ImageBrush(new BitmapImage(new Uri(path + "\\Retro.png")));
            Carte[3].Background = Carte[5].Background = Carte[5].Background = new ImageBrush(new BitmapImage(new Uri(path + "\\RetroMeta.png")));
            RetroCartePiccolo = new ImageBrush(new BitmapImage(new Uri(path + "\\RetroPiccolo.png")));
            CartaBriscola = new ImageBrush(new BitmapImage(new Uri(_partita.Briscola.Img)));
            _carte[0].Background = new ImageBrush(new BitmapImage(new Uri(Giocatore?.MazzoGiocatore[0]?.Img)));
            _carte[1].Background = new ImageBrush(new BitmapImage(new Uri(Giocatore?.MazzoGiocatore[1]?.Img)));
            _carte[2].Background = new ImageBrush(new BitmapImage(new Uri(Giocatore?.MazzoGiocatore[2]?.Img)));
            /*carte[3].Background = new ImageBrush(new BitmapImage(new Uri(partita.Giocatori[1].MazzoGiocatore[0].Img)));
            carte[4].Background = new ImageBrush(new BitmapImage(new Uri(partita.Giocatori[1].MazzoGiocatore[1].Img)));
            carte[5].Background = new ImageBrush(new BitmapImage(new Uri(partita.Giocatori[1].MazzoGiocatore[2].Img)));*/

            CanPlayerPlay = false;
            _carte[3].IsHitTestVisible = false;
            _carte[4].IsHitTestVisible = false;
            _carte[5].IsHitTestVisible = false;
            OpacitaTavola = 0;
        }

        /// <summary>
        /// Anima lo spostamento delle carte di entrambi i giocatori
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick()
        {
            _tempoCpu++;
            if (_tempoCpu == 2)
            {
                IsTurnoCpu = true;
                _tempoCpu = 0;

                Carta cartaCpu;
                int index;

                if (_isG1PrimoTurno)
                {
                    cartaCpu = GiocaCPU(1, out index);
                    _mazzoTavola.Add(cartaCpu);//creare metodo animazione carte cpu
                }
                else
                {
                    cartaCpu = GiocaCPU(0, out index);
                    _mazzoTavola.Add(cartaCpu);//creare metodo animazione carte cpu
                }


                //PRELEVO LE INFORMAZIONI DELLA CARTA GIOCATA PER LA RELATIVA ANIMAZIONE

                switch (index)
                {
                    case 0:
                        _carte[3].Background = new ImageBrush(new BitmapImage(new Uri(cartaCpu.Img)));
                        _carte[3].Height = 360;
                        _posCartaGiocataCpu.X = Canvas.GetLeft(_carte[3]);
                        _posCartaGiocataCpu.Y = Canvas.GetTop(_carte[3]);
                        _cartaGiocataCpu = _carte[3];
                        _cartaLanciataCpu = 1;
                        LanciaCarta(_carte[3]);
                        break;
                    case 1:
                        _carte[4].Background = new ImageBrush(new BitmapImage(new Uri(cartaCpu.Img)));
                        _carte[4].Height = 360;
                        _posCartaGiocataCpu.X = Canvas.GetLeft(_carte[4]);
                        _posCartaGiocataCpu.Y = Canvas.GetTop(_carte[4]);
                        _cartaGiocataCpu = _carte[4];
                        _cartaLanciataCpu = 2;
                        LanciaCarta(_carte[4]);
                        break;
                    case 2:
                        _carte[5].Background = new ImageBrush(new BitmapImage(new Uri(cartaCpu.Img)));
                        _carte[5].Height = 360;
                        _posCartaGiocataCpu.X = Canvas.GetLeft(_carte[5]);
                        _posCartaGiocataCpu.Y = Canvas.GetTop(_carte[5]);
                        _cartaGiocataCpu = _carte[5];
                        _cartaLanciataCpu = 3;
                        LanciaCarta(_carte[5]);
                        break;
                }
                IsTurnoCpu = false;
                IsTurnoG1 = true;
                if (_isG1PrimoTurno)
                {
                    PescaGioca = "PESCA";
                }

                CanPlayerPlay = false;
            }
        }

        /// <summary>
        /// Metodo per il turno di CPU
        /// </summary>
        private void TurnoCpu()
        {
            IsTurnoCpu = true;
            //_timer = new DispatcherTimer
            //{
            //    IsEnabled = true,
            //    Interval = TimeSpan.FromSeconds(2)
            //};

            Helper.RunTemporized(() => Timer_Tick(), TimeSpan.FromSeconds(2));
            _tempoCpu = 0;
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

            } while (num == _posizionePrecedenteCarta);

            _posizionePrecedenteCarta = num;

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

            if (_cartaGiocataCpu != null)
            {
                Grid.SetZIndex(_cartaGiocataCpu, 1);
                Grid.SetZIndex(bottone, 5);
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
            Canvas.SetLeft(b, Canvas.GetLeft(CartaMazzo));
            Canvas.SetTop(b, Canvas.GetTop(CartaMazzo));
            b.Width = CartaMazzo.Width;
            b.Height = CartaMazzo.Height;
            b.Background = CartaMazzo.Background;
            b.BorderThickness = CartaMazzo.BorderThickness;

            OnCartaPescata(null, null);

            DoubleAnimation animation = new DoubleAnimation() { To = ToX, Duration = TimeSpan.FromMilliseconds(1) };
            DoubleAnimation animation2 = new DoubleAnimation() { To = ToY, Duration = TimeSpan.FromMilliseconds(1) };

            b.RenderTransform = new RotateTransform(0);
            b.BeginAnimation(Canvas.LeftProperty, animation);
            b.BeginAnimation(Canvas.TopProperty, animation2);
            b.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation() { To = 100, Duration = TimeSpan.FromMilliseconds(600) });
            b.IsHitTestVisible = true;

            if (cpu)
            {
                if (_partita.Mazzo.Count == 2)
                {
                    IsCartaMazzoVisibile = true;
                }
                else if (_partita.Mazzo.Count == 1)
                {
                    IsUltimoTurno = true;
                }

                b.Height = 180;
                Canvas.SetTop(b, 0);
                b.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\CarteTrevisane\\RetroMeta.png")));
            }
            else
            {
                if (_partita.Mazzo.Count == 2)
                {
                    IsCartaMazzoVisibile = true;
                    b.Background = new ImageBrush(new BitmapImage(new Uri(cartaPescata.Img)));
                }
                else if (_partita.Mazzo.Count == 1)
                {
                    b.Background = CartaBriscola;
                    IsUltimoTurno = true;
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

            if (winner?.Username == Giocatore?.Username)
            {
                animation.To = 1710;
                animation2.To = 830;
            }
            else
            {
                animation.To = 1710;
                animation2.To = 10;
            }
            _cartaGiocataG1.IsHitTestVisible = false;
            _cartaGiocataCpu.IsHitTestVisible = false;
            _cartaGiocataG1.BeginAnimation(Canvas.LeftProperty, animation);
            _cartaGiocataG1.BeginAnimation(Canvas.TopProperty, animation2);
            _cartaGiocataCpu.BeginAnimation(Canvas.LeftProperty, animation);
            _cartaGiocataCpu.BeginAnimation(Canvas.TopProperty, animation2);
            _cartaGiocataG1.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(600) });
            _cartaGiocataCpu.BeginAnimation(UIElement.OpacityProperty, new DoubleAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(600) });
            PescaGioca = "PESCA";
            CanPlayerPlay = true;
        }

        /// <summary>
        /// Pulisce il tavolo, togliendo le carte dal mazzo e pescando le carte ai giocatori
        /// </summary>
        private void PulisciTavola()
        {
            //se ultimo turno devo pescare le 2 carte in tavola
            if (_partita.Mazzo.Count > 0)
            {
                foreach (Giocatore g in _partita.Giocatori)
                {
                    Carta c = _partita.Pesca();

                    if (g.Username == Giocatore?.Username)
                    {
                        g.MazzoGiocatore.Insert(_cartaLanciataG1 - 1, c);
                        if (IsCarta1Selezionata)
                        {
                            PescaCarta(_carte[0], c, _posCartaGiocataG1.X, _posCartaGiocataG1.Y);
                        }

                        else if (IsCarta2Selezionata)
                        {
                            PescaCarta(_carte[1], c, _posCartaGiocataG1.X, _posCartaGiocataG1.Y);
                        }

                        else
                        {
                            PescaCarta(_carte[2], c, _posCartaGiocataG1.X, _posCartaGiocataG1.Y);
                        }
                    }
                    else
                    {
                        g.MazzoGiocatore.Insert(_cartaLanciataCpu - 1, c);

                        switch (_cartaLanciataCpu)
                        {
                            case 1:
                                PescaCarta(_carte[3], c, _posCartaGiocataCpu.X, _posCartaGiocataCpu.Y, true);
                                break;
                            case 2:
                                PescaCarta(_carte[4], c, _posCartaGiocataCpu.X, _posCartaGiocataCpu.Y, true);
                                break;
                            case 3:
                                PescaCarta(_carte[5], c, _posCartaGiocataCpu.X, _posCartaGiocataCpu.Y, true);
                                break;
                        }
                    }
                    _cartePescate++;
                }
            }
        }

        /// <summary>
        /// Clic della carta del GIOCATORE 1 => LAVORO VISUALE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelezionaCarta(object sender)
        {
            if (_cartaGiocataG1 != null)
            {
                _cartaGiocataG1 = null;
            }

            if (IsTurnoG1 && !_lanciata)
            {
                Button bottone = sender as Button;

                switch (bottone.Name)
                {
                    #region CARTA 1
                    case "Carta1":
                        if (!IsCarta1Selezionata)
                        {
                            _carte[1].Visibility = Visibility.Hidden;
                            _carte[2].Visibility = Visibility.Hidden;
                            IsCarta1Selezionata = true;
                            Canvas.SetTop((UIElement)sender, Canvas.GetTop((UIElement)sender));
                            CanPlayerPlay = true;
                        }
                        else
                        {
                            Canvas.SetTop((UIElement)sender, Canvas.GetTop((UIElement)sender));
                            IsCarta1Selezionata = false;

                            _carte[1].Visibility = Visibility.Visible;
                            _carte[2].Visibility = Visibility.Visible;
                            CanPlayerPlay = false;
                        }
                        break;
                    #endregion
                    #region CARTA 2
                    case "Carta2":
                        if (!IsCarta2Selezionata)
                        {
                            _carte[0].Visibility = Visibility.Hidden;
                            _carte[2].Visibility = Visibility.Hidden;
                            IsCarta2Selezionata = true;
                            Canvas.SetTop(_carte[1], Canvas.GetTop(_carte[1]));
                            CanPlayerPlay = true;
                        }
                        else
                        {
                            Canvas.SetTop(_carte[1], Canvas.GetTop(_carte[1]));
                            IsCarta2Selezionata = false;

                            _carte[0].Visibility = Visibility.Visible;
                            _carte[2].Visibility = Visibility.Visible;
                            CanPlayerPlay = false;
                        }
                        break;
                    #endregion
                    #region CARTA 3
                    case "Carta3":
                        if (!IsCarta3Selezionata)
                        {
                            _carte[0].Visibility = Visibility.Hidden;
                            _carte[1].Visibility = Visibility.Hidden;
                            IsCarta3Selezionata = true;
                            Canvas.SetTop(_carte[2], Canvas.GetTop(_carte[2]));
                            CanPlayerPlay = true;
                        }
                        else
                        {
                            Canvas.SetTop(_carte[2], Canvas.GetTop(_carte[2]));
                            IsCarta3Selezionata = false;
                            _carte[0].Visibility = Visibility.Visible;
                            _carte[1].Visibility = Visibility.Visible;
                            CanPlayerPlay = false;
                        }
                        break;
                        #endregion
                }
            }
        }



        /// <summary>
        /// Azioni che succedono il clic del pulsante GIOCA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EseguiPescaGioca(object p)
        {
            if (PescaGioca == "GIOCA")
            {
                _numeroTurno++;
                if (IsCarta1Selezionata)
                {
                    _cartaGiocataG1 = _carte[0];
                    _cartaLanciataG1 = 1;
                }
                else if (IsCarta2Selezionata)
                {
                    _cartaGiocataG1 = _carte[1];
                    _cartaLanciataG1 = 2;
                }
                else
                {
                    _cartaGiocataG1 = _carte[2];
                    _cartaLanciataG1 = 3;
                }
                _posCartaGiocataG1.X = (int)Canvas.GetLeft(_cartaGiocataG1);
                _posCartaGiocataG1.Y = (int)Canvas.GetTop(_cartaGiocataG1);

                LanciaCarta(_cartaGiocataG1);

                _carte[0].Visibility = Visibility.Visible;
                _carte[1].Visibility = Visibility.Visible;
                _carte[2].Visibility = Visibility.Visible;

                _lanciata = true;
                Giocatore.CartaGiocata = Giocatore?.MazzoGiocatore[_cartaLanciataG1 - 1];
                GiocaCarta(Giocatore, Giocatore?.CartaGiocata);

                if (_isG1PrimoTurno)
                {
                    IsTurnoG1 = false;
                    TurnoCpu();
                }

                CanPlayerPlay = false;
                IsTurnoG1 = false;
                IsTurnoCpu = false;
            }
            else if (PescaGioca == "PESCA")
            {
                GiocaTurno();
                PescaGioca = "GIOCA";
                CanPlayerPlay = false;
                CarteRimaste = 40 - _cartePescate;
            }
        }

        /// <summary>
        /// Gioco del turno completo
        /// </summary>
        public void GiocaTurno()
        {
            if (_partita.Mazzo.Count >= 0 && !_partita.IsUltimoTurno)
            {
                _mazzoTavola.Clear();

                #region ORDINE PRIMO TURNO
                if (_numeroTurno == 0)
                {
                    Thread.Sleep(100);
                    Random rnd = new Random();
                    _partita.OrdineTurno(rnd.Next(0, 2));
                }
                #endregion

                PulisciTavola();
                AzzeraTurno();

                if (_isG1PrimoTurno)
                {
                    IsTurnoG1 = true;
                }

                else
                {
                    IsTurnoG1 = false;
                    TurnoCpu();
                }
                _numeroTurno++;/*
                btnCarteRimaste.Text = (40 - cartePescate).ToString();
                btnPuntiG1.Text = giocatore.Punti.ToString();
                btnPuntiCpu.Text = partita.Giocatori.Find(x => x.Username == Partita.CPU1).Punti.ToString();*/
            }
            else //SE ULTIMO TURNO (mazzi carte vuoto e mazzi giocatori vuoti)
            {
                _partita.ConfrontaVincitore();
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
                cartaG1 = _partita.Giocatori[_partita.Giocatori.FindIndex(x => x.Username == Giocatore.Username)].CartaGiocata;
            }

            if (i == 0)
            {
                _partita.Giocatori[i].CartaGiocata = TwoPlayers.GetCartaCpu(_partita.Giocatori[i].MazzoGiocatore,
                                                                           _partita.Briscola,
                                                                           out index);
            }
            else
            {
                _partita.Giocatori[i].CartaGiocata = TwoPlayers.GetCartaCpu(_partita.Giocatori[i].MazzoGiocatore,
                                                                           _partita.Briscola, out index,
                                                                           _partita.Giocatori[0].CartaGiocata);
            }

            GiocaCarta(_partita.Giocatori[i], _partita.Giocatori[i].CartaGiocata);
            return _partita.Giocatori[i].CartaGiocata;
        }

        /// <summary>
        /// Successivamente al clic del tavolo si procede ad un nuovo turno
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PulisciTavola(object p)
        {
            //if (cartaGiocataG1 != null && cartaGiocataCpu != null)
            //{
            _isG1PrimoTurno = _partita.OrdineTurno(_partita.Giocatori.IndexOf(_partita.Confronto()));
            AssegnaCartePunti(_partita.Giocatori[0]);
            PuntiCpu = _partita.Giocatori.Find(x => x.Username == Partita.CPU1).Punti;
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
            _mazzoTavola.Add(c);
        }

        /// <summary>
        /// Visualizzazione delle carte vinte dal giocatore
        /// </summary>
        private void VisCartePunti(object p)
        {
            MazzoPunti form = new MazzoPunti(Giocatore);
            form.ShowDialog();
        }

        #endregion
    }
}
