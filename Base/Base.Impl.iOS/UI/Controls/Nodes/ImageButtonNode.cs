#if !SimulCompatibility
using Drastic.SDWebImage;
#endif
using Drastic.Texture;

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes
{
    //[SpecificLogger(AdvancedLogConstants.LogChatCell)]
    public class ImageButtonNode : ASControlNode
    {
        public event EventHandler TouchUp;
        public event EventHandler TouchDown;
        public string Url { get; set; }
        public int CornerR { get; set; }
        UIImageView imgView;

        public ImageButtonNode(string url, int cornerRadius) //: base(new ASDKImageCacheImpl(), new ASDKImageDownloaderImpl())
        {
            this.Url = url;
            this.CornerR = cornerRadius;

            //if (cornerRadius > 0)
            //{
            //    this.CornerRadius = cornerRadius;
            //    this.CornerRoundingType = ASCornerRoundingType.Precomposited;
            //    this.ClipsToBounds = true;
            //}
        }

        public override async void DidLoad()
        {
            base.DidLoad();

            this.AddTarget(this, new ObjCRuntime.Selector("OnTouchDown:"), ASControlNodeEvent.TouchDown);
            this.AddTarget(this, new ObjCRuntime.Selector("OnTouchUpInside:"), ASControlNodeEvent.TouchUpInside);
            this.AddTarget(this, new ObjCRuntime.Selector("OnTouchCancel:"), ASControlNodeEvent.TouchUpOutside);
            this.AddTarget(this, new ObjCRuntime.Selector("OnTouchCancel:"), ASControlNodeEvent.TouchCancel);


            imgView = new UIImageView();
            imgView.ContentMode = UIViewContentMode.ScaleAspectFill;
            imgView.Layer.CornerRadius = CornerR;
            imgView.Layer.MasksToBounds = true;
            imgView.Layer.ShouldRasterize = false;

#if !SimulCompatibility
                //use SDWebImage to download, cache and display
                imgView.SetImage(new NSUrl(Url));  
#else
            //manually download image with httpclient and display
            await this.SetImageFromUrlAsync(imgView, Url);
#endif

            this.View.AddSubview(imgView);
        }

        public override void LayoutDidFinish()
        {
            base.LayoutDidFinish();

            this.imgView.Frame = new CGRect(CGPoint.Empty, this.CalculatedSize);
        }

        [Export("OnTouchDown:")]
        public virtual void OnTouchDown(NSObject sender)
        {
            if (this.TouchDown != null)
            {
                this.TouchDown.Invoke(this, EventArgs.Empty);
            }
        }

        [Export("OnTouchUpInside:")]
        public virtual void OnTouchUpInside(NSObject sender)
        {
            if (this.TouchUp != null)
            {
                this.TouchUp.Invoke(this, EventArgs.Empty);
            }
        }

        [Export("OnTouchCancel:")]
        public virtual void OnTouchCancel(NSObject sender)
        {

        }

#if SimulCompatibility
        private async Task SetImageFromUrlAsync(UIImageView imageView, string url)
        {
            using (var httpClient = new HttpClient())
            {
                var data = await httpClient.GetByteArrayAsync(url);
                var image = UIImage.LoadFromData(NSData.FromArray(data));
                InvokeOnMainThread(() =>
                {
                    imageView.Image = image;
                });
            }
        }
#endif
    }
}