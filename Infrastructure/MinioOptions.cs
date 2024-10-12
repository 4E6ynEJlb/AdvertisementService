namespace Infrastructure
{
    public class MinioOptions
    {
        public const string OPTIONS_NAME = "MinioOptions";
        public required string AccessKey { get; set; }
        public required string SecretKey { get; set; }
        public required string Endpoint { get; set; }
        public required string BucketName { get; set; }
    }
}