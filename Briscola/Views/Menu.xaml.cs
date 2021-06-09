using Briscola.Models;
using MessageBox;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Briscola
{
    /// <summary>
    /// Logica di interazione per Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
            //gridMain.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Immagini\\Sfondi\\Legno.png")));
            connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + Environment.CurrentDirectory + "\\Briscola.accdb");
            //UpdateClassifica();
            btnTwoPlayers.IsEnabled = false;
            //btnThreePlayers.IsEnabled = false;
            btnFourPlayers.IsEnabled = false;
            //Hide();
        }

        private readonly OleDbConnection connection;
        private Giocatore giocatore;

        private void UpdateClassifica()
        {
            connection.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM GIOCATORI", connection);
            OleDbDataReader reader;
            reader = cmd.ExecuteReader();
            List<Giocatore> giocatori = new List<Giocatore>();

            while (reader.Read())
            {
                Giocatore giocatore = new Giocatore(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                giocatore.GetPlayerStats(connection);
                giocatori.Add(giocatore);
            }

            dgClassifica.BorderThickness = new Thickness(5);
            dgClassifica.CanUserAddRows = false;
            dgClassifica.CanUserDeleteRows = false;
            dgClassifica.CanUserResizeColumns = false;
            dgClassifica.CanUserSortColumns = false;
            dgClassifica.IsReadOnly = true;
            dgClassifica.AutoGenerateColumns = false;
            dgClassifica.ItemsSource = giocatori;
            connection.Close();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (btnLogin.Content.ToString() == "Login")
            {
                Login login = null;
                bool registrato = true;
                while (registrato)
                {
                    login = new Login(connection);
                    if (login.ShowDialog() == true)
                    {
                        registrato = login.Registrazione;
                    }
                    else
                    {
                        registrato = false;
                    }
                }
                if (login.Loggato)
                {
                    giocatore = login.Giocatore;
                    imgAdmin.Fill = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Immagini\\utente.png")));
                    UpdatePlayerData();
                    UpdateClassifica();
                    lblUsername.Text = giocatore.Username;
                    btnLogin.Content = "Logout";
                    btnTwoPlayers.IsEnabled = true;
                    btnThreePlayers.IsEnabled = true;
                    btnFourPlayers.IsEnabled = true;
                }
            }
            else
            {
                RestorePlayerData();
                lblUsername.Text = "";
                btnLogin.Content = "Login";
                btnTwoPlayers.IsEnabled = false;
                btnThreePlayers.IsEnabled = false;
                btnFourPlayers.IsEnabled = false;
            }
        }

        private void UpdatePlayerData()
        {
            Statistiche stats = giocatore.GetPlayerStats(connection);
            lblUsername.Text = $"Username: {giocatore.Username}";
            lblCognome.Text = $"Cognome: {giocatore.Cognome}";
            lblNome.Text = $"Nome: {giocatore.Nome}";
            lblEta.Text = $"Età: {giocatore.Eta}";
            lblPuntiTotali.Text = $"Punti Totali: {stats.PunteggioTotale}";
            lblPuntiMaxPartita.Text = $"Punti Max in partita: {stats.PunteggioMaxPartita}";
            lblPartiteVinte.Text = $"Partite Vinte: {stats.PartiteVinte}";
            lblPartitePerse.Text = $"Partite Perse: {stats.PartitePerse}";
        }

        private void RestorePlayerData()
        {
            lblUsername.Text = $"Username";
            lblCognome.Text = $"Cognome: ";
            lblNome.Text = $"Nome: ";
            lblEta.Text = $"Età: ";
            lblPuntiTotali.Text = $"Punti Totali: ";
            lblPuntiMaxPartita.Text = $"Punti Max in partita: ";
            lblPartiteVinte.Text = $"Partite Vinte: ";
            lblPartitePerse.Text = $"Partite Perse: ";
            dgClassifica.ItemsSource = null;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (MsgBox.Show("Attenzione", "Sicuro di voler chiudere il gioco?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void btnTwoPlayers_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Gioco partita = new Gioco(giocatore, "", new BitmapImage());
            //partita.Closed += (s, args) => this.Close();
            //partita.Show();
        }

        private void btnThreePlayers_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            LoadingScreen load = new LoadingScreen(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Immagini\\Sfondi\\TappetinoRosso.jpg")), giocatore.Username, new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\Sfondi\\ScrittaBriscola.png")));
            load.ShowDialog();
            Show();
            //Gioco partita = new Gioco(3, giocatore);
        }

        private void btnFourPlayers_Click(object sender, RoutedEventArgs e)
        {
            //Gioco partita = new Gioco(4, giocatore);
        }
    }
}
