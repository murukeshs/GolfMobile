using Golf.Utils;
using UIKit;

namespace Golf.iOS.Utils
{
    public class KeyboardUtiliOS : IKeyboardUtil
    {
        public void HideKeyboard()
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }
    }
}