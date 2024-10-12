namespace Application.Models.ViewModels
{
    public class AdvertisementsPagesOutput
    {
        public required AdvertisementOutputModel[] Advertisements { get; set; }
        public int PagesCount { get; set; }
    }
}
