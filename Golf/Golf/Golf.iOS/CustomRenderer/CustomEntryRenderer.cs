using CoreGraphics;
using Golf.Controls;
using Golf.Enums;
using Golf.iOS.CustomRenderer;
using System.Drawing;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Golf.iOS.CustomRenderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntry CustomEntryElement => Element as CustomEntry;
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var view = (CustomEntry)Element;

                Control.LeftView = new UIView(new CGRect(0f, 0f, 9f, 20f));
                Control.LeftViewMode = UITextFieldViewMode.Always;
                Control.KeyboardAppearance = UIKeyboardAppearance.Dark;
                Control.ReturnKeyType = UIReturnKeyType.Done;
                // Radius for the curves  
                // Thickness of the Border Color  
                Control.Layer.BorderColor = view.BorderColor.ToCGColor();
                // Thickness of the Border Width  
                Control.Layer.BorderWidth = view.BorderWidth;
                Control.ClipsToBounds = true;

                var textField = this.Control;
                if (!string.IsNullOrEmpty(CustomEntryElement.Image))
                {
                    switch (CustomEntryElement.ImageAlignment)
                    {
                        case ImageAlignment.Left:
                            textField.LeftViewMode = UITextFieldViewMode.Always;
                            textField.LeftView = GetImageView(CustomEntryElement.Image, CustomEntryElement.ImageHeight, CustomEntryElement.ImageWidth);
                            break;
                        case ImageAlignment.Right:
                            textField.RightViewMode = UITextFieldViewMode.Always;
                            textField.RightView = GetImageView(CustomEntryElement.Image, CustomEntryElement.ImageHeight, CustomEntryElement.ImageWidth);
                            break;
                    }
                }
            }
        }

        private UIView GetImageView(string imagePath, int height, int width)
        {
            var uiImageView = new UIImageView(UIImage.FromBundle(imagePath))
            {
                Frame = new RectangleF(0, 0, width, height)
            };
            UIView objLeftView = new UIView(new System.Drawing.Rectangle(0, 0, width + 10, height));
            objLeftView.AddSubview(uiImageView);

            return objLeftView;
        }
    }
}