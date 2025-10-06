using Android.Views;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.Droid.Pages.Movies.Adapter;

public class MoviesItems_Adapter : RecyclerView.Adapter
{
    private readonly MoviesPage page;

    public MoviesItems_Adapter(MoviesPage page)
    {
        this.page = page;
    }

    public override int ItemCount => (this.page?.ViewModel?.MovieItems?.Count).GetValueOrDefault();

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        var cellView = LayoutInflater.From(parent.Context)
                                     .Inflate(Resource.Layout.cell_movie, parent, false);

        var viewHolder = new MovieItem_ViewHolder(cellView);
        return viewHolder;
    }

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        var model = this.page.ViewModel.MovieItems[position];
        var viewHolder = holder as MovieItem_ViewHolder;
        viewHolder.SetData(model);
    }
}
