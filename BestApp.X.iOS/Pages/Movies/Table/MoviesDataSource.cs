using BestApp.X.iOS.Pages.Movies.Table;
using Drastic.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.X.iOS.Pages.Movies;
public class MoviesDataSource : ASTableDataSource
{
    public override nint NumberOfSectionsInTableNode(ASTableNode tableNode)
    {
        return 1;
    }

    public override nint NumberOfRowsInSection(ASTableNode tableNode, nint section)
    {
        return 20;
    }

    public override ASCellNodeBlock NodeBlockForRowAtIndexPath(ASTableNode tableNode, NSIndexPath indexPath)
    {        
        return () =>
        {
            return new CellMovie();
        };

        //var model = this.chatListNode.ChatListVm.ChatRoomList[indexPath.Row];

        //return () =>
        //{
        //    var cellNode = new ChatlistCell(model);
        //    return cellNode;
        //};
    }
}
