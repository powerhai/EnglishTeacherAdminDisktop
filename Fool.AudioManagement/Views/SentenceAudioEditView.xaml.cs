using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Fool.AudioManagement.ViewModels;
namespace Fool.AudioManagement.Views
{
    /// <summary>
    /// Interaction logic for SentenceAudioEditView
    /// </summary>
    public partial class SentenceAudioEditView : UserControl
    {
        public SentenceAudioEditView()
        {
            InitializeComponent();
        }
        public SentenceAudioEditViewModel ViewModel
        {
            get
            {
                return this.DataContext as SentenceAudioEditViewModel;
            }
        }
        private void RangeSlider_OnHigherValueChanged(object sender, RoutedEventArgs e)
        {
            var sen = sender as FrameworkElement; 
            var c = Mouse.LeftButton; 
            if(c == MouseButtonState.Pressed && sen.IsMouseOver )
            {
                this.ViewModel.ResetRangeCommand.Execute(null);
            }
        }
    }
}
