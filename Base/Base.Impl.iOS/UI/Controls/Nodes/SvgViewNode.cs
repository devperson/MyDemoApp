using Drastic.Texture;

#if !SimulCompatibility
using SushiHangover.SVGKit;
#endif

namespace KYChat.iOS.Renderers
{
    //[SpecificLogger(AdvancedLogConstants.LogChatCell)]
    public class SvgViewNode : ASDisplayNode
    {
        private readonly string svgFile;
#if !SimulCompatibility
        private SVGKFastImageView svgImgView;        
#endif
        public SvgViewNode(string svgFile)
        {
            this.svgFile = svgFile;
        }

#if !SimulCompatibility
        public override void DidLoad()
        {
            base.DidLoad();

            var svgResource = new SVGKImage(this.svgFile);
            this.svgImgView = new SVGKFastImageView(svgResource);
            this.svgImgView.UserInteractionEnabled = false;
            this.View.UserInteractionEnabled = false;
            this.View.AddSubview(svgImgView);
        }

        public override void LayoutDidFinish()
        {
            base.LayoutDidFinish();
            
            this.svgImgView.Frame = new CGRect(CGPoint.Empty, this.CalculatedSize);
        }
#endif
    }
}