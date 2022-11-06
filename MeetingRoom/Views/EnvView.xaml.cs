using System.Windows;
using System.Windows.Controls;


namespace MeetingRoom.Views
{
    /// <summary>
    /// Interaction logic for EnvView.xaml
    /// </summary>
    public partial class EnvView : UserControl
    {
        public EnvView()
        {
            InitializeComponent();
        }

        public void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }
    }
}
