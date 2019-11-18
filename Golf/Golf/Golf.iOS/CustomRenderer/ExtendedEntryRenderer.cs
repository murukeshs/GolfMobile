using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using Golf.Controls;
using Golf.Enums;
using Golf.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace Golf.iOS.CustomRenderer
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntry ExtendedEntryElement => Element as ExtendedEntry;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    Control.BorderStyle = UIKit.UITextBorderStyle.None;
                }

                UpdateLineColor();
                UpdateCursorColor();

                var textField = this.Control;
                if (!string.IsNullOrEmpty(ExtendedEntryElement.Image))
                {
                    switch (ExtendedEntryElement.ImageAlignment)
                    {
                        case ImageAlignment.Left:
                            textField.LeftViewMode = UITextFieldViewMode.Always;
                            textField.LeftView = GetImageView(ExtendedEntryElement.Image, ExtendedEntryElement.ImageHeight, ExtendedEntryElement.ImageWidth);
                            break;
                        case ImageAlignment.Right:
                            textField.RightViewMode = UITextFieldViewMode.Always;
                            textField.RightView = GetImageView(ExtendedEntryElement.Image, ExtendedEntryElement.ImageHeight, ExtendedEntryElement.ImageWidth);
                            break;
                    }
                }

                //textField.BorderStyle = UITextBorderStyle.None;
                CALayer bottomBorder = new CALayer
                {
                    Frame = new CGRect(0.0f, ExtendedEntryElement.HeightRequest - 1, this.Frame.Width, 1.0f),
                    BorderWidth = 2.0f,
                    BorderColor = ExtendedEntryElement.LineColor.ToCGColor()
                };

                textField.Layer.AddSublayer(bottomBorder);
                textField.Layer.MasksToBounds = true;
            }


        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals(nameof(ExtendedEntry.LineColor)))
            {
                UpdateLineColor();
            }
            else if (e.PropertyName.Equals(Entry.TextColorProperty.PropertyName))
            {
                UpdateCursorColor();
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var lineLayer = GetOrAddLineLayer();
            lineLayer.Frame = new CGRect(0, Frame.Size.Height - LineLayer.LineHeight, Control.Frame.Size.Width, LineLayer.LineHeight);
        }

        void UpdateLineColor()
        {
            var lineLayer = GetOrAddLineLayer();
            lineLayer.BorderColor = ExtendedEntryElement.LineColor.ToCGColor();
        }

        LineLayer GetOrAddLineLayer()
        {
            var lineLayer = Control.Layer.Sublayers?.OfType<LineLayer>().FirstOrDefault();

            if (lineLayer == null)
            {
                lineLayer = new LineLayer();

                Control.Layer.AddSublayer(lineLayer);
                Control.Layer.MasksToBounds = true;
            }

            return lineLayer;
        }

        void UpdateCursorColor() => Control.TintColor = Element.TextColor.ToUIColor();

        class LineLayer : CALayer
        {
            public static nfloat LineHeight = 2f;

            public LineLayer() => BorderWidth = LineHeight;
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