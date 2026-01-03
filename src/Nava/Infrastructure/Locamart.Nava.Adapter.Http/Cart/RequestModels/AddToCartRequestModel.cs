namespace Locamart.Nava.Adapter.Http.Cart.RequestModels;

public class AddToCartRequestModel
{
    public Guid InventoryId { get; set; }
    public int Quantity { get; set; }
}

