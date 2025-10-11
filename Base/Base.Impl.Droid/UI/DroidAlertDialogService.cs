using Android.Content;
using Base.Abstractions.UI;
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

    public Task DisplayAlert(string title, string message)
    {
        var arguments = new AlertArguments(title, message, null, "Close");
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

    private class AlertArguments
    {
        public AlertArguments(string title, string message, string accept, string cancel)
        {
            Title = title;
            Message = message;
            Accept = accept;
            Cancel = cancel;
            Result = new TaskCompletionSource<bool>();
        }

        /// <summary>
        ///     Gets the text for the accept button. Can be null.
        /// </summary>
        public string Accept { get; private set; }

        /// <summary>
        ///     Gets the text of the cancel button.
        /// </summary>
        public string Cancel { get; private set; }

        /// <summary>
        ///     Gets the message for the alert. Can be null.
        /// </summary>
        public string Message { get; private set; }

        public TaskCompletionSource<bool> Result { get; }

        /// <summary>
        ///     Gets the title for the alert. Can be null.
        /// </summary>
        public string Title { get; private set; }

        public void SetResult(bool result)
        {
            Result.TrySetResult(result);
        }
    }

    private class ActionSheetArguments
    {
        public ActionSheetArguments(string title, string cancel, string destruction, IEnumerable<string> buttons)
        {
            Title = title;
            Cancel = cancel;
            Destruction = destruction;
            Buttons = buttons?.Where(c => c != null);
            Result = new TaskCompletionSource<string>();
        }

        /// <summary>
        ///     Gets titles of any buttons on the action sheet that aren't <see cref="Cancel" /> or <see cref="Destruction" />. Can
        ///     be <c>null</c>.
        /// </summary>
        public IEnumerable<string> Buttons { get; private set; }

        /// <summary>
        ///     Gets the text for a cancel button. Can be null.
        /// </summary>
        public string Cancel { get; private set; }

        /// <summary>
        ///     Gets the text for a destructive button. Can be null.
        /// </summary>
        public string Destruction { get; private set; }

        public TaskCompletionSource<string> Result { get; }

        /// <summary>
        ///     Gets the title for the action sheet. Can be null.
        /// </summary>
        public string Title { get; private set; }

        public void SetResult(string result)
        {
            Result.TrySetResult(result);
        }
    }
}
