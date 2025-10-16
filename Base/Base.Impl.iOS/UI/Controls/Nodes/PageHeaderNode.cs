using Base.Impl.Texture.iOS.UI.Utils.Styles;
using Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
using Base.Impl.UI;
using Drastic.Texture;

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes
{
    public class PageHeaderNode : ASDisplayNode
    {
        public IconButtonNode leftBtnNode, rightBtnNode;
        private ASDisplayNode leftEmptySpacer, rightEmptySpacer;
        private ASTextNode titleNode;

        public PageHeaderNode(string title, string leftIcon = null, string rightIcon = null)
        {
            this.AutomaticallyManagesSubnodes = true;

            this.BackgroundColor = ColorConstants.HeaderBgColor.ToUIColor();            
            this.titleNode = TextStyles.Create_pageMediumTitleStyle(title);

            if (leftIcon != null)
            {
                this.leftBtnNode = ButtonStyles.CreateIconButton(leftIcon);

                if (rightIcon == null)
                {
                    var btnSize = this.leftBtnNode.svgNode.Style.PreferredSize.Width + this.leftBtnNode.iconPadding;
                    this.rightEmptySpacer = new ASDisplayNode();
                    this.rightEmptySpacer.Style.PreferredSize = new CGSize(btnSize, btnSize);
                }
            }

            if (rightIcon != null)
            {
                this.rightBtnNode = ButtonStyles.CreateIconButton(rightIcon);

                if (leftIcon == null)
                {
                    var btnSize = this.rightBtnNode.svgNode.Style.PreferredSize.Width + this.rightBtnNode.iconPadding;
                    this.leftEmptySpacer = new ASDisplayNode();
                    this.leftEmptySpacer.Style.PreferredSize = new CGSize(btnSize, btnSize);
                }
            }
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

            if (leftBtnNode != null && rightBtnNode != null)
            {
                headerStack.Children = [leftBtnNode, centerTitle, rightBtnNode];
            }
            else if(leftBtnNode != null && rightEmptySpacer != null)
            {
                headerStack.Children = [leftBtnNode, centerTitle, rightBtnNode];
            }
            else if (leftEmptySpacer != null && rightBtnNode != null)
            {
                headerStack.Children = [leftBtnNode, centerTitle, rightBtnNode];
            }
            else
            {
                headerStack.Children = [centerTitle];
            }

            var headerInset = new ASInsetLayoutSpec();
            headerInset.Child = headerStack;
            headerInset.Insets = new UIEdgeInsets(NumConstants.PageHeaderVPadding, NumConstants.PageHeaderHPadding, NumConstants.PageHeaderVPadding, NumConstants.PageHeaderHPadding);

            return headerInset;
        }
    }
}
