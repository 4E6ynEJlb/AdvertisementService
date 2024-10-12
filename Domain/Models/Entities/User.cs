namespace Domain.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<Advertisement> Advertisements { get; set; }
        public User()
        {
            Advertisements = new List<Advertisement>();
        }
    }
}
