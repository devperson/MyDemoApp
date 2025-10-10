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
    private Button btnSave, btnDelete;
    private ImageView imgView;
    private TextView txtName, txtDescription;

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
        btnDelete = fragmentView.FindViewById<Button>(Resource.Id.btnDelete);

        imgView = fragmentView.FindViewById<ImageView>(Resource.Id.imgView);
        txtName = fragmentView.FindViewById<TextView>(Resource.Id.txtName);
        txtDescription = fragmentView.FindViewById<TextView>(Resource.Id.txtDescription);

        this.txtName.Text = this.ViewModel.Model.Name;
        this.txtDescription.Text = this.ViewModel.Model.Overview;

        this.OnPhotoChanged();
        
        this.txtName.TextChanged += TxtName_TextChanged;
        this.txtDescription.TextChanged += TxtDesc_TextChanged;
        this.btnDelete.Click += BtnDelete_Click;
        this.btnPhoto.Click += BtnPhoto_Click;
        this.btnSave.Click += BtnSave_Click;

        return fragmentView;
    }

    public override async void OnViewCreated(View view, Bundle savedInstanceState)
    {
        base.OnViewCreated(view, savedInstanceState);

        //btnDelete is disabled in XML and we only enable it after a bit delay
        //this is so to avoid user accidental click while navigating, because delete is located in same place where we have edit button in prev page
        await Task.Delay(600);
        btnDelete.Enabled = true;
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        this.ViewModel.DeleteCommand.Execute();
    }

    private void TxtName_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
    {
        this.ViewModel.Model.Name = txtName.Text;
    }

    private void TxtDesc_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
    {
        this.ViewModel.Model.Overview = txtDescription.Text;
    }

    private async void BtnPhoto_Click(object sender, EventArgs e)
    {
        await this.ViewModel.ChangePhotoCommand.ExecuteAsync();
        this.OnPhotoChanged();
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        this.ViewModel.SaveCommand.Execute();
    }

    private void OnPhotoChanged()
    {
        if (!string.IsNullOrEmpty(this.ViewModel.Model.PosterUrl))
        {
            Glide.With(this.Context)
                      .Load(this.ViewModel.Model.PosterUrl)
                      .Override(200, 300)
                      .SetDiskCacheStrategy(DiskCacheStrategy.All)
                      .Into(imgView);
        }
        else
        {
            imgView.SetImageDrawable(null);
        }
    }
}
