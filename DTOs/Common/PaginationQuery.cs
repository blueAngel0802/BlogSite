namespace BlogApp.Api.DTOs.Common;

public class PaginationQuery
{
    private const int MaxSize = 50;

    public int Page { get; set; } = 1;

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxSize ? MaxSize : value;
    }

    public int Skip => (Page - 1) * PageSize;
}