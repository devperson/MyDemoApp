using Base.Abstractions.PlatformServices;
using Base.Abstractions.UI;
using Base.Impl.iOS.UI;
using DryIoc;
using Base.Impl.PlatformServices;
using Base.Impl.iOS.PlatformServices;
using Base.MVVM.Navigation;
using Base.Impl.iOS.UI.Navigation;
using Base.Impl.Texture.iOS.UI;
using Base.Impl.Droid.UI;

namespace Base.Impl.iOS
{
    public static class Registrar
    {
        public static IContainer appContainer;
        
        public static void RegisterTypes(IContainer container)
        {
            appContainer = container;

            container.Register<IPreferencesService, PreferencesService>(Reuse.Singleton);
            container.Register<IDirectoryService, DirectoryService>(Reuse.Singleton);            
            container.Register<IPlatformErrorService, PlatformErrorService>(Reuse.Singleton);
            container.Register<IDevice, DeviceInfoService>(Reuse.Singleton);
            container.Register<IDeviceThreadService, DeviceThreadService>(Reuse.Singleton);
            container.Register<IResizeImageService, ResizeImageService>(Reuse.Singleton);
            container.Register<IVideoService, VideoService>(Reuse.Singleton);
            container.Register<IZipService, ZipService>(Reuse.Singleton);
            container.Register<IShareFileService, ShareFileService>(Reuse.Singleton);

            //UI            
            container.Register<IAlertDialogService, iOSAlertDialogService>(Reuse.Singleton);
            container.Register<IMediaPickerService, MediaPickerService>(Reuse.Singleton);
            container.Register<IPageNavigationService, iOSPageNavigationController>(Reuse.Singleton);
            container.Register<ISnackbarService, iOSSnackbarService>(Reuse.Singleton);            
        }
    }
}
