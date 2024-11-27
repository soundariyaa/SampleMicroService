namespace SampleMicroService.Infrastructure;

public class DatabaseSettings
{
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
    public  IDictionary<string, object> Options { get;set; } = new Dictionary<string, object>();
}
