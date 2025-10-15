using Base.Impl.Texture.iOS.Pages;
using Base.Impl.Texture.iOS.UI.Utils.Styles;
using Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
using Base.Impl.UI;
using Base.MVVM.ViewModels;
using Drastic.Texture;
using System.Collections;
using System.Reflection;

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes;

public class BasePageNode : ASDisplayNode
{    
    protected readonly iOSLifecyclePage Page;    
    public virtual PageViewModel ViewModel 
    {
        get
        {
            return this.Page.ViewModel;
        }      
    }

    public BasePageNode(iOSLifecyclePage page)
    {        
        this.BackgroundColor = ColorConstants.BgColor.ToUIColor();
        this.AutomaticallyManagesSubnodes = true;

        this.Page = page;
    }

    public virtual void OnViewModelPropertyChanged(string propertyName)
    {

    }

    public virtual void Destroy()
    {

    }    
}
