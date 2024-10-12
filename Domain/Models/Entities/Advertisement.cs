using Application.Models.ViewModels;

namespace Domain.Models.Entities
{
    public class Advertisement
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public required string Text { get; set; }
        public string? ImageName { get; set; }
        public string? ImageContentType { get; set; }
        public double? Rating { get; set; }
        public int AlreadyRated { get; set; }
        public DateTime Created { get; set; }
        public DateTime WillBeDeleted { get; set; }
        public AdvertisementOutputModel ToAdvertisementOutputModel(string linkTemplate)
        {
            string? imageLink = ImageName is null ?
                null : $"{linkTemplate}{ImageName}";
            return new AdvertisementOutputModel()
            {
                Id = Id,
                Number = Number,
                UserId = UserId,
                Text = Text,
                ImageLink = imageLink,
                Rating = Rating,
                Created = Created,
                WillBeDeleted = WillBeDeleted
            };
        }
    }
}
