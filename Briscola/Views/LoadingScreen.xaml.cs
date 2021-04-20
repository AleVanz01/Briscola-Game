using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

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
            cnv.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\Sfondi\\Legno.png")));
            txtUsername.Text = username != "" ? username : null;
            cnv.Background = new ImageBrush(image);
            cnvScritta.Background = new ImageBrush(scritta);
            timer = new DispatcherTimer();
            pgb.Minimum = 0;
            pgb.Maximum = 1000;
            pgb.Value = 0;
            timer.IsEnabled = true;
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        DispatcherTimer timer;

        public double Progress { get => pgb.Value; set { pgb.Value = value; } }

        private void timer_Tick(object sender, EventArgs e)
        {
            pgb.Value += 10;
            if (pgb.Value == pgb.Maximum)
            {
                timer.Stop();
                timer.IsEnabled = false;
                Close();
            }
        }
    }
}
