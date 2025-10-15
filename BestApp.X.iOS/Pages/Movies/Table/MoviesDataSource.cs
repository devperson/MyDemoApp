using Base.Impl.Texture.iOS.UI.Utils.XF.Extensions;
using BestApp.ViewModels.Movies.ItemViewModel;
using BestApp.X.iOS.Pages.Movies.Table;
using Drastic.Texture;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BestApp.X.iOS.Pages.Movies;
public class MoviesDataSource : ASTableDataSource
{        
    private readonly MoviesPage._Node pageNode;
    private ObservableCollection<MovieItemViewModel> collection;

    public MoviesDataSource(MoviesPage._Node pageNode)
    {
        this.pageNode = pageNode;
    }

    public void OnCollectionSet()
    {
        if (this.pageNode.ViewModel.MovieItems != null)
        {
            if (collection != null)
            {
                collection.CollectionChanged -= Collection_CollectionChanged;
            }

            collection = this.pageNode.ViewModel.MovieItems;
            collection.CollectionChanged += Collection_CollectionChanged;
        }

        pageNode.tableNode.ReloadData();
    }

    public override nint NumberOfSectionsInTableNode(ASTableNode tableNode)
    {
        return 1;
    }

    public override nint NumberOfRowsInSection(ASTableNode tableNode, nint section)
    {
        var count = this.collection?.Count;
        return count.GetValueOrDefault();
    }

    public override ASCellNodeBlock NodeBlockForRowAtIndexPath(ASTableNode tableNode, NSIndexPath indexPath)
    {
        var model = this.collection[indexPath.Row];

        return () =>
        {
            var cellNode = new CellMovie(model);
            return cellNode;
        };
    }

    private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            var indexes = IndexPathHelpers.GenerateIndexPathRange(0, e.NewStartingIndex, e.NewItems.Count);
            pageNode.tableNode.InsertRowsAtIndexPaths(indexes, UITableViewRowAnimation.Automatic);
            pageNode.tableNode.ScrollToRowAtIndexPath(NSIndexPath.Create(0, 0), UITableViewScrollPosition.Middle, true);
        }
        else if (e.Action == NotifyCollectionChangedAction.Replace)
        {            
            var indexes = IndexPathHelpers.GenerateIndexPathRange(0, e.NewStartingIndex, e.NewItems.Count);
            pageNode.tableNode.ReloadRowsAtIndexPaths(indexes, UITableViewRowAnimation.Automatic);
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            var indexes = IndexPathHelpers.GenerateIndexPathRange(0, e.OldStartingIndex, e.OldItems.Count);
            pageNode.tableNode.DeleteRowsAtIndexPaths(indexes, UITableViewRowAnimation.Automatic);
        }
    }
}
