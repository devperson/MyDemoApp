using Base.Abstractions.Diagnostic;
using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using Base.MVVM.Navigation;
using Base.MVVM.ViewModels;
using Drastic.Texture;
using DryIoc;

namespace Base.Impl.Texture.iOS.Pages;

public class iOSPage : UIViewController, IPage 
{    
    protected ILoggingService loggingService { get; set; }
    public IAsPageNode Node { get; set; }
    public PageViewModel ViewModel { get; set; }

    public iOSPage()
    {
        if (this.loggingService == null)
            this.loggingService = Base.Impl.iOS.Registrar.appContainer.Resolve<ILoggingService>();
    }

    protected virtual void SetPageNode(IAsPageNode node)
    {
        this.Node = node;

        this.AddAndStretchNode(node as ASDisplayNode);
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
