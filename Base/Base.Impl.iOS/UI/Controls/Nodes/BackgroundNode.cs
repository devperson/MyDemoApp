using CoreAnimation;

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes
{
    //[SpecificLogger(AdvancedLogConstants.LogChatCell)]
    public class BackgroundNode : BaseControlNode
    {
        public bool? isMyMessage;

        public BackgroundNode(UIColor normalColor) : base(normalColor)
        {
        }

        public BackgroundNode(UIColor normalColor, UIColor pressedColor) : base(normalColor, pressedColor)
        {
        }

        public BackgroundNode(UIColor normalColor, UIColor pressedColor, UIColor selectedColor, bool isSelected) : base(normalColor, pressedColor, selectedColor, isSelected)
        {
           
        }

       

        public override void DidLoad()
        {
            base.DidLoad();

            this.ClipsToBounds = true;

            if (isMyMessage != null)
            {
                this.View.Layer.MaskedCorners = CACornerMask.MinXMinYCorner;

                if (isMyMessage.Value)
                {
                    this.View.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MinXMaxYCorner | CACornerMask.MaxXMinYCorner;
                }
                else
                {
                    this.View.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMaxYCorner | CACornerMask.MaxXMinYCorner;
                }
            }
        }
    }
}