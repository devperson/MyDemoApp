using Base.Impl.Texture.iOS.Pages;
using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using BestApp.ViewModels.Movies;
using Drastic.Texture;

namespace BestApp.X.iOS.Pages.Movies;

public class MoviesPage : iOSLifecyclePage
{
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var loginNode = new MoviesPageNode(this);
        SetPageNode(loginNode);
    }
}

public class MoviesPageNode : BasePageNode
{
    private PageHeaderNode headerNode;
    private ASTableNode tableNode;

    public MoviesPageNode(iOSLifecyclePage page) : base(page)
    {
        this.headerNode = new PageHeaderNode("Movies", "SvgImages/threeline.svg", "SvgImages/plus.svg");
        this.headerNode.leftBtnNode.TouchUp += LeftBtnNode_TouchUp;
        this.headerNode.rightBtnNode.TouchUp += RightBtnNode_TouchUp;
        this.tableNode = new ASTableNode();
        this.tableNode.Style.FlexGrow = 1.0f;  // allow table to stretch
        this.tableNode.DataSource = new MoviesDataSource();
        this.tableNode.Delegate = new MoviesDelegate();
        this.tableNode.View.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
    }

    private void RightBtnNode_TouchUp(object sender, EventArgs e)
    {
        (this.ViewModel as MoviesPageViewModel).AddCommand.Execute();
    }

    private void LeftBtnNode_TouchUp(object sender, EventArgs e)
    {
        AppDelegate.Instance.flyoutController.OpenLeft();
    }

    public override ASLayoutSpec LayoutSpecThatFits(ASSizeRange constrainedSize)
    {
        var verticalStack = new ASStackLayoutSpec
        {
            Direction = ASStackLayoutDirection.Vertical,
            AlignItems = ASStackLayoutAlignItems.Stretch,
            JustifyContent = ASStackLayoutJustifyContent.Start,
            Children = [ this.headerNode, this.tableNode ]
        };

        return verticalStack;
    }
}
