using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Infrastructure
{
    public class FileClient : IFileClient
    {
        private readonly IMinioClient _client;
        private readonly string _bucketName;
        public FileClient(IMinioClient client, IOptions<MinioOptions> options)
        {
            _client = client;
            _bucketName = options.Value.BucketName;
            BucketEnsureCreated();
        }
        public async Task<MemoryStream> GetAsync(string name, CancellationToken cancellationToken)
        {
            StatObjectArgs statObjectArgs = new StatObjectArgs().WithBucket(_bucketName).WithObject(name);
            var statArgs = await _client.StatObjectAsync(statObjectArgs);
            MemoryStream stream = new MemoryStream(Convert.ToInt32(statArgs.Size));
            GetObjectArgs args = new GetObjectArgs().WithBucket(_bucketName).WithObject(name).WithCallbackStream(async str =>
            {
                await str.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);
            });
            try
            {
                await _client.GetObjectAsync(args, cancellationToken);
            }
            catch(ObjectNotFoundException e)
            {
                throw new FileDoesNotExistException(e);
            }
            return stream;
        }

        public async Task DeleteAsync(string name, CancellationToken cancellationToken)
        {
            RemoveObjectArgs args = new RemoveObjectArgs().WithBucket(_bucketName).WithObject(name);
            await _client.RemoveObjectAsync(args, cancellationToken);
        }
                
        public async Task SaveAsync(Stream stream, string name, CancellationToken cancellationToken)
        {            
            PutObjectArgs args = new PutObjectArgs().WithBucket(_bucketName).WithObject(name).WithStreamData(stream).WithObjectSize(stream.Length).WithContentType("application/octet-stream");
            await _client.PutObjectAsync(args, cancellationToken);
        }

        private async void BucketEnsureCreated()
        {
            BucketExistsArgs bucketExistsArgs = new BucketExistsArgs().WithBucket(_bucketName);
            if (!await _client.BucketExistsAsync(bucketExistsArgs))
            {
                MakeBucketArgs makeBucketArgs = new MakeBucketArgs().WithBucket(_bucketName);
                await _client.MakeBucketAsync(makeBucketArgs);
            }
        }
    }
}
