namespace Application.Models.ApplicationModels
{
    public class HostCredentials
    {
        public const string OPTIONS_NAME = "HostCredentials";
        public required string HostLogin { get; set; }
        public required string HostPassword { get; set; }
    }
}
