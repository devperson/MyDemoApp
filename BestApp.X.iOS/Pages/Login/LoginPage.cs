using Base.Impl.Texture.iOS.Pages;
using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using BestApp.ViewModels.Login;
using Drastic.Texture;
using KYChat.iOS.Renderers;

namespace BestApp.X.iOS.Pages.Login;
public class LoginPage : iOSLifecyclePage
{
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var loginNode = new LoginPageNode(this);
        SetPageNode(loginNode);
    } 
}

public class LoginPageNode : BasePageNode
{    
    public LoginPageNode(iOSLifecyclePage page) : base(page)
    {        
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
        await (Page.ViewModel as LoginPageViewModel).SubmitCommand.ExecuteAsync();
    }
}