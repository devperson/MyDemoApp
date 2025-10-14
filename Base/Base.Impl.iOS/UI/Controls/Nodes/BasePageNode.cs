using Base.Impl.Texture.iOS.Pages;
using Base.Impl.Texture.iOS.UI.Utils.Styles;
using Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
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

    protected void SetCurrentPropertyValues()
    {
        var constants = this.GetConstants();
        Console.WriteLine($"Got {constants.Length} constants for {this.GetType().Name}");

        foreach (var constant in constants)
        {
            var propertyName = constant.GetValue(this)?.ToString();

            Console.WriteLine($"Constant {constant.Name} value {propertyName}");

            if (!string.IsNullOrEmpty(propertyName))
            {
                OnViewModelPropertyChanged(propertyName);
            }
        }
    }   

    protected FieldInfo[] GetConstants()
    {
        ArrayList constants = new ArrayList();

        FieldInfo[] fieldInfos = this.GetType().GetFields(
            // Gets all public and static fields

            BindingFlags.Public | BindingFlags.Static |
            // This tells it to get the fields from all base types as well

            BindingFlags.FlattenHierarchy);

        // Go through the list and only pick out the constants
        foreach (FieldInfo fi in fieldInfos)
            // IsLiteral determines if its value is written at 
            //   compile time and not changeable
            // IsInitOnly determines if the field can be set 
            //   in the body of the constructor
            // for C# a field which is readonly keyword would have both true 
            //   but a const field would have only IsLiteral equal to true
            if (fi.IsLiteral && !fi.IsInitOnly)
                constants.Add(fi);

        // Return an array of FieldInfos
        return (FieldInfo[])constants.ToArray(typeof(FieldInfo));
    }
}
