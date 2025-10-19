using Base.Impl.Texture.iOS.UI.Utils.Styles;
using Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
using Drastic.Texture;

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes
{
    //[SpecificLogger(AdvancedLogConstants.LogChatCell)]
    public class BaseControlNode : ASControlNode
    {
        public UIColor NormalColor { get; set; }
        public UIColor PressedColor { get; set; }
        public UIColor SelectedColor { get; set; }
        public event EventHandler TouchUp;
        public event EventHandler TouchDown;

        public BaseControlNode(UIColor normalColor) : this(normalColor, ColorConstants.Gray100.ToUIColor())
        {            
        }

        public BaseControlNode(UIColor normalColor, UIColor pressedColor) : this(normalColor, pressedColor, pressedColor, false)
        {

        }

        public BaseControlNode(UIColor normalColor, UIColor pressedColor, UIColor selectedColor, bool isSelected)
        {
            this.AutomaticallyManagesSubnodes = true;

            this.NormalColor = normalColor;
            this.PressedColor = pressedColor;
            this.SelectedColor = selectedColor;

            this.SetSelected(isSelected);
        }

        protected virtual void Initialize()
        {

        }

        public void ChangeNormalColor(UIColor normalColor)
        {
            this.NormalColor = normalColor;
            this.BackgroundColor = normalColor;
        }

        public void SetSelected(bool isSelected)
        {
            this.Selected = isSelected;

            if (isSelected)
            {
                this.BackgroundColor = this.SelectedColor;
            }
            else
            {
                this.BackgroundColor = this.NormalColor;
            }
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
            this.BackgroundColor = this.PressedColor;

            if (this.TouchDown != null)
            {
                this.TouchDown.Invoke(this, EventArgs.Empty);
            }
        }

        [Export("OnTouchUpInside:")]
        public virtual void OnTouchUpInside(NSObject sender)
        {
            if (this.Selected)
                return;

            this.BackgroundColor = this.NormalColor;

            if (this.TouchUp != null)
            {
                this.TouchUp.Invoke(this, EventArgs.Empty);
            }
        }

        [Export("OnTouchCancel:")]
        public virtual void OnTouchCancel(NSObject sender)
        {
            this.BackgroundColor = this.NormalColor;
        }
    }
}