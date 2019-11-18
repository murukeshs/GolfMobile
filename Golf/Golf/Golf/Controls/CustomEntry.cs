using Golf.Enums;
using Xamarin.Forms;

namespace Golf.Controls
{
    public class CustomEntry : Entry
    {
        public static readonly BindableProperty BorderColorProperty =
       BindableProperty.Create(nameof(BorderColor),
           typeof(Color), typeof(CustomEntry), Color.Gray);
        // Gets or sets BorderColor value  
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty DisableColorProperty =
      BindableProperty.Create(nameof(DisableColor),
          typeof(Color), typeof(CustomEntry), Color.LightGray);
        // Gets or sets BorderColor value  
        public Color DisableColor
        {
            get => (Color)GetValue(DisableColorProperty);
            set => SetValue(DisableColorProperty, value);
        }

        #pragma warning disable CS0618 // Type or member is obsolete
        public static readonly BindableProperty BorderWidthProperty =  BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(CustomEntry), Device.OnPlatform<int>(1, 2, 2));
        #pragma warning restore CS0618 // Type or member is obsolete

        // Gets or sets BorderWidth value  
        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        #pragma warning disable CS0618 // Type or member is obsolete
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius),typeof(double), typeof(CustomEntry), Device.OnPlatform<double>(6, 7, 7));
        #pragma warning restore CS0618 // Type or member is obsolete
      
        // Gets or sets CornerRadius value  
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly BindableProperty IsCurvedCornersEnabledProperty =
        BindableProperty.Create(nameof(IsCurvedCornersEnabled),
            typeof(bool), typeof(CustomEntry), true);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool IsCurvedCornersEnabled
        {
            get => (bool)GetValue(IsCurvedCornersEnabledProperty);
            set => SetValue(IsCurvedCornersEnabledProperty, value);
        }

        public static readonly BindableProperty ImageProperty =
           BindableProperty.Create(nameof(Image), typeof(string), typeof(CustomEntry), string.Empty);

        public static readonly BindableProperty ImageHeightProperty =
           BindableProperty.Create(nameof(ImageHeight), typeof(int), typeof(CustomEntry), 40);

        public static readonly BindableProperty ImageWidthProperty =
          BindableProperty.Create(nameof(ImageWidth), typeof(int), typeof(CustomEntry), 40);

        public static readonly BindableProperty ImageAlignmentProperty =
           BindableProperty.Create(nameof(ImageAlignment), typeof(ImageAlignment), typeof(CustomEntry), ImageAlignment.Left);

        public int ImageWidth
        {
            get { return (int)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public int ImageHeight
        {
            get { return (int)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public ImageAlignment ImageAlignment
        {
            get { return (ImageAlignment)GetValue(ImageAlignmentProperty); }
            set { SetValue(ImageAlignmentProperty, value); }
        }

        public CustomEntry()
        {
            BorderWidth = 1;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.Center;
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Entry));
        }
    }
}
