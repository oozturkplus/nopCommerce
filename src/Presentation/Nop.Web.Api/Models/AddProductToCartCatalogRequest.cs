namespace Nop.Web.Api.Models;

public class AddProductToCartCatalogRequest
{
    public int ProductId { get; set; }
    public int ShoppingCartTypeId { get; set; }
    public int Quantity { get; set; }
    public bool ForceRedirection { get; set; }
}
