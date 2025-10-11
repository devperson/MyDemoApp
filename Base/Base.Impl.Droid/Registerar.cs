using Base.Abstractions.Platform;
using Base.Abstractions.UI;
using Base.Impl.Droid.PlatformServices;
using Base.Impl.Droid.PlatformServices.Video;
using Base.Impl.Droid.UI;
using Base.Impl.PlatformServices;
using DryIoc;
using Mapster;

namespace Base.Impl.Droid
{
    public static class Registerar
    {
        public static void RegisterTypes(IContainer container)
        {
            container.Register<IPreferencesService, PreferencesService>(Reuse.Singleton);
            container.Register<IDirectoryService, DirectoryService>(Reuse.Singleton);
            container.Register<IPopupAlert, MockPopup>(Reuse.Singleton);
            container.Register<IPlatformErrorService, PlatformErrorService>(Reuse.Singleton);
            container.Register<IAlertDialogService, DroidAlertDialogService>(Reuse.Singleton);
            container.Register<IDevice, DeviceInfoService>(Reuse.Singleton);
            container.Register<IDeviceThreadService, DeviceThreadService>(Reuse.Singleton);
            container.Register<IResizeImageService, ResizeImageService>(Reuse.Singleton);
            container.Register<IVideoService, VideoService>(Reuse.Singleton);
            container.Register<IZipService, ZipService>(Reuse.Singleton);
            container.Register<IShareFileService, ShareFileService>(Reuse.Singleton);
        }
    }
}
