using Briscola.Models;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Briscola
{
    /// <summary>
    /// Logica di interazione per LoadingScreen.xaml
    /// </summary>
    public partial class LoadingScreen : Window
    {
        public LoadingScreen(BitmapImage image, string username = "", BitmapImage scritta = null)
        {
            InitializeComponent();
            grid.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Immagini\\Sfondi\\Legno.png")));
            txtUsername.Text = username != "" ? username : null;
            //grid.Background = new ImageBrush(image);
            gridScritta.Background = new ImageBrush(scritta);

            pgb.Minimum = 0;
            pgb.Maximum = 1000;
            pgb.Value = 0;

            Helper.RunTemporized(() => Caricamento(), TimeSpan.FromMilliseconds(1));
        }

        public double Progress { get => pgb.Value; set => pgb.Value = value; }

        private void Caricamento()
        {
            pgb.Value += 10;

            if (pgb.Value == pgb.Maximum)
            {
                Close();
            }
        }
    }
}