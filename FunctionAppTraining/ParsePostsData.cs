using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAppTraining;

public static class ParsePostsData
{
    [FunctionName("ParsePostsData")]
    public static async Task RunAsync(
        [BlobTrigger("posts-data/{name}")] Stream postsBlob,
        string name, ILogger log,
        [Queue("posts-queue")]ICollector<Post> postsQueue
        )
    {
        log.LogInformation($"ParsePostsData: C# Blob trigger function Processed blob\n Name:{name} \n Size: {postsBlob.Length} Bytes");

        byte[] contents = new byte[postsBlob.Length];

        var readAsync = await postsBlob.ReadAsync(contents, 0, (int)postsBlob.Length);
        log.LogDebug($"readAsync: {readAsync}");
        
        var result = Encoding.UTF8.GetString(contents);

        ICollection<Post> posts = JsonConvert.DeserializeObject<ICollection<Post>>(result);

        int count = 0;
        foreach (var post in posts)
        {
            postsQueue.Add(post);
            count++;
        }
        
        log.LogInformation($"ParsePostsData: Wrote {count} posts.");
    }
}