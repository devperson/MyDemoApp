using Base.Impl.Texture.iOS.Pages;
using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using BestApp.ViewModels.Movies;
using Drastic.Texture;

namespace BestApp.X.iOS.Pages.Movies;
public class MovieDetailPage : iOSLifecyclePage
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

    public class _Node : AsPageNode<MovieDetailPage>
    {
        private PageHeaderNode headerNode;                

        public _Node(MovieDetailPage page) : base(page)
        {
            this.headerNode = new PageHeaderNode("Movies", "SvgImages/backarrowblack.svg", "SvgImages/edit.svg");
            this.headerNode.leftBtnNode.TouchUp += page.OnBackBtnPressed;            
            this.headerNode.rightBtnNode.TouchUp += headerRightBtnNode_TouchUp;
        }

        public override void OnViewModelPropertyChanged(string propertyName)
        {
            base.OnViewModelPropertyChanged(propertyName);

           
        }

        private void headerRightBtnNode_TouchUp(object sender, EventArgs e)
        {
            
        }
    }
}
