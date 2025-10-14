using Base.Abstractions.UI;
using Base.Impl.Texture.iOS.UI.Utils.XF;
using Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
using Base.Impl.UI;
using Drastic.Texture;

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes;

public class SnackbarNode : ASDisplayNode
{
    private ButtonNode btnNode;
    private ASTextNode txtNode;
    private RectangleNode rectangleNode;
    public bool IsOpen { get; set; }

    public SnackbarNode()
    {
        this.AutomaticallyManagesSubnodes = true;
        this.Hidden = true;

        this.txtNode = new ASTextNode();
        this.txtNode.MaximumNumberOfLines = 5;
        this.txtNode.Style.FlexGrow = 1;
        this.txtNode.Style.FlexShrink = 1;

        this.btnNode = new ButtonNode(UIColor.White, UIColor.White);
        this.btnNode.Style.PreferredSize = new CGSize(80, 50);
        this.btnNode.Style.FlexShrink = 1;
        this.btnNode.Style.FlexGrow = 0;
        this.btnNode.TouchUp += BtnNode_TouchUp;

        this.rectangleNode = new RectangleNode(16);
        this.rectangleNode.TouchDown += RectangleNode_TouchDown;
    }
    
    public void SetText(string text, SeverityType severty, string fontFamily = null, string fontFamily2 = null)
    {
        this.rectangleNode.BackgroundColor = severty.GetBackgroundColor().ToUIColor();

        var textAttr = new UIStringAttributes();

        if (!string.IsNullOrEmpty(fontFamily))
        {
            textAttr.Font = UIFont.FromName(fontFamily, 15);
        }
        textAttr.ForegroundColor = severty.GetTextColor().ToUIColor();
        this.txtNode.AttributedText = new NSAttributedString(text, textAttr);

        textAttr = new UIStringAttributes();
        if (!string.IsNullOrEmpty(fontFamily2))
        {
            textAttr.Font = UIFont.FromName(fontFamily2, 16);
        }
        textAttr.ForegroundColor = severty.GetTextColor().ToUIColor();
        this.btnNode.BackgroundColor = this.btnNode.normalColor = severty.GetBackgroundColor().ToUIColor();
        this.btnNode.pressedColor = this.btnNode.normalColor.MakeDarker(0.1f).ColorWithAlpha(0.2f);
        this.btnNode.SetAttributedTitle(new NSAttributedString("Close", textAttr), UIControlState.Normal);        
    }

    public void Show()
    {
        if (IsOpen)
            return;

        this.IsOpen = true;
        this.Hidden = false;        
        this.View.Fade(true);
    }

    public void Close()
    {
        this.View.Fade(false, onFinished: () =>
        {
            this.Hidden = true;
        });
        this.IsOpen = false;
    }

    public override ASLayoutSpec LayoutSpecThatFits(ASSizeRange constrainedSize)
    {        
        var stack = new ASStackLayoutSpec();
        stack.Direction = ASStackLayoutDirection.Horizontal;
        stack.JustifyContent = ASStackLayoutJustifyContent.Center;
        stack.AlignItems = ASStackLayoutAlignItems.Center;
        stack.Spacing = 7;
        stack.Children = [txtNode, btnNode];

        var stackMargin = new ASInsetLayoutSpec();
        stackMargin.Insets = new UIEdgeInsets(20, 20, 20, 20);
        stackMargin.Child = stack;

        //var width = DeviceDisplay.MainDisplayInfo.Width - 100;
        var bgRect = new ASBackgroundLayoutSpec();
        bgRect.Background = rectangleNode;
        bgRect.Child = stackMargin;
        //bgRect.Style.MaxWidth = new ASDimension { unit = ASDimensionUnit.Points, value = (float)width };

        var rectInset = new ASInsetLayoutSpec();
        rectInset.Insets = new UIEdgeInsets(60, 25, nfloat.PositiveInfinity, 25);
        rectInset.Child = bgRect;

        var overlay = new BackgroundNode(UIColor.Black.ColorWithAlpha(0.6f));
        overlay.TouchDown += Overlay_TouchDown;

        var bg = new ASBackgroundLayoutSpec();
        bg.Background = overlay;
        bg.Child = rectInset;

        return bg;
    }

    private void Overlay_TouchDown(object sender, EventArgs e)
    {
        this.Close();
    }

    private void RectangleNode_TouchDown(object sender, EventArgs e)
    {
        this.Close();
    }

    private void BtnNode_TouchUp(object sender, EventArgs e)
    {
        this.Close();
    }
}
