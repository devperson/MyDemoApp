using Base.Impl.Texture.iOS.Pages;
using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using Base.Impl.Texture.iOS.UI.Utils.Styles;
using BestApp.ViewModels.Login;
using Drastic.Texture;
using KYChat.iOS.Renderers;
using Microsoft.Maui.Graphics;
using UIKit;

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
    private ASEditTextNode txtLogin, txtPassword;    
    private ButtonNode btnSubmit;

    public LoginPageNode(iOSLifecyclePage page) : base(page)
    {        
        this.txtLogin = new ASEditTextNode();
        this.txtLogin.TextField.Placeholder = "Login";

        this.txtPassword = new ASEditTextNode();
        this.txtPassword.TextField.Placeholder = "Password";
        this.txtPassword.TextField.SecureTextEntry = true; // hide characters
        this.txtPassword.TextField.AutocapitalizationType = UITextAutocapitalizationType.None;
        this.txtPassword.TextField.AutocorrectionType = UITextAutocorrectionType.No;
        this.txtPassword.TextField.ClearButtonMode = UITextFieldViewMode.WhileEditing;
        this.txtPassword.TextField.ReturnKeyType = UIReturnKeyType.Done;        

        this.btnSubmit = ButtonStyles.CreatePrimaryButton("Submit");


        this.txtLogin.TextField.EditingChanged += txtLogin_EditingChanged;
        this.txtPassword.TextField.EditingChanged += txtPassword_EditingChanged;
        this.btnSubmit.TouchUp += BtnLogin_TouchUp;
    }

    public override ASLayoutSpec LayoutSpecThatFits(ASSizeRange constrainedSize)
    {
        var verticalStack = new ASStackLayoutSpec
        {
            Direction = ASStackLayoutDirection.Vertical,
            Spacing = 16,
            JustifyContent = ASStackLayoutJustifyContent.Center,
            AlignItems = ASStackLayoutAlignItems.Stretch,
            Children = new ASDisplayNode[] { txtLogin, txtPassword, btnSubmit }
        };

        // Add left/right margin = 20, and center vertically        
        var insetSpec = new ASInsetLayoutSpec();
        insetSpec.Insets = new UIEdgeInsets(0, 20, 0, 20);
        insetSpec.Child = verticalStack;

        // Center everything on screen
        var centerSpec = new ASCenterLayoutSpec
        {
            CenteringOptions = ASCenterLayoutSpecCenteringOptions.Y,
            SizingOptions = ASCenterLayoutSpecSizingOptions.MinimumXY,
            Child = insetSpec
        };

        return centerSpec;
    }

    private void txtLogin_EditingChanged(object sender, EventArgs e)
    {
        (Page.ViewModel as LoginPageViewModel).Login = txtLogin.TextField.Text;
    }

    private void txtPassword_EditingChanged(object sender, EventArgs e)
    {
        (Page.ViewModel as LoginPageViewModel).Password = txtPassword.TextField.Text;
    }

    private async void BtnLogin_TouchUp(object sender, EventArgs e)
    {
        await (Page.ViewModel as LoginPageViewModel).SubmitCommand.ExecuteAsync();
    }
}