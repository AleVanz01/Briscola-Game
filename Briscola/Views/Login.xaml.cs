using Briscola.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Briscola
{
    /// <summary>
    /// Logica di interazione per Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly LoginViewModel _loginViewModel;

        public Login(LoginViewModel loginViewModel)
        {
            InitializeComponent();

            _loginViewModel = loginViewModel;
            _loginViewModel.OnClosing += (send, ev) => Close();
            _loginViewModel.OnRegistrazioneRichiesta += _loginViewModel_OnRegistrazioneRichiesta;

            DataContext = _loginViewModel;
        }

        private void _loginViewModel_OnRegistrazioneRichiesta(object sender, EventArgs e)
        {
            TextBox textBox;
            ComboBox comboBox;

            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        textBox = CreaControllo(typeof(TextBox), "Cognome") as TextBox;
                        textBox.Name = "txtCognome";
                        stp1.Children.Add(textBox);
                        break;
                    case 1:
                        textBox = CreaControllo(typeof(TextBox), "Nome") as TextBox;
                        textBox.Name = "txtNome";
                        stp1.Children.Add(textBox);
                        break;
                    case 2:
                        comboBox = CreaControllo(typeof(ComboBox), "Età") as ComboBox;
                        comboBox.Name = "cbEta";
                        stp1.Children.Add(comboBox);
                        break;
                }
            }

            _loginViewModel.Controlli = new List<UIElement>();

            for (int i = 0; i < stp1.Children.Count; i++)
            {
                if (i > 1 && i < 5)
                {
                    continue;
                }
                else
                {
                    _loginViewModel.Controlli.Add(stp1.Children[i]);
                }
            }

            _loginViewModel.Controlli.Add(stp1.Children[2]);
            _loginViewModel.Controlli.Add(stp1.Children[3]);
            _loginViewModel.Controlli.Add(stp1.Children[4]);
            stp1.Children.Clear();

            for (int i = 0; i < _loginViewModel.Controlli.Count; i++)
            {
                stp1.Children.Add(_loginViewModel.Controlli[i]);
            }
        }

        private Control CreaControllo(Type tipo, string testo)
        {
            if (tipo == typeof(TextBox))
            {
                TextBox t;
                StringBuilder builder = new StringBuilder();
                builder.Append("<TextBox xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' ");
                builder.Append("Margin ='20' Style = '{StaticResource TextBoxCustom}' ");
                builder.Append("xmlns:materialDesign='http://materialdesigninxaml.net/winfx/xaml/themes'");
                builder.Append($" VerticalAlignment='Top' Height = '40' AcceptsReturn = 'True' TextWrapping = 'Wrap'");
                builder.Append($" VerticalScrollBarVisibility='Auto'> <materialDesign:HintAssist.Hint> ");
                string textBlock = $@"<TextBlock xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Name = 
                                    'hint{testo.Substring(0, testo.Length - 1)}' Background = 'WhiteSmoke'> {testo} </TextBlock>";
                builder.Append($"{textBlock}  </materialDesign:HintAssist.Hint> </TextBox>");
                t = (TextBox)XamlReader.Parse(builder.ToString());

                t.MaxLength = 15;
                t.PreviewTextInput += (sender, e) =>
                {
                    if (!char.IsLetter(e.Text[0]))
                    {
                        e.Handled = true;
                    }
                };

                return t;
            }
            else if (tipo == typeof(ComboBox))
            {
                List<int> rangeEta = new List<int>();
                for (int i = 0; i < 99; i++)
                {
                    rangeEta.Add(i + 1);
                }

                ComboBox c;

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

            return null;
        }

        private void txtUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == "")
            {
                hintUsername.Foreground = Brushes.Red;
            }
            else
            {
                hintUsername.Foreground = Brushes.Black;
            }
        }

        private void txtPsw_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPsw.Password == "")
            {
                hintPsw.Foreground = Brushes.Red;
            }
            else
            {
                hintPsw.Foreground = Brushes.Black;
            }
        }
    }
}
