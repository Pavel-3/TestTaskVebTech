using TestTaskVebTech.Data.Entities;
namespace TestTaskVebTech.Requests
{
    public class UserFiltRequest
    {
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
    }
}
