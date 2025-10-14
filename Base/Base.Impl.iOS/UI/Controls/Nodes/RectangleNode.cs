using CoreAnimation;
using Drastic.Texture;

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes;

public class RectangleNode : ASControlNode
{
    private readonly bool roundTop;
    private readonly bool roundAll;
    public event EventHandler TouchUp;
    public event EventHandler TouchDown;

    public RectangleNode(int corner, bool roundTop = false)
    {
        this.CornerRadius = corner;
        this.roundTop = roundTop;        
    }

    public override void DidLoad()
    {
        base.DidLoad();
        this.ClipsToBounds = true;

        this.AddTarget(this, new ObjCRuntime.Selector("OnTouchDown:"), ASControlNodeEvent.TouchDown);
        this.AddTarget(this, new ObjCRuntime.Selector("OnTouchUpInside:"), ASControlNodeEvent.TouchUpInside);
        this.AddTarget(this, new ObjCRuntime.Selector("OnTouchCancel:"), ASControlNodeEvent.TouchUpOutside);
        this.AddTarget(this, new ObjCRuntime.Selector("OnTouchCancel:"), ASControlNodeEvent.TouchCancel);

        if (this.roundTop)
        {
            this.View.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner;
        }        
    }

    [Export("OnTouchDown:")]
    public virtual void OnTouchDown(NSObject sender)
    {        
        if (this.TouchDown != null)
        {
            this.TouchDown.Invoke(this, EventArgs.Empty);
        }
    }

    [Export("OnTouchUpInside:")]
    public virtual void OnTouchUpInside(NSObject sender)
    {        
        if (this.TouchUp != null)
        {
            this.TouchUp.Invoke(this, EventArgs.Empty);
        }
    }

    [Export("OnTouchCancel:")]
    public virtual void OnTouchCancel(NSObject sender)
    {
        
    }
}
