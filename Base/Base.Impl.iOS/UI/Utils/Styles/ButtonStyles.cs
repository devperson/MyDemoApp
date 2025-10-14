using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using Drastic.Texture;

namespace Base.Impl.Texture.iOS.UI.Utils.Styles;

public static class ButtonStyles
{
    public static IconButtonNode CreateIconButton(string icon, UIColor textColor)
    {
        var iconBtn = new IconButtonNode(UIColor.White, textColor, icon); //UIColorConstants.BlueColor2
        return iconBtn;
    }

    //public static TabButtonNode CreateTabButton(string icon, string iconSelected, string text, bool isSelected = false)
    //{
    //    var iconBtn = new TabButtonNode(icon, iconSelected, text, isSelected);     
    //    iconBtn.Style.FlexGrow = 1;
    //    iconBtn.Style.Width = new ASDimension { unit = ASDimensionUnit.Fraction, value = 0.25f };
    //    return iconBtn;
    //}

    public static ButtonNode CreatePrimaryButton(string text, UIColor normalColor, UIColor pressedColor) //UIColorConstants.BlueColor, UIColorConstants.DarkBlueColor
    {        
        var txtAttr = new UIStringAttributes();
        txtAttr.Font = UIFont.FromName("Sen-Bold", 18);
        txtAttr.ForegroundColor = UIColor.White;

        var btn = new ButtonNode(normalColor, pressedColor);
        btn.CornerRadius = NumberConstants.BtnHeight / 2;
        btn.SetAttributedTitle(new NSAttributedString(text, txtAttr), UIControlState.Normal);
        btn.Style.PreferredLayoutSize = new ASLayoutSize
        {            
            width = new ASDimension { unit = ASDimensionUnit.Auto },
            height = new ASDimension { unit = ASDimensionUnit.Points, value = NumberConstants.BtnHeight },
        };

        return btn;
    }

    //public static ButtonNode CreateSecondaryButton(string text)
    //{        
    //    var btn = CreatePrimaryButton(text);
    //    btn.normalColor = btn.BackgroundColor = UIColorConstants.LabelColor;
    //    btn.pressedColor = UIColorConstants.LabelColor.MakeDarker(0.5f);

    //    return btn;
    //}
}
