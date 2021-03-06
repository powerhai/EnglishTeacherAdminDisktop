using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fool.AudioManagement.ViewModels;
namespace Fool.AudioManagement.Views
{
    /// <summary>
    /// Interaction logic for AudioPlayerView.xaml
    /// </summary>
    public partial class AudioPlayerView : UserControl
    {
        public AudioPlayerView()
        {
            InitializeComponent();
            this.Loaded += AudioPlayerView_Loaded;
        }
        private SentenceAudioEditViewModel mViewModel;
        private void AudioPlayerView_Loaded(object sender, RoutedEventArgs e)
        {
            mViewModel = this.DataContext as SentenceAudioEditViewModel;
        }

        private bool mIsManual = false;
        private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(mIsManual)
                Debug.WriteLine(e.NewValue);

        }
        private void Slider_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            mIsManual = true;
            this.mViewModel.SetIsManual(true);
        }
        private void Slider_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            mIsManual = false;
            this.mViewModel.SetIsManual(false);
            var fater = e.Source as FrameworkElement; 
            Point mousePoint = Mouse.GetPosition(fater);
            var br =  (double)(mousePoint.X / fater.ActualWidth);
            this.mViewModel.JumpTo( br);
        }
    }
}
