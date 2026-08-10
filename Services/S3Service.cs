using Amazon.S3;
using Amazon.S3.Model;

namespace EmployeeManagement.Services;

public class S3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly IConfiguration _configuration;

    public S3Service(
        IAmazonS3 s3Client,
        IConfiguration configuration)
    {
        _s3Client = s3Client;
        _configuration = configuration;
    }

    public async Task<string> UploadFileAsync(
        IFormFile file)
    {
        var bucketName =
            _configuration["AWS:BucketName"];

        if (string.IsNullOrEmpty(bucketName))
        {
            throw new InvalidOperationException(
                "S3 bucket name is not configured.");
        }

        var fileName =
            $"{Guid.NewGuid()}-{file.FileName}";

        using var stream = file.OpenReadStream();

        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = fileName,
            InputStream = stream,
            ContentType = file.ContentType
        };

        await _s3Client.PutObjectAsync(request);

        return fileName;
    }
}