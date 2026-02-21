namespace Survey_Basket.Abstractions;

public class PaginatedList<T>
{

    public List<T> Items { get; private set; }
    public int PageNumber { get; private set; }
    public int TotalPage { get; private set; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPage;

    public PaginatedList(List<T> items, int pageNumber, int count, int pageSize)
    {
        Items = items;
        PageNumber = pageNumber;
        TotalPage = (int)Math.Ceiling(count / (double)pageSize);
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageSize, int pageNumber,CancellationToken cancellationToken=default!)
    {
        var count = await source.CountAsync();

        var items =await source.Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();


        // calculate the number of items to skip based on the page number and page size
        // (page Number - 1) * page Size

        return new PaginatedList<T>(items, pageNumber, count, pageSize);
    }
}
