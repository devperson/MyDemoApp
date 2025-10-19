using Base.Impl.Texture.iOS.Pages;
using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using BestApp.ViewModels.Movies;
using BestApp.X.iOS.Utils;
using Drastic.Texture;
using Microsoft.Maui.ApplicationModel;

namespace BestApp.X.iOS.Pages.Movies
{
    public class SideMenuController : iOSPage
    {        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            this.AddAndStretchNode(new SideMenuNode(this));
        }
    }

    public class SideMenuNode : ASDisplayNode
    {
        private readonly SideMenuController sideMenu;
        private IconTextButtonNode btnLogout;
        private ASTextNode txtVersion;
        private ASDisplayNode spacer1, spacer2;

        public SideMenuNode(SideMenuController sideMenu) 
        {
            this.AutomaticallyManagesSubnodes = true;
            this.sideMenu = sideMenu;
            this.BackgroundColor = UIColor.White;           

            btnLogout = this.GetMenuButton("SvgImages/logout.svg", "Logout");
            btnLogout.TouchUp += BtnLogout_TouchUp;

            this.spacer1 = new ASDisplayNode();
            this.spacer1.Style.PreferredSize = new CGSize(60, 60);

            this.spacer2 = new ASDisplayNode();
            this.spacer2.Style.FlexGrow = 1;

            var appVersion = $"Version: {AppInfo.VersionString} ({AppInfo.BuildString})";
            var txtAttr = new UIStringAttributes();
            txtAttr.Font = UIFont.FromName("Sen-SemiBold", 12);
            txtAttr.ForegroundColor = UIColorConstants.LabelColor;
            this.txtVersion = new ASTextNode();
            this.txtVersion.AttributedText = new NSAttributedString(appVersion, txtAttr);
        }

        public override ASLayoutSpec LayoutSpecThatFits(ASSizeRange constrainedSize)
        {
            var relative = new ASRelativeLayoutSpec();
            relative.HorizontalPosition = ASRelativeLayoutSpecPosition.End;
            relative.VerticalPosition = ASRelativeLayoutSpecPosition.End;
            relative.Child = this.txtVersion;

            var verInset = new ASInsetLayoutSpec();
            verInset.Child = relative;
            verInset.Insets = new UIEdgeInsets(30, 30, 50, 30);

            var stack = new ASStackLayoutSpec();
            stack.Direction = ASStackLayoutDirection.Vertical;
            stack.Spacing = 2;
            stack.AlignItems = ASStackLayoutAlignItems.Stretch;
            stack.Children = [spacer1, btnLogout, spacer2, verInset];            

            return stack;
        }

        private void BtnLogout_TouchUp(object sender, EventArgs e)
        {
            var homeVm = AppDelegate.Instance.pageNavigationService.GetCurrentPageModel() as MoviesPageViewModel;
            homeVm.MenuTappedCommand.Execute(new MenuItem { Type = MenuType.Logout });

            AppDelegate.Instance.flyoutController.CloseLeft();
        }

        private IconTextButtonNode GetMenuButton(string icon, string text)
        {
            var btn = new IconTextButtonNode(UIColor.White, UIColorConstants.BlueColor2);
            btn.SetIconText(icon, text, spacing: 20, iconSize: 24, textFont: UIFont.FromName("Sen", 22), hPadding: 30, hAligment: ASStackLayoutJustifyContent.Start, corner: 0);

            return btn;
        }
    }
}
