using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Services
{
    public interface IAdjustScreenSize
    {
        //to avoid screen overlapping Issue In ANdroid this this interface is used this is only for android specific 
        //for ios we use plugin to avoid keyboadoverlapping issue
        void AdjustScreen(); //when using this screen is automatically resized when user click the keyboard
        void UnAdjustScreen();//when using this screen is fixed or static.
    }
}
