using Android.Views;
using BestApp.ViewModels.Movies;
using BestApp.X.Droid.Pages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.Droid.Pages.Movies;
public class MovieDetailPage : LifecyclePage
{
    private Button btnEdit;
    private ImageView imgView;
    private TextView txtName, txtDescription;

    public new MovieDetailPageViewModel ViewModel
    {
        get
        {
            return base.ViewModel as MovieDetailPageViewModel;
        }
        set
        {
            base.ViewModel = value;
        }
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var fragmentView = inflater.Inflate(Resource.Layout.page_movie_add_edit, container, false);
        btnEdit = fragmentView.FindViewById<Button>(Resource.Id.btnEdit);        
        imgView = fragmentView.FindViewById<ImageView>(Resource.Id.imgView);
        txtName = fragmentView.FindViewById<TextView>(Resource.Id.txtName);
        txtDescription = fragmentView.FindViewById<TextView>(Resource.Id.txtDescription);


        return fragmentView;
    }
}
