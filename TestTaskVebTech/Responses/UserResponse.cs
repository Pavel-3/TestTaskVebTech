using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace TestTaskVebTech.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public List<string> Roles { get; set; }
    }
}
