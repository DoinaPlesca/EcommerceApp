namespace EcommerceApp.Models;

public class ListingDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Condition { get; set; }
    public string SellerId { get; set; }
    public List<string> ImageUrls { get; set; }
}