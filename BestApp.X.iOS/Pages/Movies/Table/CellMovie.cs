using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using Base.Impl.Texture.iOS.UI.Utils.Styles;
using BestApp.ViewModels.Movies;
using BestApp.ViewModels.Movies.ItemViewModel;
using BestApp.X.iOS.Utils;
using Drastic.Texture;
using KYChat.iOS.Renderers.ASChatListView.Utils.CustomNodes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BestApp.X.iOS.Pages.Movies.Table;

public class CellMovie : ASCellNode
{
    private BackgroundNode backgroundNode;
    private readonly UIImageViewNode imgView;
    private readonly ASTextNode txtName;
    private readonly ASTextNode txtDescription;
    private readonly MovieItemViewModel model;

    public CellMovie(MovieItemViewModel model)
    {
        this.model = model;

        // Automatically manage subnodes
        AutomaticallyManagesSubnodes = true;

        backgroundNode = new BackgroundNode(UIColor.White, UIColorConstants.Gray100);
        backgroundNode.TouchUp += BackgroundNode_TouchUp;

        // Image Node
        imgView = new UIImageViewNode();
        imgView.Url = model.PosterUrl;
        imgView.Style.PreferredLayoutSize = new ASLayoutSize
        {
            width = new ASDimension { value = 100, unit = ASDimensionUnit.Points },
            height = new ASDimension { value = 120, unit = ASDimensionUnit.Points },
        };

        // Name Text Node
        txtName = new ASTextNode();        
        var nameAttributes = new UIStringAttributes
        {
            Font = UIFont.FromName("Sen-Bold", 14),
            ForegroundColor = UIColorConstants.LabelColor
        };
        txtName.AttributedText = new NSAttributedString(model.Name, nameAttributes);

        // Description Text Node
        txtDescription = new ASTextNode();        
        var descAttributes = new UIStringAttributes
        {
            Font = UIFont.FromName("Sen-Regular", 14),
            ForegroundColor = UIColorConstants.LabelColor
        };
        txtDescription.AttributedText = new NSAttributedString(model.Overview, descAttributes);      
    }

    public override ASLayoutSpec LayoutSpecThatFits(ASSizeRange constrainedSize)
    {
        // Vertical stack for name and description
        var textStack = new ASStackLayoutSpec();
        textStack.Direction = ASStackLayoutDirection.Vertical;
        textStack.Spacing = 5;        
        textStack.Children = [ txtName, txtDescription ];       
        textStack.Style.FlexShrink = 1;

        // Horizontal stack: image + text
        var horizontalStack = new ASStackLayoutSpec();
        horizontalStack.Direction = ASStackLayoutDirection.Horizontal;
        horizontalStack.Spacing = 5;       
        horizontalStack.Children = [ imgView, textStack ];

        var insetSpec = new ASInsetLayoutSpec();
        insetSpec.Insets = new UIEdgeInsets(15, 15, 15, 20);
        insetSpec.Child = horizontalStack;

        var overlay = new ASBackgroundLayoutSpec();
        overlay.Background = backgroundNode;
        overlay.Child = insetSpec;        

        return overlay;
    }

    private void BackgroundNode_TouchUp(object sender, EventArgs e)
    {
        var vm = AppDelegate.Instance.pageNavigationService.GetCurrentPageModel() as MoviesPageViewModel;
        if (vm != null)
        {
            vm.ItemTappedCommand.Execute(this.model);
        }
    }
}
