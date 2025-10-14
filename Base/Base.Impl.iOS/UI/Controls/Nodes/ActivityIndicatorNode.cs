using Drastic.Texture;

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes
{
    //[SpecificLogger(AdvancedLogConstants.LogChatCell)]
    public class ActivityIndicatorNode : ASDisplayNode
    {
        private UIColor color;
        private UIActivityIndicatorView activityIndicatorView;

        public ActivityIndicatorNode(UIColor color)
        {
            this.color = color;
        }

        public override void DidLoad()
        {
            base.DidLoad();

            this.activityIndicatorView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White);
            this.activityIndicatorView.Color = color;
            this.View.AddSubview(this.activityIndicatorView);

            this.activityIndicatorView.StartAnimating();
        }

        public override void LayoutDidFinish()
        {
            base.LayoutDidFinish();

            this.activityIndicatorView.Frame = new CGRect(CGPoint.Empty, this.CalculatedSize);
        }
    }
}