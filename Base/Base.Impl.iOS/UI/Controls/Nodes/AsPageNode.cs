using Base.Impl.Texture.iOS.Pages;
using Base.Impl.Texture.iOS.UI.Utils.Styles;
using Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
using Base.Impl.UI;
using Base.MVVM.ViewModels;
using Drastic.Texture;
using System.Collections;
using System.Reflection;

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes;

public class AsPageNode<T> : ASDisplayNode, IAsPageNode where T : iOSLifecyclePage
{    
    public readonly T Page;        

    public AsPageNode(T page)
    {
        this.Page = page;
        this.BackgroundColor = ColorConstants.BgColor.ToUIColor();
        this.AutomaticallyManagesSubnodes = true;
    }

    public virtual void OnViewModelPropertyChanged(string propertyName)
    {

    }

    public virtual void Destroy()
    {

    }    
}

public interface IAsPageNode
{
    UIView View { get; }
    void OnViewModelPropertyChanged(string propertyName);
    void Destroy();    
}
