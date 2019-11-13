using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Golf.Controls
{
    /// <summary>
	/// The check box.
	/// </summary>
	public class CustomCheckBox : View
    {
        /// <summary>
        /// The checked state property.
        /// </summary>
        /// 
        public static readonly BindableProperty CheckedProperty =
            BindableProperty.Create<CustomCheckBox, bool>(
                p => p.Checked, false, BindingMode.TwoWay, propertyChanged: OnCheckedPropertyChanged);

        /// <summary>
        /// The checked text property.
        /// </summary>
        public static readonly BindableProperty CheckedTextProperty =
            BindableProperty.Create<CustomCheckBox, string>(
                p => p.CheckedText, string.Empty, BindingMode.TwoWay);

        /// <summary>
        /// The unchecked text property.
        /// </summary>
        public static readonly BindableProperty UncheckedTextProperty =
            BindableProperty.Create<CustomCheckBox, string>(
                p => p.UncheckedText, string.Empty);

        /// <summary>
        /// The default text property.
        /// </summary>
        public static readonly BindableProperty DefaultTextProperty =
            BindableProperty.Create<CustomCheckBox, string>(
                p => p.Text, string.Empty);

        /// <summary>
        /// Identifies the TextColor bindable property.
        /// </summary>
        /// 
        /// <remarks/>
        public static readonly BindableProperty TextColorProperty =
          BindableProperty.Create("TextColor", typeof(Color), typeof(CustomRadioButton), Color.Default);

        public static readonly BindableProperty BorderColorProperty =
          BindableProperty.Create("BorderColor", typeof(Color), typeof(CustomRadioButton), Color.Default);


        /// <summary>
        /// The font size property
        /// </summary>
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create<CustomCheckBox, double>(
                p => p.FontSize, -1);

        /// <summary>
        /// The font name property.
        /// </summary>
        public static readonly BindableProperty FontNameProperty =
            BindableProperty.Create<CustomCheckBox, string>(
                p => p.FontName, string.Empty);


        /// <summary>
		/// The checked changed event.
		/// </summary>
		public event EventHandler<EventArg<bool>> CheckedChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value>The checked state.</value>
        public bool Checked
        {

            get => (bool)GetValue(CheckedProperty);
            set
            {
                if (Checked != value)
                {
                    SetValue(CheckedProperty, value);
                    //CheckedChanged.Invoke(this, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the checked text.
        /// </summary>
        /// <value>The checked state.</value>
        /// <remarks>
        /// Overwrites the default text property if set when checkbox is checked.
        /// </remarks>
        public string CheckedText
        {
            get => (string)GetValue(CheckedTextProperty);
            set => SetValue(CheckedTextProperty, value);
        }

        public static readonly BindableProperty DefaultValueProperty =
          BindableProperty.Create("Text", typeof(string), typeof(CustomRadioButton), string.Empty);

        public string DefaultValue
        {
            get => (string)GetValue(DefaultValueProperty);
            set => SetValue(DefaultValueProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value>The checked state.</value>
        /// <remarks>
        /// Overwrites the default text property if set when checkbox is checked.
        /// </remarks>
        public string UncheckedText
        {
            get => (string)GetValue(UncheckedTextProperty);
            set => SetValue(UncheckedTextProperty, value);
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string DefaultText
        {
            get => (string)GetValue(DefaultTextProperty);
            set => SetValue(DefaultTextProperty, value);
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
        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get
            {
                return Checked
                    ? (string.IsNullOrEmpty(CheckedText) ? DefaultText : CheckedText)
                        : (string.IsNullOrEmpty(UncheckedText) ? DefaultText : UncheckedText);
            }
        }

        /// <summary>
        /// Called when [checked property changed].
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldvalue">if set to <c>true</c> [oldvalue].</param>
        /// <param name="newvalue">if set to <c>true</c> [newvalue].</param>
        private static void OnCheckedPropertyChanged(BindableObject bindable, bool oldvalue, bool newvalue)
        {
            var checkBox = (CustomCheckBox)bindable;
            checkBox.Checked = newvalue;
        }
    }
}
