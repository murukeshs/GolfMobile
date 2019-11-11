using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Golf.Controls
{
    public class UnderlineLabel : Label
    {
        public UnderlineLabel()
        {
            TextDecorations = TextDecorations.Underline;
            TextColor = Color.White;
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
        }
    }
}
