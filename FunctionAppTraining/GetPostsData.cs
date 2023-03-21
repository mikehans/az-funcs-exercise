using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FunctionAppTraining;

public static class GetPostsData
{
    private static HttpClient _httpClient = new HttpClient();
    
    [FunctionName("GetPostsData")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, 
        ILogger log,
        [Blob("posts-data/{rand-guid}.json", FileAccess.Write)] Stream blobby
        )
    {
        log.LogInformation("GetPostsData: Initiated posts GET.");

        try
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");

            response.EnsureSuccessStatusCode();

            var content = response.Content;
            
            // write data to blob
            Stream readStream = await content.ReadAsStreamAsync();
            await readStream.CopyToAsync(blobby);
            log.LogInformation("GetPostsData: Wrote data to blob");
            return new OkObjectResult($"Wrote data to blob");

        }
        catch (Exception e)
        {
            log.LogError($"GetPostsData: Could not get data. Message: {e.Message}");
            return new BadRequestObjectResult("Error while processing messages. See logs.");
        }
    }
}

