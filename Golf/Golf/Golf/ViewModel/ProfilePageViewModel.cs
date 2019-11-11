using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public ImageSource ProfileImage
        {
            get { return _ProfileImage; }
            set
            {
                _ProfileImage = value;
                OnPropertyChanged(nameof(ProfileImage));
            }
        }
        private ImageSource _ProfileImage = "profile_defalut_pic.png";
    }
}
