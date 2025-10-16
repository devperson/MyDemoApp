using Base.Impl.Texture.iOS.Pages;
using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using Base.MVVM.ViewModels;
using BestApp.ViewModels.Login;
using BestApp.ViewModels.Movies;
using Drastic.Texture;

namespace BestApp.X.iOS.Pages.Movies;

public class MoviesPage : iOSLifecyclePage
{
    public new MoviesPageViewModel ViewModel
    {
        get => base.ViewModel as MoviesPageViewModel;
        set => base.ViewModel = value;
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var page = new _Node(this);
        SetPageNode(page);
    }

    public class _Node : AsPageNode<MoviesPage>
    {
        private PageHeaderNode headerNode;
        private readonly UIRefreshControl refreshControl;
        internal ASTableNode tableNode;
        private MoviesDataSource moviesDataSource;

        public _Node(MoviesPage page) : base(page)
        {
            this.headerNode = new PageHeaderNode("Movies", "SvgImages/threeline.svg", "SvgImages/plus.svg");
            this.headerNode.leftBtnNode.TouchUp += LeftBtnNode_TouchUp;
            this.headerNode.rightBtnNode.TouchUp += RightBtnNode_TouchUp;

            this.moviesDataSource = new MoviesDataSource(this);
            this.tableNode = new ASTableNode();
            this.tableNode.Style.FlexGrow = 1.0f;  // allow table to stretch
            this.tableNode.DataSource = moviesDataSource;
            this.tableNode.Delegate = new MoviesDelegate();
            this.tableNode.View.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;

            // Create refresh control
            refreshControl = new UIRefreshControl();
            refreshControl.ValueChanged += OnRefresh;

            // Attach refresh control to ASTableNode's UITableView
            tableNode.View.RefreshControl = refreshControl;
        }

        public override ASLayoutSpec LayoutSpecThatFits(ASSizeRange constrainedSize)
        {
            var verticalStack = new ASStackLayoutSpec
            {
                Direction = ASStackLayoutDirection.Vertical,
                AlignItems = ASStackLayoutAlignItems.Stretch,
                JustifyContent = ASStackLayoutJustifyContent.Start,
                Children = [this.headerNode, this.tableNode]
            };

            var inset = new ASInsetLayoutSpec();
            inset.Insets = this.View.SafeAreaInsets;
            inset.Child = verticalStack;

            return inset;
        }

        public override void OnViewModelPropertyChanged(string propertyName)
        {
            base.OnViewModelPropertyChanged(propertyName);

            if (propertyName == nameof(this.Page.ViewModel.MovieItems))
            {
                this.moviesDataSource.OnCollectionSet();
            }
            else if (propertyName == nameof(this.Page.ViewModel.IsRefreshing))
            {
                if (this.Page.ViewModel.IsRefreshing == false)
                    this.refreshControl.EndRefreshing();
            }
        }

        private void RightBtnNode_TouchUp(object sender, EventArgs e)
        {
            Page.ViewModel.AddCommand.Execute();
        }

        private void LeftBtnNode_TouchUp(object sender, EventArgs e)
        {
            AppDelegate.Instance.flyoutController.OpenLeft();
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            Page.ViewModel.RefreshCommand.Execute();
        }
    }
}


