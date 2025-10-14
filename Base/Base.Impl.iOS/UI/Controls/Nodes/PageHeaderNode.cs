using Base.Impl.Texture.iOS.UI.Utils.Styles;
using Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
using Drastic.Texture;

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes
{
    public class PageHeaderNode : ASDisplayNode
    {
        private IconButtonNode btnBackNode;
        private ASTextNode titleNode;
        private ASDisplayNode emptyRightButton;

        public PageHeaderNode(string title)
        {
            this.AutomaticallyManagesSubnodes = true;

            this.titleNode = TextStyles.Create_pageMediumTitleStyle(title, ColorConstants.DefaultTextColor.ToUIColor());
            this.btnBackNode = ButtonStyles.CreateIconButton("SvgImages/backarrowblack.svg", ColorConstants.DefaultTextColor.ToUIColor());

            var btnSize = this.btnBackNode.svgNode.Style.PreferredSize.Width + this.btnBackNode.iconPadding;
            this.emptyRightButton = new ASDisplayNode();
            this.emptyRightButton.Style.PreferredSize = new CoreGraphics.CGSize(btnSize, btnSize);
        }

        public override ASLayoutSpec LayoutSpecThatFits(ASSizeRange constrainedSize)
        {
            var centerTitle = new ASCenterLayoutSpec();
            centerTitle.Child = titleNode;
            centerTitle.CenteringOptions = ASCenterLayoutSpecCenteringOptions.Xy;
            centerTitle.Style.FlexGrow = 1;

            var headerStack = new ASStackLayoutSpec();
            headerStack.Direction = ASStackLayoutDirection.Horizontal;
            headerStack.Spacing = 0;
            headerStack.AlignItems = ASStackLayoutAlignItems.Center;
            headerStack.Children = [btnBackNode, centerTitle, emptyRightButton];

            var headerInset = new ASInsetLayoutSpec();
            headerInset.Child = headerStack;
            headerInset.Insets = new UIEdgeInsets(0, 15, 0, 15);

            return headerInset;
        }
    }
}
