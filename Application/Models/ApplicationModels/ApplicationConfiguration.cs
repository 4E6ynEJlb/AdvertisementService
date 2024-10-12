namespace Application.Models.ApplicationModels
{
    public class ApplicationConfiguration
    {
        public const string OPTIONS_NAME = "ApplicationConfiguration";
        public int UserAdvertisementsMaxCount { get; set; }
        public int AdvertisementLifetimeDays { get; set; }
        public int AdvertisementsRemovationTicksInterval { get; set; }
        public required string ImagesLinkTemplate { get; set; }
        public int CacheSlidingExpirationMinutes { get; set; }
    }
}