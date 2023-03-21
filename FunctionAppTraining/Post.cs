using System.Text.Json.Serialization;

namespace FunctionAppTraining;

public class Post
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}