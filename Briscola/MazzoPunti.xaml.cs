using Briscola.Models;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Briscola
{
    /// <summary>
    /// Logica di interazione per MazzoPunti.xaml
    /// </summary>
    public partial class MazzoPunti : Window
    {
        public MazzoPunti(Giocatore giocatore)
        {
            InitializeComponent();
            lblNomeGiocatore.Content = giocatore.Username;
            lblPunti.Content = giocatore.Punti.ToString();
            for (int i = 0; i < giocatore.MazzoPunti.Count; i++)
            {
                Rectangle r = new Rectangle();
                r.Width = 170;
                r.Height = 360;
                if (i == giocatore.MazzoPunti.Count - 1)
                    r.Margin = new Thickness(30, 1, 30, 17);
                else
                    r.Margin = new Thickness(30, 1, 0, 17);
                r.Fill = new ImageBrush(new BitmapImage(new Uri(giocatore.MazzoPunti[i].Img)));
                stk.Children.Add(r);
            }
        }

        private void btnIndietro_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
