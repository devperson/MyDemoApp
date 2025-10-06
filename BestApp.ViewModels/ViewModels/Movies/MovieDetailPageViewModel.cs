using BestApp.ViewModels.Base;
using Logging.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.ViewModels.Movies;

[LogMethods]
public class MovieDetailPageViewModel : PageViewModel
{
    public MovieDetailPageViewModel(InjectedServices services) : base(services)
    {
    }
}
