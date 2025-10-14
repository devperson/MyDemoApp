using Drastic.Texture;

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes
{    
    public class ButtonNode : ASButtonNode
    {
        public UIColor normalColor;
        public UIColor pressedColor;
        public event EventHandler TouchUp;
        public event EventHandler TouchDown;

        public ButtonNode(UIColor normalColor, UIColor pressedColor)
        {
            this.normalColor = normalColor;
            this.pressedColor = pressedColor;

            this.BackgroundColor = normalColor;
        }

        public override void DidLoad()
        {
            base.DidLoad();

            this.AddTarget(this, new ObjCRuntime.Selector("OnTouchDown:"), ASControlNodeEvent.TouchDown);
            this.AddTarget(this, new ObjCRuntime.Selector("OnTouchUpInside:"), ASControlNodeEvent.TouchUpInside);
            this.AddTarget(this, new ObjCRuntime.Selector("OnTouchCancel:"), ASControlNodeEvent.TouchUpOutside);
            this.AddTarget(this, new ObjCRuntime.Selector("OnTouchCancel:"), ASControlNodeEvent.TouchCancel);
        }

        [Export("OnTouchDown:")]
        public virtual void OnTouchDown(NSObject sender)
        {
            this.BackgroundColor = this.pressedColor;

            if (this.TouchDown != null)
            {
                this.TouchDown.Invoke(this, EventArgs.Empty);
            }
        }

        [Export("OnTouchUpInside:")]
        public virtual void OnTouchUpInside(NSObject sender)
        {
            this.BackgroundColor = this.normalColor;

            if (this.TouchUp != null)
            {
                this.TouchUp.Invoke(this, EventArgs.Empty);
            }
        }

        [Export("OnTouchCancel:")]
        public virtual void OnTouchCancel(NSObject sender)
        {
            this.BackgroundColor = this.normalColor;
        }
    }
}