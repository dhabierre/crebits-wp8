
namespace WP8.Crebits.ViewModels
{
    using WP8.Crebits.Helpers;
    using WP8.Toolkit;

    public class NotesPageViewModel : WP8.Toolkit.ViewModels.PageViewModel
    {
        #region [ Constructor ]

        public NotesPageViewModel()
        {
            #region [ Register Commands ]

            this.SaveCommand = new DelegateCommand(this.Save, this.CanExecuteSave);
            base.RegisterCommand(this.SaveCommand);

            #endregion

            this.LoadData();
        }

        #endregion

        #region [ Properties ]

        #region [ Notes ]

        private string _notes;

        public string Notes
        {
            get { return _notes; }
            set { base.SetProperty(ref _notes, value); }
        }

        #endregion

        #endregion

        #region [ Methods ]

        private void LoadData()
        {
            this.Notes = SettingsHelper.GetNotes();
        }

        #endregion

        #region [ Commands ]

        #region [ SaveCommand ]

        public DelegateCommand SaveCommand { get; set; }

        private bool CanExecuteSave(object parameter)
        {
            return true;
        }

        private void Save(object parameter)
        {
            SettingsHelper.SetNotes(this.Notes);
        }

        public bool Save()
        {
            if (this.SaveCommand.CanExecute(null))
            {
                this.SaveCommand.Execute(null);
                return true;
            }

            return false;
        }

        #endregion

        #endregion

        #region [ PageViewModel Overrides ]

        public override string PageTitle
        {
            get
            {
                return WP8.Crebits.Resources.AppResources.Notes;
            }
        }

        #endregion
    }
}