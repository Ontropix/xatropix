using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xatropix.Controls
{
    public class BaseCarousel : ScrollView
    {
        public BaseCarousel()
        {
            Orientation = ScrollOrientation.Horizontal;
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<BaseCarousel, ObservableCollection<string>>(p => p.ItemsSource, null, BindingMode.TwoWay);

        public ObservableCollection<string> ItemsSource
        {
            get { return (ObservableCollection<string>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create<BaseCarousel, int>(p => p.SelectedIndex, 1, BindingMode.TwoWay);

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
    }
}
