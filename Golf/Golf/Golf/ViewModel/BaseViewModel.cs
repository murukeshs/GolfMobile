using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        public BaseViewModel()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName]string propertyName = "", Action onChanged = null)
        {
            bool isNewValueDifferent = false;
            try
            {
                if (!EqualityComparer<T>.Default.Equals(backingStore, value))
                {
                    backingStore = value;
                    onChanged?.Invoke();
                    OnPropertyChanged(propertyName);
                    isNewValueDifferent = true;
                }
            }
            catch (Exception ex)
            {
                //ExceptionHandlerService.HandleException(ex);
            }
            return isNewValueDifferent;
        }

        #region INotifyPropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                //ExceptionHandlerService.HandleException(ex);
            }
        }
        #endregion
    }
}
