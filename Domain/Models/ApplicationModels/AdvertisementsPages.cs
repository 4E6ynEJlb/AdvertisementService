using Application.Models.ViewModels;
using Domain.Models.Entities;

namespace Domain.Models.ApplicationModels
{
    public class AdvertisementsPages
    {
        public required Advertisement[] Advertisements { get; set; }
        public int PagesCount { get; set; }
        public AdvertisementsPagesOutput ToAdvertisementsPagesOutput(string linkTemplate)
        {
            AdvertisementOutputModel[] resultArray = new AdvertisementOutputModel[Advertisements.Length];
            for (int index = 0; index < Advertisements.Length; index++)
            {
                resultArray[index] = Advertisements[index].ToAdvertisementOutputModel(linkTemplate);
            }
            return new AdvertisementsPagesOutput()
            {
                PagesCount = PagesCount,
                Advertisements = resultArray
            };
        }
    }
}
