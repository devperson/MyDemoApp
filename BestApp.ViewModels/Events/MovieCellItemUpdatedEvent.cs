using BestApp.ViewModels.Movies.ItemViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.ViewModels.Events;
public class MovieCellItemUpdatedEvent : PubSubEvent<MovieItemViewModel>
{
}
