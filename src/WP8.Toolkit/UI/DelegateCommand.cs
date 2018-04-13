
namespace WP8.Toolkit
{
    using System;
    using System.Windows.Input;

    public class DelegateCommand : ICommand
    {
        #region [ Members ]

        private Func<object, bool> _canExecute;
        private Action<object> _action;

        #endregion

        public DelegateCommand(Action<object> action)
            : this(action, null)
        {
        }

        public DelegateCommand(Action<object> action, Func<object, bool> canExecute)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return (_canExecute != null) ? _canExecute(parameter) : true;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            var handler = this.CanExecuteChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
