using Android.Views;
using Android.Widget;
using BestApp.ViewModels.Login;
using BestApp.ViewModels.Movies;
using BestApp.X.Droid.Pages.Base;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.Droid.Pages.Movies;

public class AddEditMoviePage : LifecyclePage
{
    private ViewGroup btnPhoto;
    private Button btnSave;
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
        btnPhoto = fragmentView.FindViewById<ViewGroup>(Resource.Id.btnPhoto);
        btnSave = fragmentView.FindViewById<Button>(Resource.Id.btnSave);

        imgView = fragmentView.FindViewById<ImageView>(Resource.Id.imgView);
        txtName = fragmentView.FindViewById<TextView>(Resource.Id.txtName);
        txtDesc = fragmentView.FindViewById<TextView>(Resource.Id.txtDesc);

        this.txtName.Text = this.ViewModel.Model.Name;
        this.txtDesc.Text = this.ViewModel.Model.Description;

        this.OnPhotoChanged();

        this.txtName.TextChanged += TxtName_TextChanged;
        this.txtDesc.TextChanged += TxtDesc_TextChanged;
        this.btnPhoto.Click += BtnPhoto_Click;
        this.btnSave.Click += BtnSave_Click;


        return fragmentView;
    }

    private void TxtName_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
    {
        this.ViewModel.Model.Name = txtName.Text;
    }

    private void TxtDesc_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
    {
        this.ViewModel.Model.Description = txtDesc.Text;
    }

    private void BtnPhoto_Click(object sender, EventArgs e)
    {
        this.ViewModel.ChangePhotoCommand.Execute();
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        this.ViewModel.SaveCommand.Execute();
    }

    private void OnPhotoChanged()
    {
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
