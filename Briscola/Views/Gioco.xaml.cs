using Briscola.ViewModels;
using System;
using System.Windows;

namespace Briscola
{
    /// <summary>
    /// Logica di interazione per Gioco.xaml
    /// </summary>
    public partial class Gioco : Window
    {
        private readonly GiocoDueGiocatoriViewModel _viewModel;

        public Gioco(GiocoDueGiocatoriViewModel viewModel)
        {
            InitializeComponent();
            gbCpu.IsEnabled = false;
            gbGiocatore1.IsEnabled = false;

            viewModel.OnCarteCreate += _viewModel_OnCarteCreate;
            viewModel.OnCartaPescata += _viewModel_OnCartaPescata;
            viewModel.GridMain = gridMain;
            _viewModel = viewModel;

            DataContext = _viewModel;
        }

        private void _viewModel_OnCartaPescata(object sender, EventArgs e)

        {
            //Canvas.SetLeft(b, Canvas.GetLeft(cartaMazzo));
            //Canvas.SetTop(b, Canvas.GetTop(cartaMazzo));
            //b.Width = cartaMazzo.Width;
            //b.Height = cartaMazzo.Height;
            //b.Background = cartaMazzo.Background;
            //b.BorderThickness = cartaMazzo.BorderThickness;
        }

        private void _viewModel_OnCarteCreate(object sender, EventArgs e)
        {
            foreach (var carta in _viewModel.Carte)
            {
                gridMain.Children.Add(carta);
            }
        }

        /// <summary>
        /// Username > 13 caratteri => Diminuzione del font (per text block giocatore)
        /// </summary>
        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if (txtG1.Text.Length > 13)
            {
                txtG1.FontSize = 33;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => _viewModel.WindowLoadedCommand.Execute(null);
    }
}