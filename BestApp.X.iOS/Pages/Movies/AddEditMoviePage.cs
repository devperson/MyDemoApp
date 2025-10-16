using Base.Abstractions.AppService;
using Base.Impl.Texture.iOS.Pages;
using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using Base.Impl.Texture.iOS.UI.Utils.Styles;
using Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
using BestApp.ViewModels.Movies;
using BestApp.X.iOS.Utils;
using Drastic.Texture;
using KYChat.iOS.Renderers;
using KYChat.iOS.Renderers.ASChatListView.Utils.CustomNodes;
using System;
using System.Threading.Tasks;

namespace BestApp.X.iOS.Pages.Movies;
public class AddEditMoviePage : iOSLifecyclePage
{
    public new AddEditMoviePageViewModel ViewModel
    {
        get => base.ViewModel as AddEditMoviePageViewModel;
        set => base.ViewModel = value;
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var page = new _Node(this);
        SetPageNode(page);
    }

    public class _Node : AsPageNode<AddEditMoviePage>
    {
        private readonly PageHeaderNode headerNode;
        private readonly BackgroundNode btnPhoto;
        private readonly UIImageViewNode imgView;
        private readonly SvgViewNode photoIcon;
        private readonly ASTextNode lblName, lblDescription;
        private readonly ASEditTextNode txtName, txtDescription;
        private readonly ButtonNode btnSave;

        public _Node(AddEditMoviePage page) : base(page)
        {
            this.headerNode = new PageHeaderNode("Movies", "SvgImages/backarrowblack.svg", "SvgImages/deleteblack.svg");
            this.headerNode.leftBtnNode.TouchUp += page.OnBackBtnPressed;
            this.headerNode.rightBtnNode.TouchUp += headerRightBtnNode_TouchUp;

            btnPhoto = new BackgroundNode(UIColor.FromRGBA(1f, 1f, 1f, 0.5f), UIColorConstants.Gray100.ColorWithAlpha(0.4f));
            btnPhoto.TouchUp += BtnPhoto_TouchUp;

            photoIcon = new SvgViewNode("SvgImages/icon_photo.svg");
            photoIcon.UserInteractionEnabled = false;
            photoIcon.Style.PreferredLayoutSize = new ASLayoutSize
            {
                width = new ASDimension { value = 45, unit = ASDimensionUnit.Points },
                height = new ASDimension { value = 45, unit = ASDimensionUnit.Points },
            };

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
            txtName = new ASEditTextNode();            
            txtName.TextField.Text = page.ViewModel.Model.Name;
            txtName.Style.FlexGrow = 1;
            txtName.Style.FlexShrink = 1;
            txtName.TextField.EditingChanged += txtName_EditingChanged;



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
            txtDescription = new ASEditTextNode();
            txtDescription.TextField.Text = page.ViewModel.Model.Overview;
            txtDescription.Style.FlexGrow = 1;
            txtDescription.Style.MaxHeight = new ASDimension() { value = 200, unit = ASDimensionUnit.Points };
            txtDescription.TextField.EditingChanged += txtDescription_EditingChanged;

            btnSave = ButtonStyles.CreatePrimaryButton("Save");
            btnSave.TouchUp += BtnSave_TouchUp;
        }

        public override ASLayoutSpec LayoutSpecThatFits(ASSizeRange constrainedSize)
        {
            var clickableImage = new ASOverlayLayoutSpec();
            clickableImage.Child = imgView;
            clickableImage.Overlay = btnPhoto;

            var centerIcon = new ASCenterLayoutSpec();
            centerIcon.CenteringOptions = ASCenterLayoutSpecCenteringOptions.Xy;
            centerIcon.SizingOptions = ASCenterLayoutSpecSizingOptions.MinimumXY;
            centerIcon.Child = photoIcon;

            var imgNode = new ASOverlayLayoutSpec();
            imgNode.Child = clickableImage;
            imgNode.Overlay = centerIcon;

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
            nameRow.AlignItems = ASStackLayoutAlignItems.Center;
            nameRow.Children = [lblName, txtName];

            // --- Description Row ---
            var descRow = new ASStackLayoutSpec();
            descRow.Direction = ASStackLayoutDirection.Horizontal;
            descRow.Spacing = 8;
            descRow.AlignItems = ASStackLayoutAlignItems.Center;
            descRow.Children = [ lblDescription, txtDescription ];

            var rowsStack = new ASStackLayoutSpec();
            rowsStack.Direction = ASStackLayoutDirection.Vertical;
            rowsStack.Spacing = 8;
            rowsStack.Children = [nameRow, descRow];

            // --- Center image using CenterLayoutSpec ---
            var centeredImage = new ASCenterLayoutSpec();
            centeredImage.CenteringOptions = ASCenterLayoutSpecCenteringOptions.X;
            centeredImage.Child = imgNode;

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
            mainStack.Children = [headerNode, contentHorizontalInset];

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
            this.Page.ViewModel.DeleteCommand.Execute();
        }

        private async void BtnPhoto_TouchUp(object sender, EventArgs e)
        {
            await this.Page.ViewModel.ChangePhotoCommand.ExecuteAsync();
            this.OnPhotoChanged();
        }

        private void txtDescription_EditingChanged(object sender, EventArgs e)
        {
            this.Page.ViewModel.Model.Name = this.txtName.TextField.Text;
        }

        private void txtName_EditingChanged(object sender, EventArgs e)
        {
            this.Page.ViewModel.Model.Overview = this.txtDescription.TextField.Text;
        }

        private void BtnSave_TouchUp(object sender, EventArgs e)
        {
            this.Page.ViewModel.SaveCommand.Execute();
        }

        private void OnPhotoChanged()
        {
            if (!string.IsNullOrEmpty(this.Page.ViewModel.Model.PosterUrl))
            {
                imgView.Url = this.Page.ViewModel.Model.PosterUrl;
            }
            else
            {
                imgView.Clear();
            }
        }
    }
}
