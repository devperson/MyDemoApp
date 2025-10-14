using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using Base.Impl.Texture.iOS.UI.Utils.Styles;
using Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
using Drastic.Texture;
using KYChat.iOS.Renderers;

#if !SimulCompatibility
using SushiHangover.SVGKit;
#endif

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes;
public class IconTextButtonNode : BackgroundNode
{
    SvgViewNode svgNode;
    ASTextNode txtNode;
    int spacing;
    private int hPadding;
    private ASStackLayoutJustifyContent hAligment;
    private int height = 60;

    public IconTextButtonNode(UIColor normalColor) : base(normalColor)
    {
        
    }

    public IconTextButtonNode(UIColor normalColor, UIColor pressedColor) : base(normalColor, pressedColor)
    {
        
    }

    public IconTextButtonNode(UIColor normalColor, UIColor pressedColor, UIColor selectedColor, bool isSelected) : base(normalColor, pressedColor, selectedColor, isSelected)
    {
        
    }

    public bool IsLayoutVertical { get; set; }

    public void SetIconText(string icon, string text, int spacing = 8, int iconSize = 34, UIColor? textColor = null, UIFont textFont = null, int hPadding = 15, ASStackLayoutJustifyContent hAligment = ASStackLayoutJustifyContent.Center, int? corner = null)
    {
        if (textColor == null)
        {
            textColor = ColorConstants.DefaultTextColor.ToUIColor();
        }

        if (textFont == null)
        {
            textFont = UIFont.FromName("Sen-Bold", 20);
        }

        if (corner == null)
        {
            this.CornerRadius = height / 2;
        }
        else
        {
            this.CornerRadius = 0;
        }

        this.spacing = spacing;
        this.hPadding = hPadding;
        this.hAligment = hAligment;

        this.svgNode = new SvgViewNode(icon);
        this.svgNode.Style.PreferredSize = new CoreGraphics.CGSize(iconSize, iconSize);

        var txtAttr = new UIStringAttributes();
        txtAttr.Font = textFont;
        txtAttr.ForegroundColor = textColor;
        this.txtNode = new ASTextNode();
        this.txtNode.AttributedText = new Foundation.NSAttributedString(text, txtAttr);
    }

    public override ASLayoutSpec LayoutSpecThatFits(ASSizeRange constrainedSize)
    {
        var stack = new ASStackLayoutSpec();
        stack.Direction = ASStackLayoutDirection.Horizontal;        
        stack.Spacing = this.spacing;
        stack.Style.Height = new ASDimension { unit = ASDimensionUnit.Points, value = height };
        stack.Children = [svgNode, txtNode];
        stack.AlignItems = ASStackLayoutAlignItems.Center;
        stack.JustifyContent = this.hAligment;

        var inset = new ASInsetLayoutSpec();
        inset.Insets = new UIEdgeInsets(0, hPadding, 0, hPadding);
        inset.Child = stack;

        return inset;
    }
}
