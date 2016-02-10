using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xatropix.Controls;
using Xatropix.iOS.Controls;

[assembly: ExportRenderer(typeof(InifiniteCarousel), typeof(InifiniteCarouselRenderer))]
namespace Xatropix.iOS.Controls
{
    public class InifiniteCarouselRenderer : ViewRenderer<InifiniteCarousel, UIScrollView>
    {
        private Timer checkForTime;
        private int width;
        private int height;
        private int currentIndex = 1;

        protected override void OnElementChanged(ElementChangedEventArgs<InifiniteCarousel> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                height = (int)e.NewElement.HeightRequest;
                SetNativeControl(new UIScrollView(new CGRect(0, 0, 200, 200)));
                width = (int)UIScreen.MainScreen.Bounds.Width;
                height = (int)UIScreen.MainScreen.Bounds.Height - (int)UIScreen.MainScreen.ApplicationFrame.Location.Y - 44;// 44 is height of navbar

                Control.Scrolled += ControlOnScrolled;

                Control.DecelerationEnded += ControlOnDecelerationEnded;
                Control.DelaysContentTouches = false;
                Control.ScrollAnimationEnded += ControlOnScrollAnimationEnded;

                Control.ContentOffset = new CGPoint(0, height);
                Control.ShowsHorizontalScrollIndicator = false;
                Control.PagingEnabled = true;
            }
        }

        private void ControlOnDecelerationEnded(object sender, EventArgs eventArgs)
        {
            checkForTime.Stop();
        }

        private void ControlOnScrollAnimationEnded(object sender, EventArgs eventArgs)
        {
            if (Element != null)
            {
                if (currentIndex == Element.ItemsSource.Count * 2)
                {
                    Control.SetContentOffset(new CGPoint((Element.ItemsSource.Count - 1) * width, 0), false);
                }
            }
        }

        void checkForTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                if (Control != null)// at navigation from moment, we have null, but stack with actions in timer isn't null
                {
                    Control.SetContentOffset(new CGPoint(currentIndex * width, 0), true);
                }
            });
        }

        private void ControlOnScrolled(object sender, EventArgs eventArgs)
        {
            if (Control != null)
            {
                var centerForRightSwipe = Control.ContentOffset.X + 10;
                var centerForLeftSwipe = Control.ContentOffset.X - (Control.Bounds.Width + 10);

                if (currentIndex > ((int)centerForLeftSwipe) / ((int)Control.Bounds.Width) + 1)
                {
                    currentIndex = ((int)centerForLeftSwipe) / ((int)Control.Bounds.Width) + 1;
                    int index = ((int)centerForLeftSwipe) / ((int)Control.Bounds.Width) + 1;
                    if (index == 0)
                    {
                        Control.SetContentOffset(new CGPoint((Element.ItemsSource.Count) * width, 0), false);
                    }
                }

                if (currentIndex < ((int)centerForRightSwipe) / ((int)Control.Bounds.Width) + 1)
                {
                    currentIndex = ((int)centerForRightSwipe) / ((int)Control.Bounds.Width) + 1;
                    if (currentIndex == Element.ItemsSource.Count * 2)
                    {
                        Control.SetContentOffset(new CGPoint((Element.ItemsSource.Count - 1) * width, 0), false);
                    }
                }

                int viewIndex = currentIndex % Element.ItemsSource.Count;
                viewIndex = viewIndex == 0 ? Element.ItemsSource.Count : viewIndex;
                Element.SelectedIndex = viewIndex;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == InifiniteCarousel.ItemsSourceProperty.PropertyName)
            {
                var urls = Element.ItemsSource.ToList();
                Control.ContentSize = new CGSize(width * urls.Count * 2, height);
                Control.SetContentOffset(new CGPoint((Element.ItemsSource.Count) * width, 0), false);

                for (int index = 0; index < urls.Count; index++)
                {
                    string url = urls[index];
                    UIImageView imageView = new UIImageView(new CGRect(width * index, 0, width, height));
                    imageView.ContentMode = UIViewContentMode.ScaleAspectFill;
                    imageView.ClipsToBounds = true;
                    imageView.Image = UIImage.FromFile(url);
                    Control.AddSubview(imageView);
                }
                for (int index = 0; index < urls.Count; index++)
                {
                    string url = urls[index];
                    UIImageView imageView = new UIImageView(new CGRect(width * (index + Element.ItemsSource.Count), 0, width, height));
                    imageView.ContentMode = UIViewContentMode.ScaleAspectFill;
                    imageView.ClipsToBounds = true;
                    imageView.Image = UIImage.FromFile(url);
                    Control.AddSubview(imageView);
                }
 
                SetTimer();
            }
        }

        private void SetTimer()
        {
            double interval = 3 * 1000; // 3 seconds 
            checkForTime = new Timer(interval);
            checkForTime.Elapsed += checkForTime_Elapsed;
            checkForTime.Enabled = true;
        }
    }
}
