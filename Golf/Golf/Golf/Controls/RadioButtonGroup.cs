using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Golf.Controls
{
    public class RadioButtonGroup : StackLayout
    {
        public List<CustomRadioButton> rads;

        public RadioButtonGroup()
        {

            rads = new List<CustomRadioButton>();
        }



        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create<RadioButtonGroup, IEnumerable>(o => o.ItemsSource, default(IEnumerable), propertyChanged: OnItemsSourceChanged);


        public static BindableProperty SelectedIndexProperty =
            BindableProperty.Create<RadioButtonGroup, int>(o => o.SelectedIndex, default(int), BindingMode.TwoWay, propertyChanged: OnSelectedIndexChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }


        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public event EventHandler<int> CheckedChanged;



        private static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
        {
            var radButtons = bindable as RadioButtonGroup;

            radButtons.rads.Clear();
            radButtons.Children.Clear();
            if (newvalue != null)
            {

                int radIndex = 0;
                foreach (var item in newvalue)
                {
                    var rad = new CustomRadioButton();
                    rad.Text = item.ToString();
                    rad.Id = radIndex;

                    rad.CheckedChanged += radButtons.OnCheckedChanged;

                    radButtons.rads.Add(rad);

                    radButtons.Children.Add(rad);
                    radIndex++;
                }
            }
        }

        private void OnCheckedChanged(object sender, EventArg<bool> e)
        {

            //if (e.Value == false) return;

            var selectedRad = sender as CustomRadioButton;

            foreach (var rad in rads)
            {
                if (!selectedRad.Id.Equals(rad.Id))
                {
                    rad.Checked = false;
                }
                else
                {
                    if (CheckedChanged != null)
                        CheckedChanged.Invoke(sender, rad.Id);

                }

            }

        }

        private static void OnSelectedIndexChanged(BindableObject bindable, int oldvalue, int newvalue)
        {
            if (newvalue == -1) return;

            var bindableRadioGroup = bindable as RadioButtonGroup;


            foreach (var rad in bindableRadioGroup.rads)
            {
                if (rad.Id == bindableRadioGroup.SelectedIndex)
                {
                    rad.Checked = true;
                }

            }


        }
    }
}
