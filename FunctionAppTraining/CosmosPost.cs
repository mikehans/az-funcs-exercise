using Newtonsoft.Json;

namespace FunctionAppTraining;

public class CosmosPost
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    
}