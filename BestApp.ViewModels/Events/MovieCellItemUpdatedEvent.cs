using Base.Abstractions.Diagnostic;
using BestApp.ViewModels.Movies.ItemViewModel;

namespace BestApp.ViewModels.Events;
public class MovieCellItemUpdatedEvent : SubMessage<MovieItemViewModel>
{
}
