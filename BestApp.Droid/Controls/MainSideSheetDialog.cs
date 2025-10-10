using Android.Content;
using Android.Views;
using BestApp.ViewModels.Helper;
using BestApp.ViewModels.Movies;
using Common.Abstrtactions;
using Google.Android.Material.SideSheet;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.Droid.Controls;
public class MainSideSheetDialog : SideSheetDialog
{
    ViewGroup btnLogout;
    TextView lblVersion;
    public MainSideSheetDialog(Context context) : base(context)
    {
    }

    public override void SetContentView(int p0)
    {
        base.SetContentView(p0);
        
        btnLogout = this.FindViewById<ViewGroup>(Resource.Id.btnLogout);
        lblVersion = this.FindViewById<TextView>(Resource.Id.lblVersion);
        btnLogout.Click += btnMenuItem_Click;

        lblVersion.Text = AppInfo.BuildString;
    }
    
    private void btnMenuItem_Click(object sender, EventArgs e)
    {
        try
        {            
            this.Dismiss();

            var btn = (ViewGroup)sender;
            var menuItem = this.GetClickedMenu(btn);
            var mainVm = MainActivity.Instance.GetRootPageViewModel() as MoviesPageViewModel;

            if (mainVm != null)
            {
                mainVm.Services.LoggingService.Log($"Selected menu details: menuItem: {menuItem}, rootPage: {mainVm}");
                mainVm.MenuTappedCommand.Execute(menuItem);
            }
        }
        catch (Exception ex)
        {
            var logger = Prism.Ioc.ContainerLocator.Current.Resolve<ILoggingService>();
            logger.TrackError(ex);
        }
    }

    private MenuItem GetClickedMenu(ViewGroup btn)
    {
        var menuEnum = MenuType.Logout;
        return new MenuItem { Type = menuEnum };
    }
}