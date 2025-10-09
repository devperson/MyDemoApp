using Android.Views;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Graphics.ColorSpace;
using BestApp.X.Droid.Utils;
using BestApp.ViewModels.Helper;
using BestApp.ViewModels.Movies.ItemViewModel;
using BestApp.ViewModels.Movies;

namespace BestApp.X.Droid.Pages.Movies.Adapter;

public class MovieItem_ViewHolder : RecyclerView.ViewHolder
{
    private readonly View itemView;
    private ImageView imgView;
    private TextView txtName, txtDescription;

    public MovieItem_ViewHolder(View itemView) : base(itemView)
    {
        this.itemView = itemView;
        imgView = itemView.FindViewById<ImageView>(Resource.Id.imgView);
        txtName = itemView.FindViewById<TextView>(Resource.Id.txtName);
        txtDescription = itemView.FindViewById<TextView>(Resource.Id.txtDescription);        
    }

    public void SetData(MovieItemViewModel model)
    {
        this.txtName.Text = model.Name;
        this.txtDescription.Text = model.Description;

        if (!string.IsNullOrEmpty(model.PosterUrl))
        {            
            Glide.With(this.itemView.Context)
                   .Load(model.PosterUrl)
                   .Override(200, 300)                  
                   .SetDiskCacheStrategy(DiskCacheStrategy.All)
                   .Into(imgView);
        }

        this.itemView.Tag = model.ToJavaObj();
        if (!this.itemView.HasOnClickListeners)
        {
            this.itemView.Click += ItemView_Click;            
        }
    }
        
    private void ItemView_Click(object sender, EventArgs e)
    {
        var itemView = sender as ViewGroup;
        var clickItem = itemView.Tag.ToModel() as MovieItemViewModel;
        var pageVm = MainActivity.Instance.GetCurrentViewModel() as MoviesPageViewModel;

        pageVm.ItemTappedCommand.Execute(clickItem);
    }
}
