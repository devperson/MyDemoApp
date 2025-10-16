using Base.Abstractions.UI;
using Base.Impl.Texture.iOS.UI.Controls.Nodes;
using Base.MVVM.Navigation;
using Base.MVVM.ViewModels;
using Drastic.Texture;
using System.ComponentModel;

namespace Base.Impl.Texture.iOS.Pages;

public class iOSLifecyclePage : iOSPage
{
    private bool onApprearedSent = false;
    public event EventHandler Appeared;
    public event EventHandler Disappeared;   

    /// <summary>
    /// Indicates is wether page was navigated with animation. 
    /// It is usefull when navigating back (pop) to check if we need to apply animation for navigation back, 
    /// so it will be consistent with forward (push) navigation. 
    /// </summary>
    internal bool pushNavAnimated = true;

    private BusyIndicatorNode busyIndicatorNode;
    internal SnackbarNode snackbarNode;

    protected override void SetPageNode(IAsPageNode node)
    {
        loggingService.Log($"{this.GetType().Name}.SetPageNode() (from base)");

        base.SetPageNode(node);

        this.busyIndicatorNode = new BusyIndicatorNode();
        this.AddAndStretchNode(this.busyIndicatorNode);

        this.snackbarNode = new SnackbarNode();
        this.AddAndStretchNode(this.snackbarNode);

        this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    public override void ViewSafeAreaInsetsDidChange()
    {
        base.ViewSafeAreaInsetsDidChange();

        //safe area inset is initialized or changed so we need to force it to recalculate page layout 
        if (this.Node != null)
        {
            (this.Node as ASDisplayNode).SetNeedsLayout();
        }
    }

    public override void ViewWillAppear(bool animated)
    {
        loggingService.Log($"{this.GetType().Name}.ViewWillAppear() (from base)");

        base.ViewWillAppear(animated);

        this.ViewModel?.OnAppearing();
    }

    public override void ViewWillDisappear(bool animated)
    {
        loggingService.Log($"{this.GetType().Name}.ViewWillDisappear() (from base)");

        base.ViewWillDisappear(animated);

        this.ViewModel?.OnDisappearing();
    }

    public override void ViewDidAppear(bool animated)
    {
        loggingService.Log($"{this.GetType().Name}.ViewDidAppear() (from base)");

        base.ViewDidAppear(animated);

        this.Appeared?.Invoke(this, EventArgs.Empty);

        this.SendOnAppeared();
    }

    public override void ViewDidDisappear(bool animated)
    {
        loggingService.Log($"{this.GetType().Name}.ViewDidDisappear() (from base)");

        base.ViewDidDisappear(animated);

      
        this.Disappeared?.Invoke(this, EventArgs.Empty);
    }

    private void SendOnAppeared()
    {        
        if (onApprearedSent)//we want to send OnAppeared only once
            return;

        onApprearedSent = true;

        loggingService.Log($"{this.GetType().Name}.SendOnAppeared() (from base)");

        this.ViewModel?.OnAppeared();
    }

    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        //loggingService.Log($"{this.GetType().Name}.ViewModel_PropertyChanged({e.PropertyName})");

        if (e.PropertyName == nameof(this.ViewModel.BusyLoading))
        {
            this.ShowLoadingIndicator(this.ViewModel.BusyLoading);            
        }        
        else
        {
            OnViewModelPropertyChanged(e.PropertyName);
        }
    }

    public virtual void ShowToasMsg(string toastMessage, SeverityType severity)
    {
        this.snackbarNode.SetText(toastMessage, severity);
        this.snackbarNode.Show();
    }

    protected virtual void ShowLoadingIndicator(bool busyLoading)
    {
        if(busyLoading)
        {
            busyIndicatorNode.Show();
        }
        else
        {
            busyIndicatorNode.Close();
        }        
    }

    protected virtual void OnViewModelPropertyChanged(string propertyName)
    {
        this.Node?.OnViewModelPropertyChanged(propertyName);
    }

    public void OnBackBtnPressed(object sender, EventArgs e)
    {
        this.ViewModel.BackCommand.Execute();
    }

    public void Destroy()
    {
        loggingService.Log($"{this.GetType().Name}.Destroy() (from base)");

        if (this.ViewModel != null)
        {
            this.ViewModel.Destroy();
            this.ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        if(this.Node != null)
        {
            this.Node.Destroy();
        }
    }
}
