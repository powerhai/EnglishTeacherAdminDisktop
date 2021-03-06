using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fool.TextManagement.ViewModels;
namespace Fool.TextManagement.Views
{
    /// <summary>
    /// Interaction logic for TextEditView
    /// </summary>
    public partial class TextEditView : UserControl
    {
        public TextEditView()
        {
            InitializeComponent();
        }
 

        public TextEditViewModel ViewModel
        {
            get
            {
                return this.DataContext as TextEditViewModel;
            }
        }
        private void RangeSlider_OnHigherValueChanged(object sender, RoutedEventArgs e)
        {
            var sen = sender as FrameworkElement;
            var c = Mouse.LeftButton;
            if (c == MouseButtonState.Pressed && sen.IsMouseOver) {
                this.ViewModel.ResetRangeCommand.Execute(null);
            }
        }
    }
}
