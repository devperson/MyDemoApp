using Base.Impl.Texture.iOS.Pages;
using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using Base.Impl.Texture.iOS.UI.Utils.Styles;
using BestApp.ViewModels.Login;
using BestApp.X.iOS.Utils;
using Drastic.Texture;
using KYChat.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.iOS.Pages;
public class LoginPage : iOSLifecyclePage
{
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var loginNode = new LoginPageNode(this);
        this.SetPageNode(loginNode);
    }
}

public class LoginPageNode : BasePageNode
{
    private RectangleNode whiteBottomRectNode;
    private IconTextButtonNode loginButton;
    private SvgViewNode logoNode;

    public LoginPageNode(iOSLifecyclePage page) : base(page)
    {
        //this.BackgroundColor = UIColorConstants.BlueColor;

        this.logoNode = new SvgViewNode("SvgImages/logo.svg");
        this.logoNode.Style.PreferredSize = new CGSize(135, 135);

        //this.loginButton = new IconTextButtonNode(UIColor.White, UIColorConstants.BlueColor2);
        //this.loginButton.SetIconText("SvgImages/apple.svg", "Sign in with Apple");
        //this.loginButton.Style.Width = new ASDimension { unit = ASDimensionUnit.Points, value = 260 };
        //this.loginButton.TouchUp += LoginButton_TouchUp;

        //whiteBottomRectNode = new RectangleNode(32);
        
        //whiteBottomRectNode.BackgroundColor = UIColorConstants.BgColor;

    }

    public override ASLayoutSpec LayoutSpecThatFits(ASSizeRange constrainedSize)
    {
        var stack = new ASStackLayoutSpec();
        stack.Direction = ASStackLayoutDirection.Vertical;
        stack.Spacing = 0;
        stack.AlignItems = ASStackLayoutAlignItems.Stretch;

        var centerLogo = new ASCenterLayoutSpec();
        centerLogo.Child = logoNode;
        centerLogo.CenteringOptions = ASCenterLayoutSpecCenteringOptions.Xy;
        centerLogo.Style.FlexGrow = 0.45f;



        var centerButton = new ASCenterLayoutSpec();
        centerButton.CenteringOptions = ASCenterLayoutSpecCenteringOptions.Xy;
        centerButton.Child = loginButton;
        var bottom = new ASBackgroundLayoutSpec();
        bottom.Background = whiteBottomRectNode;
        bottom.Child = centerButton;
        bottom.Style.FlexGrow = 0.55f;

        stack.Children = [centerLogo, bottom];

        return stack;
    }

    private async void LoginButton_TouchUp(object sender, EventArgs e)
    {
        await (this.Page.ViewModel as LoginPageViewModel).SubmitCommand.ExecuteAsync();
    }
}