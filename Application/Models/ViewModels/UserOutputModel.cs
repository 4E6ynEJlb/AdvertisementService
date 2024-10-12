namespace Application.Models.ViewModels
{
    public class UserOutputModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public bool IsAdmin { get; set; }
    }
}
