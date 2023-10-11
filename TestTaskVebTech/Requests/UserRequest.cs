namespace TestTaskVebTech.Requests
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public List<string> Roles { get; set; }
    }
}
