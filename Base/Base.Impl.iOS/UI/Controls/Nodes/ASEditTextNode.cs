using Base.Abstractions.AppService;
using Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
using Drastic.Texture;

namespace Base.Impl.Texture.iOS.UI.Controls.Nodes
{
    public class ASEditTextNode : ASDisplayNode
    {        
        private int minAllowedHeight = 45;
        public UIColor NormalBorderColor { get; set; } = UIColor.Clear;
        public UIColor FocusedBorderColor { get; set; } = XfColor.FromHex("#1989FC").ToUIColor();

        public UITextField TextField { get; set; }

        public ASEditTextNode()
        {
            this.Style.Height = new ASDimension { unit = ASDimensionUnit.Points, value = minAllowedHeight };
            
            this.TextField = new UITextField();
            this.TextField.BackgroundColor = UIColor.White;
            this.TextField.Layer.CornerRadius = minAllowedHeight / 2;
            this.TextField.Layer.BorderWidth = 2;
            this.TextField.Layer.BorderColor = NormalBorderColor.CGColor;            
            this.TextField.Font = UIFont.FromName("Sen", 15);
            this.TextField.TextColor = UIColor.Black;

            this.TextField.LeftView = new UIView(new CGRect(0, 0, 20, 10));
            this.TextField.LeftViewMode = UITextFieldViewMode.Always;            

            //set right search icon
            var rightView = new UIView(new CGRect(0, 0, 20, 24));
            this.TextField.RightView = rightView;
            this.TextField.RightViewMode = UITextFieldViewMode.Always;

            this.TextField.ShouldReturn = this.txtSearch_ShouldReturn;            
            this.TextField.EditingDidEndOnExit += TextField_EditingDidEndOnExit;
            this.TextField.EditingDidEnd += TextField_EditingDidEnd;
            this.TextField.EditingDidBegin += TextField_EditingDidBegin;
        }

        public override void DidLoad()
        {
            this.View.AddSubview(this.TextField);
        }

        public override void LayoutDidFinish()
        {
            base.LayoutDidFinish();

            this.TextField.Frame = new CGRect(CGPoint.Empty, this.CalculatedSize);
        }

        private void TextField_EditingDidBegin(object sender, EventArgs e)
        {
            this.TextField.Layer.BorderColor = FocusedBorderColor.CGColor;
        }

        private void TextField_EditingDidEnd(object sender, EventArgs e)
        {
            this.TextField.ResignFirstResponder();
            this.TextField.Layer.BorderColor = NormalBorderColor.CGColor;
        }

        private void TextField_EditingDidEndOnExit(object sender, EventArgs e)
        {
            this.TextField.ResignFirstResponder();
            this.TextField.Layer.BorderColor = NormalBorderColor.CGColor;
        }

        private bool txtSearch_ShouldReturn(UITextField textField)
        {
            this.TextField.ResignFirstResponder();
            this.TextField.Layer.BorderColor = NormalBorderColor.CGColor;

            return false;
        }
    }
}
