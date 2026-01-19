namespace Locamart.Nava.Application.Contracts.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Images { get; set; }
    public byte Status { get; set; }
}

