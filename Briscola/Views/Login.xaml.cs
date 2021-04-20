using Briscola.Models;
using MessageBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Briscola
{
    /// <summary>
    /// Logica di interazione per Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private OleDbConnection _connection;
        private DataTable _utenti;
        private List<object> _controlli;

        public Login(OleDbConnection connection)
        {
            InitializeComponent();
            _utenti = new DataTable("Utenti");
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM GIOCATORI", connection);
            adapter.Fill(_utenti);
            this._connection = connection;

            if (File.Exists(Environment.CurrentDirectory + "\\login.txt"))
            {
                StreamReader reader = new StreamReader(new FileStream(Environment.CurrentDirectory + "\\login.txt", FileMode.Open));
                string user = reader.ReadLine();
                string psw = reader.ReadLine();
                txtUsername.Text = user;
                txtPsw.Password = psw;
                chkRicordami.IsChecked = true;
                reader.Close();
                File.Delete(Environment.CurrentDirectory + "\\Login.txt");
            }
        }

        public Giocatore Giocatore { get; private set; }

        public bool Registrazione { get; private set; }

        public bool Loggato { get; private set; }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == "" || txtPsw.Password == "")
                MsgBox.Show("Attenzione", "Inserire tutti i campi obbligatori", MessageBoxButton.OK, MessageBoxIcon.Warning);
            else
            {
                if (txtLogin.Text == "Login")
                {
                    if (CheckLogin(txtUsername.Text, txtPsw.Password, out string errore))
                    {
                        MsgBox.Show("Accesso Eseguito");
                        if (chkRicordami.IsChecked == true)
                        {
                            StreamWriter writer = new StreamWriter(new FileStream(Environment.CurrentDirectory + "\\Login.txt", FileMode.Create));
                            writer.WriteLine(txtUsername.Text);
                            writer.WriteLine(txtPsw.Password);
                            writer.Close();
                            File.Encrypt(Environment.CurrentDirectory + "\\Login.txt");
                        }
                        Loggato = true;
                        DialogResult = true;
                        Close();
                    }
                    else
                        MsgBox.Show("Attenzione", "Accesso Fallito: " + errore, MessageBoxButton.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (CheckRegistrazione(GetDataTextBox(), out string errore))
                    {
                        MsgBox.Show("Registrazione eseguita con successo!", "Attenzione", MessageBoxButton.OK, MessageBoxIcon.Information);
                        Registrazione = true;
                        DialogResult = true;
                        Close();
                    }
                    else
                        MsgBox.Show("Attenzione", "Registrazione fallita: " + errore, MessageBoxButton.OK, MessageBoxIcon.Information);
                }
            }
        }
        #region REGISTRAZIONE
        private void btnRegistra_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = new TextBox();
            ComboBox comboBox = new ComboBox();
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0: textBox = CreaTextbox(textBox, "Cognome"); textBox.Name = "txtCognome"; stp1.Children.Add(textBox); break;
                    case 1: textBox = CreaTextbox(textBox, "Nome"); textBox.Name = "txtNome"; stp1.Children.Add(textBox); break;
                    case 2: comboBox = CreaComboBox(comboBox, "Età"); comboBox.Name = "cbEta"; stp1.Children.Add(comboBox); break;
                }
            }
            _controlli = new List<object>();
            for (int i = 0; i < stp1.Children.Count; i++)
            {
                if (i > 1 && i < 5)
                    continue;
                else
                    _controlli.Add(stp1.Children[i]);
            }
            _controlli.Add(stp1.Children[2]);
            _controlli.Add(stp1.Children[3]);
            _controlli.Add(stp1.Children[4]);
            stp1.Children.Clear();

            for (int i = 0; i < _controlli.Count; i++)
                stp1.Children.Add((UIElement)_controlli[i]);

            txtLogin.Text = "Registrati";
            stpLogin.Width += 30;
            stpRegistra.Visibility = Visibility.Collapsed;
            Width += 50;
            grid.Width += 50;
            Height += 230;
            Top -= 150;
        }

        bool CheckRegistrazione(string[] dati, out string errore)
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

        string[] GetDataTextBox()
        {
            string[] dati = new string[5];

            TextBox t1;
            PasswordBox p1;
            ComboBox c1;
            int j = 0;
            for (int i = 0; i < _controlli.Count; i++)
            {
                if (_controlli[i] is StackPanel)
                {
                    StackPanel s1 = _controlli[i] as StackPanel;
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
                else if (_controlli[i] is TextBox)
                {
                    t1 = _controlli[i] as TextBox;
                    dati[j] = t1.Text;
                    j++;
                }
                else if (_controlli[i] is ComboBox)
                {
                    c1 = _controlli[i] as ComboBox;
                    dati[j] = c1.Text;
                    j++;
                }
            }

            return dati;
        }
        #endregion
        bool CheckLogin(string username, string psw, out string errore)
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
                        errore = "L'username inserito non è corretto: Riprovare";
                    else
                        errore = "L'username e la password inseriti non sono corretti: Riprovare.";
                }
            }
            return trovato;
        }

        #region Creazione COMBOBOX
        ComboBox CreaComboBox(ComboBox c, string testo)
        {
            List<int> rangeEta = new List<int>();
            for (int i = 0; i < 99; i++)
                rangeEta.Add(i + 1);

            StringBuilder builder = new StringBuilder();
            builder.Append("<ComboBox xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Margin='20' ");
            builder.Append("ItemsSource='{ Binding rangeEta}'");
            builder.Append(" Style = '{StaticResource MaterialDesignFloatingHintComboBox}' ");
            builder.Append("xmlns:materialDesign='http://materialdesigninxaml.net/winfx/xaml/themes' ");
            builder.Append($"VerticalAlignment='Top' Height = '40' materialDesign:TextFieldAssist.UnderlineBrush='LimeGreen' materialDesign:HintAssist.Foreground='LimeGreen' > <materialDesign:HintAssist.Hint>");
            string textBlock = $"<TextBlock xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Name = 'hint{testo}' Background = 'WhiteSmoke'> {testo} </TextBlock>";
            builder.Append($"{textBlock}  </materialDesign:HintAssist.Hint>");
            builder.Append($"<ComboBox.ItemsPanel> <ItemsPanelTemplate> <VirtualizingStackPanel/> </ItemsPanelTemplate> </ComboBox.ItemsPanel> </ComboBox>");
            c = (ComboBox)XamlReader.Parse(builder.ToString());
            c.ItemsSource = rangeEta;

            return c;
        }
        #endregion
        #region Creazione TEXTBOX   
        TextBox CreaTextbox(TextBox t, string testo)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<TextBox xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' ");
            builder.Append("Margin ='20' Style = '{StaticResource TextBoxCustom}' ");
            builder.Append("xmlns:materialDesign='http://materialdesigninxaml.net/winfx/xaml/themes'");
            builder.Append($" VerticalAlignment='Top' Height = '40' AcceptsReturn = 'True' TextWrapping = 'Wrap'");
            builder.Append($" VerticalScrollBarVisibility='Auto'> <materialDesign:HintAssist.Hint> ");
            string textBlock = $"<TextBlock xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Name = 'hint{testo.Substring(0, testo.Length - 1)}' Background = 'WhiteSmoke'> {testo} </TextBlock>";
            builder.Append($"{textBlock}  </materialDesign:HintAssist.Hint> </TextBox>");
            t = (TextBox)XamlReader.Parse(builder.ToString());

            t.MaxLength = 15;
            t.PreviewTextInput += txt_PreviewTextInput;
            return t;
        }

        private void txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetter(e.Text[0]))
                e.Handled = true;
        }
        #endregion

        private void btnChiudi_Click(object sender, RoutedEventArgs e) => Close();

        private void txtUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == "")
                hintUsername.Foreground = Brushes.Red;
            else
                hintUsername.Foreground = Brushes.Black;
        }

        private void txtPsw_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPsw.Password == "")
                hintPsw.Foreground = Brushes.Red;
            else
                hintPsw.Foreground = Brushes.Black;
        }
    }
}
