using Android.Views;
using Android.Widget;
using BestApp.ViewModels.Login;
using BestApp.ViewModels.Movies;
using BestApp.X.Droid.Pages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.Droid.Pages.Movies;

public class AddEditMoviePage : LifecyclePage
{
    private Button btnPhoto, btnSave;
    private ImageView imgView;
    private TextView txtName, txtDesc;

    public new AddEditMoviePageViewModel ViewModel
    {
        get
        {
            return base.ViewModel as AddEditMoviePageViewModel;
        }
        set
        {
            base.ViewModel = value;
        }
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var fragmentView = inflater.Inflate(Resource.Layout.page_movie_add_edit, container, false);
        btnPhoto = fragmentView.FindViewById<Button>(Resource.Id.btnPhoto);
        btnSave = fragmentView.FindViewById<Button>(Resource.Id.btnSave);

        imgView = fragmentView.FindViewById<ImageView>(Resource.Id.imgView);
        txtName = fragmentView.FindViewById<TextView>(Resource.Id.txtName);
        txtDesc = fragmentView.FindViewById<TextView>(Resource.Id.txtDesc);


        return fragmentView;
    }
}
