using Briscola.ViewModels;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Media;

namespace Briscola
{
    /// <summary>
    /// Logica di interazione per Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly LoginViewModel _loginViewModel;

        public Login(OleDbConnection connection)
        {
            _loginViewModel = new LoginViewModel(connection);
            _loginViewModel.OnClosing += (send, ev) => Close();

            DataContext = _loginViewModel;
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
