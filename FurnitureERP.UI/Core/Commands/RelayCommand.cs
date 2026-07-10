using System;
using System.Windows.Input;

namespace FurnitureERP.UI.Core.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // ✅ constructor جديد يدعم parameter
        private readonly Action<object?>? _executeWithParam;
        private readonly Func<object?, bool>? _canExecuteWithParam;

        public RelayCommand(Action<object?> executeWithParam, Func<object?, bool>? canExecuteWithParam = null)
        {
            _executeWithParam = executeWithParam;
            _canExecuteWithParam = canExecuteWithParam;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecuteWithParam != null)
                return _canExecuteWithParam(parameter);

            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object? parameter)
        {
            if (_executeWithParam != null)
                _executeWithParam(parameter);
            else
                _execute();
        }

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}