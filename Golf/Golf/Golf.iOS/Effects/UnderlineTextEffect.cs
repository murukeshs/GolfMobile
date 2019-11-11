using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Golf.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(UnderlineTextEffect), "UnderlineTextEffect")]
namespace Golf.iOS.Effects
{
    public class UnderlineTextEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var element = Element as Label;

            if (Control is UILabel label && element != null)
            {
                var attributes = new UIStringAttributes { UnderlineStyle = NSUnderlineStyle.Single };
                var attrString = new NSAttributedString(element.Text, attributes);
                label.AttributedText = attrString;
            }
        }

        protected override void OnDetached()
        {
        }
    }
}