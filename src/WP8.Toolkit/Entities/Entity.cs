
namespace WP8.Toolkit.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class Entity : INotifyPropertyChanged
    {
        #region [ Constructors ]

        public Entity()
        {
            this.PropertyChanged += OnPropertyChanged;
        }

        #endregion

        #region [ Events ]

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        #endregion

        #region [ INotifyPropertyChanged Members ]

        public event PropertyChangedEventHandler PropertyChanged = null;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
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