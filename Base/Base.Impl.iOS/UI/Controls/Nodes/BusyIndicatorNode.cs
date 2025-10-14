using Base.Abstractions.AppService;
using Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
using Base.Impl.UI;
using Drastic.Texture;

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes;

public class BusyIndicatorNode : ASDisplayNode
{
    private ActivityIndicatorNode activityIndicatorNode;
    private ASTextNode txtNode;    
    private RectangleNode rectangleNode;
    public bool IsShowing { get; set; }

    public BusyIndicatorNode()
    {
        this.AutomaticallyManagesSubnodes = true;
        this.Hidden = true;
        this.activityIndicatorNode = new ActivityIndicatorNode(ColorConstants.PrimaryColor.ToUIColor());
        this.activityIndicatorNode.Style.PreferredSize = new CoreGraphics.CGSize(32, 32);
        this.txtNode = new ASTextNode();
        this.SetText("On it...");        

        rectangleNode = new RectangleNode(16);
        rectangleNode.BackgroundColor = ColorConstants.BgColor.ToUIColor();
    }

    public void SetText(string text)
    {
        var textAttr = new UIStringAttributes();
        textAttr.Font = UIFont.FromName("Sen-SemiBold", 15);
        textAttr.ForegroundColor = ColorConstants.Gray800.ToUIColor();
        this.txtNode.AttributedText = new NSAttributedString("On it...", textAttr);
    }

    public override ASLayoutSpec LayoutSpecThatFits(ASSizeRange constrainedSize)
    {       
        var stack = new ASStackLayoutSpec();
        stack.Direction = ASStackLayoutDirection.Vertical;
        stack.JustifyContent = ASStackLayoutJustifyContent.Center;
        stack.AlignItems = ASStackLayoutAlignItems.Center;
        stack.Spacing = 7;
        stack.Children = [activityIndicatorNode, txtNode];

        var stackMargin = new ASInsetLayoutSpec();
        stackMargin.Insets = new UIEdgeInsets(20, 40, 20, 40);
        stackMargin.Child = stack;

        
        var bgRect = new ASBackgroundLayoutSpec();
        bgRect.Background = rectangleNode;
        bgRect.Child = stackMargin;

        var centerContainer = new ASCenterLayoutSpec();
        centerContainer.Child = bgRect;
        centerContainer.CenteringOptions = ASCenterLayoutSpecCenteringOptions.Xy;

        var overlay = new ASDisplayNode();
        overlay.BackgroundColor = UIColor.Black.ColorWithAlpha(0.6f);

        var bg = new ASBackgroundLayoutSpec();
        bg.Background = overlay;
        bg.Child = centerContainer;
        return bg;
    }

    public void Show()
    {
        if (IsShowing)
            return;

        this.IsShowing = true;
        this.Hidden = false;
        this.View.Fade(true);
    }

    public void Close()
    {
        this.View.Fade(false, onFinished: () =>
        {
            this.Hidden = true;
        });
        this.IsShowing = false;
    }
}
