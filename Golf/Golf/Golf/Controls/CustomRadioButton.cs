using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Golf.Controls
{
    /// <summary>
    /// Class CustomRadioButton.
    /// </summary>
    public class CustomRadioButton : View
    {
        /// <summary>
        /// The checked property
        /// </summary>
        public static readonly BindableProperty CheckedProperty =
            BindableProperty.Create<CustomRadioButton, bool>(
                p => p.Checked, false, BindingMode.TwoWay, propertyChanged: OnCheckedPropertyChanged);

        /// <summary>
        ///     The default text property.
        /// </summary>
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create<CustomRadioButton, string>(
                p => p.Text, string.Empty);

       
        /// <summary>
        ///     The default text property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create("TextColor", typeof(Color), typeof(CustomRadioButton), Color.Default);

        /// <summary>
        /// The font size property
        /// </summary>
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create<CustomRadioButton, double>(
                p => p.FontSize, -1);

        /// <summary>
        /// The font name property.
        /// </summary>
        public static readonly BindableProperty FontNameProperty =
            BindableProperty.Create<CustomRadioButton, string>(
                p => p.FontName, string.Empty);

        /// <summary>
        ///     The checked changed event.
        /// </summary>
        public EventHandler<EventArg<bool>> CheckedChanged;

        /// <summary>
        ///     Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value>The checked state.</value>
        public bool Checked
        {
            get => (bool)GetValue(CheckedProperty);
            set => SetValue(CheckedProperty, value);
        }

        public static readonly BindableProperty CheckedChangedCommandProperty = BindableProperty.Create(nameof(CheckedChangedCommand), typeof(Command), typeof(CustomRadioButton), null, BindingMode.OneWay);

        public Command CheckedChangedCommand
        {
            get { return (Command)GetValue(CheckedChangedCommandProperty); }
            set { SetValue(CheckedChangedCommandProperty, value); }
        }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty =
         BindableProperty.Create("BorderColor", typeof(Color), typeof(CustomRadioButton), Color.Default);

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }
        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>The size of the font.</value>
        public double FontSize
        {
            get
            {
                return (double)GetValue(FontSizeProperty);
            }
            set
            {
                SetValue(FontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        public string FontName
        {
            get
            {
                return (string)GetValue(FontNameProperty);
            }
            set
            {
                SetValue(FontNameProperty, value);
            }
        }

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        /// <summary>
        /// Called when [checked property changed].
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldvalue">if set to <c>true</c> [oldvalue].</param>
        /// <param name="newvalue">if set to <c>true</c> [newvalue].</param>
        private static void OnCheckedPropertyChanged(BindableObject bindable, bool oldvalue, bool newvalue)
        {
            var radioButton = (CustomRadioButton)bindable;
            radioButton.Checked = newvalue;
        }
    }
}
