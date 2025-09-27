using BestApp.Abstraction.General.AppService.Services;
using BestApp.Abstraction.General.Platform;
using BestApp.ViewModels.Base;
using BestApp.ViewModels.ItemViewModel;
using Common.Abstrtactions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.ViewModels
{
    public class MainViewModel : PageViewModel
    {
        private readonly Lazy<IProductService> productService;

        public MainViewModel(InjectedServices services, Lazy<IProductService> productService) : base(services)
        {
            this.productService = productService;
        }

        public ObservableCollection<ProductItemViewModel> ProductItems { get; set; }

        public override void OnFirstTimeAppears()
        {
            try
            {
                base.OnFirstTimeAppears();
            }
            catch (Exception ex)
            {
                
            }

        }
    }
}
