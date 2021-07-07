using Briscola.ViewModels;
using System;
using System.Windows;

namespace Briscola
{
    /// <summary>
    /// Logica di interazione per Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        private readonly MenuViewModel _menuViewModel;

        public Menu()
        {
            InitializeComponent();

            _menuViewModel = new MenuViewModel();
            _menuViewModel.OnHide += Nascondi;
            _menuViewModel.OnShow += Mostra;
            DataContext = _menuViewModel;
        }

        private void Nascondi(object sender, EventArgs e) => Hide();

        private void Mostra(object sender, EventArgs e) => Show();
    }
}
