using Base.Impl.Texture.iOS.UI.Utils.Styles;
using BestApp.X.iOS.Utils;
using Drastic.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.iOS.Pages.Movies.Table;

public class CellMovie : ASCellNode
{
    private ASTextNode txtNode;
    public CellMovie()
    {
        this.AutomaticallyManagesSubnodes = true;

        BackgroundColor = UIColor.White;

        var style = TextStyles.Create_boldMediumStyle();
        this.txtNode = new ASTextNode();
        this.txtNode.AttributedText = new NSAttributedString("Hello", style); 
    }

    public override ASLayoutSpec LayoutSpecThatFits(ASSizeRange constrainedSize)
    {
        // Center the text both horizontally and vertically
        var centerSpec = new ASCenterLayoutSpec
        {
            CenteringOptions = ASCenterLayoutSpecCenteringOptions.Xy,
            Child = this.txtNode
        };

        // Add padding around text
        var insetSpec = new ASInsetLayoutSpec
        {
            Insets = new UIEdgeInsets(8, 16, 8, 16),
            Child = centerSpec
        };

        return insetSpec;
    }
}
