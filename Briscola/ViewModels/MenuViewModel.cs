using Briscola.Models;
using MessageBox;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Briscola.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly OleDbConnection _connection;
        private ImageBrush _imgSfondo;
        private ImageBrush _imgProfilo;
        private Giocatore _giocatore;
        private InfoGiocatore _infoGiocatore;
        private bool _isPulsantiAbilitati;
        private string _tipoLogin;
        private List<Giocatore> _giocatori;

        public MenuViewModel()
        {
            _connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + Environment.CurrentDirectory + "\\Briscola.accdb");
            ImgSfondo = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\Sfondi\\Legno.png")));
            WindowLoadedCommand = new RelayCommand(UpdateClassifica);
            IsPulsantiAbilitati = false;
        }

        public ICommand EseguiLoginCommand => new RelayCommand(ApriLogin);

        public ICommand PartitaDueGiocatoriCommand => new RelayCommand(PartitaDueGiocatori);

        public ICommand PartitaTreGiocatoriCommand => new RelayCommand(PartitaTreGiocatori);

        public ICommand PartitaQuattroGiocatoriCommand => new RelayCommand((p) => MsgBox.Show("Coming Soon"));

        public ICommand ChiudiCommand => new RelayCommand(Chiudi);

        public event EventHandler OnHide;

        public event EventHandler OnClose;

        public ImageBrush ImgSfondo
        {
            get => _imgSfondo;
            set => SetProperty(ref _imgSfondo, value);
        }

        public ImageBrush ImgProfilo
        {
            get => _imgProfilo;
            set => SetProperty(ref _imgProfilo, value);
        }

        public Giocatore Giocatore
        {
            get => _giocatore;
            set => SetProperty(ref _giocatore, value);
        }

        public InfoGiocatore InfoGiocatore
        {
            get => _infoGiocatore;
            set => SetProperty(ref _infoGiocatore, value);
        }

        public List<Giocatore> Giocatori
        {
            get => _giocatori;
            set => SetProperty(ref _giocatori, value);
        }

        public bool IsPulsantiAbilitati
        {
            get => _isPulsantiAbilitati;
            set => SetProperty(ref _isPulsantiAbilitati, value);
        }

        public string TipoLogin
        {
            get => _tipoLogin;
            set => SetProperty(ref _tipoLogin, value);
        }

        private void UpdateClassifica(object p)
        {
            _connection.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM GIOCATORI ORDER BY USERNAME ASC", _connection);
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();
            Giocatori = new List<Giocatore>();

            while (reader.Read())
            {
                Giocatore giocatore = new Giocatore(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                giocatore.GetPlayerStats(_connection);
                Giocatori.Add(giocatore);
            }

            _connection.Close();
        }

        private void ApriLogin(object p)
        {
            if (TipoLogin == "Login")
            {
                //Login login = null;
                //bool registrato = true;
                //while (registrato)
                //{
                //    login = new Login(_connection);
                //    if (login.ShowDialog() == true)
                //    {
                //        registrato = login.Registrazione;
                //    }
                //    else
                //    {
                //        registrato = false;
                //    }
                //}
                //if (login.Loggato)
                //{
                //    Giocatore = login.Giocatore;
                //    ImgProfilo = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\utente.png")));
                //    UpdatePlayerData();
                //    UpdateClassifica(null);
                //    TipoLogin = "Logout";
                //    IsPulsantiAbilitati = true;
                //}
            }
            else
            {
                RestorePlayerData();
                TipoLogin = "Login";
                IsPulsantiAbilitati = false;
            }
        }

        private void UpdatePlayerData()
        {
            Statistiche stats = Giocatore.GetPlayerStats(_connection);

            InfoGiocatore = new InfoGiocatore(Giocatore, stats);
        }

        private void RestorePlayerData()
        {
            Giocatore = null;
            InfoGiocatore = new InfoGiocatore();
            Giocatori = new List<Giocatore>();
        }

        private void PartitaDueGiocatori(object p)
        {
            OnHide(null, null);
            Gioco partita = new Gioco(Giocatore, "", new BitmapImage());

            partita.Show();
        }

        private void PartitaTreGiocatori(object p)
        {
            OnHide(null, null);

            LoadingScreen load = new LoadingScreen(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\Sfondi\\TappetinoRosso.jpg")), Giocatore?.Username ?? "--", new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\Sfondi\\ScrittaBriscola.png")));
            load.ShowDialog();
            //Show();
            //Gioco partita = new Gioco(3, giocatore);
        }

        private void Chiudi(object p)
        {
            if (MsgBox.Show("Attenzione", "Sicuro di voler chiudere il gioco?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
