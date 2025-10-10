using BestApp.Abstraction.Main.PlatformServices;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace BestApp.Impl.Droid.PlatformServices;

public class ShareFileService : IShareFileService
{
    public Task ShareFile(string title, string path)
    {
        return Share.RequestAsync(new ShareFileRequest
        {
            Title = title,
            File = new ShareFile(path)
        });
    }

    public async Task ShareUri(string uri)
    {
        await Share.RequestAsync(new ShareTextRequest
        {
            Uri = uri,
            Title = "Share Invite Link"
        });
    }
}
