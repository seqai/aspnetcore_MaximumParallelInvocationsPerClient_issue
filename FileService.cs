using System.Buffers;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.AspNetCore.Components.Forms;

namespace FileUploadSample;

public class FileService
{
    public async Task<string> UploadFileAsync(IBrowserFile file)
    {
        var sourceStream = file.OpenReadStream(int.MaxValue);
        var targetStream = new MemoryStream();
        var cancellationToken = new CancellationToken();

        await sourceStream.CopyToAsync(targetStream, cancellationToken);

        using (var md5 = MD5.Create())
        {
            var hashBytes = md5.ComputeHash(targetStream.ToArray());
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}