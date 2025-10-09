using Android.Views;
using AndroidX.RecyclerView.Widget;
using BestApp.ViewModels.Movies.ItemViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.Droid.Pages.Movies.Adapter;

public class MoviesItems_Adapter : RecyclerView.Adapter
{
    private ObservableCollection<MovieItemViewModel> collection;
    private readonly MoviesPage page;

    public MoviesItems_Adapter(MoviesPage page)
    {
        this.page = page;
    }

    public override int ItemCount => (collection?.Count).GetValueOrDefault();

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

    public void OnCollectionSet()
    {
        if (this.page.ViewModel.MovieItems != null)
        {
            if (collection != null)
            {
                collection.CollectionChanged -= Collection_CollectionChanged;
            }

            collection = this.page.ViewModel.MovieItems;
            collection.CollectionChanged += Collection_CollectionChanged;
        }
    }

    private void Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            if (e.NewStartingIndex == 0 && e.NewItems.Count == 1)//if inserted to 0 then it is new msg entered
            {
                this.NotifyItemInserted(0);
            }
            else
            {
                this.NotifyItemRangeInserted(e.NewStartingIndex, e.NewItems.Count);
            }
        }
        else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
        {
            //item get updated
            this.NotifyItemChanged(e.NewStartingIndex);
        }
        else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
        {
            this.NotifyItemRemoved(e.OldStartingIndex);
        }
    }
}
