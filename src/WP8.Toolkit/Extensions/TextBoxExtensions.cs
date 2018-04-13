
namespace System.Windows.Controls
{
    public static class TextBoxExtensions
    {
        public static void ForceUpdateSource(this TextBox source)
        {
            var expression = source.GetBindingExpression(TextBox.TextProperty);
            if (expression != null)
            {
                expression.UpdateSource();
            }
        }
    }
}
