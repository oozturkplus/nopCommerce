using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Core.Infrastructure;
using Nop.Services.Catalog;
using Nop.Services.Orders;
using Nop.Services.Seo;
using Nop.Web.Api.Models;
using Nop.Web.Factories;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Web.Api.Controllers;

[ApiController]
[Route("api/shoppingcart")]
public class ShoppingCartApiController : ControllerBase
{
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IShoppingCartModelFactory _shoppingCartModelFactory;
    private readonly IProductService _productService;
    private readonly IProductAttributeParser _productAttributeParser;
    private readonly IProductAttributeService _productAttributeService;
    private readonly IWorkContext _workContext;
    private readonly IStoreContext _storeContext;
    private readonly INopUrlHelper _nopUrlHelper;
    private readonly IUrlRecordService _urlRecordService;

    public ShoppingCartApiController(
        IShoppingCartService shoppingCartService,
        IShoppingCartModelFactory shoppingCartModelFactory,
        IProductService productService,
        IProductAttributeParser productAttributeParser,
        IProductAttributeService productAttributeService,
        IWorkContext workContext,
        IStoreContext storeContext,
        INopUrlHelper nopUrlHelper,
        IUrlRecordService urlRecordService)
    {
        _shoppingCartService = shoppingCartService;
        _shoppingCartModelFactory = shoppingCartModelFactory;
        _productService = productService;
        _productAttributeParser = productAttributeParser;
        _productAttributeService = productAttributeService;
        _workContext = workContext;
        _storeContext = storeContext;
        _nopUrlHelper = nopUrlHelper;
        _urlRecordService = urlRecordService;
    }

    [HttpPost("catalog")]
    public async Task<IActionResult> AddProductToCartCatalog([FromBody] AddProductToCartCatalogRequest request)
    {
        var product = await _productService.GetProductByIdAsync(request.ProductId);
        if (product == null)
            return NotFound();

        var redirectUrl = await _nopUrlHelper.RouteGenericUrlAsync<Product>(new { SeName = await _urlRecordService.GetSeNameAsync(product) });

        if (product.ProductType != ProductType.SimpleProduct ||
            product.OrderMinimumQuantity > request.Quantity ||
            product.CustomerEntersPrice ||
            product.IsRental ||
            _productService.ParseAllowedQuantities(product).Length > 0)
        {
            return Ok(new { redirect = redirectUrl });
        }

        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();
        var cartType = (ShoppingCartType)request.ShoppingCartTypeId;

        var warnings = await _shoppingCartService.AddToCartAsync(customer, product, cartType, store.Id, quantity: request.Quantity);
        if (warnings.Any())
            return Ok(new { success = false, message = warnings });

        return Ok(new { success = true });
    }

    [HttpPost("details")]
    public async Task<IActionResult> AddProductToCartDetails([FromBody] AddProductToCartDetailsRequest request)
    {
        var product = await _productService.GetProductByIdAsync(request.ProductId);
        if (product == null)
            return NotFound();

        if (product.ProductType != ProductType.SimpleProduct)
            return BadRequest(new { message = "Only simple products could be added to the cart" });

        var formDict = request.Form.ToDictionary(k => k.Key, k => new StringValues(k.Value));
        var formCollection = new FormCollection(formDict);

        var addToCartWarnings = new List<string>();
        var customerEnteredPrice = await _productAttributeParser.ParseCustomerEnteredPriceAsync(product, formCollection);
        var quantity = _productAttributeParser.ParseEnteredQuantity(product, formCollection);
        var attributes = await _productAttributeParser.ParseProductAttributesAsync(product, formCollection, addToCartWarnings);
        _productAttributeParser.ParseRentalDates(product, formCollection, out var rentalStartDate, out var rentalEndDate);

        var cartType = (ShoppingCartType)request.ShoppingCartTypeId;
        var customer = await _workContext.GetCurrentCustomerAsync();
        var store = await _storeContext.GetCurrentStoreAsync();

        addToCartWarnings.AddRange(await _shoppingCartService.AddToCartAsync(customer, product, cartType, store.Id, attributes, customerEnteredPrice, rentalStartDate, rentalEndDate, quantity));

        if (addToCartWarnings.Any())
            return Ok(new { success = false, message = addToCartWarnings });

        return Ok(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> GetShoppingCart()
    {
        var store = await _storeContext.GetCurrentStoreAsync();
        var cart = await _shoppingCartService.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), ShoppingCartType.ShoppingCart, store.Id);
        var model = new Nop.Web.Models.ShoppingCart.ShoppingCartModel();
        model = await _shoppingCartModelFactory.PrepareShoppingCartModelAsync(model, cart);
        return Ok(model);
    }
}
