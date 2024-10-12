namespace Application.Interfaces
{
    public interface IAdvertisementAdminService
    {
        public Task DeleteAdvertisementAdminAsync(Guid advertisementId, Guid adminId, CancellationToken cancellationToken);
    }
}
