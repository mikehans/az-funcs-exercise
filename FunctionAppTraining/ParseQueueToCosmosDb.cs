using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionAppTraining;

public static class ParseQueueToCosmosDb
{
    [FunctionName("ParseQueueToCosmosDb")]
    public static void RunAsync(
        [QueueTrigger("posts-queue")] Post myQueueItem,
        ILogger log,
        [CosmosDB(
            databaseName: "Stuff",
            containerName: "posts",
            Connection = "CosmosDbConnectionString",
            PartitionKey = "/id",
            CreateIfNotExists = true
        )] out CosmosPost postsDb)
    {
        log.LogInformation($"ParseQueueToCosmosDb: C# Queue trigger function processed: {myQueueItem.Id}");

        log.LogDebug(
            $"myQueueItem: Id:{myQueueItem.Id}, userId: {myQueueItem.UserId}, title: {myQueueItem.Title.Substring(0, 10)}, body: {myQueueItem.Body.Substring(0, 10)}");
        try
        {
            log.LogInformation("Trying to add item");
            postsDb = new CosmosPost()
            {
                Id = System.Guid.NewGuid().ToString(),
                PostId = myQueueItem.Id,
                UserId = myQueueItem.UserId,
                Title = myQueueItem.Title,
                Body = myQueueItem.Body
            };
            log.LogInformation($"ParseQueueToCosmosDb: Wrote post to CosmosDB (maybe)");
        }
        catch (Exception e)
        {
            log.LogError($"Error in ParseQueueToCosmosDb: {e.Message}");
            postsDb = null;
        }
    }
}