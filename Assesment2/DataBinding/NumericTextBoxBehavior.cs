using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Assesment2.DataBinding;

public static class NumericTextBoxBehavior
{
    public static void AttachNumericBehavior(TextBox textBox)
    {
        textBox.PreviewTextInput += (sender, e) => { e.Handled = !IsTextNumeric(e.Text); };

        DataObject.AddPastingHandler(textBox, (sender, e) =>
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var text = (string)e.DataObject.GetData(DataFormats.Text);
                if (!IsTextNumeric(text)) e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        });
    }

    private static bool IsTextNumeric(string text)
    {
        return Regex.IsMatch(text, "^[0-9]+$");
    }
}