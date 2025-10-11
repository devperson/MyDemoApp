using Base.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.MVVM.Navigation
{
    public interface IPage
    {
        AppPageViewModel ViewModel { get; set; }
    }
}
