using Drastic.Texture;
using KYChat.iOS.Renderers;

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes;

public class IconButtonNode : BackgroundNode
{
    internal readonly int iconPadding;
    internal SvgViewNode svgNode;    
    
    public IconButtonNode(UIColor normalColor, UIColor pressedColor, string icon, int iconSize = 26, int iconPadding = 12) : base(normalColor, pressedColor)
    {
        this.BackgroundColor = normalColor;
        this.CornerRadius = (iconSize + (2 * iconPadding)) / 2;
        this.svgNode = new SvgViewNode(icon);
        this.svgNode.Style.PreferredSize = new CoreGraphics.CGSize(iconSize, iconSize);
        this.iconPadding = iconPadding;
    }

    public override ASLayoutSpec LayoutSpecThatFits(ASSizeRange constrainedSize)
    {        
        var inset = new ASInsetLayoutSpec();
        inset.Insets = new UIEdgeInsets(iconPadding, iconPadding, iconPadding, iconPadding);
        inset.Child = svgNode;

        return inset;
    }
}
