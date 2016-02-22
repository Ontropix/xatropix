using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;

namespace Xatropix.Droid.Controls
{
    public class SimpleCarouselAdapter : PagerAdapter
    {
        public List<string> Urls = new List<string>();
        private ImageView imageView;
        private ImageLoader imageLoader;

        public SimpleCarouselAdapter(List<string> urls, Context context)
        {
            Urls = urls;

            DisplayImageOptions defaultOptions = new DisplayImageOptions.Builder()
                .CacheOnDisc(true)
                .ImageScaleType(ImageScaleType.Exactly)
                .BitmapConfig(Bitmap.Config.Rgb565)
                .CacheInMemory(false)
                .Build();

            ImageLoaderConfiguration config = new ImageLoaderConfiguration.Builder(
                context)
                .WriteDebugLogs()
                .ThreadPoolSize(1)
                .DiskCacheExtraOptions(480, 320, null)
                .DefaultDisplayImageOptions(defaultOptions)
                //.DenyCacheImageMultipleSizesInMemory()
                .Build();
            ImageLoader.Instance.Init(config);
            imageLoader = ImageLoader.Instance;
        }

        public override Object InstantiateItem(ViewGroup container, int position)
        {
            imageView = new ImageView(container.Context);
            imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
            imageLoader.DisplayImage(Urls[position], imageView);
            container.AddView(imageView);

            return imageView;
        }

        public override bool IsViewFromObject(View view, Object objectValue)
        {
            return view == objectValue;
        }

        public override void DestroyItem(ViewGroup container, int position, Object objectValue)
        {
            GC.Collect(); // feel my pain bro
            imageLoader.ClearMemoryCache();
            container.RemoveView(objectValue as View);
        }

        public override int Count
        {
            get { return Urls.Count; }
        }

        public override float GetPageWidth(int position)
        {
            return 1f;
        }
    }
}