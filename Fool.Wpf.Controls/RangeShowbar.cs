using System;
using System.Collections.Generic;
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

namespace Fool.Wpf.Controls
{

    public class RangeShowbar : Control
    {
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(long), typeof(RangeShowbar), new PropertyMetadata(default(long), MaximumPropertyChanged));

        public long Maximum
        {
            get { return (long)GetValue(MaximumProperty); }
            set
            {
                SetValue(MaximumProperty, value); 
            }
        }
         
        public static readonly DependencyProperty LowerValueProperty = DependencyProperty.Register("LowerValue", typeof(long), typeof(RangeShowbar), new PropertyMetadata(default(long), LowerValuePropertyChanged));

        public long LowerValue
        {
            get { return (long)GetValue(LowerValueProperty); }
            set
            {
                SetValue(LowerValueProperty, value);  
            }
        }
        public static readonly DependencyProperty HigherValueProperty = DependencyProperty.Register("HigherValue", typeof(long), typeof(RangeShowbar), new PropertyMetadata(default(long), HigherValuePropertyChanged));
        private static void HigherValuePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var control = dependencyObject as RangeShowbar;
            control.AdjustView();
        }
        protected virtual void HigherValuePropertyChanged(double oldValue, double newValue)
        {
            this.AdjustView();
        }
        private static void LowerValuePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var control = dependencyObject as RangeShowbar;
            control.AdjustView();
        }
        protected virtual void LowerValuePropertyChanged(double oldValue, double newValue)
        {
            this.AdjustView();
        }
        private static void MaximumPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var control = dependencyObject as RangeShowbar;
            control.AdjustView();
        }
        public static readonly DependencyProperty LeftMarginProperty = DependencyProperty.Register("LeftMargin", typeof(double), typeof(RangeShowbar), new PropertyMetadata(default(double)));

        public double LeftMargin
        {
            get { return (double)GetValue(LeftMarginProperty); }
            set { SetValue(LeftMarginProperty, value); }
        }
        public static readonly DependencyProperty RightMarginProperty = DependencyProperty.Register("RightMargin", typeof(double), typeof(RangeShowbar), new PropertyMetadata(default(double)));

        public double RightMargin
        {
            get { return (double)GetValue(RightMarginProperty); }
            set { SetValue(RightMarginProperty, value); }
        }
        protected virtual void MaximumPropertyChanged(double oldValue, double newValue)
        {
            this.AdjustView();
        }
        public long HigherValue
        {
            get { return (long)GetValue(HigherValueProperty); }
            set
            {
                SetValue(HigherValueProperty, value);  
            }
        }
        static RangeShowbar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeShowbar), new FrameworkPropertyMetadata(typeof(RangeShowbar)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Loaded += OnLoaded;
            this.SizeChanged += RangeShowbar_SizeChanged;
        }

        private void RangeShowbar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.AdjustView();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
             this.AdjustView();
        }
   
        private void AdjustView()
        {
            double left = ActualWidth;
            double right = 0;
            if(Maximum <= 0 || HigherValue <= 0)
            {
                
            }
            else
            {
                left = ActualWidth * ((double)LowerValue / (double)Maximum);
                right = ActualWidth -  (ActualWidth *  ((double)HigherValue / (double)Maximum));
            }
               
            this.LeftMargin = left;
            this.RightMargin = right;

        }
         
    }
}
