using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xatropix.Controls;
using Xatropix.iOS.Controls;

[assembly: ExportRenderer(typeof(SimpleCarousel), typeof(SimpleCarouselRenderer))]
namespace Xatropix.iOS.Controls
{
    public class SimpleCarouselRenderer : ViewRenderer<SimpleCarousel, UIScrollView>
    {
        private int height;
        private int width;
        protected override void OnElementChanged(ElementChangedEventArgs<SimpleCarousel> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                height = (int)e.NewElement.HeightRequest;
                SetNativeControl(new UIScrollView(new CGRect(0, 0, 200, 200)));
                width = (int)UIScreen.MainScreen.Bounds.Width;

                Control.Scrolled += ControlOnScrolled;
                Control.ContentOffset = new CGPoint(0, height);
                Control.ShowsHorizontalScrollIndicator = false;
                Control.PagingEnabled = true;
            }
        }

        private void ControlOnScrolled(object sender, EventArgs eventArgs)
        {
            if (Control != null)
            {
                var center = Control.ContentOffset.X + (Control.Bounds.Width / 2);
                Element.SelectedIndex = (int)center / (int)Control.Bounds.Width + 1;
            }

        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == SimpleCarousel.ItemsSourceProperty.PropertyName)
            {
                var list = Element.ItemsSource.ToList();
                Control.ContentSize = new CGSize(width * list.Count, height);
                for (int index = 0; index < list.Count; index++)
                {
                    string url = list[index];
                    UIImageView imageView = new UIImageView(new CGRect(width * index, 0, width, height));
                    imageView.ContentMode = UIViewContentMode.ScaleAspectFill;
                    imageView.ClipsToBounds = true;
                    Task.Factory.StartNew(() =>
                    {
                        InvokeOnMainThread(() =>
                        {
                            if (imageView != null)
                            {
                                imageView.Image = FromUrl(url);
                            }
                        });
                    });
                    Control.AddSubview(imageView);
                }
            }
        }

        public UIImage FromUrl(string uri)
        {
            using (var url = new NSUrl(uri))
            using (var data = NSData.FromUrl(url))
                return UIImage.LoadFromData(data);
        }
    }
}
