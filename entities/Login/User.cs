
namespace entities
{
    public class User
    {
        public User()
        {
            Roles = new List<string>();
        }
        public string? Username { get; set; }
        public List<String> Roles { get; set; }
    }

}
