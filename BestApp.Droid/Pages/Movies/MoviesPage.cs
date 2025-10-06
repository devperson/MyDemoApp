using Android.Views;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using BestApp.ViewModels.Movies;
using BestApp.X.Droid.Pages.Base;
using BestApp.X.Droid.Pages.Movies.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.Droid.Pages.Movies;

public class MoviesPage : LifecyclePage
{
    private Button btnPlus;
    private SwipeRefreshLayout swipeRefreshLayout;
    private RecyclerView recyclerView;
    private MoviesItems_Adapter adapter;

    public new MoviesPageViewModel ViewModel
    {
        get
        {
            return base.ViewModel as MoviesPageViewModel;
        }
        set
        {
            base.ViewModel = value;
        }
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var fragmentView = inflater.Inflate(Resource.Layout.page_movies, container, false);
        btnPlus = fragmentView.FindViewById<Button>(Resource.Id.btnPlus);
        swipeRefreshLayout = fragmentView.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);
        recyclerView = fragmentView.FindViewById<RecyclerView>(Resource.Id.recyclerView);

        adapter = new MoviesItems_Adapter(this);
        recyclerView.SetLayoutManager(new LinearLayoutManager(this.Context));
        recyclerView.SetAdapter(adapter);

        btnPlus.Click += BtnPlus_Click;
        swipeRefreshLayout.Refresh += SwipeRefreshLayout_Refresh;

        return fragmentView;
    }

    protected override void OnViewModelPropertyChanged(string propertyName)
    {
        base.OnViewModelPropertyChanged(propertyName);

        if(propertyName == nameof(this.ViewModel.MovieItems))
        {
            adapter.NotifyDataSetChanged();
        }
        else if(propertyName == nameof(this.ViewModel.IsRefreshing))
        {
            this.swipeRefreshLayout.Refreshing = this.ViewModel.IsRefreshing;
        }
    }

    private void SwipeRefreshLayout_Refresh(object sender, EventArgs e)
    {
        this.ViewModel.RefreshCommand.Execute();
    }

    private void BtnPlus_Click(object sender, EventArgs e)
    {
        this.ViewModel.AddCommand.Execute();
    }
}

