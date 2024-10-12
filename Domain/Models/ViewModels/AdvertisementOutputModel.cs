namespace Application.Models.ViewModels
{
    public class AdvertisementOutputModel
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Guid UserId { get; set; }
        public required string Text { get; set; }
        public string? ImageLink { get; set; }
        public double? Rating { get; set; }
        public DateTime Created { get; set; }
        public DateTime WillBeDeleted { get; set; }
    }
}
