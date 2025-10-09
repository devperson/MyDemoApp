using Android.Views;
using BestApp.ViewModels.Movies;
using BestApp.X.Droid.Pages.Base;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Graphics.ColorSpace;

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

        this.OnModelUpdated();
     
        this.btnEdit.Click += BtnEdit_Click;

        return fragmentView;
    }

    protected override void OnViewModelPropertyChanged(string propertyName)
    {
        base.OnViewModelPropertyChanged(propertyName);

        if(propertyName == nameof(this.ViewModel.Model))
        {
            this.OnModelUpdated();
        }
    }

    private void BtnEdit_Click(object sender, EventArgs e)
    {
        this.ViewModel.EditCommand.Execute();
    }

    private void OnModelUpdated()
    {
        this.txtName.Text = this.ViewModel.Model.Name;
        this.txtDescription.Text = this.ViewModel.Model.Description;

        if (string.IsNullOrEmpty(this.ViewModel.Model.PosterUrl))
        {
            Glide.With(this.Context)
                      .Load(this.ViewModel.Model.PosterUrl)
                      .Override(200, 300)
                      .SetDiskCacheStrategy(DiskCacheStrategy.All)
                      .Into(imgView);
        }
    }
}
