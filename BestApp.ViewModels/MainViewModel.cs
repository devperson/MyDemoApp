using BestApp.Abstraction.General.AppService;
using BestApp.ViewModels.Base;
using BestApp.ViewModels.ItemViewModel;
using Logging.Aspects;
using System.Collections.ObjectModel;

namespace BestApp.ViewModels
{
    [LogMethods]
    public class MainViewModel : PageViewModel
    {
        private readonly Lazy<IProductService> productService;

        public MainViewModel(InjectedServices services, Lazy<IProductService> productService) : base(services)
        {
            this.productService = productService;

            AddCommand = new AsyncCommand(OnAddCommand);
            ItemSelectedCommand = new AsyncCommand(OnItemSelectedCommand);
        }

        

        public ObservableCollection<ProductItemViewModel> ProductItems { get; set; }
        public AsyncCommand AddCommand { get; set; }
        public AsyncCommand ItemSelectedCommand { get; set; }

        public override async void OnFirstTimeAppears()
        {
            base.OnFirstTimeAppears();

            await LoadData();
        }

       

        public override void OnNavigatedTo(Abstraction.General.Platform.INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            try
            {
                if (parameters.ContainsKey(CreateProductViewModel.NEW_PRODUCT_PARAM))
                {
                    var newProduct = parameters.GetValue<ProductItemViewModel>(CreateProductViewModel.NEW_PRODUCT_PARAM);
                    ProductItems.Add(newProduct);
                }
            }
            catch (Exception ex)
            {
                Services.LoggingService.TrackError(ex);
            }
        }

        private async Task OnAddCommand(object arg)
        {
            try
            {
                await Navigate(nameof(CreateProductViewModel));
            }
            catch (Exception ex)
            {
                HandleUIError(ex);
            }
        }

        private async Task OnItemSelectedCommand(object arg)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(Exception ex)
            {
                HandleUIError(ex);
            }
        }


        public async Task LoadData()
        {
            try
            {
                var result = await productService.Value.GetSome(10, 0);
                if (result.Success)
                {
                    var list = result.Value.Select(x => new ProductItemViewModel(x));
                    ProductItems = new ObservableCollection<ProductItemViewModel>(list);
                }
            }
            catch (Exception ex)
            {
                Services.LoggingService.TrackError(ex);
            }
        }
    }
}
