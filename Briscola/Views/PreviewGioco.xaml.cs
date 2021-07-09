using Briscola.Models.Enumeratori;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Briscola
{
    /// <summary>
    /// Logica di interazione per PreviewGioco.xaml
    /// </summary>
    public partial class PreviewGioco : Window
    {
        public PreviewGioco()
        {
            InitializeComponent();
            gridMain.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\Sfondi\\Legno.png")));
            btnCarteTrevisane.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\CarteTrevisane\\Carte.png")));
            btnCarteNapoletane.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\CarteNapoletane\\Carte.png")));
            btnTappetoLegno.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\Sfondi\\Legno2.jpg")));
            btnTappetoRosso.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\Sfondi\\TappetinoRosso.jpg")));
            btnTappetoVerde.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\Sfondi\\TappetinoAqua.jpg")));
            this.Icon = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\Sfondi\\ICONA GIOCO 2.ico"));

            obj = bdg_TappetoVerde.Badge;
            objCarta = bdg_CarteTrevisane.Badge;
            bdg_TappetoVerde.Badge = null;
            bdg_TappetoRosso.Badge = null;
            bdg_CarteNapoletane.Badge = null;
        }

        private readonly object obj;
        private readonly object objCarta;

        public TipoCarta TipoCarte { get; private set; }

        public string TipoSfondo { get; private set; }

        private void btnContinua_Click(object sender, RoutedEventArgs e) => DialogResult = true;

        private void btnCarteTrevisane_Click(object sender, RoutedEventArgs e)
        {
            Button botton = sender as Button;

            if (botton.Name == "btnCarteTrevisane")
            {
                bdg_CarteTrevisane.Badge = objCarta;
                bdg_CarteNapoletane.Badge = null;
                TipoCarte = TipoCarta.Trevisana;
            }
            else
            {
                bdg_CarteTrevisane.Badge = null;
                bdg_CarteNapoletane.Badge = objCarta;
                TipoCarte = TipoCarta.Napoletana;
            }

        }

        private void btnTappetoLegno_Click(object sender, RoutedEventArgs e)
        {
            Button botton = sender as Button;

            switch (botton.Name)
            {
                case "btnTappetoLegno":
                    bdg_TappetoLegno.Badge = obj;
                    bdg_TappetoVerde.Badge = null;
                    bdg_TappetoRosso.Badge = null;
                    TipoSfondo = "Legno2.jpg";
                    break;
                case "btnTappetoVerde":
                    bdg_TappetoLegno.Badge = null;
                    bdg_TappetoVerde.Badge = obj;
                    bdg_TappetoRosso.Badge = null;
                    TipoSfondo = "TappetinoAqua.jpg";
                    break;
                case "btnTappetoRosso":
                    bdg_TappetoLegno.Badge = null;
                    bdg_TappetoVerde.Badge = null;
                    bdg_TappetoRosso.Badge = obj;
                    TipoSfondo = "TappetinoRosso.jpg";
                    break;
            }
        }

        private void btnIndietro_Click(object sender, RoutedEventArgs e) => Close();
    }
}
