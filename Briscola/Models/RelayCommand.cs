using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Briscola.Models
{
    public class RelayCommand : ICommand
    {
        #region Campi

        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        #endregion 

        #region Costruttori

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        #endregion 

        #region Metodi

        [DebuggerStepThrough] //In debug non entra qua
        public bool CanExecute(object parameter = null) => _canExecute == null ? true : _canExecute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter = null) => _execute(parameter);

        #endregion 
    }
}
