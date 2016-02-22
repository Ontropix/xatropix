using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xatropix.Controls;
using Xatropix.Droid.Controls;

[assembly: ExportRenderer(typeof(SimpleCarousel), typeof(SimpleCarouselRenderer))]

namespace Xatropix.Droid.Controls
{
    class SimpleCarouselRenderer : ViewRenderer<SimpleCarousel, ViewPager>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SimpleCarousel> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                SetNativeControl(new ViewPager(Forms.Context));
                Control.PageScrolled += ControlOnPageScrolled;
            }
        }

        private void ControlOnPageScrolled(object sender, ViewPager.PageScrolledEventArgs e)
        {
            Element.SelectedIndex = e.Position + 1;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == SimpleCarousel.ItemsSourceProperty.PropertyName)
            {
                var list = Element.ItemsSource.ToList();
                Control.Adapter = new SimpleCarouselAdapter(list, Forms.Context);
            }
        }
    }
}