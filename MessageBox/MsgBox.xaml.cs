using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MessageBox
{
    /// <summary>
    /// Logica di interazione per MsgBox.xaml
    /// </summary>
    public enum MessageBoxType
    {
        ConfirmationWithYesNo,
        ConfirmationWithYesNoCancel,
        Information,
        Error,
        Warning
    }

    public enum MessageBoxIcon
    {
        Warning,
        Question,
        Information,
        Error,
        None
    }
    public partial class MsgBox : Window
    {
        private MsgBox()
        {
            InitializeComponent();
        }
        static MsgBox _messageBox;
        static MessageBoxResult _result = MessageBoxResult.No;

        public static MessageBoxResult Show(string caption, string msg, MessageBoxType type)
        {
            switch (type)
            {
                case MessageBoxType.ConfirmationWithYesNo:
                    return Show(caption, msg, MessageBoxButton.YesNo,
                    MessageBoxIcon.Question);
                case MessageBoxType.ConfirmationWithYesNoCancel:
                    return Show(caption, msg, MessageBoxButton.YesNoCancel,
                    MessageBoxIcon.Question);
                case MessageBoxType.Information:
                    return Show(caption, msg, MessageBoxButton.OK,
                    MessageBoxIcon.Information);
                case MessageBoxType.Error:
                    return Show(caption, msg, MessageBoxButton.OK,
                    MessageBoxIcon.Error);
                case MessageBoxType.Warning:
                    return Show(caption, msg, MessageBoxButton.OK,
                    MessageBoxIcon.Warning);
                default:
                    return MessageBoxResult.No;
            }
        }
        public static MessageBoxResult Show(string msg, MessageBoxType type)
        {
            return Show(string.Empty, msg, type);
        }
        public static MessageBoxResult Show(string msg)
        {
            return Show(string.Empty, msg,
            MessageBoxButton.OK, MessageBoxIcon.None);
        }
        public static MessageBoxResult Show(string caption, string text)
        {
            return Show(caption, text,
            MessageBoxButton.OK, MessageBoxIcon.None);
        }
        public static MessageBoxResult Show(string caption, string text, MessageBoxButton button)
        {
            return Show(caption, text, button,
            MessageBoxIcon.None);
        }
        public static MessageBoxResult Show(string caption, string text, MessageBoxButton button, MessageBoxIcon image)
        {
            _messageBox = new MsgBox { txtMsg = { Text = text }, MessageTitle = { Text = caption } };
            SetVisibilityOfButtons(button);
            SetImageOfMessageBox(image);
            _messageBox.ShowDialog();
            return _result;
        }
        private static void SetVisibilityOfButtons(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OK:
                    _messageBox.btnCancel.Visibility = Visibility.Collapsed;
                    _messageBox.btnNo.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Visibility = Visibility.Collapsed;
                    _messageBox.btnOk.Focus();
                    break;
                case MessageBoxButton.OKCancel:
                    _messageBox.btnNo.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Focus();
                    break;
                case MessageBoxButton.YesNo:
                    _messageBox.btnOk.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Focus();
                    break;
                case MessageBoxButton.YesNoCancel:
                    _messageBox.btnOk.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Focus();
                    break;
                default:
                    break;
            }
        }
        private static void SetImageOfMessageBox(MessageBoxIcon image)
        {
            switch (image)
            {
                case MessageBoxIcon.Warning:
                    _messageBox.SetImage("Warning.png");
                    break;
                case MessageBoxIcon.Question:
                    _messageBox.SetImage("Question.png");
                    break;
                case MessageBoxIcon.Information:
                    _messageBox.SetImage("Information.png");
                    break;
                case MessageBoxIcon.Error:
                    _messageBox.SetImage("Error.png");
                    break;
                default:
                    _messageBox.img.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnOk)
                _result = MessageBoxResult.OK;
            else if (sender == btnYes)
                _result = MessageBoxResult.Yes;
            else if (sender == btnNo)
                _result = MessageBoxResult.No;
            else if (sender == btnCancel)
                _result = MessageBoxResult.Cancel;
            else
                _result = MessageBoxResult.None;
            _messageBox.Close();
            _messageBox = null;
        }
        private void SetImage(string imageName)
        {
            string uri = string.Format("Resources/{0}", imageName);
            var uriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
            img.Source = new BitmapImage(uriSource);
        }

        private void btnOk_MouseEnter(object sender, MouseEventArgs e)
        {
            btnOk.Background = Brushes.MediumAquamarine;
        }

        private void btnOk_MouseLeave(object sender, MouseEventArgs e)
        {
            btnOk.Background = Brushes.Transparent;
        }
        private void btnYes_MouseEnter(object sender, MouseEventArgs e)
        {
            btnYes.Background = Brushes.MediumAquamarine;
        }

        private void btnYes_MouseLeave(object sender, MouseEventArgs e)
        {
            btnYes.Background = Brushes.Transparent;
        }

        private void btnCancel_MouseEnter(object sender, MouseEventArgs e)
        {
            btnOk.Background = Brushes.LightPink;
        }

        private void btnCancel_MouseLeave(object sender, MouseEventArgs e)
        {
            btnOk.Background = Brushes.Transparent;
        }

        private void btnNo_MouseEnter(object sender, MouseEventArgs e)
        {
            btnNo.Background = Brushes.LightPink;
        }

        private void btnNo_MouseLeave(object sender, MouseEventArgs e)
        {
            btnNo.Background = Brushes.Transparent;
        }
    }
}
