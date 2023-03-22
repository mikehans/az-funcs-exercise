using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace FunctionAppTraining;

public static class GetPostByPostId
{
    [FunctionName("GetPostByPostId")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log,
        [CosmosDB(
            databaseName: "Stuff",
            containerName: "posts",
            Connection = "CosmosDbConnectionString")]CosmosClient client)
    {   
        log.LogInformation("C# HTTP trigger function processed a request.");
        
        log.LogInformation("Path: " + req.Path);

        string queryId = req.Query["id"].ToString();

        if (string.IsNullOrWhiteSpace(queryId))
        {
            return new NotFoundResult();
        }

        Container container = client.GetDatabase("Stuff").GetContainer("posts");
        

        log.LogInformation($"Querying for {queryId}");

        QueryDefinition queryDefinition = new QueryDefinition(
            "SELECT p.Title, p.id FROM p WHERE p.PostId = @postId").WithParameter("@postId", queryId);
        
        log.LogInformation($"Query Definition: {queryDefinition.QueryText}");
        
        IReadOnlyList<(string Name, object Value)> queryParameters = queryDefinition.GetQueryParameters();
        foreach ((string name, object value) in queryParameters)
        {
            log.LogInformation($"Parameter: {name}, {value}");   
        }

        using FeedIterator<CosmosPost> iterator = container.GetItemQueryIterator<CosmosPost>(queryDefinition);

        List<CosmosPost> cosmosPosts = new List<CosmosPost>();
        while (iterator.HasMoreResults)
        {
            log.LogInformation("iterator has results");
            var response = await iterator.ReadNextAsync();
            
            log.LogInformation($"StatusCode: {response.StatusCode}, Count: {response.Count}");

            foreach (var post in response)
            {
                cosmosPosts.Add(post);                
            }
            
            log.LogInformation($"How many posts in cosmosPosts? {cosmosPosts.Count()}");
            
            // IEnumerable<CosmosPost> responseResource = response.Resource;
            // log.LogInformation($"Count: {responseResource.Count()}");
            // var post = response.First();
            // log.LogInformation(post.Title);
            cosmosPosts.Add(new CosmosPost(){Title="test"});
        }


        // log.LogInformation($"Posts received: {posts.Count()}");
        // var postsSerialised = JsonConvert.SerializeObject(posts);

        return new OkObjectResult(cosmosPosts);
    }
}