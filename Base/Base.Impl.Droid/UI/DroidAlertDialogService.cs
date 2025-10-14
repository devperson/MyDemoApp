using Android.Content;
using Base.Abstractions.UI;
using Base.Impl.UI;
using Microsoft.Maui.ApplicationModel;

namespace Base.Impl.Droid.UI;

public class DroidAlertDialogService : IAlertDialogService
{
    public Task<bool> ConfirmAlert(string title, string message, params string[] buttons)
    {
        var accept = buttons.ElementAtOrDefault(0);
        var cancel = buttons.ElementAtOrDefault(1);
        var arguments = new AlertArguments(title, message, accept, cancel);

        this.PostExecuteAlert(arguments);

        return arguments.Result.Task;
    }

    public Task DisplayAlert(string title, string message, string cancel = "Close")
    {
        var arguments = new AlertArguments(title, message, null, cancel);
        this.PostExecuteAlert(arguments);

        return arguments.Result.Task;
    }

    public Task<string> DisplayActionSheet(string title, params string[] buttons)
    {
        return this.DisplayActionSheet(title, null, null, buttons);
    }

    public Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
    {
        var arguments = new ActionSheetArguments(title, cancel, destruction, buttons);

        this.PostExecuteAlertSheet(arguments);

        return arguments.Result.Task;
    }


    private async void PostExecuteAlert(AlertArguments arguments)
    {
        await Task.Delay(10);

        var alert = new AlertDialog.Builder(Platform.CurrentActivity).Create();
        alert.SetTitle(arguments.Title);
        alert.SetMessage(arguments.Message);
        if (arguments.Accept != null)
        {
            alert.SetButton((int)DialogButtonType.Positive, arguments.Accept, (o, args) => arguments.SetResult(true));
        }
        alert.SetButton((int)DialogButtonType.Negative, arguments.Cancel, (o, args) => arguments.SetResult(false));
        alert.CancelEvent += (o, args) => arguments.SetResult(false);
        alert.Show();
    }

    private async void PostExecuteAlertSheet(ActionSheetArguments arguments)
    {
        await Task.Delay(10);

        var builder = new AlertDialog.Builder(Platform.CurrentActivity);
        builder.SetTitle(arguments.Title);
        string[] items = arguments.Buttons.ToArray();
        builder.SetItems(items, (o, args) => arguments.Result.TrySetResult(items[args.Which]));

        if (arguments.Cancel != null)
            builder.SetPositiveButton(arguments.Cancel, (o, args) => arguments.Result.TrySetResult(arguments.Cancel));

        if (arguments.Destruction != null)
            builder.SetNegativeButton(arguments.Destruction, (o, args) => arguments.Result.TrySetResult(arguments.Destruction));

        var dialog = builder.Create();
        builder.Dispose();

        dialog.SetCanceledOnTouchOutside(true);
        dialog.CancelEvent += (o, e) => arguments.SetResult(null);
        dialog.Show();
    }
}
