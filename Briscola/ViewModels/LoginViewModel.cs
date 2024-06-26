﻿using Briscola.Models;
using MessageBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Briscola.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly OleDbConnection _connection;
        private readonly DataTable _utenti;
        private Giocatore _giocatore;
        private bool _isRicordamiAbilitato;
        private string _operazione;
        private Visibility _panelRegistrazione;
        private double _panelLoginWidth;
        private double _windowWidth;
        private double _windowHeight;
        private double _windowTop;
        private double _gridWidth;

        public LoginViewModel(OleDbConnection connection)
        {
            _utenti = new DataTable("Utenti");
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM GIOCATORI", connection);
            adapter.Fill(_utenti);
            _connection = connection;

            if (File.Exists(Environment.CurrentDirectory + "\\Login.txt"))
            {
                Giocatore = new Giocatore();
                StreamReader reader = new StreamReader(new FileStream(Environment.CurrentDirectory + "\\Login.txt", FileMode.Open));
                Giocatore.Username = reader.ReadLine();
                Giocatore.Password = reader.ReadLine();

                IsRicordamiAbilitato = true;
                reader.Close();
                File.Delete(Environment.CurrentDirectory + "\\Login.txt");
            }

            Operazione = "Login";
            Giocatore = new Giocatore();

        }

        public ICommand ChiudiCommand => new RelayCommand((param) => OnClosing(null, null));

        public ICommand LoginCommand => new RelayCommand(EseguiLogin);

        public ICommand RegistraCommand => new RelayCommand(EseguiRegistrazione);

        public event EventHandler OnClosing;
        public event EventHandler OnRegistrazioneRichiesta;

        public Giocatore Giocatore
        {
            get => _giocatore;
            set => SetProperty(ref _giocatore, value);
        }

        public List<UIElement> Controlli { get; set; }

        public bool IsRicordamiAbilitato
        {
            get => _isRicordamiAbilitato;
            set => SetProperty(ref _isRicordamiAbilitato, value);
        }

        public string Operazione
        {
            get => _operazione;
            set => SetProperty(ref _operazione, value);
        }

        public double WindowTop
        {
            get => _windowTop;
            set => SetProperty(ref _windowTop, value);
        }

        public double WindowHeight
        {
            get => _windowHeight;
            set => SetProperty(ref _windowHeight, value);
        }

        public double WindowWidth
        {
            get => _windowWidth;
            set => SetProperty(ref _windowWidth, value);
        }

        public double PanelLoginWidth
        {
            get => _panelLoginWidth;
            set => SetProperty(ref _panelLoginWidth, value);
        }

        public double GridWidth
        {
            get => _gridWidth;
            set => SetProperty(ref _gridWidth, value);
        }

        public Visibility PanelRegistrazione
        {
            get => _panelRegistrazione;
            set => SetProperty(ref _panelRegistrazione, value);
        }

        public bool Registrazione { get; private set; }

        public bool IsLoggato { get; private set; }

        public void EseguiLogin(object p)
        {
            try
            {
                if (Giocatore?.Username == "" || Giocatore?.Password == "")
                {
                    MsgBox.Show("Attenzione", "Inserire tutti i campi obbligatori", MessageBoxType.Warning);
                }
                else
                {
                    if (Operazione == "Login")
                    {
                        if (CheckLogin(Giocatore?.Username, Giocatore?.Password ?? "345", out string errore))
                        {
                            MsgBox.Show("Accesso Eseguito", MessageBoxType.Information);
                            if (IsRicordamiAbilitato == true)
                            {
                                using (FileStream file = new FileStream(Environment.CurrentDirectory + "\\Login.txt", FileMode.Create))
                                {
                                    using (StreamWriter writer = new StreamWriter(file))
                                    {
                                        writer.WriteLine(Giocatore?.Username);
                                        writer.WriteLine(Giocatore?.Password);
                                    }
                                }
                            }

                            IsLoggato = true;
                            OnClosing(null, null);
                        }
                        else
                        {
                            MsgBox.Show("Attenzione", "Accesso Fallito: " + errore, MessageBoxType.Warning);
                        }
                    }
                    else
                    {
                        if (CheckRegistrazione(GetDataTextBox(), out string errore))
                        {
                            MsgBox.Show("Registrazione eseguita con successo!", "Attenzione", MessageBoxType.Information);
                            Registrazione = true;
                            OnClosing(null, null);
                        }
                        else
                        {
                            MsgBox.Show("Attenzione", "Registrazione fallita: " + errore, MessageBoxType.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show("Attenzione", ex.Message, MessageBoxType.Error);
            }
        }

        public void EseguiRegistrazione(object p)
        {
            Operazione = "Registrati";
            PanelLoginWidth += 30;
            PanelRegistrazione = Visibility.Collapsed;
            WindowWidth += 50;
            GridWidth += 50;
            WindowHeight += 230;
            WindowTop -= 150;

            OnRegistrazioneRichiesta(null, null);
        }

        private bool CheckLogin(string username, string psw, out string errore)
        {
            errore = "";
            bool trovato = false;
            foreach (DataRow item in _utenti.Rows)
            {
                if (item[0].ToString() == username && item[1].ToString() == psw)
                {
                    Giocatore = new Giocatore(item[0].ToString(), item[1].ToString(), item[2].ToString(), item[3].ToString(), item[4].ToString());
                    return true;
                }
                else
                {
                    if (item[1].ToString() == psw)
                    {
                        errore = "L'username inserito non è corretto: Riprovare";
                    }
                    else
                    {
                        errore = "L'username e la password inseriti non sono corretti: Riprovare.";
                    }
                }
            }
            return trovato;
        }

        private bool CheckRegistrazione(string[] dati, out string errore)
        {
            errore = "";
            foreach (DataRow item in _utenti.Rows)
            {
                if (item[0].ToString() == dati[0])
                {
                    errore = "Esiste già un profilo con l'username inserito";
                    return false;
                }

            }

            _connection.Open();
            string sqlCmd = string.Format($"INSERT INTO GIOCATORI VALUES ('{dati[0]}','{dati[1]}','{dati[2]}','{dati[3]}','{dati[4]}')");
            OleDbCommand cmd = new OleDbCommand(sqlCmd, _connection);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                errore = "Assicurarsi di aver compilato correttamente tutti i campi";
                return false;
            }
            finally
            {
                _connection.Close();
            }

        }

        private string[] GetDataTextBox()
        {
            string[] dati = new string[5];

            TextBox t1;
            PasswordBox p1;
            ComboBox c1;
            int j = 0;
            for (int i = 0; i < Controlli.Count; i++)
            {
                if (Controlli[i] is StackPanel)
                {
                    StackPanel s1 = Controlli[i] as StackPanel;
                    if (s1.Children[0] is TextBox)
                    {
                        t1 = s1.Children[0] as TextBox;
                        dati[0] = t1.Text;
                        j++;
                    }
                    else if (s1.Children[0] is PasswordBox)
                    {
                        p1 = s1.Children[0] as PasswordBox;
                        dati[1] = p1.Password;
                        j++;
                    }
                }
                else if (Controlli[i] is TextBox)
                {
                    t1 = Controlli[i] as TextBox;
                    dati[j] = t1.Text;
                    j++;
                }
                else if (Controlli[i] is ComboBox)
                {
                    c1 = Controlli[i] as ComboBox;
                    dati[j] = c1.Text;
                    j++;
                }
            }

            return dati;
        }
    }
}
