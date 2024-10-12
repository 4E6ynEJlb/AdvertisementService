namespace Application.Interfaces
{
    public interface IUserHostService
    {
        public Task AssignAsAdminAsync(Guid userId, CancellationToken cancellationToken);
        public Task UnassignAsAdminAsync(Guid userId, CancellationToken cancellationToken);
    }
}
