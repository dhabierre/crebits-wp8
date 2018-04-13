
namespace WP8.Toolkit.Controls
{
    using System.Windows.Controls;

    public class TextBoxUpdate : TextBox
    {
        public TextBoxUpdate()
        {
            this.TextChanged += this.OnTextBoxTextChanged;
        }

        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).ForceUpdateSource();
        }
    }
}
