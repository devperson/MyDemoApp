using Base.Abstractions.PlatformServices;
using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using DryIoc;

namespace Base.Impl.Texture.iOS.UI.Utils
{
    public class KeyboardViewResize
    {
        private nfloat bottomMargin;
        private readonly BasePageNode pageNode;
        private NSObject keyboardShowObserver;
        private NSObject keyboardHideObserver;
        private IKeyboardResizeTypeService keyboardResizeType;

        public KeyboardViewResize(BasePageNode pageNode)
        {
            this.pageNode = pageNode;
            this.bottomMargin = this.pageNode.View.SafeAreaInsets.Bottom;
            this.keyboardResizeType = Base.Impl.iOS.Registrar.appContainer.Resolve<IKeyboardResizeTypeService>();
            this.RegisterForKeyboardNotifications();
        }

        public nfloat GetBottomMargin()
        {            
            return this.bottomMargin;
        }

        public void SetBottomMarin(nfloat value)
        {
            this.bottomMargin = value;
        }

        public void RegisterForKeyboardNotifications()
        {
            if (keyboardShowObserver == null)
                keyboardShowObserver = UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShow);
            if (keyboardHideObserver == null)
                keyboardHideObserver = UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHide);
        }

        public void Destroy()
        {
            if (keyboardShowObserver != null)
            {
                keyboardShowObserver.Dispose();
                keyboardShowObserver = null;
            }

            if (keyboardHideObserver != null)
            {
                keyboardHideObserver.Dispose();
                keyboardHideObserver = null;
            }
        }

        private void OnKeyboardShow(object sender, UIKeyboardEventArgs args)
        {
            var result = (NSValue)args.Notification.UserInfo.ObjectForKey(new NSString(UIKeyboard.FrameEndUserInfoKey));
            var keyboardSize = result.RectangleFValue.Size;

            if (this.keyboardResizeType.CanResizeContent(this.pageNode.ViewModel.InstanceId))
            {
                this.bottomMargin = (int)keyboardSize.Height;
                this.pageNode.SetNeedsLayout();
                this.pageNode.LayoutIfNeeded();
            }
        }

        private void OnKeyboardHide(object sender, UIKeyboardEventArgs args)
        {
            if (this.keyboardResizeType.CanResizeContent(this.pageNode.ViewModel.InstanceId))
            {
                this.bottomMargin = this.pageNode.View.SafeAreaInsets.Bottom;
                this.pageNode.SetNeedsLayout();
                this.pageNode.LayoutIfNeeded();
            }
        }
    }
}
