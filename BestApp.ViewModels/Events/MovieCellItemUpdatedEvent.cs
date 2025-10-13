using Base.Abstractions.Messaging;
using BestApp.ViewModels.Movies.ItemViewModel;

namespace BestApp.ViewModels.Events;
public class MovieCellItemUpdatedEvent : SubMessage<MovieItemViewModel>
{
}
