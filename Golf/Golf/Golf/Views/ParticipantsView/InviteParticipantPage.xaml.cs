﻿using Rg.Plugins.Popup.Pages;
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
	public partial class InviteParticipantPage : PopupPage
    {
		public InviteParticipantPage ()
		{
			InitializeComponent ();
		}

        private void SendInviteButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}