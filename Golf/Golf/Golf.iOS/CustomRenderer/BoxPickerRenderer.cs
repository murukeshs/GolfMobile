using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using Golf.Controls;
using Golf.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BoxPicker), typeof(BoxPickerRenderer))]
namespace Golf.iOS.CustomRenderer
{
    public class BoxPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);


            var view = (BoxPicker)Element;

            Control.LeftView = new UIView(new CGRect(0f, 0f, 9f, 20f));
            Control.LeftViewMode = UITextFieldViewMode.Always;

            Control.KeyboardAppearance = UIKeyboardAppearance.Dark;
            Control.ReturnKeyType = UIReturnKeyType.Done;
            // Radius for the curves  
            //Control.Layer.CornerRadius = Convert.ToSingle(view.CornerRadius);
            // Thickness of the Border Color  
            Control.Layer.BorderColor = view.BorderColor.ToCGColor();
            // Thickness of the Border Width  
            Control.Layer.BorderWidth = view.BorderWidth;
            Control.ClipsToBounds = true;

            if (this.Control != null && this.Element != null && !string.IsNullOrEmpty(view.Image))
            {
                var downarrow = UIImage.FromBundle(view.Image);
                Control.RightViewMode = UITextFieldViewMode.Always;
                Control.RightView = new UIImageView(downarrow);
            }
        }
    }
}

