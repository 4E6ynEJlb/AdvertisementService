namespace Domain.Models.ViewModels
{
    public class GetAdvertisementsOptions
    {
        public SortCriteria Criterion { get; set; }
        public bool IsASC { get; set; }
        public string? KeyWord { get; set; }
        public double? RatingLow { get; set; }
        public double? RatingHigh { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
