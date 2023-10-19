using Localidades.Application.ViewModels.ResultsViewModels;

public class PagedResultViewModel<T> : ResultViewModel<T>
{
    public PagedResultViewModel(T data, int skip, int take, int total, List<string> errors)
        : base(data, errors)
    {
        this.Paging = new PagingInfo
        {
            Skip = skip,
            Take = take,
            Total = total
        };
    }

    public PagedResultViewModel(T data, int skip, int take, int total)
        : base(data)
    {
        this.Paging = new PagingInfo
        {
            Skip = skip,
            Take = take,
            Total = total
        };
    }

    public PagingInfo Paging { get; set; }

    public class PagingInfo
    {
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public int Total { get; set; }
    }
}