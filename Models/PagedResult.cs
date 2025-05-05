namespace EcommerceApp.Models;

public class PagedResult<T>
{
    public List<T> Items { get; set; }
    public long TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}