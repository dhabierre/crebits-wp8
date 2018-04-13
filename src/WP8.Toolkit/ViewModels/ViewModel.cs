
namespace WP8.Toolkit.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ViewModel : INotifyPropertyChanged
    {
        #region [ Constructor ]

        public ViewModel()
        {
            this.Commands = new List<DelegateCommand>();

            this.PropertyChanged += OnPropertyChanged;
        }

        #endregion

        #region [ Events ]

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        #endregion

        #region [ Commands ]

        private IList<DelegateCommand> Commands { get; set; }

        protected void RegisterCommand(DelegateCommand command)
        {
            if (command != null)
            {
                if (!this.Commands.Contains(command))
                {
                    this.Commands.Add(command);
                }
            }
        }

        protected virtual void RaiseCanExecuteChanged()
        {
            foreach (var command in this.Commands)
            {
                command.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region [ INotifyPropertyChanged Members ]

        public event PropertyChangedEventHandler PropertyChanged = null;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(member, value))
            {
                return false;
            }

            member = value;

            this.NotifyPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}