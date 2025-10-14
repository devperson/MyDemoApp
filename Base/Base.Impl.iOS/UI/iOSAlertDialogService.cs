using Base.Abstractions.UI;
using Base.Impl.UI;
using SQLite;
using System.Drawing;
using System.Threading.Channels;

namespace Base.Impl.Texture.iOS.UI;
public class iOSAlertDialogService : IAlertDialogService
{
    public Task DisplayAlert(string title, string message, string cancel = "Close")
    {
        var args = new AlertArguments(title, message, null, cancel);

        this.PresentAlert(args);

        return args.Result.Task;
    }

    public Task<bool> ConfirmAlert(string title, string message, params string[] buttons)
    {
        var accept = buttons.ElementAtOrDefault(0);
        var cancel = buttons.ElementAtOrDefault(1);
        var arguments = new AlertArguments(title, message, accept, cancel);

        var args = new AlertArguments(title, message, accept, cancel);
        this.PresentAlert(args);

        return args.Result.Task;
    }

    public Task<string> DisplayActionSheet(string title, params string[] buttons)
    {
        return this.DisplayActionSheet(title, null, null, buttons);
    }

    public Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
    {
        var args = new ActionSheetArguments(title, cancel, destruction, buttons);
        this.PresentActionSheet(args);

        return args.Result.Task;
    }
   

    readonly int _alertPadding = 10;
    void PresentAlert(AlertArguments arguments)
    {
        var window = new UIWindow { BackgroundColor = UIColor.Clear };

        var alert = UIAlertController.Create(arguments.Title, arguments.Message, UIAlertControllerStyle.Alert);
        var oldFrame = alert.View.Frame;
        alert.View.Frame = new CGRect(oldFrame.X, oldFrame.Y, oldFrame.Width, oldFrame.Height - _alertPadding * 2);

        if (arguments.Cancel != null)
        {
            alert.AddAction(CreateActionWithWindowHide(arguments.Cancel, UIAlertActionStyle.Cancel,
                () => arguments.SetResult(false), window));
        }

        if (arguments.Accept != null)
        {
            alert.AddAction(CreateActionWithWindowHide(arguments.Accept, UIAlertActionStyle.Default,
                () => arguments.SetResult(true), window));
        }

        PresentPopUp(window, alert);
    }    

    void PresentActionSheet(ActionSheetArguments arguments)
    {
        var alert = UIAlertController.Create(arguments.Title, null, UIAlertControllerStyle.ActionSheet);
        var window = new UIWindow { BackgroundColor = UIColor.Clear };

        // Clicking outside of an ActionSheet is an implicit cancel on iPads. If we don't handle it, it freezes the app.
        if (arguments.Cancel != null || UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
        {
            alert.AddAction(CreateActionWithWindowHide(arguments.Cancel ?? "", UIAlertActionStyle.Cancel, () => arguments.SetResult(arguments.Cancel), window));
        }

        if (arguments.Destruction != null)
        {
            alert.AddAction(CreateActionWithWindowHide(arguments.Destruction, UIAlertActionStyle.Destructive, () => arguments.SetResult(arguments.Destruction), window));
        }

        foreach (var label in arguments.Buttons)
        {
            if (label == null)
                continue;

            var blabel = label;

            alert.AddAction(CreateActionWithWindowHide(blabel, UIAlertActionStyle.Default, () => arguments.SetResult(blabel), window));
        }

        PresentPopUp(window, alert, arguments);
    }

    static void PresentPopUp(UIWindow window, UIAlertController alert, ActionSheetArguments arguments = null)
    {
        window.RootViewController = new UIViewController();
        window.RootViewController.View.BackgroundColor = UIColor.Clear;
        window.WindowLevel = UIWindowLevel.Alert + 1;
        window.MakeKeyAndVisible();

        if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad && arguments != null)
        {
            UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();
            var observer = NSNotificationCenter.DefaultCenter.AddObserver(UIDevice.OrientationDidChangeNotification,
                n => { alert.PopoverPresentationController.SourceRect = window.RootViewController.View.Bounds; });

            arguments.Result.Task.ContinueWith(t =>
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(observer);
                UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications();
            }, TaskScheduler.FromCurrentSynchronizationContext());

            alert.PopoverPresentationController.SourceView = window.RootViewController.View;
            alert.PopoverPresentationController.SourceRect = window.RootViewController.View.Bounds;
            alert.PopoverPresentationController.PermittedArrowDirections = 0; // No arrow
        }

        window.RootViewController.PresentViewController(alert, true, null);
    }

    // Creates a UIAlertAction which includes a call to hide the presenting UIWindow at the end
    UIAlertAction CreateActionWithWindowHide(string text, UIAlertActionStyle style, Action setResult, UIWindow window)
    {
        return UIAlertAction.Create(text, style,
                a =>
                {
                    window.Hidden = true;
                    setResult();
                });
    }
}
