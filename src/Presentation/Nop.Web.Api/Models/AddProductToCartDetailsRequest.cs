using System.Collections.Generic;

namespace Nop.Web.Api.Models;

public class AddProductToCartDetailsRequest
{
    public int ProductId { get; set; }
    public int ShoppingCartTypeId { get; set; }
    public Dictionary<string, string> Form { get; set; } = new();
}
