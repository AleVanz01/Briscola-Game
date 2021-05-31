using Briscola.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Briscola.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        public BackgroundWorker RefreshWorker { get; set; }

        public ICommand WindowLoadedCommand { get; set; }

        public ICommand RefreshCommand => new RelayCommand(param => RefreshWorker.RunWorkerAsync());
    }
}
