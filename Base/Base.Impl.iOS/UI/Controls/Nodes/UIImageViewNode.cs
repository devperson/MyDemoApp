using Drastic.Texture;
#if !SimulCompatibility
using Drastic.SDWebImage;
#endif


namespace KYChat.iOS.Renderers.ASChatListView.Utils.CustomNodes
{    
    public class UIImageViewNode : ASDisplayNode
    {
        private UIImageView imgView;
        public int CornerR { get; set; } = 12;
        public string Url { get; set; }        
        public string PlaceHolderImg { get; set; }
        public string FilePath { get; set; }        
        public bool IsPortrate { get; set; }        

        public override async void DidLoad()
        {
            base.DidLoad();

            imgView = new UIImageView();
            imgView.ContentMode = UIViewContentMode.ScaleAspectFill;
            imgView.Layer.CornerRadius = CornerR;
            imgView.Layer.MasksToBounds = true;
            imgView.Layer.ShouldRasterize = false;

            if (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
            {
                imgView.Image = UIImage.FromFile(FilePath);
            }
            else if (!string.IsNullOrEmpty(Url))
            {

#if !SimulCompatibility
                //use SDWebImage to download, cache and display
                imgView.SetImage(new NSUrl(Url));  
#else
                //manually download image with httpclient and display
                await this.SetImageFromUrlAsync(imgView, Url);
#endif
            }
            
            this.View.AddSubview(imgView);
        }

        public override void LayoutDidFinish()
        {
            base.LayoutDidFinish();

            this.imgView.Frame = new CGRect(CGPoint.Empty, this.CalculatedSize);
        }

        public void UseForChatCell(bool isPortrateImage)
        {            
            if (isPortrateImage)
            {
                this.PlaceHolderImg = "imgplaceholderport.jpg";
            }
            else
            {
                this.PlaceHolderImg = "imgplaceholderlans.jpg";
            }
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