using Xamarin.Forms;

namespace Golf.Controls
{
    public class UpdateViewCell : ViewCell
    {

        /// <summary>
        /// The SelectedBackgroundColor property.
        /// </summary>
        public static readonly BindableProperty SelectedBackgroundColorProperty =
            BindableProperty.Create("SelectedBackgroundColor", typeof(Color), typeof(UpdateViewCell), Color.Default);

        /// <summary>
        /// Gets or sets the SelectedBackgroundColor.
        /// </summary>
        public Color SelectedBackgroundColor
        {
            get { return (Color)GetValue(SelectedBackgroundColorProperty); }
            set { SetValue(SelectedBackgroundColorProperty, value); }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Device.RuntimePlatform == Device.Android)
            {
                this.ForceUpdateSize();
            }
        }
    }
}
