using Base.Impl.Texture.iOS.Pages;
using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using BestApp.ViewModels.Movies;
using BestApp.X.iOS.Utils;
using Drastic.Texture;
using KYChat.iOS.Renderers.ASChatListView.Utils.CustomNodes;
using UIKit;

namespace BestApp.X.iOS.Pages.Movies;
public class MovieDetailPage : iOSLifecyclePage
{
    public new MovieDetailPageViewModel ViewModel
    {
        get => base.ViewModel as MovieDetailPageViewModel;
        set => base.ViewModel = value;
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var page = new _Node(this);
        SetPageNode(page);
    }

    public class _Node : AsPageNode<MovieDetailPage>
    {
        private PageHeaderNode headerNode;

        private readonly UIImageViewNode imgView;
        private readonly ASTextNode lblName, txtName, lblDescription, txtDescription;        

        public _Node(MovieDetailPage page) : base(page)
        {
            this.headerNode = new PageHeaderNode("Movies", "SvgImages/backarrowblack.svg", "SvgImages/edit.svg");
            this.headerNode.leftBtnNode.TouchUp += page.OnBackBtnPressed;            
            this.headerNode.rightBtnNode.TouchUp += headerRightBtnNode_TouchUp;

            // --- Image ---
            imgView = new UIImageViewNode();
            imgView.Url = page.ViewModel.Model.PosterUrl;
            imgView.Style.PreferredLayoutSize = new ASLayoutSize
            {
                width = new ASDimension { value = 200, unit = ASDimensionUnit.Points },
                height = new ASDimension { value = 300, unit = ASDimensionUnit.Points },
            };
            
            var rightAligned = new NSMutableParagraphStyle();
            rightAligned.Alignment = UITextAlignment.Right;

            lblName = new ASTextNode();
            lblName.AttributedText = new NSAttributedString("Name:",
                                                            new UIStringAttributes
                                                            {
                                                                Font = UIFont.FromName("Sen", 15),
                                                                ForegroundColor = UIColorConstants.LabelColor,
                                                                ParagraphStyle = rightAligned
                                                            });

            // --- Name Value ---
            txtName = new ASTextNode();
            txtName.Style.FlexGrow = 1;
            txtName.Style.FlexShrink = 1;
            txtName.AttributedText = new NSAttributedString(page.ViewModel.Model.Name,
                                                            new UIStringAttributes
                                                            {
                                                                Font = UIFont.FromName("Sen-SemiBold", 15),
                                                                ForegroundColor = UIColorConstants.LabelColor
                                                            });
            

            // --- Description Label ---
            lblDescription = new ASTextNode();
            lblDescription.AttributedText = new NSAttributedString("Description:",
                                                                    new UIStringAttributes
                                                                    {
                                                                        Font = UIFont.FromName("Sen", 15),
                                                                        ForegroundColor = UIColorConstants.LabelColor,
                                                                        ParagraphStyle = rightAligned
                                                                    });

            // --- Description Value ---
            txtDescription = new ASTextNode();
            txtDescription.MaximumNumberOfLines = 0;
            txtDescription.TruncationMode = UILineBreakMode.WordWrap;
            txtDescription.Style.FlexGrow = 1;
            txtDescription.Style.FlexShrink = 1;
            txtDescription.AttributedText = new NSAttributedString(this.Page.ViewModel.Model.Overview,
                                                                    new UIStringAttributes
                                                                    {
                                                                        Font = UIFont.FromName("Sen-Medium", 15),
                                                                        ForegroundColor = UIColorConstants.LabelColor
                                                                    });
         
        }

        public override ASLayoutSpec LayoutSpecThatFits(ASSizeRange constrainedSize)
        {
            
            // --- Measure widest label for alignment ---
            var nameSize = lblName.CalculateSizeThatFits(constrainedSize.max);
            var descSize = lblDescription.CalculateSizeThatFits(constrainedSize.max);
            var maxLabelWidth = (nfloat)Math.Max(nameSize.Width, descSize.Width);

            lblName.Style.Width = new ASDimension() { value = maxLabelWidth, unit = ASDimensionUnit.Points };
            lblDescription.Style.Width = new ASDimension() { value = maxLabelWidth, unit = ASDimensionUnit.Points };

            // --- Name Row ---
            var nameRow = new ASStackLayoutSpec();
            nameRow.Direction = ASStackLayoutDirection.Horizontal;
            nameRow.Spacing = 8;
            nameRow.AlignItems = ASStackLayoutAlignItems.Start;
            nameRow.Children = [lblName, txtName];

            // --- Description Row ---
            var descRow = new ASStackLayoutSpec();
            descRow.Direction = ASStackLayoutDirection.Horizontal;
            descRow.Spacing = 8;
            descRow.AlignItems = ASStackLayoutAlignItems.Start;
            descRow.Children = new[] { lblDescription, txtDescription };

            var rowsStack = new ASStackLayoutSpec();
            rowsStack.Direction = ASStackLayoutDirection.Vertical;
            rowsStack.Spacing = 8;
            rowsStack.Children = [ nameRow, descRow ];

            // --- Center image using CenterLayoutSpec ---
            var centeredImage = new ASCenterLayoutSpec();
            centeredImage.CenteringOptions = ASCenterLayoutSpecCenteringOptions.X;
            centeredImage.Child = imgView;

            var contentStack = new ASStackLayoutSpec();
            contentStack.Direction = ASStackLayoutDirection.Vertical;
            contentStack.Spacing = 25;
            contentStack.AlignItems = ASStackLayoutAlignItems.Stretch;
            contentStack.Children = [centeredImage, rowsStack];

            // --- Insets ---
            var contentHorizontalInset = this.GetPageInsets();
            contentHorizontalInset.Child = contentStack;

            // --- Main Stack ---
            var mainStack = new ASStackLayoutSpec();
            mainStack.Direction = ASStackLayoutDirection.Vertical;
            mainStack.Spacing = 25;
            mainStack.AlignItems = ASStackLayoutAlignItems.Stretch;
            mainStack.Children = [ headerNode, contentHorizontalInset];

            var safeAreaInset = new ASInsetLayoutSpec();
            safeAreaInset.Insets = this.View.SafeAreaInsets;
            safeAreaInset.Child = mainStack;

            return safeAreaInset;
        }

        public override void OnViewModelPropertyChanged(string propertyName)
        {
            base.OnViewModelPropertyChanged(propertyName);
        }

        private void headerRightBtnNode_TouchUp(object sender, EventArgs e)
        {
            this.Page.ViewModel.EditCommand.Execute();
        }
    }
}
