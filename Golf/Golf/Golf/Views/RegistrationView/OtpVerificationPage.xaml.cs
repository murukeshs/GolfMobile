using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OtpVerificationPage : ContentPage
	{
		public OtpVerificationPage ()
		{
			InitializeComponent ();
		}

        private void OtpVerifyButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}