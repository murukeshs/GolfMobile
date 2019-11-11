using System;
using Xamarin.Forms;

namespace Golf.Views.MenuView
{

    public class MenuPageMenuItem
    {
        public string Icon { get; set; }

        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }

        public bool IsUnderlined { get; set; }

        public Thickness Indentation { get; set; }

        public Color TextColor { get; set; }
    }
}