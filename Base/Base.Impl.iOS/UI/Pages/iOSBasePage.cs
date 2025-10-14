using Base.Abstractions.Diagnostic;
using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using Base.MVVM.Navigation;
using Base.MVVM.ViewModels;
using Drastic.Texture;
using DryIoc;

namespace Base.Impl.Texture.iOS.Pages;

public class iOSBasePage : UIViewController, IPage
{
    protected BasePageNode PageNode { get; set; }
    protected ILoggingService loggingService { get; set; }
    public PageViewModel ViewModel { get; set; }

    public iOSBasePage()
    {
        if (this.loggingService == null)
            this.loggingService = Base.Impl.iOS.Registrar.appContainer.Resolve<ILoggingService>();
    }

    protected virtual void SetPageNode(BasePageNode node)
    {
        this.PageNode = node;

        this.AddAndStretchNode(this.PageNode);
    }

    protected void AddAndStretchNode(ASDisplayNode node)
    {
        this.View.AddSubview(node.View);
        node.View.TranslatesAutoresizingMaskIntoConstraints = false;
        this.View.AddConstraints(new[]
        {
            node.View.TopAnchor.ConstraintEqualTo(this.View.TopAnchor),
            node.View.LeftAnchor.ConstraintEqualTo(this.View.LeftAnchor),
            node.View.RightAnchor.ConstraintEqualTo(this.View.RightAnchor),
            node.View.BottomAnchor.ConstraintEqualTo(this.View.BottomAnchor),
            node.View.CenterYAnchor.ConstraintEqualTo(this.View.CenterYAnchor),
        });
    }
}
