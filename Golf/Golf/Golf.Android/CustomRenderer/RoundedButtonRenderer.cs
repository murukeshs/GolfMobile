using Android.Content;
using Golf.Controls;
using Golf.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedButton), typeof(RoundedButtonRenderer))]
namespace Golf.Droid.CustomRenderer
{
    public class RoundedButtonRenderer : ButtonRenderer
    {
        public RoundedButtonRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var control = Control as Android.Widget.Button;
                control.SetAllCaps(false);
            }
        }
    }
}