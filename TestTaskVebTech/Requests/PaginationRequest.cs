namespace TestTaskVebTech.Requests
{
    public class PaginationRequest
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
