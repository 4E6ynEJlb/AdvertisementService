namespace Infrastructure.Interfaces
{
    public interface IFileClient
    {
        public Task<MemoryStream> GetAsync(string name, CancellationToken cancellationToken);
        public Task SaveAsync(Stream stream, string name, CancellationToken cancellationToken);
        public Task DeleteAsync(string name, CancellationToken cancellationToken);
    }
}
