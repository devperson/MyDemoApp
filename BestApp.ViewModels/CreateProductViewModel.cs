using BestApp.Abstraction.General.AppService;
using BestApp.Abstraction.General.UI;
using BestApp.ViewModels.Base;
using BestApp.ViewModels.ItemViewModel;
using Logging.Aspects;

namespace BestApp.ViewModels
{
    [LogMethods]
    public class CreateProductViewModel : PageViewModel
    {
        private readonly Lazy<IProductService> productService;
        private readonly Lazy<IPopupAlert> popupAlert;
        public const string NEW_PRODUCT_PARAM = "newProduct";

        public CreateProductViewModel(InjectedServices services, 
                                      Lazy<IProductService> productService, 
                                      Lazy<IPopupAlert> popupAlert) : base(services)
        {
            CreateCommand = new AsyncCommand(OnCreateCommand);
            this.productService = productService;
            this.popupAlert = popupAlert;
        }

        

        public string Name { get; set; }
        public string Quantity { get; set; }
        public string Cost { get; set; }

        public AsyncCommand CreateCommand { get; set; }

        private async Task OnCreateCommand(object arg)
        {
            try
            {
                int quantity = 0;
                if (!int.TryParse(Quantity, out quantity))
                {
                    throw new InvalidCastException("quantity");
                }
                decimal cost = 0;
                if (!decimal.TryParse(Cost, out cost))
                {
                    throw new InvalidCastException("cost");
                }

                var result = await productService.Value.Add(Name, quantity, cost);
                if(result.Success)
                {
                    var product = result.Value;
                    var productItem = new ProductItemViewModel(product);

                    await NavigateBack(new NavigationParameters()
                    {
                        {NEW_PRODUCT_PARAM, productItem}
                    });
                }
                else
                {
                    await popupAlert.Value.ShowError("Error: Oops something went wrong. Please try again later");
                }                
            }
            catch (InvalidCastException ex) when (ex.Message.Contains("quantity"))
            {
                await popupAlert.Value.ShowError("Error: Please enter valid value for Quantity field");
            }
            catch (InvalidCastException ex) when (ex.Message.Contains("cost"))
            {
                await popupAlert.Value.ShowError("Error: Please enter valid value for Decimal field");
            }
            catch (Exception ex)
            {
                await popupAlert.Value.ShowError("Error: Oops something went wrong. Please try again later");
                Services.LoggingService.TrackError(ex);
            }
        }
    }
}
