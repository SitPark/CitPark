using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CitPark.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NumericUpDown : ContentView
	{
        #region Constructor(s)
        public NumericUpDown ()
		{
			InitializeComponent ();
		}
        #endregion

        #region Maximum (Bindable double)
        public static readonly BindableProperty MaximumProperty = BindableProperty.Create(
            "Maximum", //Public name to use
            typeof(double), //The type
            typeof(NumericUpDown), //Parent type (this control)
            10.0); //Default value
        
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        #endregion

        #region Minimum (Bindable double)
        public static readonly BindableProperty MinimumProperty = BindableProperty.Create(
            "Minimum", //Public name to use
            typeof(double), //The type
            typeof(NumericUpDown), //Parent type (this control)
            0.0); //Default value

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        #endregion

        #region Increment (Bindable double)
        public static readonly BindableProperty IncrementProperty = BindableProperty.Create(
            "Increment", //Public name to use
            typeof(double), //The type
            typeof(NumericUpDown), //Parent type (this control)
            1.0); //Default value

        public double Increment
        {
            get { return (double)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }
        #endregion

        #region Value (Bindable double)
        public static readonly BindableProperty ValueProperty = BindableProperty.Create(
            "Value", //Public name to use
            typeof(double), //The type
            typeof(NumericUpDown), //Parent type (this control)
            1.0); //Default value

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        private void DownButton_Clicked(object sender, EventArgs e)
        {
            if(Value - Increment < Minimum)
            {
                Value = Minimum;
            }
            else
            {
                Value -= Increment;
            }

            DownButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void UpButton_Clicked(object sender, EventArgs e)
        {
            if(Value + Increment > Maximum)
            {
                Value = Maximum;
            }
            else
            {
                Value += Increment;
            }

            UpButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            if(Value > Maximum)
            {
                Value = Maximum;
            }
            else if(Value < Minimum)
            {
                Value = Minimum;
            }

            EntryUnfocused?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler UpButtonClicked;

        public event EventHandler DownButtonClicked;

        public event EventHandler EntryUnfocused;
    }
}