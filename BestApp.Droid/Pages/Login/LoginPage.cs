using Android.Views;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using BestApp.ViewModels.Login;
using BestApp.ViewModels.Movies;
using BestApp.X.Droid.Pages.Base;
using BestApp.X.Droid.Pages.Movies.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.Droid.Pages.Login;

public class LoginPage : LifecyclePage
{
    private Button btnSubmit;
    private EditText txtLogin, txtPassword;

    public new LoginPageViewModel ViewModel
    {
        get
        {
            return base.ViewModel as LoginPageViewModel;
        }
        set
        {
            base.ViewModel = value;
        }
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var fragmentView = inflater.Inflate(Resource.Layout.page_login, container, false);
        btnSubmit = fragmentView.FindViewById<Button>(Resource.Id.btnSubmit);
        txtLogin = fragmentView.FindViewById<EditText>(Resource.Id.txtLogin);
        txtPassword = fragmentView.FindViewById<EditText>(Resource.Id.txtPassword);


        txtLogin.TextChanged += TxtLogin_TextChanged;
        txtPassword.TextChanged += TxtPassword_TextChanged;
        btnSubmit.Click += BtnSubmit_Click;

        return fragmentView;
    }

    private void TxtLogin_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
    {
        this.ViewModel.Login = this.txtLogin.Text;
    }

    private void TxtPassword_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
    {
        this.ViewModel.Password = this.txtPassword.Text;
    }

    private void BtnSubmit_Click(object sender, EventArgs e)
    {
        this.ViewModel.SubmitCommand.Execute();
    }
}
