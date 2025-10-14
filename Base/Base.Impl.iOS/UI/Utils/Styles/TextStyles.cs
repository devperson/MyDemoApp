using Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
using Base.Impl.UI;
using Drastic.Texture;

namespace Base.Impl.Texture.iOS.UI.Utils.Styles;

public static class TextStyles
{
    public static NSAttributedString Get_pageTitle_Attr(string text, bool centerText = true)
    {
        var fullRange = new NSRange(0, text.Length);
        var paragraStyle = new NSMutableParagraphStyle()
        {            
            Alignment = UITextAlignment.Center,
        };

        var attrString = new NSMutableAttributedString(text);
        attrString.AddAttribute(UIStringAttributeKey.Font, UIFont.FromName("Sen-Bold", 24), fullRange);
        attrString.AddAttribute(UIStringAttributeKey.ForegroundColor, ColorConstants.DefaultTextColor.ToUIColor(), fullRange); //UIColorConstants.LabelColor

        if (centerText)
        {
            attrString.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraStyle, fullRange);
        }

        return attrString;
    }

    public static NSAttributedString Get_pageSubTitle_Attr(string text, bool centerText = true)
    {
        var fullRange = new NSRange(0, text.Length);
        var paragraStyle = new NSMutableParagraphStyle()
        {            
            LineHeightMultiple = new nfloat(1.5),
            Alignment = UITextAlignment.Center,
        };

        var attrString = new NSMutableAttributedString(text);
        attrString.AddAttribute(UIStringAttributeKey.Font, UIFont.FromName("Sen", 19), fullRange);
        attrString.AddAttribute(UIStringAttributeKey.ForegroundColor, ColorConstants.DefaultTextColor.ToUIColor(), fullRange);

        if (centerText)
        {
            attrString.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraStyle, fullRange);
        }

        return attrString;
    }

    public static UIStringAttributes Get_pageMediumTitle_Attr()
    {
        var txtAttr = new UIStringAttributes();
        txtAttr.Font = UIFont.FromName("Sen", 18);
        txtAttr.ForegroundColor = ColorConstants.DefaultTextColor.ToUIColor();

        return txtAttr;
    }

    public static ASTextNode Create_pageMediumTitleStyle(string text)
    {        
        var attrStr = Get_pageMediumTitle_Attr();
        var txtNode = new ASTextNode();
        txtNode.AttributedText = new NSAttributedString(text, attrStr); 
        txtNode.MaximumNumberOfLines = 1;
        txtNode.TruncationMode = UILineBreakMode.TailTruncation;
        txtNode.TruncationAttributedText = new NSAttributedString("...", attrStr);

        return txtNode;
    }

    public static UIStringAttributes Get_pageTitleLoading_Attr()
    {
        var txtAttr = new UIStringAttributes();
        txtAttr.Font = UIFont.FromName("Sen", 16);
        txtAttr.ForegroundColor = ColorConstants.DefaultTextColor.ToUIColor();
        
        return txtAttr;
    }
    public static ASTextNode Create_pageTitleLoadingStyle()
    {
        var txtAttr = Get_pageTitleLoading_Attr();       
        var attrStr = new NSAttributedString("Updating...", txtAttr);
        var txtNode = new ASTextNode();
        txtNode.AttributedText = attrStr;        

        return txtNode;
    }


    public static NSAttributedString Create_homepageMediumTitleStyle(string text)
    {
        var fullRange = new NSRange(0, text.Length);
        var paragraStyle = new NSMutableParagraphStyle()
        {
            LineSpacing = 4.0f,
            Alignment = UITextAlignment.Center,
        };

        var attrString = new NSMutableAttributedString(text);
        attrString.AddAttribute(UIStringAttributeKey.Font, UIFont.FromName("Sen", 18), fullRange);
        attrString.AddAttribute(UIStringAttributeKey.ForegroundColor, ColorConstants.DefaultTextColor.ToUIColor(), fullRange);
        attrString.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraStyle, fullRange);

        return attrString;
    }


    public static ASTextNode Create_centeredTextNode(string text, int fontSize, UIColor color, string family = "Sen")
    {
        var fullRange = new NSRange(0, text.Length);
        var paragraStyle = new NSMutableParagraphStyle()
        {
            LineSpacing = 4.0f,
            Alignment = UITextAlignment.Center,
        };

        var attrString = new NSMutableAttributedString(text);
        attrString.AddAttribute(UIStringAttributeKey.Font, UIFont.FromName(family, fontSize), fullRange);
        attrString.AddAttribute(UIStringAttributeKey.ForegroundColor, color, fullRange);
        attrString.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraStyle, fullRange);

        var txtNode = new ASTextNode();
        txtNode.AttributedText = attrString;
        return txtNode;
    }

    public static UIStringAttributes Create_boldMediumStyle()
    {
        var txtAttr = new UIStringAttributes();
        txtAttr.Font = UIFont.FromName("Sen-Bold", 16);
        txtAttr.ForegroundColor = ColorConstants.DefaultTextColor.ToUIColor();

        return txtAttr;
    }

    public static UIStringAttributes Create_regularMediumStyle()
    {
        var txtAttr = new UIStringAttributes();
        txtAttr.Font = UIFont.FromName("Sen", 16);
        txtAttr.ForegroundColor = ColorConstants.DefaultTextColor.ToUIColor();

        return txtAttr;
    }

    public static NSAttributedString Create_regularSmallStyle(string text, UIColor textColor, bool centerText = true)
    {
        var fullRange = new NSRange(0, text.Length);
        var paragraStyle = new NSMutableParagraphStyle()
        {
            LineHeightMultiple = new nfloat(1.3),
            Alignment = UITextAlignment.Center,
        };

        var attrString = new NSMutableAttributedString(text);
        attrString.AddAttribute(UIStringAttributeKey.Font, UIFont.FromName("Sen", 14), fullRange);
        attrString.AddAttribute(UIStringAttributeKey.ForegroundColor, textColor, fullRange);

        if (centerText)
        {
            attrString.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraStyle, fullRange);
        }

        return attrString;
    }

    public static UIStringAttributes Create_FaStyle(UIColor color, int size)
    {
        var txtAttr = new UIStringAttributes();
        txtAttr.Font = UIFont.FromName("Font Awesome 5 Free Solid", size);
        txtAttr.ForegroundColor = color;

        return txtAttr;
    }

}
