namespace Application.Models.ViewModels
{
    public class RegisterUserModel
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
    }
}
