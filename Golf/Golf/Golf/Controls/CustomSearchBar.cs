using Xamarin.Forms;

namespace Golf.Controls
{
    public class CustomSearchBar : SearchBar
    {
        public static readonly BindableProperty BorderColorProperty =
       BindableProperty.Create(nameof(BorderColor),
           typeof(Color), typeof(CustomSearchBar), Color.Default);
        // Gets or sets BorderColor value  
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty DisableColorProperty =
      BindableProperty.Create(nameof(DisableColor),
          typeof(Color), typeof(CustomSearchBar), Color.Default);
        // Gets or sets BorderColor value  
        public Color DisableColor
        {
            get => (Color)GetValue(DisableColorProperty);
            set => SetValue(DisableColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty =
        BindableProperty.Create(nameof(BorderWidth), typeof(int),
#pragma warning disable CS0618 // Type or member is obsolete
            typeof(CustomSearchBar), Device.OnPlatform<int>(1, 2, 2));
#pragma warning restore CS0618 // Type or member is obsolete
        // Gets or sets BorderWidth value  
        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }
        public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius),
#pragma warning disable CS0618 // Type or member is obsolete
            typeof(double), typeof(CustomSearchBar), Device.OnPlatform<double>(6, 7, 7));
#pragma warning restore CS0618 // Type or member is obsolete
        // Gets or sets CornerRadius value  
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly BindableProperty IsCurvedCornersEnabledProperty =
        BindableProperty.Create(nameof(IsCurvedCornersEnabled),
            typeof(bool), typeof(CustomSearchBar), true);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool IsCurvedCornersEnabled
        {
            get => (bool)GetValue(IsCurvedCornersEnabledProperty);
            set => SetValue(IsCurvedCornersEnabledProperty, value);
        }

        public CustomSearchBar()

        {
            //CornerRadius = 10;
            BorderColor = (Color)App.Current.Resources["TextGrayColor"];
            BorderWidth = 1;
            CornerRadius = 10;
            IsCurvedCornersEnabled = true;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.Center;
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(SearchBar));
            TextColor = (Color)App.Current.Resources["TextGrayColor"];
            //BackgroundColor = WpcStylingColors.BackgroundColorWhite;
        }
    }
}
